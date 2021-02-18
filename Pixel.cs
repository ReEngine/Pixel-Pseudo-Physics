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
        public Color color;
        public byte Material;

        //physics stuff
        public float MaterialFlow;
        public float Pressure;



        //
        public bool UpdatedThisFrame = false;
        bool direction;


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

            }
            if (Material == Water)
            {
                Random rnd = new Random();
                byte _RG = Convert.ToByte(rnd.Next(0, 100));
                this.color = new Color(_RG, _RG, Convert.ToByte(rnd.Next(115, 255)));
                this.MaterialFlow = 1;
            }
        }
        public void Update(uint x, uint y)
        {
            Random rnd = new Random();
            if (this.Material == Sand)
            {
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
                if (Up(Water, x, y))
                {
                    byte _RG = Convert.ToByte((this.color.R + Up(x, y).color.R) / 2);
                    byte _B = Convert.ToByte((this.color.B + Up(x, y).color.B) / 2);
                    this.color = new Color(_RG, _RG, _B);
                }
                if (UpRight(Water, x, y))
                {
                    byte _RG = Convert.ToByte((this.color.R + UpRight(x, y).color.R) / 2);
                    byte _B = Convert.ToByte((this.color.B + UpRight(x, y).color.B) / 2);
                    this.color = new Color(_RG, _RG, _B);
                }
                if (UpLeft(Water, x, y))
                {
                    byte _RG = Convert.ToByte((this.color.R + UpLeft(x, y).color.R) / 2);
                    byte _B = Convert.ToByte((this.color.B + UpLeft(x, y).color.B) / 2);
                    this.color = new Color(_RG, _RG, _B);
                }
                if (Left(Water, x, y))
                {
                    byte _RG = Convert.ToByte((this.color.R + Left(x, y).color.R) / 2);
                    byte _B = Convert.ToByte((this.color.B + Left(x, y).color.B) / 2);
                    this.color = new Color(_RG, _RG, _B);
                }
                if (Right(Water, x, y))
                {
                    byte _RG = Convert.ToByte((this.color.R + Right(x, y).color.R) / 2);
                    byte _B = Convert.ToByte((this.color.B + Right(x, y).color.B) / 2);
                    this.color = new Color(_RG, _RG, _B);
                }
                if (Down(Water, x, y))
                {
                    byte _RG = Convert.ToByte((this.color.R + Down(x, y).color.R) / 2);
                    byte _B = Convert.ToByte((this.color.B + Down(x, y).color.B) / 2);
                    this.color = new Color(_RG, _RG, _B);
                }
                if (DownLeft(Water, x, y))
                {
                    byte _RG = Convert.ToByte((this.color.R + DownLeft(x, y).color.R) / 2);
                    byte _B = Convert.ToByte((this.color.B + DownLeft(x, y).color.B) / 2);
                    this.color = new Color(_RG, _RG, _B);
                }
                if (DownRight(Water, x, y))
                {
                    byte _RG = Convert.ToByte((this.color.R + DownRight(x, y).color.R) / 2);
                    byte _B = Convert.ToByte((this.color.B + DownRight(x, y).color.B) / 2);
                    this.color = new Color(_RG, _RG, _B);
                }


                UpdatedThisFrame = false;
                //if (y + 1 < Program._Height)
                //{
                //    if (Down(Air, x, y) & !UpdatedThisFrame)
                //    {
                //        MoveDown(x, y);
                //    }

                //    else if (DownLeft(Air, x, y) & DownRight(Air, x, y) & !UpdatedThisFrame)
                //    {
                //        if (rnd.Next(0, 2) == 0)
                //        {
                //            MoveDownLeft(x, y);
                //            direction = dLeft;
                //        }
                //        else
                //        {
                //            MoveDownRight(x, y);
                //            direction = dRight;
                //        }
                //    }
                //    else if (DownLeft(Air, x, y) & !UpdatedThisFrame)
                //    {
                //        MoveDownLeft(x, y);
                //        direction = dLeft;
                //    }
                //    else if (DownRight(Air, x, y) & !UpdatedThisFrame)
                //    {
                //        MoveDownRight(x, y);
                //        direction = dRight;
                //    }
                //    //else if (Left(Air, x, y) & Right(Air, x, y) & !UpdatedThisFrame)
                //    //{
                //    //    if (rnd.Next(0, 2) == 0)
                //    //    {
                //    //        MoveLeft(x, y);
                //    //    }
                //    //    else
                //    //    {
                //    //        MoveRight(x, y);
                //    //    }
                //    //}
                //    else if (Left(Air, x, y) & !UpdatedThisFrame & direction == dLeft)
                //    {
                //        MoveLeft(x, y);
                //    }
                //    else if (Right(Air, x, y) & !UpdatedThisFrame & direction == dRight)
                //    {
                //        MoveRight(x, y);
                //    }
                //    else if (!Right(Air, x, y))
                //    {
                //        direction = !dRight;
                //    }
                //    else if (!Left(Air, x, y))
                //    {
                //        direction = !dLeft;
                //    }
                //}
            }
            if (this.Material == Generator)
            {
                if (y + 1 < Program._Height & Program.Placing)
                    Program.field[x, y + 1] = new Pixel(Program.mP);
                if (y != 0 & Program.voiding)
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

        public Pixel Up(uint x, uint y)
        {
            if (y - 1 >= 0)
                return (Program.field[x, y - 1]);
            else return new Pixel(Air);
        }
        public Pixel Down(uint x, uint y)
        {
            if (y + 1 < Program._Height)
                return (Program.field[x, y + 1]);
            else return new Pixel(Air);
        }
        public Pixel Left(uint x, uint y)
        {
            if (x - 1 >= 0)
                return (Program.field[x - 1, y]);
            else return new Pixel(Air);
        }
        public Pixel Right(uint x, uint y)
        {
            if (x + 1 < Program._Width)
                return (Program.field[x + 1, y]);
            else return new Pixel(Air);
        }
        public Pixel UpLeft(uint x, uint y)
        {
            if (x - 1 >= 0 & y - 1 >= 0)
                return (Program.field[x - 1, y - 1]);
            else return new Pixel(Air);
        }
        public Pixel UpRight(uint x, uint y)
        {
            if (x + 1 < Program._Width & y - 1 >= 0)
                return (Program.field[x + 1, y - 1]);
            else return new Pixel(Air);
        }
        public Pixel DownLeft(uint x, uint y)
        {
            if (x - 1 >= 0 & y + 1 < Program._Height)
                return (Program.field[x - 1, y + 1]);
            else return new Pixel(Air);
        }
        public Pixel DownRight(uint x, uint y)
        {
            if (x + 1 < Program._Width & y + 1 < Program._Height)
                return (Program.field[x + 1, y + 1]);
            else return new Pixel(Air);
        }

        public bool Up(byte material, uint x, uint y)
        {
            return (Program.field[x, y - 1].Material == material & y - 1 >= 0);
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
            if (x != 0 & y - 1 >= 0)
            {
                return (Program.field[x - 1, y - 1].Material == material);
            }
            else return false;
        }
        public bool UpRight(byte material, uint x, uint y)
        {
            if (x + 1 < Program._Width & y - 1 >= 0)
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
