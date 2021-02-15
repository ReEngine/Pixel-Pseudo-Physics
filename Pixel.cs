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
                this.color = new Color(_RG, _RG, Convert.ToByte(rnd.Next(100,_RG)));

            }
        }
        public void Update(uint x, uint y)
        {
            if (this.Material == Sand)
            {
                if (y + 1 < Program._Height)
                {
                    if (Down(Air,x,y))
                    {
                        MoveDown(x,y);
                    }
                }
            }
        }
        public bool Down(byte material,uint x, uint y)
        {
            return (Program.field[x, y + 1].Material == material & y < Program._Height);
        }

        public void MoveDown(uint x, uint y)
        {
            Pixel tmp = Program.field[x, y + 1];
            Program.field[x, y + 1] = this;
            Program.field[x, y] = tmp;
        }
    }
}
