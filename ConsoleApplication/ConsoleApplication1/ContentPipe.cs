using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK.Graphics.OpenGL;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace ConsoleApplication1
{
    class ContentPipe
    {
            
        public static Textura2D LoadTexture(string filePath, bool pixelated = false)
        {
            filePath = "Content/" + filePath;
            if (!File.Exists(filePath))
            {
                throw new Exception("Arquivo '" + filePath + "' não existe" );
            }

            Bitmap bmp = new Bitmap(filePath);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), 
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp.Width, bmp.Height, 0, 
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bmp.UnlockBits(data);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 
                pixelated ? (int)TextureMinFilter.Nearest : (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                pixelated ? (int)TextureMagFilter.Nearest : (int)TextureMagFilter.Linear);

            return new Textura2D(id, new Vector2(bmp.Width, bmp.Height));

        }
    }
}
