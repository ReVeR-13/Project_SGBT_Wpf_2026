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
    public class CompatibiliteDetailViewModel:BaseViewModel
    {
        private Compatibilite? _compatibiliteSelectionne;
        private string? _id;
        private string? _nom;
        private DateTime _date;
        private string? _description;
        private string? _message;
        private string? _title;

        public ICommand AjouteCommand { get; }
        public ICommand SupprimerCommand { get; }
        public ICommand NouveauCommand { get; }
        public CompatibiliteDetailViewModel(Compatibilite? compatibilite)
        {
            _compatibiliteSelectionne = compatibilite;
            _id = compatibilite?.Id;
            _nom = compatibilite?.Nom;
            _date = compatibilite == null? DateTime.Now: compatibilite.DateCreation;
            _description = compatibilite?.Details;
            _message = string.Empty;

            _title = compatibilite == null ? "CREATION DE COMPATIBILITE": $"FICHE DE LA COMPATIBILITE N° [ {compatibilite.Id} ]";

            NouveauCommand = new RelayCommand(_ => NouveauCompatibilite(), _ => true);
            AjouteCommand = new RelayCommand(_ => AjouteOuMettreAJour(), _ => PeutEnregistrer());
            SupprimerCommand = new RelayCommand(_ => SupprimerCompatibilite(), _ => this.CompatibiliteSelectionne != null);

        }

        public string? Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }
        public string Nom
        {
            get { return _nom; }
            set { _nom = value; OnPropertyChanged(); }
        }
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(); }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(); }
        }
        public Compatibilite CompatibiliteSelectionne
        {
            get { return _compatibiliteSelectionne; }
            set
            {
                _compatibiliteSelectionne = value;
                OnPropertyChanged();

                if (value != null)
                {
                    Id = value.Id;
                    Nom = value.Nom;
                    Description = value.Details;
                    Message = string.Empty;
                }
            }
        }

        private void NouveauCompatibilite()
        {
            CompatibiliteSelectionne = null;
            Nom = string.Empty;
            Description = string.Empty;
            Date = DateTime.Now;
            Id = string.Empty;
            Description = string.Empty;
            Title = "CREATION DE COMPTIBILITE";
        }
        private bool PeutEnregistrer()
        {
            return !string.IsNullOrWhiteSpace(Nom)
                ;
        }
        private void AjouteOuMettreAJour()
        {
            this.Message = LesMessage.ErrorAjout;

            try
            {
                if (this.CompatibiliteSelectionne == null)
                {
                    if (Nom != null)
                    {
                        Compatibilite nouveau = Compatibilite.Creer(Nom, Description);
                        if (Compatibilite.Save(nouveau) == 1)
                        {
                            AllCompatibilite.DB_Sync();
                            this.Message = LesMessage.SuccesAjout;
                        }

                    }
                }
                else
                {
                    if (this.CompatibiliteSelectionne.Update(Nom, Description) == 1)
                    {
                        AllCompatibilite.DB_Sync();
                        this.Message = LesMessage.SuccesMaj;
                    }

                }
                NouveauCompatibilite();
            }
            catch (Exception ex)
            {
                this.Message = "[Erreur] : " + ex.Message;
            }


        }
        private void SupprimerCompatibilite()
        {
            Message = LesMessage.ErrorSuppression;

            if (this.CompatibiliteSelectionne != null)
            {
                try
                {
                    if (Compatibilite.Delete(this.CompatibiliteSelectionne) == 1)
                    {
                        AllCompatibilite.DB_Sync();

                        Message = LesMessage.SuccesSuppression;

                        NouveauCompatibilite();
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
