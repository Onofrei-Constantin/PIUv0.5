using LibrarieModele;
using System.Collections.Generic;

namespace NivelAccesDate
{
    public interface IStocareData
    {
        void AddMasina(Masina s);
        void Stergere();
        List<Masina> GetMasini();
        Masina GetMasina(string numeVanzator, string tip);
        bool UpdateMasina(Masina s);
        Masina GetMasinaByIndex(int index);

        List<Masina> GetMasiniNoi();
    }
}
