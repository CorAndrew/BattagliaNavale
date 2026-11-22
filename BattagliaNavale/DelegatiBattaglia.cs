using System;

namespace BattagliaNavale
{
    public class SparoEventArgs : EventArgs
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string MessaggioLog { get; set; }

        public SparoEventArgs(int x, int y, string messaggioLog)
        {
            X = x;
            Y = y;
            MessaggioLog = messaggioLog;
        }
    }

    public class NaveAffondataEventArgs : EventArgs
    {
        public int DimensioneNave { get; set; }
        public int NaviRimaste { get; set; }

        public NaveAffondataEventArgs(int dimensioneNave, int naviRimaste)
        {
            DimensioneNave = dimensioneNave;
            NaviRimaste = naviRimaste;
        }
    }

    public class StatoPartitaEventArgs : EventArgs
    {
        public int Mosse { get; set; }
        public int NaviRimaste { get; set; }

        public StatoPartitaEventArgs(int mosse, int naviRimaste)
        {
            Mosse = mosse;
            NaviRimaste = naviRimaste;
        }
    }

    public class InserimentoNaveEventArgs : EventArgs
    {
        public string MessaggioLog { get; set; }
        public bool FlottaCompleta { get; set; }

        public InserimentoNaveEventArgs(string messaggioLog, bool flottaCompleta)
        {
            MessaggioLog = messaggioLog;
            FlottaCompleta = flottaCompleta;
        }
    }
}