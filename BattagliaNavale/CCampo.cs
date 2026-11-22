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

        public EsitoSparoHandler OnEsitoSparo;
        public NaveAffondataHandler OnNaveAffondata;
        public StatoPartitaHandler OnStatoAggiornato;
        public InserimentoNaveHandler OnNaveInserita;

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

        private void InizializzaFlottaRichiesta()
        {
            FlottaRichiesta.Clear();
            // 1 nave da 4 celle
            FlottaRichiesta.Add(4);
            // 2 navi da 3 celle
            FlottaRichiesta.Add(3);
            FlottaRichiesta.Add(3);
            // 2 navi da 2 celle
            FlottaRichiesta.Add(2);
            FlottaRichiesta.Add(2);
            // 1 nave da 1 cella
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

            // Verifica limiti griglia e sovrapposizioni
            for (int k = 0; k < dimensione; k++)
            {
                int cx = startX;
                int cy = startY;

                if (orizzontale)
                {
                    cy = startY + k;
                }
                else
                {
                    cx = startX + k;
                }

                if (cx < 0 || cx >= DimensioneGriglia || cy < 0 || cy >= DimensioneGriglia)
                {
                    errore = "La nave esce dai bordi della griglia 10x10.";
                    return false;
                }

                CNave cellaOccupata = Griglia[cx, cy] as CNave;
                if (cellaOccupata != null)
                {
                    errore = "C'e' gia' una nave in posizione (" + cx + "," + cy + ").";
                    return false;
                }
            }

            // Creazione celle della nave
            List<CNave> celleNave = new List<CNave>();
            for (int k = 0; k < dimensione; k++)
            {
                int cx = startX;
                int cy = startY;

                if (orizzontale)
                {
                    cy = startY + k;
                }
                else
                {
                    cx = startX + k;
                }

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

            if (OnNaveInserita != null)
            {
                OnNaveInserita(msg, completa);
            }

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
                if (OnEsitoSparo != null)
                {
                    OnEsitoSparo(x, y, "Casella (" + x + "," + y + ") gia' colpita.");
                }
                return;
            }

            cella.Colpita = true;
            MosseEffettuate++;

            CNave naveColpita = cella as CNave;
            if (naveColpita != null)
            {
                string msgColpito = "Colpito in posizione (" + x + "," + y + ")";
                if (OnEsitoSparo != null)
                {
                    OnEsitoSparo(x, y, msgColpito);
                }

                if (naveColpita.IsAffondata())
                {
                    int rimaste = ContaNaviNonAffondate();
                    if (OnNaveAffondata != null)
                    {
                        OnNaveAffondata(naveColpita.Dimensione, rimaste);
                    }
                }
            }
            else
            {
                if (OnEsitoSparo != null)
                {
                    OnEsitoSparo(x, y, "Acqua !!");
                }
            }

            if (OnStatoAggiornato != null)
            {
                OnStatoAggiornato(MosseEffettuate, ContaNaviNonAffondate());
            }
        }
    }
}