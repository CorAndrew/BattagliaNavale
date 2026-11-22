using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace BattagliaNavale
{
    public partial class GiocoView : UserControl
    {
        private CCampo campo;
        private Button[,] bottoniGioco;

        public GiocoView(CCampo campoRiferimento)
        {
            InitializeComponent();
            campo = campoRiferimento;
            bottoniGioco = new Button[CCampo.DimensioneGriglia, CCampo.DimensioneGriglia];

            campo.OnEsitoSparo = GestisciEsitoSparo;
            campo.OnNaveAffondata = GestisciNaveAffondata;
            campo.OnStatoAggiornato = GestisciStatoAggiornato;

            CostruisciGriglia();
            AggiornaVista();
        }

        private void CostruisciGriglia()
        {
            GrigliaGioco.Children.Clear();

            for (int x = 0; x < CCampo.DimensioneGriglia; x++)
            {
                for (int y = 0; y < CCampo.DimensioneGriglia; y++)
                {
                    Button btn = new Button();
                    btn.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
                    btn.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch;
                    btn.Margin = new Avalonia.Thickness(1);
                    btn.Tag = x + "," + y;
                    btn.Click += CellaGioco_Click;

                    bottoniGioco[x, y] = btn;
                    GrigliaGioco.Children.Add(btn);
                }
            }
        }

        private void CellaGioco_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null || btn.Tag == null)
            {
                return;
            }

            string coords = btn.Tag.ToString();
            string[] parti = coords.Split(',');
            if (parti.Length != 2)
            {
                return;
            }

            int x = 0;
            int y = 0;
            if (!int.TryParse(parti[0], out x) || !int.TryParse(parti[1], out y))
            {
                return;
            }

            campo.Spara(x, y);
            AggiornaVista();
        }

        private void GestisciEsitoSparo(int x, int y, string messaggioLog)
        {
            TxtLogGioco.Text = TxtLogGioco.Text + messaggioLog + "\n";
        }

        private void GestisciNaveAffondata(int dimensioneNave, int naviRimaste)
        {
            TxtLogGioco.Text = TxtLogGioco.Text + "Nave da " + dimensioneNave + " celle AFFONDATA! (Navi rimaste: " + naviRimaste + ")\n";
        }

        private void GestisciStatoAggiornato(int mosse, int naviRimaste)
        {
            TxtMosse.Text = "Mosse: " + mosse;
            TxtNaviRimaste.Text = "Navi non affondate: " + naviRimaste;
        }

        public void AggiornaVista()
        {
            TxtMosse.Text = "Mosse: " + campo.MosseEffettuate;
            TxtNaviRimaste.Text = "Navi non affondate: " + campo.ContaNaviNonAffondate();

            for (int x = 0; x < CCampo.DimensioneGriglia; x++)
            {
                for (int y = 0; y < CCampo.DimensioneGriglia; y++)
                {
                    CCella cella = campo.Griglia[x, y];
                    if (cella.Colpita)
                    {
                        bottoniGioco[x, y].Content = cella.SimboloStato();
                    }
                    else
                    {
                        bottoniGioco[x, y].Content = "";
                    }
                }
            }
        }
    }
}