using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace SFMLTryout
{
    class Program
    {
        const byte Generator = 0;
        const byte Air = 1;
        const byte Sand = 2;


        public static int TestCounter = 0;

        public static uint _SWidth = VideoMode.DesktopMode.Width;
        public static uint _SHeight = VideoMode.DesktopMode.Height;

        const uint _ResMult = 10;

        public static uint _Width = _SWidth / _ResMult;
        public static uint _Height = _SHeight / _ResMult;

        public static Texture MainViewPort = new Texture(_Width, _Height);

        public static byte[] pixels = new byte[_Width * _Height * 4];
        public static Color[] cpixels = new Color[_Width * _Height];

        public static Pixel[,] field = new Pixel[_Width, _Height];

        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(new SFML.Window.VideoMode(_Width, _Height), "Pixels");
            window.SetVerticalSyncEnabled(true);

            for (int y = 0; y < _Height; y++)
            {
                for (int x = 0; x < _Width; x++)
                {
                    field[x, y] = new Pixel(Air);
                }
            }
            window.SetMouseCursorVisible(false);
            //for (int y = 0; y < _Height; y += 10)
            //{
            //    for (int x = 0; x < _Width; x += 10)
            //    {
            //        field[x, y].color = Color.Blue;
            //    }
            //}
            field[_Width/2, 10] = new Pixel(Sand);
            Vector2i mousePosition;
            while (window.IsOpen)
            {
                Update();

                window.Clear();
                window.DispatchEvents();

                Sprite mainviewport = new Sprite(MainViewPort);
                window.Draw(mainviewport);
                window.Display();
                mousePosition = Mouse.GetPosition(window);
                field[_Width/2, 10] = new Pixel(Sand);




                if (window.HasFocus())
                {
                    Draw();
                    //Drawing function
                }
            }
        }

        static void Draw()
        {
            for (uint y = _Height - 1; y > 0; y--)
            {
                for (uint x = 0; x < _Width; x++)
                {
                    field[x, y].Update(x, y);
                }
            }
        }


        static void Update()
        {
            TestCounter++;
            if (TestCounter > _Width - 1) { TestCounter = 0; }

            for (uint x = 0; x < _Width; x++)
            {
                for (uint y = 0; y < _Height; y++)
                {
                    cpixels[x + (_Width * y)] = field[x, y].color;
                }
            }
            for (int i = 0; i < _Width * _Height * 4; i += 4)
            {

                pixels[i + 0] = cpixels[i / 4].R;
                pixels[i + 1] = cpixels[i / 4].G;
                pixels[i + 2] = cpixels[i / 4].B;
                pixels[i + 3] = cpixels[i / 4].A;
            }

            MainViewPort.Update(pixels);
        }
    }
}
