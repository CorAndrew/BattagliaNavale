using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace BattagliaNavale
{
    public partial class InserimentoView : UserControl
    {
        private CCampo campo;
        private Button[,] bottoniGriglia;

        public InserimentoView(CCampo campoRiferimento)
        {
            InitializeComponent();
            campo = campoRiferimento;
            bottoniGriglia = new Button[CCampo.DimensioneGriglia, CCampo.DimensioneGriglia];

            // Collegamento tramite delegato
            campo.OnNaveInserita = GestisciNaveInserita;

            CostruisciGriglia();
            AggiornaVista();
        }

        private void CostruisciGriglia()
        {
            GrigliaInserimento.Children.Clear();

            for (int x = 0; x < CCampo.DimensioneGriglia; x++)
            {
                for (int y = 0; y < CCampo.DimensioneGriglia; y++)
                {
                    Button btn = new Button();
                    btn.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
                    btn.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch;
                    btn.Margin = new Avalonia.Thickness(1);
                    btn.Tag = x + "," + y;
                    btn.Click += CellaInserimento_Click;

                    bottoniGriglia[x, y] = btn;
                    GrigliaInserimento.Children.Add(btn);
                }
            }
        }

        private void CellaInserimento_Click(object sender, RoutedEventArgs e)
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

            bool orizzontale = false;
            if (ChkOrizzontale.IsChecked == true)
            {
                orizzontale = true;
            }

            string errore = "";
            bool ok = campo.PosizionaNave(x, y, orizzontale, out errore);
            if (!ok)
            {
                TxtLogInserimento.Text = TxtLogInserimento.Text + "Errore: " + errore + "\n";
            }

            AggiornaVista();
        }

        private void GestisciNaveInserita(string messaggioLog, bool flottaCompleta)
        {
            TxtLogInserimento.Text = TxtLogInserimento.Text + messaggioLog + "\n";
            if (flottaCompleta)
            {
                TxtLogInserimento.Text = TxtLogInserimento.Text + "Flotta completa! Puoi passare a Gioco.\n";
            }
        }

        public void BtnResetCampo_Click(object sender, RoutedEventArgs e)
        {
            campo.PulisciCampo();
            TxtLogInserimento.Text = "Campo svuotato.\n";
            AggiornaVista();
        }

        public void AggiornaVista()
        {
            int prossimaDim = campo.ProssimaDimensioneDaInserire();
            if (prossimaDim > 0)
            {
                TxtNaveCorrente.Text = "Posiziona nave da " + prossimaDim + " celle (" + campo.NaviInserite.Count + "/" + campo.FlottaRichiesta.Count + ")";
            }
            else
            {
                TxtNaveCorrente.Text = "Tutte le 6 navi inserite.";
            }

            for (int x = 0; x < CCampo.DimensioneGriglia; x++)
            {
                for (int y = 0; y < CCampo.DimensioneGriglia; y++)
                {
                    CCella cella = campo.Griglia[x, y];
                    bottoniGriglia[x, y].Content = cella.SimboloStato();
                }
            }
        }
    }
}