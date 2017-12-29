using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using BBeBLib;
using BBeBLib.Serializer;

namespace lrfdecomp
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				if (args.Length < 1)
				{
					throw new ApplicationException("Usage: lrfdecomp <fname> [fname1 ... fnamen]");
				}

				string strWriteDir = "C:\\lrfdecomp";
				Directory.CreateDirectory(strWriteDir);

				foreach (string lrf in args)
				{
					BBeBSerializer serializer = new BBeBSerializer();

					FileInfo finfo = new FileInfo(lrf);
					
					FileStream bbeb = File.OpenRead(lrf);
					BBeB book;
					try
					{
						Debug.WriteLine("Reading " + lrf);
						book = serializer.Deserialize(bbeb);

						string basename = finfo.Name.Substring( 0, finfo.Name.Length - finfo.Extension.Length );
						string strLogFile = Path.Combine(finfo.DirectoryName, basename) + ".log";

						File.Delete(strLogFile);
						StreamWriter writer = new StreamWriter(strLogFile);
						try
						{
							book.WriteDebugInfo(writer);
						}
						finally
						{
							writer.Close();
						}
					}
					finally
					{
						bbeb.Close();
					}

					// Now write it all back out again
					string strTestLrfFile = Path.Combine(strWriteDir, finfo.Name);
					File.Delete(strTestLrfFile);
					FileStream newLrfStream = File.OpenWrite(strTestLrfFile);
					try
					{
						serializer.Serialize(newLrfStream, book);
					}
					finally
					{
						newLrfStream.Close();
					}
				}
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine("Error: " + ex.Message);
				Environment.ExitCode = 1;
			}
		}
	}
}
