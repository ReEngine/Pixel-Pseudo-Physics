using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;



namespace SFMLTryout
{

    class Material
    {

        public static Material Generator = new Material(0);
        public static Material Air = new Material(1);
        public static Material Sand = new Material(2);
        public static Material Water = new Material(3);
        public static Material Gas = new Material(4);
        public byte id;
        string name;
        public Material(byte Id)
        {
            this.id = Id;
        }

    }

}
