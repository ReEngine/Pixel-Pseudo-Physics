using SFML.Graphics;
using SFML.System;
using SimplexNoise;
using System;

namespace SFMLTryout
{
    internal class Pixel
    {
        private const bool dLeft = true;
        private const bool dRight = false;
        private const byte Generator = 0;
        private const byte Air = 1;
        private const byte Sand = 2;
        private const byte Water = 3;
        private const byte Dirt = 4;
        public Color color;
        public byte Material;
        public bool UpdatedThisFrame = false;
        private bool direction;
        private byte moisture = 0;
        private Color OriginColor;

        //Physics stuff
        private double Pressure = 0;
        private readonly float MaterialMaxCompress = 2;
        // Noisy stuff

        //
        public Vector2i position;
        public Pixel(byte material)
        {
            Material = material;
            if (material == Generator)
            {
                color = Color.Magenta;
            }
            if (material == Air)
            {
                color = Color.Transparent;
                //color = new Color(Convert.ToByte(Math.Min(Math.Clamp(Program._uy, 0, Program._Height), 255)), Convert.ToByte(Math.Min(Math.Clamp(Program._uy, Program._Height / 2, Program._Height), 255)), 255);
            }

            if (material == Sand)
            {
                //this.color = Color.Yellow;
                Random rnd = new Random();
                byte _RG = Convert.ToByte(rnd.Next(200, 255));
                color = new Color(_RG, _RG, Convert.ToByte(rnd.Next(100, _RG)));
                OriginColor = color;

            }
            if (Material == Water)
            {
                Random rnd = new Random();
                byte _RG = Convert.ToByte(rnd.Next(0, 100));
                color = new Color(_RG, _RG, Convert.ToByte(rnd.Next(115, 255)));
                Pressure = 1;
                OriginColor = color;
            }
            if (Material == Dirt)
            {
                uint x = Program._ux;
                uint y = Program._uy;
                int xInt = Convert.ToInt32(x + Program.xOffset);
                int yInt = Convert.ToInt32(y - Program.yOffset);
                byte _RGB = Convert.ToByte(Math.Abs(128 - Noise.CalcPixel2D(xInt, yInt, 0.2f)));
                if (Convert.ToByte(Math.Abs(128 - Noise.CalcPixel2D(xInt, yInt, 0.1f))) > 100)
                {
                    _RGB += Convert.ToByte(Math.Abs(128 - Noise.CalcPixel2D(xInt, yInt, 0.1f)));
                }

                if (Convert.ToByte(Math.Abs(128 - Noise.CalcPixel2D(xInt, yInt, 0.01f))) > 100)
                {
                    _RGB += Convert.ToByte(Math.Abs(128 - Noise.CalcPixel2D(xInt, yInt, 0.01f)));
                }

                byte _R = Convert.ToByte((_RGB + 60) / 2);
                byte _G = Convert.ToByte((_RGB + 35) / 2);
                byte _B = Convert.ToByte((_RGB + 3) / 2);
                color = new Color(_R, _G, _B);
                if (Up(Air, x, y))
                {
                    _RGB = Convert.ToByte(Math.Abs(128 - Noise.CalcPixel2D(xInt, yInt, 0.2f)));
                    color = new Color(0, _RGB, 0);
                }
                if (Up(Water, x, y))
                {
                    Program.field[x, y] = new Pixel(Sand);
                }
                if (Up().Up(Water, x, y))
                {
                    Program.field[x, y] = new Pixel(Sand);
                }
            }
        }
        public void Update(uint x, uint y)
        {
            position = new Vector2i(Convert.ToInt32(x), Convert.ToInt32(y));
            Random rnd = new Random();
            Pixel[] neighbors = new Pixel[] { Up(), UpRight(), UpLeft(), Left(), Right(), DownLeft(), DownRight(), Down() };

            if (Material == Sand)
            {
                byte max = 0;
                moisture = 0;
                foreach (Pixel neighbor in neighbors)
                {
                    if (neighbor.Material == Water)
                    {
                        moisture = max = 10;
                    }

                    if (neighbor.Material == Sand)
                    {
                        if (neighbor.moisture - 1 > max)
                        {
                            moisture = Convert.ToByte(neighbor.moisture - 1);
                        }
                    }
                }

                color.A = Convert.ToByte(255 - moisture * 5);
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
                                {
                                    Program.field[x, y].color = new Color(255, 255, 255);
                                }
                            }
                        }
                        else
                        {
                            if (x + 1 < Program._Width)
                            {
                                Program.field[x, y].MoveRight(x, y);
                                if (Program.field[x, y].Material == Water)
                                {
                                    Program.field[x, y].color = new Color(255, 255, 255);
                                }
                            }
                        }
                    }

                    else if ((DownLeft(Air, x, y) | (DownLeft(Water, x, y))) & (DownRight(Air, x, y) | (DownRight(Water, x, y))) & !UpdatedThisFrame)
                    {
                        if (rnd.Next(0, 2) == 0)
                        {
                            MoveDownLeft(x, y);
                            if (Program.field[x, y].Material == Water)
                            {
                                Program.field[x, y].color = new Color(255, 255, 255);
                            }
                        }
                        else
                        {
                            MoveDownRight(x, y);
                            if (Program.field[x, y].Material == Water)
                            {
                                Program.field[x, y].color = new Color(255, 255, 255);
                            }
                        }
                    }
                    else if ((DownLeft(Air, x, y) | (DownLeft(Water, x, y))) & !UpdatedThisFrame)
                    {
                        MoveDownLeft(x, y);
                        if (Program.field[x, y].Material == Water)
                        {
                            Program.field[x, y].color = new Color(255, 255, 255);
                        }
                    }
                    else if ((DownRight(Air, x, y) | (DownRight(Water, x, y))) & !UpdatedThisFrame)
                    {
                        MoveDownRight(x, y);
                        if (Program.field[x, y].Material == Water)
                        {
                            Program.field[x, y].color = new Color(255, 255, 255);
                        }
                    }
                }

            }
            else if (Material == Water)
            {
                foreach (Pixel neighbor in neighbors)
                {
                    if (neighbor.Material == Water)
                    {
                        byte _B = Convert.ToByte(((255 - Pressure * 50) + neighbor.color.B) / 2);
                        byte _RG = Convert.ToByte((neighbor.color.R + color.R) / 2);
                        color = new Color(_RG, _RG, _B);
                        _RG = Convert.ToByte((255 - Math.Abs(Noise.CalcPixel3D(Convert.ToInt32(x), Convert.ToInt32(y), Convert.ToInt32(Program.CurrentTick), 0.05f) - 128) - Pressure * 50 + _RG) / 10);
                        if (color.B != OriginColor.B)
                        {
                            _B = Convert.ToByte(color.B - ((color.B - OriginColor.B) / (Math.Abs(color.B - OriginColor.B))));
                        }

                        color = new Color(_RG, _RG, _B);
                        //OriginColor = new Color(Convert.ToByte(rnd.Next(200, 255)), Convert.ToByte(rnd.Next(200, 255)), Convert.ToByte(rnd.Next(115, 255)));
                        //if (Up(Air, x, y))
                        //{
                        //    this.color = Color.White;
                        //}
                    }
                }


                UpdatedThisFrame = false;
                if (y + 1 < Program._Height)
                {
                    if (Down(Air, x, y) & !UpdatedThisFrame )
                    {
                        MoveDown(x, y);
                    }

                    else if (DownLeft(Air, x, y) & DownRight(Air, x, y) & !UpdatedThisFrame)
                    {
                        int rndVal = rnd.Next(0, 2);
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
            else if (Material == Dirt)
            {

            }
            if (Material == Generator)
            {
                if (y + 1 < Program._Height & Program.Placing)
                {
                    Program.field[x, y + 1] = new Pixel(Program.mP);
                }

                if (y - 1 > 0 & Program.voiding)
                {
                    Program.field[x, y - 1] = new Pixel(Air);
                    if (x != 0)
                    {
                        Program.field[x - 1, y - 1] = new Pixel(Air);
                    }

                    if (x + 1 < Program._Width)
                    {
                        Program.field[x + 1, y - 1] = new Pixel(Air);
                    }
                }
            }

            UpdatedThisFrame = true;
        }


        private void FluidPhysics(Pixel[] neighbors)
        {
            foreach (Pixel neighbor in neighbors)
            {
                if (Up() == neighbor)
                {
                    //if (Pressure < MaterialMaxCompress)
                    Pressure = neighbor.Pressure + 0.05;

                    Pressure = Math.Clamp(Pressure, 1, MaterialMaxCompress);
                }
            }
        }


        public Pixel Up()
        {
            if (position.Y - 1 > 0)
            {
                return (Program.field[position.X, position.Y - 1]);
            }
            else
            {
                return new Pixel(Air);
            }
        }
        public Pixel Down()
        {
            if (position.Y + 1 < Program._Height)
            {
                return (Program.field[position.X, position.Y + 1]);
            }
            else
            {
                return new Pixel(Air);
            }
        }
        public Pixel Left()
        {
            if (position.X != 0)
            {
                return (Program.field[position.X - 1, position.Y]);
            }
            else
            {
                return new Pixel(Air);
            }
        }
        public Pixel Right()
        {
            if (position.X + 1 < Program._Width)
            {
                return (Program.field[position.X + 1, position.Y]);
            }
            else
            {
                return new Pixel(Air);
            }
        }
        public Pixel UpLeft()
        {
            if (position.X != 0 & position.Y - 1 > 0)
            {
                return (Program.field[position.X - 1, position.Y - 1]);
            }
            else
            {
                return new Pixel(Air);
            }
        }
        public Pixel UpRight()
        {
            if (position.X + 1 < Program._Width & position.Y - 1 > 0)
            {
                return (Program.field[position.X + 1, position.Y - 1]);
            }
            else
            {
                return new Pixel(Air);
            }
        }
        public Pixel DownLeft()
        {
            if (position.X != 0 & position.Y + 1 < Program._Height)
            {
                return (Program.field[position.X - 1, position.Y + 1]);
            }
            else
            {
                return new Pixel(Air);
            }
        }
        public Pixel DownRight()
        {
            if (position.X + 1 < Program._Width & position.Y + 1 < Program._Height)
            {
                return (Program.field[position.X + 1, position.Y + 1]);
            }
            else
            {
                return new Pixel(Air);
            }
        }

        public bool Up(byte material, uint x, uint y)
        {
            if (y != 0)
            {
                return (Program.field[x, y - 1].Material == material);
            }
            else
            {
                return false;
            }
        }
        public bool Down(byte material, uint x, uint y)
        {
            if (y + 1 < Program._Height)
            {
                return (Program.field[x, y + 1].Material == material);
            }
            else
            {
                return false;
            }
        }
        public bool Left(byte material, uint x, uint y)
        {
            if (x != 0)
            {
                return (Program.field[x - 1, y].Material == material);
            }
            else
            {
                return false;
            }
        }
        public bool Right(byte material, uint x, uint y)
        {
            if (x + 1 < Program._Width)
            {
                return (Program.field[x + 1, y].Material == material);
            }
            else
            {
                return false;
            }
        }
        public bool DownLeft(byte material, uint x, uint y)
        {
            if (x != 0 & y + 1 < Program._Height)
            {
                return (Program.field[x - 1, y + 1].Material == material);
            }
            else
            {
                return false;
            }
        }
        public bool DownRight(byte material, uint x, uint y)
        {
            if (x + 1 < Program._Width & y + 1 < Program._Height)
            {
                return Program.field[x + 1, y + 1].Material == material;
            }
            else
            {
                return false;
            }
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
            else
            {
                return false;
            }
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
