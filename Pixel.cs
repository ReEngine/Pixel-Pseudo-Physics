using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLTryout
{
    class Pixel
    {
        const bool dLeft = true;
        const bool dRight = false;
        const byte Generator = 0;
        const byte Air = 1;
        const byte Sand = 2;
        const byte Water = 3;
        const byte Blood = 4;
        public Color color;
        public byte Material;
        public bool UpdatedThisFrame = false;
        bool direction;
        byte moisture = 0;
        Color OriginColor;
        //Physics stuff

        double Pressure = 0;
        float MaterialMaxCompress = 2;
        //
        public Vector2i position;
        public Pixel(byte material)
        {
            this.Material = material;
            if (material == Generator)
                this.color = Color.Magenta;
            if (material == Air)
                this.color = Color.Black;
            if (material == Sand)
            {
                //this.color = Color.Yellow;
                Random rnd = new Random();
                byte _RG = Convert.ToByte(rnd.Next(200, 255));
                this.color = new Color(_RG, _RG, Convert.ToByte(rnd.Next(100, _RG)));
                this.OriginColor = color;

            }
            if (Material == Water)
            {
                Random rnd = new Random();
                byte _RG = Convert.ToByte(rnd.Next(0, 100));
                this.color = new Color(_RG, _RG, Convert.ToByte(rnd.Next(115, 255)));
                this.Pressure = 1;
                this.OriginColor = color;
            }
            if (Material == Blood)
            {
                this.color = new Color(138, 3, 3);
                //this.Material = Water;
            }
        }
        public void Update(uint x, uint y)
        {
            this.position = new Vector2i(Convert.ToInt32(x), Convert.ToInt32(y));
            Random rnd = new Random();
            Pixel[] neighbors = new Pixel[] { Up(), UpRight(), UpLeft(), Left(), Right(), DownLeft(), DownRight(), Down() };
            if (this.Material == Sand)
            {
                byte max = 0;
                moisture = 0;
                foreach (Pixel neighbor in neighbors)
                {
                    if (neighbor.Material == Water)
                        moisture = max = 10;

                    if (neighbor.Material == Sand)
                    {
                        if (neighbor.moisture - 1 > max)
                            moisture = Convert.ToByte(neighbor.moisture - 1);
                    }
                }

                this.color.A = Convert.ToByte(255 - moisture * 5);
                if (y + 1 < Program._Height)
                {
                    if ((Down(Air, x, y) | (Down(Water, x, y))) & !UpdatedThisFrame)
                    {
                        MoveDown(x, y);
                        if (rnd.Next(0, 2) == 0)
                        {
                            if (x != 0)
                            {
                                Program.field[x, y].MoveLeft(x, y);
                                if (Program.field[x, y].Material == Water)
                                    Program.field[x, y].color = new Color(255, 255, 255);
                            }
                        }
                        else
                        {
                            if (x + 1 < Program._Width)
                            {
                                Program.field[x, y].MoveRight(x, y);
                                if (Program.field[x, y].Material == Water)
                                    Program.field[x, y].color = new Color(255, 255, 255);
                            }
                        }
                    }

                    else if ((DownLeft(Air, x, y) | (DownLeft(Water, x, y))) & (DownRight(Air, x, y) | (DownRight(Water, x, y))) & !UpdatedThisFrame)
                    {
                        if (rnd.Next(0, 2) == 0)
                        {
                            MoveDownLeft(x, y);
                            if (Program.field[x, y].Material == Water)
                                Program.field[x, y].color = new Color(255, 255, 255);
                        }
                        else
                        {
                            MoveDownRight(x, y);
                            if (Program.field[x, y].Material == Water)
                                Program.field[x, y].color = new Color(255, 255, 255);
                        }
                    }
                    else if ((DownLeft(Air, x, y) | (DownLeft(Water, x, y))) & !UpdatedThisFrame)
                    {
                        MoveDownLeft(x, y);
                        if (Program.field[x, y].Material == Water)
                            Program.field[x, y].color = new Color(255, 255, 255);
                    }
                    else if ((DownRight(Air, x, y) | (DownRight(Water, x, y))) & !UpdatedThisFrame)
                    {
                        MoveDownRight(x, y);
                        if (Program.field[x, y].Material == Water)
                            Program.field[x, y].color = new Color(255, 255, 255);
                    }
                }

            }
            else if (this.Material == Water)
            {
                foreach (Pixel neighbor in neighbors)
                {
                    if (neighbor.Material == Water)
                    {
                        byte _B = Convert.ToByte(((255 - Pressure * 50) + neighbor.color.B) / 2);
                        Byte _RG = Convert.ToByte((neighbor.color.R + color.R) / 2);
                        this.color = new Color(_RG, _RG, _B);
                        this.color = new Color(_RG, _RG, _B);
                        //if (Up(Air, x, y))
                        //{
                        //    this.color = Color.White;
                        //}
                    }
                }


                UpdatedThisFrame = false;
                if (y + 1 < Program._Height)
                {
                    if (Down(Air, x, y) & !UpdatedThisFrame)
                    {
                        MoveDown(x, y);
                    }

                    else if (DownLeft(Air, x, y) & DownRight(Air, x, y) & !UpdatedThisFrame)
                    {
                        var rndVal = rnd.Next(0, 2);
                        //Console.WriteLine(rndVal);
                        if (rndVal == 0)
                        {
                            MoveDownLeft(x, y);
                            direction = dLeft;
                        }
                        else
                        {
                            MoveDownRight(x, y);
                            direction = dRight;
                        }
                    }
                    //else if (DownLeft(Air, x, y) & !UpdatedThisFrame)
                    //{
                    //    MoveDownLeft(x, y);
                    //    direction = dLeft;
                    //}
                    //else if (DownRight(Air, x, y) & !UpdatedThisFrame)
                    //{
                    //    MoveDownRight(x, y);
                    //    direction = dRight;
                    //}



                    //else if (Left(Air, x, y) & Right(Air, x, y) & !UpdatedThisFrame)
                    //{
                    //    if (rnd.Next(0, 2) == 0)
                    //    {
                    //        MoveLeft(x, y);
                    //    }
                    //    else
                    //    {
                    //        MoveRight(x, y);
                    //    }
                    //}
                    else if (Right(Air, x, y) & !UpdatedThisFrame & direction == dRight)
                    {
                        MoveRight(x, y);
                    }
                    else if (Left(Air, x, y) & !UpdatedThisFrame & direction == dLeft)
                    {
                        MoveLeft(x, y);
                    }
                    else if (!Right(Air, x, y))
                    {
                        direction = !dRight;
                    }
                    else if (!Left(Air, x, y))
                    {
                        direction = !dLeft;
                    }
                }
                FluidPhysics(neighbors);
                //this.color.B = Convert.ToByte(255 - NewPressure);
            }
            if (this.Material == Generator)
            {
                if (Program.DrawCircle)
                {
                    DrawCircle(Convert.ToInt32(x), Convert.ToInt32(y), 5);
                }
                if (y + 1 < Program._Height & Program.Placing)
                    Program.field[x, y + 1] = new Pixel(Program.mP);
                if (y - 1 > 0 & Program.voiding)
                {
                    Program.field[x, y - 1] = new Pixel(Air);
                    if (x != 0)
                        Program.field[x - 1, y - 1] = new Pixel(Air);
                    if (x + 1 < Program._Width)
                        Program.field[x + 1, y - 1] = new Pixel(Air);
                }
            }

            UpdatedThisFrame = true;
        }
        public static void DrawLine(int x0, int x1, int y0, int y1)
        {
            uint deltax = Convert.ToUInt32(Math.Abs(x1 - x0));
            uint deltay = Convert.ToUInt32(Math.Abs(y1 - y0));
            double error = 0;
            float deltaerr = (deltay + 1) / (deltax + 1);
            int y = y0;
            int diry = y1 - y0;
            if (diry > 0)
                diry = 1;
            if (diry < 0)
                diry = -1;
            for (int x = x0; x < x1; x++)
            {
                if (x < Program._Width & y < Program._Height)
                    Program.field[x, y] = new Pixel(Program.mP);
                error += deltaerr;
                if (error >= 1.0)
                    y = y + diry;
                error -= 1.0;
            }


        }
        public static void DrawCircle(int X0, int Y0, int R)
        {

            int f = 1 - R;
            int ddF_x = 1;
            int ddF_y = -2 * R;
            int x = 0;
            int y = R;

            DrawLine(X0 - R, Y0, X0 + R, Y0);
            DrawLine(X0, Y0 - R, X0, Y0 + R);

            while (x < y)
            {
                if (f >= 0)
                {
                    y--;
                    ddF_y += 2;
                    f += ddF_y;
                }
                x++;
                ddF_x += 2;
                f += ddF_x;

                DrawLine(X0 + x, Y0 + y, X0 - x, Y0 + y);
                DrawLine(X0 + x, Y0 - y, X0 - x, Y0 - y);
                DrawLine(X0 + y, Y0 + x, X0 - y, Y0 + x);
                DrawLine(X0 + y, Y0 - x, X0 - y, Y0 - x);

            }
        }
        void FluidPhysics(Pixel[] neighbors)
        {
            foreach (Pixel neighbor in neighbors)
            {
                if (Up() == neighbor)
                {
                    //if (Pressure < MaterialMaxCompress)
                    this.Pressure = neighbor.Pressure + 0.05;

                    Pressure = Math.Clamp(Pressure, 1, MaterialMaxCompress);
                }
            }
        }


        public Pixel Up()
        {
            if (position.Y - 1 > 0)
                return (Program.field[position.X, position.Y - 1]);
            else return new Pixel(Air);
        }
        public Pixel Down()
        {
            if (position.Y + 1 < Program._Height)
                return (Program.field[position.X, position.Y + 1]);
            else return new Pixel(Air);
        }
        public Pixel Left()
        {
            if (position.X != 0)
                return (Program.field[position.X - 1, position.Y]);
            else return new Pixel(Air);
        }
        public Pixel Right()
        {
            if (position.X + 1 < Program._Width)
                return (Program.field[position.X + 1, position.Y]);
            else return new Pixel(Air);
        }
        public Pixel UpLeft()
        {
            if (position.X != 0 & position.Y - 1 > 0)
                return (Program.field[position.X - 1, position.Y - 1]);
            else return new Pixel(Air);
        }
        public Pixel UpRight()
        {
            if (position.X + 1 < Program._Width & position.Y - 1 > 0)
                return (Program.field[position.X + 1, position.Y - 1]);
            else return new Pixel(Air);
        }
        public Pixel DownLeft()
        {
            if (position.X != 0 & position.Y + 1 < Program._Height)
                return (Program.field[position.X - 1, position.Y + 1]);
            else return new Pixel(Air);
        }
        public Pixel DownRight()
        {
            if (position.X + 1 < Program._Width & position.Y + 1 < Program._Height)
                return (Program.field[position.X + 1, position.Y + 1]);
            else return new Pixel(Air);
        }

        public bool Up(byte material, uint x, uint y)
        {
            return (Program.field[x, y - 1].Material == material & y - 1 > 0);
        }
        public bool Down(byte material, uint x, uint y)
        {
            if (y + 1 < Program._Height)
                return (Program.field[x, y + 1].Material == material);
            else return false;
        }
        public bool Left(byte material, uint x, uint y)
        {
            if (x != 0)
                return (Program.field[x - 1, y].Material == material);
            else return false;
        }
        public bool Right(byte material, uint x, uint y)
        {
            if (x + 1 < Program._Width)
                return (Program.field[x + 1, y].Material == material);
            else return false;
        }
        public bool DownLeft(byte material, uint x, uint y)
        {
            if (x != 0 & y + 1 < Program._Height)
                return (Program.field[x - 1, y + 1].Material == material);
            else return false;
        }
        public bool DownRight(byte material, uint x, uint y)
        {
            if (x + 1 < Program._Width & y + 1 < Program._Height)
            {
                return Program.field[x + 1, y + 1].Material == material;
            }
            else return false;
        }
        public bool UpLeft(byte material, uint x, uint y)
        {
            return (Program.field[x - 1, y - 1].Material == material & x != 0 & y - 1 > 0);
        }
        public bool UpRight(byte material, uint x, uint y)
        {
            if (x + 1 < Program._Width & y - 1 > 0)
            {
                return Program.field[x + 1, y - 1].Material == material;
            }
            else return false;
        }


        public void MoveDown(uint x, uint y)
        {
            Pixel tmp = Program.field[x, y + 1];
            Program.field[x, y + 1] = this;
            Program.field[x, y] = tmp;
            UpdatedThisFrame = true;

        }
        public void MoveLeft(uint x, uint y)
        {
            Pixel tmp = Program.field[x - 1, y];
            Program.field[x - 1, y] = this;
            Program.field[x, y] = tmp;
            UpdatedThisFrame = true;
        }
        public void MoveRight(uint x, uint y)
        {
            Pixel tmp = Program.field[x + 1, y];
            Program.field[x + 1, y] = this;
            Program.field[x, y] = tmp;
            UpdatedThisFrame = true;
        }
        public void MoveDownLeft(uint x, uint y)
        {
            Pixel tmp = Program.field[x - 1, y + 1];
            Program.field[x - 1, y + 1] = this;
            Program.field[x, y] = tmp;
            UpdatedThisFrame = true;
        }
        public void MoveDownRight(uint x, uint y)
        {
            Pixel tmp = Program.field[x + 1, y + 1];
            Program.field[x + 1, y + 1] = this;
            Program.field[x, y] = tmp;
            UpdatedThisFrame = true;
        }



    }
}
