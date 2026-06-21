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
    public class CouleurDetailViewModel:BaseViewModel
    {
        private Couleur? _couleurSelectionne;
        private string? _id;
        private DateTime _date;
        private string? _nom;
        private string? _message;
        private string? _title;

        public ICommand AjouteCommand { get; }
        public ICommand SupprimerCommand { get; }
        public ICommand NouveauCommand { get; }

        public CouleurDetailViewModel(Couleur? couleur)
        {
            _couleurSelectionne = couleur;
            _id = couleur != null ? couleur.Id : string.Empty;
            _date = couleur != null ? couleur.DateCreation : DateTime.Now;
            _nom = couleur != null ? couleur.Nom : string.Empty;
            _message = string.Empty;

            _title = couleur == null ? "CREATION DE COULEUR" : $"FICHE DE LA COULEUR N° [ {couleur.Id} ]";

            NouveauCommand = new RelayCommand(_ => NouveauCouleur(), _ => true);
            AjouteCommand = new RelayCommand(_ => AjouteOuMettreAJour(), _ => PeutEnregistrer());
            SupprimerCommand = new RelayCommand(_ => SupprimerCouleur(), _ => this.CouleurSelectionne != null);

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
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(); }
        }
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(); }
        }
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }
        public Couleur CouleurSelectionne
        {
            get { return _couleurSelectionne; }
            set
            {
                _couleurSelectionne = value;
                OnPropertyChanged();

                if (value != null)
                {
                    Id = value.Id;
                    Nom = value.Nom;
                    Date = value.DateCreation;
                    Message = string.Empty;
                }
            }
        }

        private void NouveauCouleur()
        {
            CouleurSelectionne = null;
            Nom = string.Empty;
            Date = DateTime.Now;
            Id = string.Empty;
            Message = string.Empty;
            Title = "CREATION DE COULEUR";
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
                if (this.CouleurSelectionne == null)
                {
                    if (Nom != null)
                    {
                        Couleur nouveau = Couleur.Creer(Nom);
                        if (Couleur.Save(nouveau) == 1)
                        {
                            AllCouleur.DB_Sync();
                            this.Message = LesMessage.SuccesAjout;
                            
                        }

                    }
                }
                else
                {
                    if (this.CouleurSelectionne.Update(Nom) != null)
                    {
                        AllCouleur.DB_Sync();
                        this.Message = LesMessage.SuccesMaj;
                    }

                }
                NouveauCouleur();
            }
            catch (Exception ex)
            {
                this.Message = "[Erreur] : " + ex.Message;
            }


        }
        private void SupprimerCouleur()
        {
            Message = LesMessage.ErrorSuppression;

            if (this.CouleurSelectionne != null)
            {
                try
                {
                    if (Couleur.Delete(this.CouleurSelectionne) == 1)
                    {
                        AllCouleur.DB_Sync();

                        Message = LesMessage.SuccesSuppression;

                        NouveauCouleur();
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
