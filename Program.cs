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
        const byte Water = 3;
        const byte Blood = 4;

        public static int TestCounter = 0;

        public static uint _SWidth = VideoMode.DesktopMode.Width;
        public static uint _SHeight = VideoMode.DesktopMode.Height;

        const uint _ResMult = 15;
        public static uint _Width = 160;//_SWidth / _ResMult;
        public static uint _Height = 90;//_SHeight / _ResMult;

        public static uint mouseMult = _SWidth / _Width;

        public static Texture MainViewPort = new Texture(_Width, _Height);

        public static byte[] pixels = new byte[_Width * _Height * 4];
        public static Color[] cpixels = new Color[_Width * _Height];

        public static Pixel[,] field = new Pixel[_Width, _Height];
        public static Pixel[,] resField = new Pixel[_Width, _Height];

        public static Vector2i prevPos = new Vector2i(0, 0);

        public static bool Placing = false;
        public static bool DrawCircle = false;
        public static byte mP = Sand;
        public static bool drawDir = true;
        public static bool voiding = false;
        public static bool ClearGenerators = false;
        public static ulong CurrentTick = 0;
        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(new SFML.Window.VideoMode(_Width, _Height), "Pixels");
            window.SetVerticalSyncEnabled(false);

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
            //field[_Width / 2, 10] = new Pixel(Sand);
            Vector2i mousePosition;
            Pixel.DrawCircle(150, 50, 5);
            while (window.IsOpen)
            {
                Update();

                window.Clear();
                window.DispatchEvents();

                Sprite mainviewport = new Sprite(MainViewPort);
                window.Draw(mainviewport);
                window.Display();
                mousePosition = Mouse.GetPosition(window);
                int mouseMultInt = Convert.ToInt32(mouseMult);

                Placing = Mouse.IsButtonPressed(Mouse.Button.Left);


                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                    window.Close();
                if (Keyboard.IsKeyPressed(Keyboard.Key.V))
                    voiding = !voiding;
                if (Keyboard.IsKeyPressed(Keyboard.Key.Num1))
                    mP = Sand;
                if (Keyboard.IsKeyPressed(Keyboard.Key.Num2))
                    mP = Water;
                if (Keyboard.IsKeyPressed(Keyboard.Key.Num3))
                    mP = Blood;
                if (Keyboard.IsKeyPressed(Keyboard.Key.F5))
                    for (int y = 0; y < _Height; y++)
                        for (int x = 0; x < _Width; x++)
                            if (field[x, y].Material == Generator)
                                field[x, y] = new Pixel(Air);
                DrawCircle = Mouse.IsButtonPressed(Mouse.Button.Middle);






                if (!Mouse.IsButtonPressed(Mouse.Button.Right))
                    field[prevPos.X, prevPos.Y] = new Pixel(Air);

                if (Mouse.GetPosition().X / mouseMult < window.Size.X & Mouse.GetPosition().Y / mouseMult < window.Size.Y)
                    if (mousePosition.X >= 0 & mousePosition.Y >= 0)
                    {
                        if (!Keyboard.IsKeyPressed(Keyboard.Key.LShift))
                        {
                            prevPos = new Vector2i(mousePosition.X / mouseMultInt, mousePosition.Y / mouseMultInt);
                            field[mousePosition.X / mouseMultInt, mousePosition.Y / mouseMultInt] = new Pixel(Generator);
                        }
                        else
                        {
                            prevPos = new Vector2i(mousePosition.X / mouseMultInt, 10 / mouseMultInt);
                            field[mousePosition.X / mouseMultInt, 10 / mouseMultInt] = new Pixel(Generator);
                        }
                    }

                if (window.HasFocus())
                {
                    Draw();
                    //Drawing function
                }
                CurrentTick++;
            }
        }

        static void Draw()
        {
            if (drawDir)
            {
                for (uint y = _Height - 1; y > 0; y--)
                {
                    for (uint x = 0; x < _Width; x++)
                    {
                        if (!field[x, y].UpdatedThisFrame)
                            field[x, y].Update(x, y);

                    }
                }
                drawDir = !drawDir;
            }
            else
            {
                for (uint y = _Height - 1; y > 0; y--)
                {
                    for (uint x = _Width - 1; x > 0; x--)
                    {
                        if (!field[x, y].UpdatedThisFrame)
                            field[x, y].Update(x, y);

                    }
                }
                drawDir = !drawDir;
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

                    uint i = 4 * (x + (_Width * y));
                    cpixels[x + (_Width * y)] = field[x, y].color;
                    field[x, y].UpdatedThisFrame = false;
                    //i *= 4;
                    pixels[i + 0] = cpixels[i / 4].R;
                    pixels[i + 1] = cpixels[i / 4].G;
                    pixels[i + 2] = cpixels[i / 4].B;
                    pixels[i + 3] = cpixels[i / 4].A;

                }
            }

            MainViewPort.Update(pixels);
        }
    }
}
