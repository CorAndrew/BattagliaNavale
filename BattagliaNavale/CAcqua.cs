using System;

namespace BattagliaNavale
{
    public class CAcqua : CCella
    {
        public CAcqua(int x, int y) : base(x, y)
        {
        }

        public override string SimboloStato()
        {
            if (Colpita)
            {
                return "~";
            }
            return ".";
        }
    }
}