using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;


namespace BBeBLib.Serializer
{
	/// <summary>
	/// A BinaryReader that knows how to read the types (and in the way) that
	/// is required for BBeB streams. This is namely strings and TagId's.
	/// Strings are length prefixed instead of null terminated, and TagId's
	/// are prefixed with 0xf5. Encoding is always Unicode.
	/// <remarks>This writer also has a handy mechanism to record offsets
	/// to points in the stream which are resolved later on.</remarks>
	/// </summary>
	public class BBeBinaryWriter : BinaryWriter
	{
		class ObjectInfo
		{
			public long offset = 0;
			public long size = 0;
		}

		class ObjectReference
		{
			object m_Object;
			long m_ReferenceOffset;

			public ObjectReference( object obj, long offset )
			{
				m_Object = obj;
				m_ReferenceOffset = offset;
			}

			public object Object
			{
				get { return m_Object; }
			}

			public long RefOffset
			{
				get { return m_ReferenceOffset; }
			}
		}

		List<ObjectReference> m_OffsetReferences = new List<ObjectReference>();
		List<ObjectReference> m_SizeReferences = new List<ObjectReference>();
		Dictionary<object, ObjectInfo> m_ObjectInfo = new Dictionary<object,ObjectInfo>();


		/// <summary>
		/// Constructor. Will construct the base BinaryWriter with Encoding.Unicode.
		/// </summary>
		/// <param name="stream">The stream to which this writer will write.</param>
		public BBeBinaryWriter(Stream stream)
			: base(stream, Encoding.Unicode)
		{
		}

		/// <summary>
		/// Write a tag to the stream.
		/// </summary>
		/// <param name="tag">The tag to write.</param>
		public void Write(TagId tag)
		{
            ushort val = (ushort)((ushort)0xf500 + (ushort)tag);
			Write(  val );
		}

		/// <summary>
		/// Write the string to the BBeB stream.
		/// </summary>
		/// <remarks>This routine writes the string length as a ushort, followed by the
		/// string characters (but not the terminating null character)</remarks>
		/// <param name="value"></param>
		public override void Write(string value)
		{
            ushort val = (ushort)(value.Length * sizeof(char));
			Write( val );	// String size in bytes

			foreach (char ch in value)
			{
				Write(ch);
			}
		}

		public void WriteStreamOffsetReference(object obj)
		{
			m_OffsetReferences.Add(new ObjectReference(obj, Position));

            uint val = 0;
			Write(val);	// ResolveReferences will come back and overwrite this value
		}

		public void WriteStreamSizeReference(object obj)
		{
			m_SizeReferences.Add(new ObjectReference(obj, Position));

            uint val = 0;
            Write(val);	// ResolveReferences will come back and overwrite this value
        }

		public void WriteObjectStart(object obj)
		{
			ObjectInfo info = new ObjectInfo();
			info.offset = Position;
			m_ObjectInfo.Add(obj, info);

			Write(TagId.ObjectStart);
		}

		public void WriteObjectEnd(object obj)
		{
			Debug.Assert(m_ObjectInfo.ContainsKey(obj), "Did you call WriteObjectStart?");

			ObjectInfo info = m_ObjectInfo[obj];

			Write(TagId.ObjectEnd);

			info.size = Position - info.offset;
		}

		public void ResolveReferences()
		{
			long nCurrentPos = Position;

			try
			{
				foreach (ObjectReference offsetRef in m_OffsetReferences)
				{
					Debug.Assert(m_ObjectInfo.ContainsKey(offsetRef.Object), "Did you call WriteObjectStart?");
					ObjectInfo info = m_ObjectInfo[offsetRef.Object];

					Position = offsetRef.RefOffset;	// Go back to where we wrote the original offset value of 0x0

					Write((uint)info.offset);	// And write the correct offset value.
				}

				foreach (ObjectReference sizeRef in m_SizeReferences)
				{
					Debug.Assert(m_ObjectInfo.ContainsKey(sizeRef.Object), "Did you call WriteObjectStart?");
					ObjectInfo info = m_ObjectInfo[sizeRef.Object];

					Position = sizeRef.RefOffset;	// Go back to where we wrote the original offset value of 0x0

					Write((uint)info.size);	// And write the correct offset value.
				}
			}
			finally
			{
				Position = nCurrentPos;
			}
		}

		public long Position
		{
			get
			{
				Flush();
				return BaseStream.Position;
			}
			set
			{
				Flush();
				BaseStream.Seek(value, SeekOrigin.Begin);
			}
		}
	}
}
