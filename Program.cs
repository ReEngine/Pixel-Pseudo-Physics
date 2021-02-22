using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SimplexNoise;


namespace SFMLTryout
{
    class Program
    {
        const byte Generator = 0;
        const byte Air = 1;
        const byte Sand = 2;
        const byte Water = 3;
        const byte Dirt = 4;

        public static int TestCounter = 0;

        public static uint _SWidth = VideoMode.DesktopMode.Width;
        public static uint _SHeight = VideoMode.DesktopMode.Height;

        const uint _ResMult = 15;
        public static uint _Width = _SWidth / _ResMult;
        public static uint _Height = _SHeight / _ResMult;

        public static uint mouseMult = _SWidth / _Width;

        public static Texture MainViewPort = new Texture(_Width, _Height);

        public static byte[] pixels = new byte[_Width * _Height * 4];
        public static Color[] cpixels = new Color[_Width * _Height];

        public static Pixel[,] field = new Pixel[_Width, _Height];
        public static Pixel[,] resField = new Pixel[_Width, _Height];


        public static uint _ux;
        public static uint _uy;
        public static Vector2i prevPos = new Vector2i(0, 0);

        public static bool Placing = false;
        public static bool DrawCircle = false;
        public static byte mP = Sand;
        public static bool drawDir = true;
        public static bool voiding = false;
        public static bool ClearGenerators = false;
        public static ulong CurrentTick = 0;
        public static int xOffset = 0;
        public static int yOffset = 0;
        //public static float sOffset = 0;

        public static long worldX;
        public static long worldY;

        public static int xOffsetAdd = 0;

        public static Vector2i fieldMP;

        static void Main(string[] args)
        {
            Noise.Seed = new Random().Next();
            RenderWindow window = new RenderWindow(new SFML.Window.VideoMode(_Width, _Height), "Pixels");
            window.SetVerticalSyncEnabled(false);

            ReDraw();
            window.SetMouseCursorVisible(false);
            Vector2i mousePosition = Mouse.GetPosition();
            int mouseMultInt = Convert.ToInt32(mouseMult);
            fieldMP = new Vector2i(mousePosition.X / mouseMultInt, mousePosition.Y / mouseMultInt);
            while (window.IsOpen)
            {
                if (window.HasFocus())
                {
                    Update();

                    window.Clear(Color.Blue);
                    window.DispatchEvents();

                    Sprite mainviewport = new Sprite(MainViewPort);
                    window.Draw(mainviewport);
                    window.Display();

                    mousePosition = Mouse.GetPosition(window);


                    Placing = Mouse.IsButtonPressed(Mouse.Button.Left);
                    if (Keyboard.IsKeyPressed(Keyboard.Key.W))
                    {
                        yOffset++;
                        ReDraw();
                    }
                    if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                    {
                        yOffset--;
                        ReDraw();
                    }
                    if (Keyboard.IsKeyPressed(Keyboard.Key.A))
                    {
                        if (Keyboard.IsKeyPressed(Keyboard.Key.LShift))
                            xOffset -= 10;
                        xOffset--;
                        ReDraw();
                    }

                    if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                    {
                        if (Keyboard.IsKeyPressed(Keyboard.Key.LShift))
                            xOffset += 10;
                        xOffset++;
                        ReDraw();
                    }
                    
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                        window.Close();
                    if (Keyboard.IsKeyPressed(Keyboard.Key.V))
                        voiding = !voiding;
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Num1))
                        mP = Sand;
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Num2))
                        mP = Water;
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Num3))
                        mP = Dirt;
                    if (Keyboard.IsKeyPressed(Keyboard.Key.F6))
                    {
                        ReDraw();
                    }
                    if (Keyboard.IsKeyPressed(Keyboard.Key.F5))
                        for (int y = 0; y < _Height; y++)
                            for (int x = 0; x < _Width; x++)
                                if (field[x, y].Material == Generator)
                                    field[x, y] = new Pixel(Air);
                    DrawCircle = Mouse.IsButtonPressed(Mouse.Button.Middle);

                    xOffset += xOffsetAdd;



                    fieldMP = new Vector2i(mousePosition.X / mouseMultInt, mousePosition.Y / mouseMultInt);
                    if (!Mouse.IsButtonPressed(Mouse.Button.Right))
                        field[prevPos.X, prevPos.Y] = new Pixel(Air);

                    if (Mouse.GetPosition().X / mouseMult < window.Size.X & Mouse.GetPosition().Y / mouseMult < window.Size.Y)
                        if (mousePosition.X >= 0 & mousePosition.Y >= 0)
                        {
                            prevPos = new Vector2i(mousePosition.X / mouseMultInt, mousePosition.Y / mouseMultInt);
                            field[mousePosition.X / mouseMultInt, mousePosition.Y / mouseMultInt] = new Pixel(Generator);

                        }


                    Draw();

                    CurrentTick++;
                }
            }
        }
        static void ReDraw()
        {
            for (uint y = 0; y < _Height; y++)
            {
                for (uint x = 0; x < _Width; x++)
                {

                    field[x, y] = new Pixel(Air);
                    //_2DNoise[x, y] = 0;
                }
            }
            yOffset = ((int)(Noise.CalcPixel1D((int)Convert.ToDouble(_Width / 2 + xOffset), 0.00099999852f) - _Height / 2));
            for (int x = 0; x < _Width; x++)
            {

                for (int y = yOffset; y < Noise.CalcPixel1D(x + xOffset, 0.00099999852f) & y < _Height + yOffset; y++)
                {
                    if (field[x, _Height - 1 - y + yOffset].Material == Air)
                        field[x, _Height - 1 - y + yOffset] = new Pixel(Dirt);
                    //_2DNoise[x, y] = 0;
                }
            }

            for (int y = (yOffset - 55); y < _Height; y++)
            {
                if (y >= 0)

                    for (int x = 0; x < _Width; x++)
                    {
                        if (field[x, y].Material == Air)
                            field[x, y] = new Pixel(Water);
                        //_2DNoise[x, y] = 0;
                    }
                //yOffset = y;
            }
            for (uint x = 0; x < _Width; x++)
            {
                for (uint y = 0; y < _Height; y++)
                {
                    _ux = x;
                    _uy = y;
                    if (field[x, y].Material == Dirt)
                    {
                        field[x, y] = new Pixel(Dirt);
                    }
                }
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
