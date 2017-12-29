using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.IO;

namespace BBeBLib.Serializer
{
    public abstract class BBeBWriter
    {
        protected struct ImageInfo
        {
            public string name;
            public string type;
            public Bitmap image;
            public Rectangle position;
        }

        protected BBeB book;

        // The Data
        protected StringBuilder data = new StringBuilder();
        public string Data
        {
            get { return data.ToString(); }
        }

        List<ImageInfo> images = new List<ImageInfo>();

        public bool HasImages
        {
            get { return images.Count > 0; }
        }


        public BBeBWriter(BBeB book)
        {
            this.book = book;

            writePages();
        }

        public void save(string path, string filename)
        {
            string dataName = Path.Combine(path, filename);
            string nameOfFile = filename.Substring(0, filename.IndexOf("."));
            
            // Relace all instances of @@IMAGENAME@@ with name of the file
            data.Replace("@@IMAGENAME@@", nameOfFile);

            // Save data file
            StreamWriter sw = new StreamWriter( dataName );
            sw.Write( Data );
            sw.Close();

            // Now save images
            foreach (ImageInfo info in images)
            {
                string imageName = Path.Combine(path, info.name.Replace( "@@IMAGENAME@@", nameOfFile ));
                ImageFormat type;
                switch( info.type )
                {
                    case "jpg":
                        type = ImageFormat.Jpeg;
                        imageName += ".jpg";
                        break;
                    case "png":
                        type = ImageFormat.Png;
                        imageName += ".png";
                        break;
                    case "bmp":
                        type = ImageFormat.Bmp;
                        imageName += ".png";
                        break;
                    case "gif":
                        type = ImageFormat.Gif;
                        imageName += ".gif";
                        break;
                    default:
                        type = ImageFormat.Png;
                        imageName += ".png";
                        break;
                }
                info.image.Save(imageName, type);
            }
        }

        private void writePages()
        {
            // Get the page tree
            PageTreeObject pageTree = null;
            foreach ( BBeBObject obj in book.Objects )
            {
                if (obj.GetType() == typeof(PageTreeObject))
                {
                    pageTree = (PageTreeObject)obj;
                    break;
                }
            }
            // If no page tree - nothing to do
            if ( pageTree == null )
                return;

            start();
            foreach( PageObject page in pageTree.Pages )
            {
                foreach( BBeBObject obj in page.Children )
                {
                    startPage();
                    if ( obj.GetType() == typeof(TextObject) )
                    {
                        write((TextObject)obj);
                    }
                    if (obj.GetType() == typeof(ImageObject))
                    {
                        write((ImageObject)obj);
                    }
                    endPage();
                }
            }
            finished();
        }

        private void write( TextObject obj )
        {
			BBeBTag prevTag = null;

			foreach (BBeBTag tag in obj.TextTags)
			{
				if (prevTag != null && !(prevTag.Id == TagId.EndPage && tag.Id == TagId.EOL))
				{
					handleTag(tag);
				}

				prevTag = tag;
			}
        }

        private void write(ImageObject obj)
        {
            // Get image details
            UInt16ArrayTag rect = (UInt16ArrayTag)obj.FindFirstTag(TagId.ImageRect);
            UInt16ArrayTag size = (UInt16ArrayTag)obj.FindFirstTag(TagId.ImageSize);
            UInt32Tag streamId = (UInt32Tag)obj.FindFirstTag(TagId.ImageStream);
            int x = rect.Value[0];
            int y = rect.Value[1];
            int w = rect.Value[2];
            int h = rect.Value[3];

            ushort imageId = (ushort)streamId.Value;
            ImageStreamObject imageObj = (ImageStreamObject)book.FindObject(imageId);

            string type = ImageObject.getImageTypeAsString(imageObj.Contents);

            MemoryStream memStream = new MemoryStream(imageObj.Data);
            Bitmap image = new Bitmap(memStream);

            ImageInfo info = new ImageInfo();
            info.image = image;
            info.position = new Rectangle(x, y, w, h);

            // @@IMAGENAME@@ will be replaced during save with the name of the file
            info.name = "@@IMAGENAME@@_" + (images.Count + 1);
            info.type = type;
            images.Add(info);

            handleImage(info);
        }

        // Override these methods in derived classes to handle certain events
        protected virtual void start()
        {
        }

        protected virtual void startPage()
        {
        }

        protected virtual void handleTag(BBeBTag tag)
        {
        }

        protected virtual void handleImage(ImageInfo info)
        {
        }

        protected virtual void endPage()
        {
        }

        protected virtual void finished()
        {
        }
    }
}

