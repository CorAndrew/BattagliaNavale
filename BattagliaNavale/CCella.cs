using System;

namespace BattagliaNavale
{
    public abstract class CCella
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool Colpita { get; set; }

        public CCella(int x, int y)
        {
            X = x;
            Y = y;
            Colpita = false;
        }

        public abstract string SimboloStato();
    }
}