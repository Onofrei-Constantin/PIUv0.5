using System;
using System.Drawing;
using System.Windows.Forms;
using LibrarieModele;
using NivelAccesDate;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Proiect_Forma_II
{
    public partial class Form1 : Form
    {
        IStocareData adminMasini;
        List<string> optiuniSelectate = new List<string>();
        public Form1()
        {
            InitializeComponent();
            adminMasini = StocareFactory.GetAdministratorStocare();
            this.Width = 1350;
        }

        private void btnAdauga_Click(object sender, EventArgs e)
        {
            ResetCuloareEtichete();

            CodEroare codValidare = Validare(txtNumeVanzator.Text, txtNumeCumparator.Text, txtTip.Text, txtAnFabricare.Text, txtDataTranzactie.Text, txtPret.Text);

            if (codValidare != CodEroare.CORECT)
            {
                MarcheazaControaleCuDateIncorecte(codValidare);
            }
            else
            {
                Masina s = new Masina(txtNumeVanzator.Text, txtNumeCumparator.Text, txtTip.Text, txtAnFabricare.Text, txtDataTranzactie.Text, txtPret.Text);


                s.ProgramCulori = GetCuloareSelectata();
                
                s.ProgramOptiuni = new List<string>();
                s.ProgramOptiuni.AddRange(optiuniSelectate);
                s.PriceRange = cmbGamaPreturi.Text;
                s.Uzura = cmbUzura.Text;
                s.DataActualizare = DateTime.Now;
                s.DataAdaugare = dtpMasina.Value;
                adminMasini.AddMasina(s);
                lblMesaj.Text = "Masina a fost adaugata";

                
                ResetareControale();
            }

        }

        private void btnAfiseaza_Click(object sender, EventArgs e)
        {
            this.Width = 1330;
            List<Masina> masini = adminMasini.GetMasini();

            AdaugaMasiniInControlListbox(masini);
            AdaugaMasiniInControlDataGridView(masini);
        }

        private void AdaugaMasiniInControlListbox(List<Masina> masini)
        {
            lstAfisare.Items.Clear();

            //personalizare sursa de date
            //var antetTabel = String.Format("{0,-5}{1,-35}{2,20}{3,10}\n", "Id", "Nume Prenume", "ProgramStudiu", "Medie");
            //lstAfisare.Items.Add(antetTabel);

            foreach (Masina s in masini)
            {
                //pentru a adauga un obiect de tip Student in colectia de Items a unui control de tip ListBox, 
                // clasa Student trebuie sa implementeze metoda ToString() specificand cuvantul cheie 'override' in definitie
                //pentru a arata ca metoda ToString a clasei de baza (Object) este suprascrisa
                lstAfisare.Items.Add(s);


                //personalizare sursa de date
                //var linieTabel = String.Format("{0,-5}{1,-35}{2,20}{3,10}\n", s.IdStudent, s.NumeComplet, s.ProgramSTD.ToString(), s.Media.ToString());
                //lstAfisare.Items.Add(linieTabel);
            }
        }
        private void AdaugaMasiniInControlDataGridView(List<Masina> masini)
        {
            //reset continut control DataGridView
            dataGridMasini.DataSource = null;

            //!!!! Controlul de tip DataGridView are ca sursa de date lista de obiecte de tip Student !!!
            //dataGridStudenti.DataSource = studenti;

            //personalizare sursa de date
            dataGridMasini.DataSource = masini.Select(s => new { s.IdMasina, s.NumeVanzator, s.NumeCumparator, s.Tip, s.AnFabricare, s.DataTranzactie, s.Pret, Optiuni = string.Join(",", s.ProgramOptiuni), s.ProgramCulori, s.PriceRange, s.Uzura , s.DataAdaugare, s.DataActualizare }).ToList();
        }

        private void btnCauta_Click(object sender, EventArgs e)
        {
            Masina s = adminMasini.GetMasina(txtNumeVanzator.Text, txtTip.Text);


            if (s != null)
            {
                lblMesaj.Text = s.ConversieLaSir_PentruForma();
            }
            else
                lblMesaj.Text = "Nu s-a gasit masina";
        }

        private void btnModifica_Click(object sender, EventArgs e)
        {
            ResetCuloareEtichete();

            CodEroare codValidare = Validare(txtNumeVanzator.Text, txtNumeCumparator.Text, txtTip.Text, txtAnFabricare.Text, txtDataTranzactie.Text, txtPret.Text);
            if (codValidare != CodEroare.CORECT)
            {
                MarcheazaControaleCuDateIncorecte(codValidare);
            }
            else
            {
                int ind = Int32.Parse(textID.Text);
                Masina s = adminMasini.GetMasinaByIndex(ind);

                s.IdMasina = ind;
                s.NumeVanzator = txtNumeVanzator.Text;
                s.NumeCumparator = txtNumeCumparator.Text;
                s.Tip = txtTip.Text;
                s.AnFabricare = txtAnFabricare.Text;
                s.DataTranzactie = txtDataTranzactie.Text;
                s.Pret = txtPret.Text;

                s.ProgramCulori = GetCuloareSelectata();
                //set Discipline
                s.ProgramOptiuni = new List<string>();
                s.ProgramOptiuni.AddRange(optiuniSelectate);
                s.PriceRange = cmbGamaPreturi.Text;
                s.Uzura = cmbUzura.Text;
                s.DataActualizare = DateTime.Now;
                s.DataAdaugare = dtpMasina.Value;

                adminMasini.UpdateMasina(s);
                lblMesaj.Text = "Masina a fost actualizata";
                

                //resetarea controalelor pentru a introduce datele unui student nou
                ResetareControale();
            }
            

        }

        private void ckbDiscipline_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBoxControl = sender as CheckBox; //operator 'as'
            //sau
            //CheckBox checkBoxControl = (CheckBox)sender;  //operator cast

            string optiuneSelectata = checkBoxControl.Text;

            //verificare daca checkbox-ul asupra caruia s-a actionat este selectat
            if (checkBoxControl.Checked == true)
                optiuniSelectate.Add(optiuneSelectata);
            else
                optiuniSelectate.Remove(optiuneSelectata);
        }

        private void ResetareControale()
        {
            txtNumeVanzator.Text = txtNumeCumparator.Text = txtTip.Text = txtAnFabricare.Text = txtDataTranzactie.Text = txtPret.Text = string.Empty;
            rdbNegru.Checked = false;
            rdbRosu.Checked = false;
            rdbVerde.Checked = false;
            rdbGalben.Checked = false;
            rdbAlb.Checked = false;
            rdbAlbastru.Checked = false;
            ckbAerConditionat.Checked = false;
            ckbIncalzireScaune.Checked = false;
            ckbCruiseControl.Checked = false;
            ckbSenzoriParcare.Checked = false;
            ckbCutieAutomata.Checked = false;
            ckbNavigatie.Checked = false;
            optiuniSelectate.Clear();
            cmbGamaPreturi.Text = string.Empty;
            cmbUzura.Text = string.Empty;
            lblMesaj.Text = string.Empty;
            lblID.Text = String.Empty;
        }

        private CodEroare Validare(string numeVanzator, string numeCumparator, string Tip, string AnFabricare, string DataTranzactie, string Pret)
        {
            CodEroare rezultatValidare = CodEroare.CORECT;
            if (numeVanzator == string.Empty)
            {
                rezultatValidare |= CodEroare.NUMEVANZATOR_INCORECT;
            }
            if (numeCumparator == string.Empty)
            {
                rezultatValidare |= CodEroare.NUMECUMPARATOR_INCORECT;
            }
            if (Tip == string.Empty)
            {
                rezultatValidare |= CodEroare.TIP_INCORECTE;
            }
            if (AnFabricare == string.Empty)
            {
                rezultatValidare |= CodEroare.ANFABRICARE_INCORECT;
            }
            if (DataTranzactie == string.Empty)
            {
                rezultatValidare |= CodEroare.DATATRANZACTIE_INCORECT;
            }
            if (Pret == string.Empty)
            {
                rezultatValidare |= CodEroare.PRET_INCORECTE;
            }

            if (optiuniSelectate.Count == 0)
            {
                rezultatValidare |= CodEroare.OPTIUNE_INCORECT;
            }

            int culoareSelectata = 0;
            foreach (var control in gpbCulori.Controls)
            {
                RadioButton rdb = null;
                try
                {
                    rdb = (RadioButton)control;
                }
                catch { }

                if (rdb != null && rdb.Checked == true)
                    culoareSelectata = 1;
            }
            if (culoareSelectata == 0)
            {
                rezultatValidare |= CodEroare.CULOARE_INCORECT;
            }

            if(optiuniSelectate.Count==0)
            {
                rezultatValidare |= CodEroare.OPTIUNE_INCORECT;
            }

            return rezultatValidare;
        }
        private void MarcheazaControaleCuDateIncorecte(CodEroare codValidare)
        {
            if ((codValidare & CodEroare.NUMEVANZATOR_INCORECT) == CodEroare.NUMEVANZATOR_INCORECT)
            {
                lblNumeVanzator.ForeColor = Color.Red;
            }
            if ((codValidare & CodEroare.NUMECUMPARATOR_INCORECT) == CodEroare.NUMECUMPARATOR_INCORECT)
            {
                lblNumeCumparator.ForeColor = Color.Red;
            }
            if ((codValidare & CodEroare.TIP_INCORECTE) == CodEroare.TIP_INCORECTE)
            {
                lblTip.ForeColor = Color.Red;
            }
            if ((codValidare & CodEroare.ANFABRICARE_INCORECT) == CodEroare.ANFABRICARE_INCORECT)
            {
                lblAnFabricare.ForeColor = Color.Red;
            }
            if ((codValidare & CodEroare.DATATRANZACTIE_INCORECT) == CodEroare.DATATRANZACTIE_INCORECT)
            {
                lblDataTranzacrie.ForeColor = Color.Red;
            }
            if ((codValidare & CodEroare.PRET_INCORECTE) == CodEroare.PRET_INCORECTE)
            {
                lblPret.ForeColor = Color.Red;
            }
            if ((codValidare & CodEroare.CULOARE_INCORECT) == CodEroare.CULOARE_INCORECT)
            {
                lblCuloare.ForeColor = Color.Red;
            }
            if ((codValidare & CodEroare.OPTIUNE_INCORECT) == CodEroare.OPTIUNE_INCORECT)
            {
                lblOptiuni.ForeColor = Color.Red;
            }
        }
        private void ResetCuloareEtichete()
        {
            lblNumeCumparator.ForeColor = Color.Black;
            lblNumeVanzator.ForeColor = Color.Black;
            lblDataTranzacrie.ForeColor = Color.Black;
            lblAnFabricare.ForeColor = Color.Black;
            lblTip.ForeColor = Color.Black;
            lblPret.ForeColor = Color.Black;
            lblOptiuni.ForeColor = Color.Black;
            lblCuloare.ForeColor = Color.Black;
        }
        private Culori GetCuloareSelectata()
        {
            if (rdbAlb.Checked)
                return Culori.Alb;
            if (rdbAlbastru.Checked)
                return Culori.Albastru;
            if (rdbGalben.Checked)
                return Culori.Galben;
            if (rdbNegru.Checked)
                return Culori.Negru;
            if (rdbRosu.Checked)
                return Culori.Rosu;
            if (rdbVerde.Checked)
                return Culori.Verde;
            return Culori.Culoare_Inexistenta;
        }
        private void lstAfisare_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetareControale();
            Masina s = null;
            try
            {
                s = adminMasini.GetMasinaByIndex(lstAfisare.SelectedIndex - 1);
            }
            catch(Exception ex)
            {
                lblMesaj.Text = "Eroare: " + ex.Message;
            }


            if (s != null)
            {
                lblID.Text = s.IdMasina.ToString();

                txtNumeVanzator.Text = s.NumeVanzator;
                txtTip.Text = s.Tip;

                foreach (var prgstd in gpbCulori.Controls)
                {
                    if (prgstd is RadioButton)
                    {
                        var prgstdBox = prgstd as RadioButton;
                        if (prgstdBox.Text == s.ProgramCulori.ToString())
                        {
                            prgstdBox.Checked = true;
                        }
                    }
                }

                foreach (var disciplina in gpbOptiuni.Controls)
                {
                    if (disciplina is CheckBox)
                    {
                        var disciplinaBox = disciplina as CheckBox;
                        foreach (String dis in s.ProgramOptiuni)
                            if (disciplinaBox.Text.Equals(dis))
                                disciplinaBox.Checked = true;
                    }
                }

                cmbGamaPreturi.Text = s.PriceRange.ToString();
                cmbUzura.Text = s.Uzura.ToString();
            }
        }
        private void filtrareMasiniNoiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Masina> noi = adminMasini.GetMasiniNoi();
            dataGridMasini.DataSource = noi.Select(s => new { s.IdMasina, s.NumeVanzator, s.NumeCumparator, s.Tip, s.AnFabricare, s.DataTranzactie, s.Pret, Optiuni = string.Join(",", s.ProgramOptiuni), s.ProgramCulori, s.PriceRange, s.Uzura,s.DataAdaugare, s.DataActualizare }).ToList();
        }

        private void salvareInFisierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Masina> masini = adminMasini.GetMasiniNoi();
            sfd.ShowDialog();
            salvareRaport(masini, sfd.FileName);
        }
        public void salvareRaport(List<Masina> masini, string numeFisier)
        {
            try
            {
                //instructiunea 'using' va apela la final swFisierText.Close();
                //al doilea parametru setat la 'true' al constructorului StreamWriter indica modul 'append' de deschidere al fisierului
                using (StreamWriter swFisierText = new StreamWriter(numeFisier, true))
                {
                    foreach (Masina s in masini)
                    {
                        swFisierText.WriteLine(s.ConversiaLaSir_PentruFisier_PentruForma());
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
        }  
    }
}
