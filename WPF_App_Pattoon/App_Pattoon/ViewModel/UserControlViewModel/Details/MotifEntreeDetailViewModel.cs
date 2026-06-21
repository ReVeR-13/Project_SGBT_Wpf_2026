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
    public class MotifEntreeDetailViewModel:BaseViewModel
    {

        private MotifEntree? _motifSelectionne;
        private string? _id;
        private DateTime _date;
        private string? _nom;
        private string _description;
        private string _message;
        private string _title;

        public ICommand AjouteCommand { get; }
        public ICommand SupprimerCommand { get; }
        public ICommand NouveauCommand { get; }

        public MotifEntreeDetailViewModel(MotifEntree motifEntree)
        {
            _motifSelectionne = motifEntree;
            _message = string.Empty;
            _id =motifEntree != null ? motifEntree.Id: string.Empty;
            _date = motifEntree != null ? motifEntree.DateCreation : DateTime.Now;
            _nom = motifEntree != null ? motifEntree.Libele : string.Empty;
            _description = motifEntree != null ? motifEntree.Details : string.Empty;

            _title = motifEntree != null ? $"FICHE DE MOTIF N° [ {motifEntree.Id} ]"  : "CREATION DE MOTIF";

            NouveauCommand = new RelayCommand(_ => NouveauMotifEntree(), _ => true);
            AjouteCommand = new RelayCommand(_ => AjouteOuMettreAJour(), _ => PeutEnregistrer());
            SupprimerCommand = new RelayCommand(_ => SupprimerMotifEntree(), _ => this.MotifSelectionne != null);

        }

        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(); }
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
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }
        public MotifEntree MotifSelectionne
        {
            get { return _motifSelectionne; }
            set
            {
                _motifSelectionne = value;
                OnPropertyChanged();

                if (value != null)
                {
                    Id = value.Id;
                    Nom = value.Libele;
                    Description = value.Details;
                    Message = string.Empty;
                }
            }
        }

        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        private void NouveauMotifEntree()
        {
            MotifSelectionne = null;
            Nom = string.Empty;
            Description = string.Empty;
            Id = string.Empty;
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
                if (this.MotifSelectionne == null)
                {
                    if (Nom != null)
                    {
                        MotifEntree nouveau = MotifEntree.Creer(Nom, Description);
                        if (MotifEntree.Save(nouveau) == 1)
                        {
                            AllMotifsEntrees.DB_Sync();
                            this.Message = LesMessage.SuccesAjout;
                        }

                    }
                }
                else
                {
                    if (this.MotifSelectionne.Update(Nom, Description) == 1)
                    {
                        AllMotifsEntrees.DB_Sync();
                        this.Message = LesMessage.SuccesMaj;
                    }

                }
                NouveauMotifEntree();
            }
            catch (Exception ex)
            {
                this.Message = "[Erreur] : " + ex.Message;
            }


        }
        private void SupprimerMotifEntree()
        {
            Message = LesMessage.ErrorSuppression;

            if (this.MotifSelectionne != null)
            {
                try
                {
                    if (MotifEntree.Delete(this.MotifSelectionne) == 1)
                    {
                        AllMotifsEntrees.DB_Sync();

                        Message = LesMessage.SuccesSuppression;

                        NouveauMotifEntree();
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
