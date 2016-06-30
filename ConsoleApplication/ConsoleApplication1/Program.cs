using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Threading;
using OpenTK.Input;

namespace ConsoleApplication1
{
    class Program
    {
        //static float theta = 0.0f;
        static Textura2D textura;

        static Vector2[] vertBuffer;
        static int VBO;
       

        static void Main(string[] args)
        {
            var game = new GameWindow(1366, 768);
            game.Load += Game_Load;
            game.RenderFrame += Game_RenderFrame;
            game.Resize += Game_Resize;
            game.UpdateFrame += Game_UpdateFrame;
            game.WindowBorder = WindowBorder.Hidden;
            game.Title = "OPEN TK SHOW TIME";
            game.WindowState = WindowState.Fullscreen;
            game.Run(60.0);
        }

        static void Game_Load(object sender, EventArgs e)
        {
            textura = ContentPipe.LoadTexture("asus.png");

            vertBuffer = new Vector2[8]
            {
                new Vector2(0, 0), new Vector2(0, 0),
                new Vector2(1, 0), new Vector2(1, 0),
                new Vector2(1, 1), new Vector2(1, -1),
                new Vector2(0, 1), new Vector2(0, -1),
            };           

            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(Vector2.SizeInBytes * vertBuffer.Length),
                vertBuffer, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public static void Game_UpdateFrame(object sender, FrameEventArgs e)
        {
            var game = (GameWindow)sender;
            if (game.Keyboard[Key.Escape])
            {
                game.Exit();
            }
        }

        public static void Game_Resize(object sender, EventArgs e)
        { 
            var game = (GameWindow)sender;
            GL.Viewport(0, 0, game.Width, game.Height);
        }

        static float theta = 0.0f;
        

        public static void Game_RenderFrame(object sender, FrameEventArgs e)
        {
            GL.ClearColor(Color.White);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);

            GL.BindTexture(TextureTarget.Texture2D, textura.ID);

            GL.PushMatrix();
            GL.Rotate(theta,0.0f,1.0f,0.0f);

            GL.Begin(BeginMode.Quads);
            GL.Color3(Color.White);
            GL.TexCoord2(0, 1); GL.Vertex2(0, 0);
            GL.TexCoord2(1, 0); GL.Vertex2(1, 0);
            GL.TexCoord2(1, 1); GL.Vertex2(1, 1);
            GL.TexCoord2(0, 1); GL.Vertex2(0, 1); 
            GL.End();

            GL.DrawArrays(BeginMode.Quads, 0, vertBuffer.Length);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.VertexPointer(2, VertexPointerType.Float, Vector2.SizeInBytes * 2, 0);
            GL.TexCoordPointer(2, TexCoordPointerType.Float, Vector2.SizeInBytes * 2, Vector2.SizeInBytes);
            //GL.Color3(Color.AntiqueWhite);
            GL.DrawArrays(BeginMode.Quads, 0, vertBuffer.Length / 2);

            GL.Flush();
            GL.PopMatrix();
            var game = (GameWindow)sender;
            game.SwapBuffers();
            theta += 1.0f;
            Thread.Sleep(10);
        }
    }
}
