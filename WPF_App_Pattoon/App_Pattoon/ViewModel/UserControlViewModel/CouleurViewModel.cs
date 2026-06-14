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
    public class CouleurViewModel : BaseViewModel
    {
        private Couleur _couleurSelectionne;
        private string _id;
        private string _nom;
        private string _message;

        private ObservableCollection<Couleur> _lesCouleurs;

        public ICommand AjouteCommand { get; }
        public ICommand SupprimerCommand { get; }
        public ICommand NouveauCommand { get; }

        public CouleurViewModel()
        {
            _lesCouleurs = new ObservableCollection<Couleur>(AllCouleur.LesStocks.Values);
            _message = string.Empty;
            _id = string.Empty;
            _nom = string.Empty;

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
                    Message = string.Empty;
                }
            }
        }

        public ObservableCollection<Couleur> LesCouleur
        {
            get { return _lesCouleurs; }
        }


        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        private void NouveauCouleur()
        {
            CouleurSelectionne = null;
            Nom = string.Empty;
            Id = string.Empty;
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
                            LesCouleur.Add(nouveau);
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
                        LesCouleur.Remove(CouleurSelectionne);

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
