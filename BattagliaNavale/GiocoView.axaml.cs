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

            campo.EsitoSparo += Campo_EsitoSparo;
            campo.NaveAffondata += Campo_NaveAffondata;
            campo.StatoAggiornato += Campo_StatoAggiornato;

            CostruisciGriglia();
            AggiornaVista();
        }

        private void Campo_EsitoSparo(object sender, SparoEventArgs e)
        {
            TxtLogGioco.Text = TxtLogGioco.Text + e.MessaggioLog + "\n";
        }

        private void Campo_NaveAffondata(object sender, NaveAffondataEventArgs e)
        {
            TxtLogGioco.Text = TxtLogGioco.Text + "Nave da " + e.DimensioneNave + " celle AFFONDATA! (Navi rimaste: " + e.NaviRimaste + ")\n";
        }

        private void Campo_StatoAggiornato(object sender, StatoPartitaEventArgs e)
        {
            TxtMosse.Text = "Mosse: " + e.Mosse;
            TxtNaviRimaste.Text = "Navi non affondate: " + e.NaviRimaste;
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