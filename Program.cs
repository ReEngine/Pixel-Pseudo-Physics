using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SimplexNoise;
using System;


namespace SFMLTryout
{
    internal class Program
    {
        private const byte Generator = 0;
        private const byte Air = 1;
        private const byte Sand = 2;
        private const byte Water = 3;
        private const byte Dirt = 4;

        static int TestCounter = 0;

        static readonly uint _SWidth = VideoMode.DesktopMode.Width;
        static readonly uint _SHeight = VideoMode.DesktopMode.Height;

        static readonly FastNoiseLite noise = new FastNoiseLite(new Random().Next());
        static readonly FastNoiseLite BGNoise = new FastNoiseLite(new Random().Next());


        const uint _ResMult = 15;
        public static uint _Width = _SWidth / _ResMult;
        public static uint _Height = _SHeight / _ResMult;

        static readonly uint mouseMult = _SWidth / _Width;

        static readonly Texture MainViewPort = new Texture(_Width, _Height);

        static readonly byte[] pixels = new byte[_Width * _Height * 4];
        static readonly Color[] cpixels = new Color[_Width * _Height];

        public static Pixel[,] field = new Pixel[_Width, _Height];
        public static Color[,] overlay = new Color[_Width, _Height];
        public static Color[,] background = new Color[_Width, _Height];


        public static uint _ux;
        public static uint _uy;
        static Vector2i prevPos = new Vector2i(0, 0);

        public static bool Placing = false;
        public static byte mP = Sand;
        static string mPString = "Sand";
        static bool drawDir = true;
        public static bool voiding = false;
        public static ulong CurrentTick = 0;
        public static int xOffset = 0;
        public static int yOffset = 0;


        static Vector2i fieldMP;

        private static void Main()
        {
            Noise.Seed = new Random().Next();
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
            noise.SetFrequency(0.0002f);
            noise.SetFractalLacunarity(1f);
            noise.SetFractalGain(2.1f);
            noise.SetFractalType(FastNoiseLite.FractalType.FBm);
            noise.SetFractalOctaves(5);
            BGNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
            BGNoise.SetFrequency(0.01f);
            BGNoise.SetFractalLacunarity(1f);
            BGNoise.SetFractalGain(2.1f);
            BGNoise.SetFractalType(FastNoiseLite.FractalType.FBm);
            BGNoise.SetFractalOctaves(5);
            RenderWindow window = new RenderWindow(new SFML.Window.VideoMode(_SWidth, _SHeight), "Pixels");
            window.SetVerticalSyncEnabled(false);
            Font font = new Font(Resources.DotGothic16_Regular);
            Texture ItemFrame = new Texture(Resources.pixil_frame_0);
            Texture SandTexture = new Texture(Resources.pixil_frame_0__3_);
            Texture WaterTexture = new Texture(Resources.pixil_frame_0__4_);
            Sprite ItemFrameSprite = new Sprite(ItemFrame);
            Sprite mPSprite = new Sprite(SandTexture);
            window.SetMouseCursorVisible(false);
            Vector2i mousePosition = Mouse.GetPosition();
            int mouseMultInt = Convert.ToInt32(mouseMult);
            fieldMP = new Vector2i(mousePosition.X / mouseMultInt, mousePosition.Y / mouseMultInt);
            ReDraw();
            while (window.IsOpen)
            {
                if (window.HasFocus())
                {
                    Update();

                    window.Clear(Color.Black);
                    window.DispatchEvents();

                    Sprite mainviewport = new Sprite(MainViewPort)
                    {
                        Scale = new Vector2f(_ResMult, _ResMult)
                    };
                    ItemFrameSprite.Scale = new Vector2f(_ResMult, _ResMult);
                    mPSprite.Scale = new Vector2f(_ResMult, _ResMult);

                    Text text = new Text(mPString, font, _SWidth / 20);
                    text.Position = new Vector2f(_SWidth / 100, _SHeight - text.GetLocalBounds().Height - _SHeight / 20);
                    window.Draw(mainviewport);
                    window.Draw(text);
                    window.Draw(ItemFrameSprite);
                    window.Draw(mPSprite);
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
                        {
                            xOffset -= 10;
                        }

                        xOffset--;
                        ReDraw();
                    }

                    if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                    {
                        if (Keyboard.IsKeyPressed(Keyboard.Key.LShift))
                        {
                            xOffset += 10;
                        }

                        xOffset++;
                        ReDraw();
                    }

                    if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                    {
                        window.Close();
                    }

                    if (Keyboard.IsKeyPressed(Keyboard.Key.V))
                    {
                        voiding = !voiding;
                    }

                    if (Keyboard.IsKeyPressed(Keyboard.Key.Num1))
                    {
                        mP = Sand;
                    }

                    if (Keyboard.IsKeyPressed(Keyboard.Key.Num2))
                    {
                        mP = Water;
                    }

                    if (Keyboard.IsKeyPressed(Keyboard.Key.Num3))
                    {
                        mP = Dirt;
                    }

                    if (Keyboard.IsKeyPressed(Keyboard.Key.F6))
                    {
                        ReDraw();
                    }

                    if (Keyboard.IsKeyPressed(Keyboard.Key.F5))
                    {
                        for (int y = 0; y < _Height; y++)
                        {
                            for (int x = 0; x < _Width; x++)
                            {
                                if (field[x, y].Material == Generator)
                                {
                                    field[x, y] = new Pixel(Air);
                                }
                            }
                        }
                    }


                    switch (mP)
                    {
                        case 0:
                            mPString = "Generator";
                            break;
                        case 1:
                            mPString = "Air";
                            break;
                        case 2:
                            mPString = "Sand";
                            mPSprite = new Sprite(SandTexture);
                            break;
                        case 3:
                            mPString = "Water";
                            mPSprite = new Sprite(WaterTexture);
                            break;
                        case 4:
                            mPString = "Dirt";
                            break;
                    }


                    fieldMP = new Vector2i(mousePosition.X / mouseMultInt, mousePosition.Y / mouseMultInt);
                    overlay[prevPos.X, prevPos.Y] = Color.Transparent;
                    if (Keyboard.IsKeyPressed(Keyboard.Key.I))
                    {
                        Console.Write(
                            "Pixel info:\n" +
                            "Position: X = " + (field[fieldMP.X, fieldMP.Y].position.X + xOffset) + "\n" +
                            "          Y = " + (field[fieldMP.X, fieldMP.Y].position.Y + yOffset) + "\n" +
                            "Material = " + field[fieldMP.X, fieldMP.Y].Material + "\n" +
                            "Colors: R = " + field[fieldMP.X, fieldMP.Y].color.R +
                            "\n        G = " + field[fieldMP.X, fieldMP.Y].color.G +
                            "\n        B = " + field[fieldMP.X, fieldMP.Y].color.B +
                            "\n        A = " + field[fieldMP.X, fieldMP.Y].color.A);
                    }

                    if (Mouse.GetPosition().X / mouseMult < window.Size.X & Mouse.GetPosition().Y / mouseMult < window.Size.Y)
                    {
                        if (mousePosition.X >= 0 & mousePosition.Y >= 0)
                        {
                            prevPos = fieldMP;
                            overlay[fieldMP.X, fieldMP.Y] = Color.Magenta;
                            if (Placing)
                            {
                                field[fieldMP.X, fieldMP.Y] = new Pixel(mP);
                            }

                            if (Mouse.IsButtonPressed(Mouse.Button.Right))
                            {
                                field[fieldMP.X, fieldMP.Y] = new Pixel(Generator);
                            }
                        }
                    }
                    Draw();

                    CurrentTick++;
                }
            }
        }

        private static void ReDraw()
        {
            for (uint y = 0; y < _Height; y++)
            {
                for (uint x = 0; x < _Width; x++)
                {
                    overlay[x, y] = Color.Transparent;
                    field[x, y] = new Pixel(Air);
                    background[x, y] = new Color(Convert.ToByte(Math.Min(Math.Clamp(Program._uy, 0, Program._Height), 255)), Convert.ToByte(Math.Min(Math.Clamp(Program._uy, Program._Height / 2, Program._Height), 255)), 255);
                    //_2DNoise[x, y] = 0;
                }
            }
            yOffset = ((int)(noise.GetNoise(_Width / 2 + xOffset, 0) * 255 - _Height / 2)); //0.00099999852f
            for (int x = 0; x < _Width; x++)
            {
                for (int y = yOffset; y < noise.GetNoise(x + xOffset, 0) * 255 & y < _Height + yOffset; y++)
                {
                    if (field[x, _Height - 1 - y + yOffset].Material == Air)
                    {
                        field[x, _Height - 1 - y + yOffset] = new Pixel(Dirt);
                    }
                    //_2DNoise[x, y] = 0;
                }
            }
            for (int x = 0; x < _Width; x++)
            {
                for (int y = 1; y < BGNoise.GetNoise(x + xOffset / 10, 0) * 128 & y < _Height; y++)
                {


                    background[x, _Height - y] = Color.Green;


                    //_2DNoise[x, y] = 0;
                }
            }
            for (int y = (yOffset + 126); y < _Height; y++)
            {
                if (y >= 0)
                {
                    for (int x = 0; x < _Width; x++)
                    {
                        if (field[x, y].Material == Air)
                        {
                            field[x, y] = new Pixel(Water);
                        }
                        //_2DNoise[x, y] = 0;
                    }
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
        private static void Draw()
        {
            if (drawDir)
            {
                for (uint y = _Height - 1; y > 0; y--)
                {
                    for (uint x = 0; x < _Width; x++)
                    {

                        if (!field[x, y].UpdatedThisFrame)
                        {
                            field[x, y].Update(x, y);
                        }
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
                        {
                            field[x, y].Update(x, y);
                        }
                    }
                }
                drawDir = !drawDir;
            }
        }

        private static void Update()
        {
            TestCounter++;
            if (TestCounter > _Width - 1) { TestCounter = 0; }

            //if (drawDir)
            for (uint x = 0; x < _Width; x++)
            {
                for (uint y = 0; y < _Height; y++)
                {

                    uint i = 4 * (x + (_Width * y));
                    cpixels[x + (_Width * y)] = field[x, y].color + overlay[x, y];
                    if (cpixels[x + (_Width * y)] == Color.Transparent)
                        cpixels[x + (_Width * y)] = background[x, y];
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
