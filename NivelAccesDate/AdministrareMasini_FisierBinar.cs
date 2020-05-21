using LibrarieModele;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace NivelAccesDate
{
    public class AdministrareMasini_FisierBinar : IStocareData
    {
        private const int ID_PRIMA_MASINA = 1;
        private const int INCREMENT = 1;
        string NumeFisier { get; set; }
        public AdministrareMasini_FisierBinar(string numeFisier)
        {
            this.NumeFisier = numeFisier;
            Stream sBinFile = File.Open(NumeFisier, FileMode.OpenOrCreate);
            sBinFile.Close();
        }
        public void AddMasina(Masina s)
        {
            s.IdMasina = GetId();

            try
            {
                BinaryFormatter b = new BinaryFormatter();

                //instructiunea 'using' va apela sBinFile.Close();
                using (Stream sBinFile = File.Open(NumeFisier, FileMode.Append, FileAccess.Write))
                {
                    //serializare unui obiect
                    b.Serialize(sBinFile, s);
                }
            }
            catch (IOException eIO)
            {
                throw new Exception("Eroare la deschiderea fisierului. Mesaj: " + eIO.Message);
            }
            catch (Exception eGen)
            {
                throw new Exception("Eroare generica. Mesaj: " + eGen.Message);
            }
        }
        public void Stergere()
        {
            throw new Exception("Optiunea Stergere nu este implementata");
        }
        public List<Masina> GetMasini()
        {
            List<Masina> masini = new List<Masina>();
            try
            {
                BinaryFormatter b = new BinaryFormatter();

                //instructiunea 'using' va apela sBinFile.Close();
                using (Stream sBinFile = File.Open(NumeFisier, FileMode.Open))
                {

                    while (sBinFile.Position < sBinFile.Length)
                    {
                        //Observati conversia!!!
                        masini.Add((Masina)b.Deserialize(sBinFile));
                    }
                }
            }
            catch (IOException eIO)
            {
                throw new Exception("Eroare la deschiderea fisierului. Mesaj: " + eIO.Message);
            }
            catch (Exception eGen)
            {
                throw new Exception("Eroare generica. Mesaj: " + eGen.Message);
            }

            return masini;
        }
        public Masina GetMasinaByIndex(int index)
        {
            Masina s = null;
            List<Masina> masini = new List<Masina>();
            try
            {
                BinaryFormatter b = new BinaryFormatter();
                int contor = 0;
                //instructiunea 'using' va apela sBinFile.Close();
                using (Stream sBinFile = File.Open(NumeFisier, FileMode.Open))
                {

                    while (sBinFile.Position < sBinFile.Length)
                    {
                        s = (Masina)b.Deserialize(sBinFile);
                        //Observati conversia!!!
                        masini.Add((Masina)b.Deserialize(sBinFile));
                        if (contor == index)
                        {
                            break;
                        }
                        contor++;
                    }
                }
            }
            catch (IOException eIO)
            {
                throw new Exception("Eroare la deschiderea fisierului. Mesaj: " + eIO.Message);
            }
            catch (Exception eGen)
            {
                throw new Exception("Eroare generica. Mesaj: " + eGen.Message);
            }

            return s;
        }
        public Masina GetMasina(string numeVanzator, string tip)
        {
            Masina s = null;
            
            try
            {
                BinaryFormatter b = new BinaryFormatter();
                //instructiunea 'using' va apela sBinFile.Close();
                using (Stream sBinFile = File.Open(NumeFisier, FileMode.Open))
                {

                    while (sBinFile.Position < sBinFile.Length)
                    {
                        s = (Masina)b.Deserialize(sBinFile);
                        if (s.NumeVanzator.Equals(numeVanzator) && s.Tip.Equals(tip))
                        {
                            break;
                        }
                        
                    } 
                }
            }
            catch (IOException eIO)
            {
                throw new Exception("Eroare la deschiderea fisierului. Mesaj: " + eIO.Message);
            }
            catch (Exception eGen)
            {
                throw new Exception("Eroare generica. Mesaj: " + eGen.Message);
            }
            return s;
        }
        public bool UpdateMasina(Masina s)
        {
            bool ok = false;
            List<Masina> masini = GetMasini();
            try
            {
                BinaryFormatter b = new BinaryFormatter();

                //instructiunea 'using' va apela sBinFile.Close();
                using (Stream sBinFile = File.Open(NumeFisier, FileMode.Truncate, FileAccess.Write))
                {
                    foreach (var mas in masini)
                    {
                        //serializare unui obiect
                        if (mas.IdMasina == s.IdMasina)
                        {
                            b.Serialize(sBinFile, s);
                        }
                        else
                            b.Serialize(sBinFile, mas);
                    }
                    ok = true;

                }
            }
            catch (IOException eIO)
            {
                throw new Exception("Eroare la deschiderea fisierului. Mesaj: " + eIO.Message);
            }
            catch (Exception eGen)
            {
                throw new Exception("Eroare generica. Mesaj: " + eGen.Message);
            }
            return ok;
        }
        private int GetId()
        {
            int IDMASINA = ID_PRIMA_MASINA;
            try
            {
                //instructiunea 'using' va apela sBinFile.Close();
                using (Stream sBinFile = File.Open(NumeFisier, FileMode.Open))
                {
                    BinaryFormatter b = new BinaryFormatter();

                    //citeste cate o linie si creaza un obiect de tip Student pe baza datelor din linia citita
                    while (sBinFile.Position < sBinFile.Length)
                    {
                        //Observati conversia!!!
                        Masina s = (Masina)b.Deserialize(sBinFile);
                        IDMASINA = s.IdMasina + INCREMENT;
                    }
                }
            }
            catch (IOException eIO)
            {
                throw new Exception("Eroare la deschiderea fisierului. Mesaj: " + eIO.Message);
            }
            catch (Exception eGen)
            {
                throw new Exception("Eroare generica. Mesaj: " + eGen.Message);
            }
            return IDMASINA;
        }
        public List<Masina> GetMasiniNoi()
        {
            List<Masina> masini = GetMasini();
            List<Masina> noi = new List<Masina>();
            foreach (Masina s in masini)
            {
                if (s.Uzura == "Noua")
                    noi.Add(s);
            }
            return noi;
        }
    }
}
