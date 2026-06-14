using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf_App_Pattoon_Animalerie.Commands;
using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;

namespace Wpf_App_Pattoon_Animalerie.ViewModel.UserControlViewModel
{
    public class VaccinViewModel : BaseViewModel
    {
        private Vaccin _vaccinSelectionne;
        private string _id;
        private string _nom;
        private string _description;
        private string _message;
        private bool _panneau;

        private ObservableCollection<Vaccin> _lesVaccins;

        public ICommand AjouteCommand { get; }
        public ICommand SupprimerCommand { get; }
        public ICommand NouveauCommand { get; }
        public ICommand CloseDetailCommand { get; }

        public VaccinViewModel()
        {
            _lesVaccins = new ObservableCollection<Vaccin>(AllVaccin.StockVaccins.Values);
            _panneau = false;
            _message = string.Empty;
            _description = string.Empty;
            _id = string.Empty;
            _nom = string.Empty;

            NouveauCommand = new RelayCommand(_ => NouveauVaccin(), _ => true);
            AjouteCommand = new RelayCommand(_ => AjouteOuMettreAJour(), _ => PeutEnregistrer());
            SupprimerCommand = new RelayCommand(_ => SupprimerVaccin(), _ => this.VaccinSelectionne != null);
            CloseDetailCommand = new RelayCommand(_ => CloseDetail(), _ => true);


        }

        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }
        public string Nom
        {
            get { return _nom; }
            set { _nom = value; OnPropertyChanged(); }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }
        public Vaccin VaccinSelectionne
        {
            get { return _vaccinSelectionne; }
            set
            {
                _vaccinSelectionne = value;
                OnPropertyChanged();

                if (value != null)
                {
                    Panneau = true;
                    Id = value.Id;
                    Nom = value.Nom;
                    Description = value.Description;
                    Message = string.Empty;
                }
            }
        }
        public bool Panneau
        {
            get { return _panneau; }
            set { _panneau = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Vaccin> LesVaccins
        {
            get { return _lesVaccins; }
        }


        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        private void NouveauVaccin()
        {
            VaccinSelectionne = null;
            Nom = string.Empty;
            Id = string.Empty;
            Description = string.Empty;
        }
        private bool PeutEnregistrer()
        {
            return !string.IsNullOrWhiteSpace(Nom)
                && !string.IsNullOrWhiteSpace(Description);
        }
        private void AjouteOuMettreAJour()
        {
            this.Message = LesMessage.ErrorAjout;

            try
            {
                if (this.VaccinSelectionne == null)
                {
                    if (Nom != null)
                    {
                        Vaccin nouveau = Vaccin.Creer(Nom, Description);
                        if (Vaccin.Save(nouveau) == 1)
                        {
                            this.Message = LesMessage.SuccesAjout;
                            LesVaccins.Add(nouveau);
                        }

                    }
                }
                else
                {
                    if (this.VaccinSelectionne.Modifier(Nom, Description) == 1)
                    {
                        this.Message = LesMessage.SuccesMaj;
                    }

                }
                NouveauVaccin();
            }
            catch (Exception ex)
            {
                this.Message = "[Erreur] : " + ex.Message;
            }


        }
        private void SupprimerVaccin()
        {
            Message = LesMessage.ErrorSuppression;

            if (this.VaccinSelectionne != null)
            {
                try
                {
                    if (Vaccin.Delete(this.VaccinSelectionne) == 1)
                    {
                        AllVaccin.DB_Sync();
                        LesVaccins.Remove(VaccinSelectionne);

                        Message = LesMessage.SuccesSuppression;

                        NouveauVaccin();
                    }
                }
                catch (Exception ex)
                {
                    this.Message = "[Erreur] : " + ex.Message;
                }

            }
        }

        private void CloseDetail()
        {
            Panneau = false;
            VaccinSelectionne = null;
        }
    }
}
