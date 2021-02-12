using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace SFMLTryout
{
    class Pixel
    {
        public Color color;
        Vector2i position;
        public Material Material;
        public Pixel(Material material, int x, int y)
        {
            this.Material = material;
            Random rnd = new Random();
            if (this.Material == Material.Sand)
            {
                byte _RG = Convert.ToByte(rnd.Next(200, 255));
                this.color = new Color(_RG, _RG, Convert.ToByte(rnd.Next(100, _RG)));
            }
            if (this.Material == Material.Generator)
                this.color = Color.Magenta;
            if (this.Material == Material.Air)
                this.color = Color.Black;
            this.position = new Vector2i(x, y);
        }

        void GetColor()
        {

        }

        public void Update()
        {
            if (this.Material == Material.Sand)
            {
                if (Down(Material.Air))
                {
                    this.MoveDown();
                }
            }
            if (this.Material == Material.Generator)
            {
                if (Down(Material.Air) & Program.placing)
                    PlaceDown(new Pixel(Program.mP, this.position.X, this.position.Y));
            }
        }


        bool Up(Material material)
        {
            if (Program.field[this.position.X, this.position.Y - 1].Material == material)
                return true;
            else return false;
        }
        bool Down(Material material)
        {
            if (this.position.Y + 2 < Program._Height)
                if (Program.field[this.position.X, this.position.Y + 1].Material == material)
                    return true;
                else return false;
            else return false;
        }
        bool Left(Material material)
        {
            if (Program.field[this.position.X - 1, this.position.Y].Material == material)
                return true;
            else return false;
        }
        bool Right(Material material)
        {
            if (Program.field[this.position.X + 1, this.position.Y].Material == material)
                return true;
            else return false;
        }
        bool UpLeft(Material material)
        {
            if (Program.field[this.position.X - 1, this.position.Y - 1].Material == material)
                return true;
            else return false;
        }
        bool UpRight(Material material)
        {
            if (Program.field[this.position.X + 1, this.position.Y - 1].Material == material)
                return true;
            else return false;
        }
        bool DownLeft(Material material)
        {
            if (Program.field[this.position.X - 1, this.position.Y + 1].Material == material)
                return true;
            else return false;
        }
        bool DownRight(Material material)
        {
            if (Program.field[this.position.X + 1, this.position.Y - 1].Material == material)
                return true;
            else return false;
        }
        bool Around(Material material)
        {
            if (Program.field[this.position.X - 1, this.position.Y - 1].Material == material | Program.field[this.position.X + 0, this.position.Y - 1].Material == material | Program.field[this.position.X + 1, this.position.Y - 1].Material == material |
                Program.field[this.position.X - 1, this.position.Y + 0].Material == material | /*  Placeholder    Placeholder    Placeholder    Placeholder    Placeholder */ Program.field[this.position.X + 1, this.position.Y + 0].Material == material |
                Program.field[this.position.X - 1, this.position.Y + 1].Material == material | Program.field[this.position.X + 0, this.position.Y - 1].Material == material | Program.field[this.position.X + 1, this.position.Y + 1].Material == material)
                return true;
            else return false;
        }

        void MoveUp()
        {
            Program.field[this.position.X, this.position.Y - 1] = this;
            Program.field[this.position.X, this.position.Y].Material = Material.Air;
        }
        void MoveDown()
        {
            Program.field[this.position.X, this.position.Y + 1] = this;
            Program.field[this.position.X, this.position.Y] = new Pixel(Material.Air, this.position.X, this.position.Y);
        }
        void MoveLeft()
        {

            Program.field[this.position.X - 1, this.position.Y] = this;
            Program.field[this.position.X, this.position.Y].Material = Material.Air;
        }
        void MoveRight()
        {

            Program.field[this.position.X + 1, this.position.Y] = this;
            Program.field[this.position.X, this.position.Y].Material = Material.Air;
        }
        void MoveUpLeft()
        {

            Program.field[this.position.X - 1, this.position.Y - 1] = this;
            Program.field[this.position.X, this.position.Y].Material = Material.Air;
        }
        void MoveUpRight()
        {
            Program.field[this.position.X + 1, this.position.Y - 1] = this;
            Program.field[this.position.X, this.position.Y].Material = Material.Air;
        }
        void MoveDownLeft()
        {
            Program.field[this.position.X - 1, this.position.Y + 1] = this;
            Program.field[this.position.X, this.position.Y].Material = Material.Air;
        }
        void MoveDownRight()
        {

            Program.field[this.position.X + 1, this.position.Y + 1] = this;
            Program.field[this.position.X, this.position.Y].Material = Material.Air;
        }

        void PlaceUp(Pixel pixel)
        {

        }
        void PlaceDown(Pixel pixel)
        {
            Program.field[this.position.X, this.position.Y + 1] = new Pixel(Material.Sand, this.position.X, this.position.Y + 1);
        }
        void PlaceLeft(Pixel pixel)
        {

        }
        void PlaceRight(Pixel pixel)
        {

        }
        void PlaceUpLeft(Pixel pixel)
        {

        }
        void PlaceUpRight(Pixel pixel)
        {

        }
        void PlaceDownLeft(Pixel pixel)
        {

        }
        void PlaceDownRight(Pixel pixel)
        {

        }


    }
}
