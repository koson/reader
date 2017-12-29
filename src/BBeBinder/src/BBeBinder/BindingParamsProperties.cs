using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using BBeBLib;


namespace BBeBinder
{
	[DefaultPropertyAttribute("Icon")]
	internal class BindingParamsProperties
	{
		BindingParams m_Params;

		public BindingParamsProperties(BindingParams bindingParams)
		{
			m_Params = bindingParams;
		}

		[DescriptionAttribute("The image file to use as the book icon. (60 x 80)"),
		CategoryAttribute("Book Settings")]
		public string IconFile
		{
			get { return m_Params.IconFile; }
			set { m_Params.IconFile = value; }
		}


		[DescriptionAttribute("The title of this book"),
			CategoryAttribute("Book Information")]
		public string Title
		{
			get { return m_Params.MetaData.BookInfo.Title; }
			set { m_Params.MetaData.BookInfo.Title = value; }
		}

		[DescriptionAttribute("The author(s) of the content"),
			CategoryAttribute("Book Information")]
		public string Author
		{
			get { return m_Params.MetaData.BookInfo.Author; }
			set { m_Params.MetaData.BookInfo.Author = value; }
		}

		[DescriptionAttribute("The ID information of the content. This ID is used to distinguish “Personal content” and the “Commercial content”. BookID for the Personal content shall be specified by the character “FB” and the following characters of 14 digits or less (16 digits or less in total). BookID for the Commercial content may be specified in Interchange Parties."),
			CategoryAttribute("Book Information")]
		public string BookID
		{
			get { return m_Params.MetaData.BookInfo.BookID; }
			set { m_Params.MetaData.BookInfo.BookID = value; }
		}

		[DescriptionAttribute("The publisher name of the content"),
			CategoryAttribute("Book Information")]
		public string Publisher
		{
			get { return m_Params.MetaData.BookInfo.Publisher; }
			set { m_Params.MetaData.BookInfo.Publisher = value; }
		}

		[DescriptionAttribute("The label of this book"),
			CategoryAttribute("Book Information")]
		public string Label
		{
			get { return m_Params.MetaData.BookInfo.Label; }
			set { m_Params.MetaData.BookInfo.Label = value; }
		}

		[DescriptionAttribute("The genre of the content."),
			CategoryAttribute("Book Information")]
		public string Category
		{
			get { return m_Params.MetaData.BookInfo.Category; }
			set { m_Params.MetaData.BookInfo.Category = value; }
		}

		[DescriptionAttribute("Information on what kind of data is included in the content. (e.g. sound, color image)."),
			CategoryAttribute("Book Information")]
		public string Classification
		{
			get { return m_Params.MetaData.BookInfo.Classification; }
			set { m_Params.MetaData.BookInfo.Classification = value; }
		}

		[DescriptionAttribute("A free description about the content. (e.g. content summary)."),
			CategoryAttribute("Book Information")]
		public string FreeText
		{
			get { return m_Params.MetaData.BookInfo.FreeText; }
			set { m_Params.MetaData.BookInfo.FreeText = value; }
		}

		[DescriptionAttribute("The main language used in the content. It should use “ISO 639 language codes”, for example Japanese: ”ja”."),
			CategoryAttribute("Document Information")]
		public string Language
		{
			get { return m_Params.MetaData.DocInfo.Language; }
			set { m_Params.MetaData.DocInfo.Language = value; }
		}

		[DescriptionAttribute("The creator or studio name of the content."),
			CategoryAttribute("Document Information")]
		public string Creator
		{
			get { return m_Params.MetaData.DocInfo.Creator; }
			set { m_Params.MetaData.DocInfo.Creator = value; }
		}

		[DescriptionAttribute("The date this BBeB file was created"),
			CategoryAttribute("Document Information")]
		public string CreationDate
		{
			get { return m_Params.MetaData.DocInfo.CreationDate; }
			set { m_Params.MetaData.DocInfo.CreationDate = value; }
		}

		[DescriptionAttribute("The producer of this book"),
			CategoryAttribute("Document Information")]
		public string Producer
		{
			get { return m_Params.MetaData.DocInfo.Producer; }
			set { m_Params.MetaData.DocInfo.Producer = value; }
		}
	}
}
