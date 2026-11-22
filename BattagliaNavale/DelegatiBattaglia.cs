using System;

namespace BattagliaNavale
{
    public delegate void EsitoSparoHandler(int x, int y, string messaggioLog);
    public delegate void NaveAffondataHandler(int dimensioneNave, int naviRimaste);
    public delegate void StatoPartitaHandler(int mosse, int naviRimaste);
    public delegate void InserimentoNaveHandler(string messaggioLog, bool flottaCompleta);
}