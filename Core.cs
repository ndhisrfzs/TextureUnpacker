using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml;

namespace TextureUnpacker
{
    public class Core 
    {
        private static XmlDocument doc;
        private static Bitmap bitmap;
        private static string savePath;

        public static void LoadXml(string path)
        {
            doc = new XmlDocument();
            doc.Load(path);
        }

        public static void LoadPng(string path)
        {
            savePath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
            bitmap = new Bitmap(path);

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
        }

        public static void Cuts()
        {
            // 获取根节点
            XmlNode root = doc.SelectSingleNode("TextureAtlas");

            XmlNodeList nodes = root.ChildNodes;

            foreach (XmlNode node in nodes)
            {
                XmlElement xe = node as XmlElement;

                string name = xe.GetAttribute("name");
                int x = Convert.ToInt32(xe.GetAttribute("x"));
                int y = Convert.ToInt32(xe.GetAttribute("y"));

                int width = Convert.ToInt32(xe.GetAttribute("width"));
                int height = Convert.ToInt32(xe.GetAttribute("height"));

                int frameX = 0, frameY = 0, frameWidth = 0, frameHeight = 0;
                bool hasFrame = false;
                if(xe.HasAttribute("frameWidth"))
                {
                    hasFrame = true;
                    frameWidth = Convert.ToInt32(xe.GetAttribute("frameWidth"));
                }
                if(xe.HasAttribute("frameHeight"))
                {
                    frameHeight = Convert.ToInt32(xe.GetAttribute("frameHeight"));
                }
                if(xe.HasAttribute("frameX"))
                {
                    frameX = Convert.ToInt32(xe.GetAttribute("frameX"));
                }
                if(xe.HasAttribute("frameY"))
                {
                    frameY = Convert.ToInt32(xe.GetAttribute("frameY"));
                }

                Cut(name, x, y, width, height, hasFrame, frameX, frameY, frameWidth, frameHeight);
            }
        }

        private static void Cut(string name, int x, int y, int width, int height, bool hasFrame, int frameX, int frameY, int frameWidth, int frameHeight)
        {
            int desWidth = width, desHeight = height;
            int offsetX = 0, offsetY = 0;
            if (hasFrame)
            {
                desWidth = frameWidth;
                desHeight = frameHeight;

                offsetX = -frameX;// + frameWidth / 2 - width / 2;
                offsetY = -frameY;// + (int)frameHeight / 2 - height / 2;
            }

            Bitmap bmp = new Bitmap(desWidth, desHeight);

            for (int w = 0; w < desWidth;  w++)
            {
                for(int h = 0; h < desHeight; h++)
                {
                    if(w >= offsetX && w < width + offsetX && h >= offsetY && h < height + offsetY)
                    {
                        bmp.SetPixel(w, h, bitmap.GetPixel(x + w - offsetX, y + h - offsetY));
                    }
                    else
                    {
                        bmp.SetPixel(w, h, Color.Transparent);
                    }
                }
            }

            bmp.Save(Path.Combine(savePath, name + ".png"), ImageFormat.Png);
        }
    }
}
