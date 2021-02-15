using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLTryout
{
    class Pixel
    {

        const byte Generator = 0;
        const byte Air = 1;
        const byte Sand = 2;
        const byte Water = 3;


        public Color color;
        public byte Material;

        public Pixel(byte material)
        {
            this.Material = material;
            if (material == 0)
                this.color = Color.Magenta;
            if (material == 1)
                this.color = Color.Black;
            if (material == 2)
            {
                //this.color = Color.Yellow;
                Random rnd = new Random();
                byte _RG = Convert.ToByte(rnd.Next(200, 255));
                this.color = new Color(_RG, _RG, Convert.ToByte(rnd.Next(100, _RG)));

            }
            if (Material == 3)
            {
                Random rnd = new Random();
                byte _RG = Convert.ToByte(rnd.Next(0, 100));
                this.color = new Color(_RG, _RG, Convert.ToByte(rnd.Next(115, 255)));
            }
        }
        public void Update(uint x, uint y)
        {
            Random rnd = new Random();
            if (this.Material == Sand)
            {
                if (y + 1 < Program._Height)
                {
                    if (Down(Air, x, y))
                    {
                        MoveDown(x, y);
                    }

                    else if (DownLeft(Air, x, y))
                        MoveDownLeft(x, y);
                    else if (DownRight(Air, x, y))
                        MoveDownRight(x, y);
                    else if (DownLeft(Air, x, y) & DownRight(Air, x, y))
                    {
                        if (rnd.Next(0, 2) == 0)
                        {
                            MoveDownLeft(x, y);
                        }
                        else
                        {
                            MoveDownRight(x, y);
                        }
                    }

                }

            }
            else if (this.Material == Water)
            {
                if (y + 1 < Program._Height)
                {
                    if (Down(Air, x, y))
                    {
                        MoveDown(x, y);
                    }

                    else if (DownLeft(Air, x, y))
                        MoveDownLeft(x, y);
                    else if (DownRight(Air, x, y))
                        MoveDownRight(x, y);
                    else if (DownLeft(Air, x, y) & DownRight(Air, x, y))
                    {
                        if (rnd.Next(0, 2) == 0)
                        {
                            MoveDownLeft(x, y);
                        }
                        else
                        {
                            MoveDownRight(x, y);
                        }
                    }
                    else if (Left(Air, x, y) & Right(Air, x, y))
                    {
                        if (rnd.Next(0, 2) == 0)
                        {
                            MoveLeft(x, y);
                        }
                        else
                        {
                            MoveRight(x, y);
                        }
                    }
                    else if (Left(Air, x, y))
                        MoveLeft(x, y);
                    else if (Right(Air, x, y))
                        MoveRight(x, y);
                }
            }
            if (this.Material == Generator)
            {
                if (y + 1 < Program._Height & Program.Placing)
                    Program.field[x, y + 1] = new Pixel(Program.mP);

            }
        }
        public bool Down(byte material, uint x, uint y)
        {
            return (Program.field[x, y + 1].Material == material & y + 1 < Program._Height);
        }
        public bool Left(byte material, uint x, uint y)
        {
            return (Program.field[x - 1, y].Material == material & x - 1 > 0);
        }
        public bool Right(byte material, uint x, uint y)
        {
            return (Program.field[x + 1, y].Material == material & x + 1 < Program._Width);
        }
        public bool DownLeft(byte material, uint x, uint y)
        {
            return (Program.field[x - 1, y + 1].Material == material & x - 1 > 0 & y + 1 < Program._Height);
        }
        public bool DownRight(byte material, uint x, uint y)
        {
            return (Program.field[x + 1, y + 1].Material == material & x + 1 < Program._Width & y + 1 < Program._Height);
        }



        public void MoveDown(uint x, uint y)
        {
            Pixel tmp = Program.field[x, y + 1];
            Program.field[x, y + 1] = this;
            Program.field[x, y] = tmp;
        }
        public void MoveLeft(uint x, uint y)
        {
            Pixel tmp = Program.field[x - 1, y];
            Program.field[x - 1, y] = this;
            Program.field[x, y] = tmp;
        }
        public void MoveRight(uint x, uint y)
        {
            Pixel tmp = Program.field[x + 1, y];
            Program.field[x + 1, y] = this;
            Program.field[x, y] = tmp;
        }
        public void MoveDownLeft(uint x, uint y)
        {
            Pixel tmp = Program.field[x - 1, y + 1];
            Program.field[x - 1, y + 1] = this;
            Program.field[x, y] = tmp;
        }
        public void MoveDownRight(uint x, uint y)
        {
            Pixel tmp = Program.field[x + 1, y + 1];
            Program.field[x + 1, y + 1] = this;
            Program.field[x, y] = tmp;
        }



    }
}
