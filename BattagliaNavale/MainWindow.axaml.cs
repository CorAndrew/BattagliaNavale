using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace BattagliaNavale
{
    public partial class MainWindow : Window
    {
        private CCampo campoCondiviso;
        private InserimentoView vistaInserimento;
        private GiocoView vistaGioco;

        public MainWindow()
        {
            InitializeComponent();
            campoCondiviso = new CCampo();
            vistaInserimento = new InserimentoView(campoCondiviso);
            vistaGioco = new GiocoView(campoCondiviso);

            ContenutoPrincipale.Content = vistaInserimento;
        }

        public void BtnInserimentoNavi_Click(object sender, RoutedEventArgs e)
        {
            vistaInserimento.AggiornaVista();
            ContenutoPrincipale.Content = vistaInserimento;
        }

        public void BtnGioco_Click(object sender, RoutedEventArgs e)
        {
            vistaGioco.AggiornaVista();
            ContenutoPrincipale.Content = vistaGioco;
        }
    }
}