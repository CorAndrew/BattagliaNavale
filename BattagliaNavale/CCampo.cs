using System;
using System.Collections.Generic;

namespace BattagliaNavale
{
    public class CCampo
    {
        public const int DimensioneGriglia = 10;
        public CCella[,] Griglia { get; set; }
        public List<CNave> NaviInserite { get; set; }
        public List<int> FlottaRichiesta { get; set; }
        public int MosseEffettuate { get; set; }

        public event EventHandler<SparoEventArgs> EsitoSparo;
        public event EventHandler<NaveAffondataEventArgs> NaveAffondata;
        public event EventHandler<StatoPartitaEventArgs> StatoAggiornato;
        public event EventHandler<InserimentoNaveEventArgs> NaveInserita;

        private int prossimoIdNave;

        public CCampo()
        {
            Griglia = new CCella[DimensioneGriglia, DimensioneGriglia];
            NaviInserite = new List<CNave>();
            FlottaRichiesta = new List<int>();
            MosseEffettuate = 0;
            prossimoIdNave = 1;

            InizializzaFlottaRichiesta();
            PulisciCampo();
        }

        protected virtual void OnEsitoSparo(SparoEventArgs e)
        {
            EsitoSparo?.Invoke(this, e);
        }

        protected virtual void OnNaveAffondata(NaveAffondataEventArgs e)
        {
            NaveAffondata?.Invoke(this, e);
        }

        protected virtual void OnStatoAggiornato(StatoPartitaEventArgs e)
        {
            StatoAggiornato?.Invoke(this, e);
        }

        protected virtual void OnNaveInserita(InserimentoNaveEventArgs e)
        {
            NaveInserita?.Invoke(this, e);
        }

        private void InizializzaFlottaRichiesta()
        {
            FlottaRichiesta.Clear();
            FlottaRichiesta.Add(4);
            FlottaRichiesta.Add(3);
            FlottaRichiesta.Add(3);
            FlottaRichiesta.Add(2);
            FlottaRichiesta.Add(2);
            FlottaRichiesta.Add(1);
        }

        public void PulisciCampo()
        {
            Griglia = new CCella[DimensioneGriglia, DimensioneGriglia];
            NaviInserite.Clear();
            MosseEffettuate = 0;
            prossimoIdNave = 1;

            for (int x = 0; x < DimensioneGriglia; x++)
            {
                for (int y = 0; y < DimensioneGriglia; y++)
                {
                    Griglia[x, y] = new CAcqua(x, y);
                }
            }
        }

        public int ProssimaDimensioneDaInserire()
        {
            int indice = NaviInserite.Count;
            if (indice < FlottaRichiesta.Count)
            {
                return FlottaRichiesta[indice];
            }
            return -1;
        }

        public bool FlottaCompleta()
        {
            return NaviInserite.Count >= FlottaRichiesta.Count;
        }

        public bool PosizionaNave(int startX, int startY, bool orizzontale, out string errore)
        {
            errore = "";
            int dimensione = ProssimaDimensioneDaInserire();

            if (dimensione <= 0)
            {
                errore = "La flotta e' gia' completa.";
                return false;
            }

            // Passo di avanzamento: orizzontale = si muove su y, verticale = si muove su x
            int dx = orizzontale ? 0 : 1;
            int dy = orizzontale ? 1 : 0;

            // Prima verifica che tutte le celle siano valide e libere
            for (int k = 0; k < dimensione; k++)
            {
                int cx = startX + k * dx;
                int cy = startY + k * dy;

                if (cx < 0 || cx >= DimensioneGriglia || cy < 0 || cy >= DimensioneGriglia)
                {
                    errore = "La nave esce dai bordi della griglia 10x10.";
                    return false;
                }

                if (Griglia[cx, cy] is CNave)
                {
                    errore = "C'e' gia' una nave in posizione (" + cx + "," + cy + ").";
                    return false;
                }
            }

            // Poi crea i pezzi della nave
            List<CNave> celleNave = new List<CNave>();
            for (int k = 0; k < dimensione; k++)
            {
                int cx = startX + k * dx;
                int cy = startY + k * dy;

                CNave pezzo = new CNave(cx, cy, dimensione, prossimoIdNave);
                Griglia[cx, cy] = pezzo;
                celleNave.Add(pezzo);
            }

            for (int i = 0; i < celleNave.Count; i++)
            {
                celleNave[i].PartiNave = celleNave;
            }

            NaviInserite.Add(celleNave[0]);
            prossimoIdNave++;

            bool completa = FlottaCompleta();
            string msg = "Inserita nave da " + dimensione + " celle in (" + startX + "," + startY + ").";
            OnNaveInserita(new InserimentoNaveEventArgs(msg, completa));

            return true;
        }

        public int ContaNaviNonAffondate()
        {
            int nonAffondate = 0;
            for (int i = 0; i < NaviInserite.Count; i++)
            {
                if (!NaviInserite[i].IsAffondata())
                {
                    nonAffondate++;
                }
            }
            return nonAffondate;
        }

        public void Spara(int x, int y)
        {
            if (x < 0 || x >= DimensioneGriglia || y < 0 || y >= DimensioneGriglia)
            {
                return;
            }

            CCella cella = Griglia[x, y];
            if (cella.Colpita)
            {
                OnEsitoSparo(new SparoEventArgs(x, y, "Casella (" + x + "," + y + ") gia' colpita."));
                return;
            }

            cella.Colpita = true;
            MosseEffettuate++;

            CNave naveColpita = cella as CNave;
            if (naveColpita != null)
            {
                OnEsitoSparo(new SparoEventArgs(x, y, "Colpito in posizione (" + x + "," + y + ")"));

                if (naveColpita.IsAffondata())
                {
                    int rimaste = ContaNaviNonAffondate();
                    OnNaveAffondata(new NaveAffondataEventArgs(naveColpita.Dimensione, rimaste));
                }
            }
            else
            {
                OnEsitoSparo(new SparoEventArgs(x, y, "Acqua !!"));
            }

            OnStatoAggiornato(new StatoPartitaEventArgs(MosseEffettuate, ContaNaviNonAffondate()));
        }
    }
}