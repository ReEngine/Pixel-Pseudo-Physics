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


        const int _Width = 640;
        const int _Height = 360;
        Random rnd = new Random();

        public static Vector2i position;
        public static Color[,] ScreenBuffer = new Color[_Width, _Height];
        public static Texture MainViewPort = new Texture(_Width, _Height);

        public static byte[] pixels = new byte[_Width * _Height * 4]; //_Width x _Height pixels x 4 bytes per pixel
        public static Color[] cpixels = new Color[_Width * _Height];//intermediary step to keep everything clear

        public static byte[,] fireField = new byte[_Width, _Height];

        public static int prevPosX;
        public static int prevPosY;

        public static bool placing = false;
        public static bool voiding = false;
        public static bool fire = false;

        public static double mult = 0;

        public static Color mP;
        //public static string mPS;

        static void Main(string[] args)
        {

            mult = _SWidth / _Width;
            RenderWindow window = new RenderWindow(new SFML.Window.VideoMode(_Width, _Height), "Test");
            window.SetVerticalSyncEnabled(false);

            for (int y = 0; y < _Height; y++)
            {
                for (int x = 0; x < _Width; x++)
                {
                    ScreenBuffer[x, y] = Color.Black;
                    fireField[x, y] = 0;
                }
            }
            window.SetMouseCursorVisible(false);
            prevPosX = 0;
            prevPosY = 0;

            

            while (window.IsOpen)
            {
                Update();

                window.Clear();
                window.DispatchEvents();

                Sprite mainviewport = new Sprite(MainViewPort);
                window.Draw(mainviewport);

                window.Display();

                position = new Vector2i(Mouse.GetPosition(window).X / Convert.ToInt32(mult), Mouse.GetPosition(window).Y / Convert.ToInt32(mult));



                if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
                    placing = !placing;
                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                    placing = true;
                else
                    placing = false;

                if (Mouse.IsButtonPressed(Mouse.Button.Right))
                    ScreenBuffer[Mouse.GetPosition(window).X / Convert.ToInt32(mult), Mouse.GetPosition(window).Y / Convert.ToInt32(mult)+1] = Color.Magenta;



                if (Keyboard.IsKeyPressed(Keyboard.Key.V))
                    voiding = !voiding;
                if (Keyboard.IsKeyPressed(Keyboard.Key.F))
                    fire = !fire;
                if (Keyboard.IsKeyPressed(Keyboard.Key.Num1))
                    mP = Color.Yellow;
                if (Keyboard.IsKeyPressed(Keyboard.Key.Num2))
                    mP = Color.Blue;
                if (Keyboard.IsKeyPressed(Keyboard.Key.Num3))
                    mP = Color.Green;
                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                    window.Close();
                ScreenBuffer[prevPosX, prevPosY] = Color.Black;


                if (Mouse.GetPosition().X / mult < window.Size.X & Mouse.GetPosition().Y / mult < window.Size.Y)
                    if (Mouse.GetPosition(window).X > 0 & Mouse.GetPosition(window).Y > 0)
                    {
                        prevPosY = Mouse.GetPosition(window).Y / Convert.ToInt32(mult);
                        prevPosX = Mouse.GetPosition(window).X / Convert.ToInt32(mult);
                        ScreenBuffer[Mouse.GetPosition(window).X / Convert.ToInt32(mult), Mouse.GetPosition(window).Y / Convert.ToInt32(mult)] = Color.Magenta;
                    }



                //if (mP == Color.Yellow)
                //    mPS = "Sand ";
                //if (mP == Color.Blue)
                //    mPS = "Water";
                //if (mP == Color.Green)
                //    mPS = "Gas  ";

                if (Mouse.GetPosition().X / mult < window.Size.X & Mouse.GetPosition().Y / mult < window.Size.Y)
                    if (Mouse.GetPosition(window).X > 0 & Mouse.GetPosition(window).Y > 0)
                        Draw();

            }
        }


        static void Draw()
        {
            for (int y = _Height - 1; y > position.Y; y--)
            {
                drawLine(y);
            }
            for (int y = 0; y <= position.Y;y++)
            {
                drawLine(y);
            }
            Update();
        }

        static void drawLine(int y)
        {
            for (int x = 0; x < _Width; x++)
            {
                if (ScreenBuffer[x, y] == Color.Magenta & placing)
                    ScreenBuffer[x, y + 1] = mP;
                if ((ScreenBuffer[x, y] == Color.Yellow) & (y + 1 < _Height) & (x + 1 < _Width) & (x - 1 >= 0))
                {
                    if ((ScreenBuffer[x, y + 1] == Color.Black) & (y + 1 < _Height))
                    {
                        ScreenBuffer[x, y + 1] = Color.Yellow;
                        ScreenBuffer[x, y] = Color.Black;
                    }
                    else if ((ScreenBuffer[x - 1, y + 1] == Color.Black) & (y + 1 < _Height))
                    {
                        ScreenBuffer[x - 1, y + 1] = Color.Yellow;
                        ScreenBuffer[x, y] = Color.Black;
                    }
                    else if ((ScreenBuffer[x + 1, y + 1] == Color.Black) & (y + 1 < _Height))
                    {
                        ScreenBuffer[x + 1, y + 1] = Color.Yellow;
                        ScreenBuffer[x, y] = Color.Black;
                    }
                    else if ((ScreenBuffer[x, y + 1] == Color.Blue) & (y + 1 < _Height))
                    {
                        ScreenBuffer[x, y + 1] = Color.Yellow;
                        ScreenBuffer[x, y] = Color.Blue;
                    }
                    else if ((ScreenBuffer[x - 1, y + 1] == Color.Blue) & (y + 1 < _Height))
                    {
                        ScreenBuffer[x - 1, y + 1] = Color.Yellow;
                        ScreenBuffer[x, y] = Color.Blue;
                    }
                    else if ((ScreenBuffer[x + 1, y + 1] == Color.Blue) & (y + 1 < _Height))
                    {
                        ScreenBuffer[x + 1, y + 1] = Color.Yellow;
                        ScreenBuffer[x, y] = Color.Blue;
                    }
                }
                if ((ScreenBuffer[x, y] == Color.Blue) & (y + 1 < _Height) & (x + 1 < _Width) & (x - 1 >= 0))
                {
                    if ((ScreenBuffer[x, y + 1] == Color.Black) & (y + 1 < _Height))
                    {
                        ScreenBuffer[x, y + 1] = Color.Blue;
                        ScreenBuffer[x, y] = Color.Black;
                    }
                    else if ((ScreenBuffer[x - 1, y + 1] == Color.Black))
                    {
                        ScreenBuffer[x - 1, y + 1] = Color.Blue;
                        ScreenBuffer[x, y] = Color.Black;
                    }
                    else if ((ScreenBuffer[x + 1, y + 1] == Color.Black))
                    {
                        ScreenBuffer[x + 1, y + 1] = Color.Blue;
                        ScreenBuffer[x, y] = Color.Black;
                    }
                    else if ((ScreenBuffer[x + 1, y] == Color.Black))
                    {
                        ScreenBuffer[x + 1, y] = Color.Blue;
                        ScreenBuffer[x, y] = Color.Black;
                    }
                    else if ((ScreenBuffer[x - 1, y] == Color.Black))
                    {
                        ScreenBuffer[x - 1, y] = Color.Blue;
                        ScreenBuffer[x, y] = Color.Black;
                    }
                }
                if ((ScreenBuffer[x, y] == Color.Green) & (y - 1 < _Height) & (x + 1 < _Width) & (x - 1 >= 0)& (y - 1 >= 0))
                {
                    if ((ScreenBuffer[x, y - 1] == Color.Black) & (y - 1 < _Height))
                    {
                        ScreenBuffer[x, y - 1] = Color.Green;
                        ScreenBuffer[x, y] = Color.Black;
                    }
                    else if ((ScreenBuffer[x - 1, y - 1] == Color.Black))
                    {
                        ScreenBuffer[x - 1, y - 1] = Color.Green;
                        ScreenBuffer[x, y] = Color.Black;
                    }
                    else if ((ScreenBuffer[x + 1, y - 1] == Color.Black))
                    {
                        ScreenBuffer[x + 1, y - 1] = Color.Green;
                        ScreenBuffer[x, y] = Color.Black;
                    }
                    else if ((ScreenBuffer[x + 1, y] == Color.Black))
                    {
                        ScreenBuffer[x + 1, y] = Color.Green;
                        ScreenBuffer[x, y] = Color.Black;
                    }
                    else if ((ScreenBuffer[x - 1, y] == Color.Black))
                    {
                        ScreenBuffer[x - 1, y] = Color.Green;
                        ScreenBuffer[x, y] = Color.Black;
                    }
                    if (ScreenBuffer[x, y] == Color.Green & ((ScreenBuffer[x - 1, y - 1] == Color.Red) | (ScreenBuffer[x, y - 1] == Color.Red) | (ScreenBuffer[x + 1, y - 1] == Color.Red) |
                                              (ScreenBuffer[x - 1, y] == Color.Red) | (ScreenBuffer[x + 1, y] == Color.Red) |
                                              (ScreenBuffer[x - 1, y + 1] == Color.Red) | (ScreenBuffer[x, y + 1] == Color.Red) | (ScreenBuffer[x + 1, y + 1] == Color.Red)))
                    {
                        ScreenBuffer[x, y] = Color.Red;
                        fireField[x, y] = 100;
                    }
                    else if (ScreenBuffer[x, y] == Color.Green & (fire) & ((ScreenBuffer[x - 1, y - 1] == Color.Magenta) | (ScreenBuffer[x, y - 1] == Color.Magenta) | (ScreenBuffer[x + 1, y - 1] == Color.Magenta) |
                                              (ScreenBuffer[x - 1, y] == Color.Magenta) | (ScreenBuffer[x + 1, y] == Color.Magenta) |
                                              (ScreenBuffer[x - 1, y + 1] == Color.Magenta) | (ScreenBuffer[x, y + 1] == Color.Magenta) | (ScreenBuffer[x + 1, y + 1] == Color.Magenta)))
                    {
                        ScreenBuffer[x, y] = Color.Red;
                        fireField[x, y] = 100;
                    }
                }




                if (y + 1 < _Height)
                    if ((ScreenBuffer[x, y + 1] == Color.Magenta) & voiding)
                    {
                        ScreenBuffer[x, y] = Color.Black;
                    }
                if (ScreenBuffer[x, y] == Color.Red)
                {
                    if (fireField[x, y] > 0)
                        fireField[x, y]--;
                    if (fireField[x, y] == 0)
                        if (ScreenBuffer[x, y] == Color.Red)
                            ScreenBuffer[x, y] = Color.Black;
                }
            }
        }

        static void Update()
        {
            TestCounter++;
            if (TestCounter > _Width - 1) { TestCounter = 0; }
            //RESET THE BUFFER (COULD BE REMOVED LATER I GUESS)
            //for (int x = 0; x < _Width; x++)
            //{
            //    for (int y = 0; y < _Height; y++)
            //    {
            //        ScreenBuffer[x, y] = Color.Black;
            //    }
            //}

            //DO STUFF

            //WRITING THE BUFFER INTO THE IMAGE
            //THIS SHOULD ALWAYS BE THE LAST STEP OF THE UPDATE METHOD

            for (int x = 0; x < _Width; x++)
            {
                for (int y = 0; y < _Height; y++)
                {
                    cpixels[x + (_Width * y)] = ScreenBuffer[x, y];//make an intermediary array the correct dimention and arrange the pixels in the correct position to be drawn (separate step to keep everything clean, I find this operation incredibly confusing mainly because I had no idea how the pixels are supposed to be arrenged in the first place(still kind of dont))
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
