using System;
using System.Collections.Generic;

namespace BattagliaNavale
{
    public class CNave : CCella
    {
        public int Dimensione { get; set; }
        public int IdNave { get; set; }
        public List<CNave> PartiNave { get; set; }

        public CNave(int x, int y, int dimensione, int idNave) : base(x, y)
        {
            Dimensione = dimensione;
            IdNave = idNave;
            PartiNave = new List<CNave>();
        }

        public bool IsAffondata()
        {
            if (PartiNave == null || PartiNave.Count == 0)
            {
                return Colpita;
            }

            for (int i = 0; i < PartiNave.Count; i++)
            {
                if (!PartiNave[i].Colpita)
                {
                    return false;
                }
            }
            return true;
        }

        public override string SimboloStato()
        {
            if (Colpita)
            {
                if (IsAffondata())
                {
                    return "X";
                }
                return "*";
            }
            return "N";
        }
    }
}