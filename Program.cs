using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace SFMLTryout
{
    class Program
    {

        public static int TestCounter = 0;

        //const int _Width = 640;
        //const int _Height = 480;

        const int _SWidth = 1920;
        const int _SHeight = 1080;

        const int _ResMult = 1;


        public const int _Width = 192 * _ResMult;
        public const int _Height = 108 * _ResMult;
        Random rnd = new Random();

        public static Vector2i position;
        public static Color[,] ScreenBuffer = new Color[_Width, _Height];
        public static Texture MainViewPort = new Texture(_Width, _Height);

        public static byte[] pixels = new byte[_Width * _Height * 4]; //_Width x _Height pixels x 4 bytes per pixel
        public static Color[] cpixels = new Color[_Width * _Height];//intermediary step to keep everything clear

        public static byte[,] fireField = new byte[_Width, _Height];
        public static Pixel[,] field = new Pixel[_Width, _Height];

        public static int prevPosX;
        public static int prevPosY;

        public static bool placing = false;
        public static bool voiding = false;
        public static bool fire = false;

        public static double mult = 0;

        public static Material mP = Material.Sand;
        public static string mPS;




        static void Main(string[] args)
        {
            //Text text = new Text();

            mult = _SWidth / _Width;
            RenderWindow window = new RenderWindow(new SFML.Window.VideoMode(_Width, _Height), "Test");
            window.SetVerticalSyncEnabled(true);

            for (int y = 0; y < _Height; y++)
            {
                for (int x = 0; x < _Width; x++)
                {
                    field[x, y] = new Pixel(Material.Air, x, y);
                    fireField[x, y] = 0;
                }
            }
            window.SetMouseCursorVisible(false);
            prevPosX = 0;
            prevPosY = 0;

            //Font font = new Font(@"C:\Users\ameli\source\repos\Pixel-Pseudo-Physics\Resources\Fonts\Montserrat-Light.ttf");
            while (window.IsOpen)
            {
                Update();

                window.Clear();
                window.DispatchEvents();

                //text = new Text(mPS, font);
                //text.FillColor = Color.White;
                //text.CharacterSize = 8;

                Sprite mainviewport = new Sprite(MainViewPort);
                window.Draw(mainviewport);
                //window.Draw(text);
                window.Display();


                position = new Vector2i(Mouse.GetPosition(window).X / Convert.ToInt32(mult), Mouse.GetPosition(window).Y / Convert.ToInt32(mult));



                if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
                    placing = !placing;
                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                    placing = true;
                else
                    placing = false;

                //if (Mouse.IsButtonPressed(Mouse.Button.Right))
                //ScreenBuffer[Mouse.GetPosition(window).X / Convert.ToInt32(mult),
                //Mouse.GetPosition(window).Y / Convert.ToInt32(mult) + 1] = Color.Magenta;



                if (Keyboard.IsKeyPressed(Keyboard.Key.V))
                    voiding = !voiding;
                if (Keyboard.IsKeyPressed(Keyboard.Key.F))
                    fire = !fire;
                if (Keyboard.IsKeyPressed(Keyboard.Key.Num1))
                    mP = Material.Sand;
                if (Keyboard.IsKeyPressed(Keyboard.Key.Num2))
                    mP = Material.Water;
                if (Keyboard.IsKeyPressed(Keyboard.Key.Num3))
                    mP = Material.Gas;
                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                    window.Close();
                if (Keyboard.IsKeyPressed(Keyboard.Key.F5))
                    for (int y = 0; y < _Height; y++)
                    {
                        for (int x = 0; x < _Width; x++)
                        {
                            field[x, y] = new Pixel(Material.Air, x, y);
                            fireField[x, y] = 0;
                        }
                    }
                field[prevPosX, prevPosY] = new Pixel(Material.Air, prevPosX, prevPosY);


                if (Mouse.GetPosition().X / mult < window.Size.X & Mouse.GetPosition().Y / mult < window.Size.Y)
                    if (Mouse.GetPosition(window).X > 0 & Mouse.GetPosition(window).Y > 0)
                    {
                        prevPosY = Mouse.GetPosition(window).Y / Convert.ToInt32(mult);
                        prevPosX = Mouse.GetPosition(window).X / Convert.ToInt32(mult);
                        field[Mouse.GetPosition(window).X / Convert.ToInt32(mult), Mouse.GetPosition(window).Y / Convert.ToInt32(mult)] = new Pixel(Material.Generator, Mouse.GetPosition(window).X / Convert.ToInt32(mult), Mouse.GetPosition(window).Y / Convert.ToInt32(mult));
                    }



                if (mP == Material.Sand)
                    mPS = "Sand ";
                if (mP == Material.Water)
                    mPS = "Water";
                if (mP == Material.Gas)
                    mPS = "Gas  ";

                if (Mouse.GetPosition().X / mult < window.Size.X & Mouse.GetPosition().Y / mult < window.Size.Y)
                    if (Mouse.GetPosition(window).X > 0 & Mouse.GetPosition(window).Y > 0)
                        Draw();

            }
        }


        static void Draw()
        {
            for (int y = _Height - 1; y > 0; y--)
                for (int x = 0; x < _Width; x++)
                    field[x, y].Update();
        }


        static void Update()
        {
            TestCounter++;
            if (TestCounter > _Width - 1) { TestCounter = 0; }

            for (int x = 0; x < _Width; x++)
            {
                for (int y = 0; y < _Height; y++)
                {
                    cpixels[x + (_Width * y)] = field[x, y].color;//make an intermediary array the correct dimention and arrange the pixels in the correct position to be drawn (separate step to keep everything clean, I find this operation incredibly confusing mainly because I had no idea how the pixels are supposed to be arrenged in the first place(still kind of dont))
                }
            }
            for (int i = 0; i < _Width * _Height * 4; i += 4)//fill the byte array
            {

                pixels[i + 0] = cpixels[i / 4].R;
                pixels[i + 1] = cpixels[i / 4].G;
                pixels[i + 2] = cpixels[i / 4].B;
                pixels[i + 3] = cpixels[i / 4].A;
            }

            MainViewPort.Update(pixels);//update the texture with the array
        }
    }
}
