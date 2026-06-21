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

namespace Wpf_App_Pattoon_Animalerie.ViewModel.UserControlViewModel.Details
{
    public class VaccinDetailViewModel:BaseViewModel, ICloseable
    {
        private Vaccin? _vaccinSelectionne;
        private string? _id;
        private DateTime _date;
        private string? _nom;
        private string? _description;
        private string? _message;
        private string? _title;

        public event Action CloseFenetre;

        public ICommand AjouteCommand { get; }
        public ICommand SupprimerCommand { get; }
        public ICommand NouveauCommand { get; }
        public VaccinDetailViewModel(Vaccin? vaccin)
        {
            _vaccinSelectionne = vaccin;
            _id = vaccin?.Id;
            Date = vaccin?.DateCreation;
            Nom = vaccin?.Nom;
            Description = vaccin?.Description;
            _message = string.Empty; 
            Title = vaccin == null ?"CREATION DE VACCIN": $"FICHE VACCIN DE [ {vaccin.Id} ]";

            NouveauCommand = new RelayCommand(_ => NouveauVaccin(), _ => true);
            AjouteCommand = new RelayCommand(_ => AjouteOuMettreAJour(), _ => PeutEnregistrer());
            SupprimerCommand = new RelayCommand(_ => SupprimerVaccin(), _ => this.VaccinSelectionne != null);
            
        }

        public Vaccin VaccinSelectionne
        {
            get { return _vaccinSelectionne; }
            set { _vaccinSelectionne = value; OnPropertyChanged(); }
        }
        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }
        public DateTime? Date
        {
            get { return _date; }
            set { _date = value ?? DateTime.Now; OnPropertyChanged();}
        }
        public string? Nom
        {
            get { return _nom; }
            set { _nom = value; OnPropertyChanged(); }
        }
        public string? Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }

        public string? Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(); }
        }
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }

        private void NouveauVaccin()
        {
            VaccinSelectionne = null;
            Nom = string.Empty;
            Id = string.Empty;
            Date = DateTime.Now;
            Message = string.Empty;
            Description = string.Empty;
            Title = "CREATION DE VACCIN";
        }
        private bool PeutEnregistrer()
        {
            return !string.IsNullOrWhiteSpace(Nom);
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
    }
}
