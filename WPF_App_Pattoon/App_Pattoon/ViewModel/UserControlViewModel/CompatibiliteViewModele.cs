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
    public class CompatibiliteViewModele : BaseViewModel
    {
        private Compatibilite _compatibiliteSelectionne;
        private string _id;
        private string _nom;
        private string _description;
        private string _message;

        private ObservableCollection<Compatibilite> _lesCompatibilites;

        public ICommand AjouteCommand { get; }
        public ICommand SupprimerCommand { get; }
        public ICommand NouveauCommand { get; }

        public CompatibiliteViewModele()
        {
            _lesCompatibilites = new ObservableCollection<Compatibilite>(AllCompatibilite.LesStocks.Values);
            _message = string.Empty;
            _description = string.Empty;
            _id = string.Empty;
            _nom = string.Empty;

            NouveauCommand = new RelayCommand(_ => NouveauCompatibilite(), _ => true);
            AjouteCommand = new RelayCommand(_ => AjouteOuMettreAJour(), _ => PeutEnregistrer());
            SupprimerCommand = new RelayCommand(_ => SupprimerCompatibilite(), _ => this.CompatibiliteSelectionne != null);


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

        public ObservableCollection<Compatibilite> LesCompatibilite
        {
            get { return _lesCompatibilites; }
        }


        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        private void NouveauCompatibilite()
        {
            CompatibiliteSelectionne = null;
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
                if (this.CompatibiliteSelectionne == null)
                {
                    if (Nom != null)
                    {
                        Compatibilite nouveau = Compatibilite.Creer(Nom, Description);
                        if (Compatibilite.Save(nouveau) == 1)
                        {
                            AllCompatibilite.DB_Sync();
                            this.Message = LesMessage.SuccesAjout;
                            LesCompatibilite.Add(nouveau);
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
                        LesCompatibilite.Remove(CompatibiliteSelectionne);

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
