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
    class Page1ViewModel : BaseViewModel
    {
        private Abri _abriSelectionne;
        private string _id;
        private string _nom;
        private string _description;
        private string _message;
        private EStatutAbri _statut;

        private ObservableCollection<Abri> _lesAbris;

        public ICommand AjouteCommand { get; }
        public ICommand SupprimerCommand { get; }
        public ICommand NouveauCommand { get; }


        public Page1ViewModel()
        {
            _lesAbris = new ObservableCollection<Abri>(AllAbri.LesStockAbris.Values);
            _statut = EStatutAbri.DISPONIBLE;
            _message = string.Empty;
            _description = string.Empty;
            _id = string.Empty;
            _nom = string.Empty;

            NouveauCommand = new RelayCommand(_ => NouveauAbri(), _ => true);
            AjouteCommand = new RelayCommand(_ => AjouteOuMettreAJour(), _ => PeutEnregistrer());
            SupprimerCommand = new RelayCommand(_ => SupprimerAbri(), _ => this.AbriSelectionne != null);

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
        public EStatutAbri Statut
        {
            get { return _statut; }
            set { _statut = value; OnPropertyChanged(); }
        }
        public Abri AbriSelectionne
        {
            get { return _abriSelectionne; }
            set
            {
                _abriSelectionne = value;
                OnPropertyChanged();

                if (value != null)
                {
                    Id = value.Id;
                    Nom = value.Libelle;
                    Description = value.Description;
                    Statut = EStatutAbri.DISPONIBLE;
                    Message = string.Empty;
                }
            }
        }

        public Array LesStatutAbris => Enum.GetValues(typeof(EStatutAbri));

        public ObservableCollection<Abri> LesAbris { get => _lesAbris; }

        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        private void NouveauAbri()
        {
            AbriSelectionne = null;
            Nom = string.Empty;
            Id = string.Empty;
            Description = string.Empty;
            Statut = EStatutAbri.DISPONIBLE;
        }
        private bool PeutEnregistrer()
        {
            return !string.IsNullOrWhiteSpace(Nom)
                && !string.IsNullOrWhiteSpace(Description)
                && Statut != null;
        }
        private void AjouteOuMettreAJour()
        {
            this.Message = LesMessage.ErrorAjout;

            try
            {
                if (this.AbriSelectionne == null)
                {
                    if (Nom != null)
                    {
                        Abri nouveau = Abri.Creer(Nom, Description);
                        if (Abri.Save(nouveau) == 1)
                        {
                            AllAbri.DB_Sync();
                            this.Message = LesMessage.SuccesAjout;
                            LesAbris.Add(nouveau);
                        }

                    }
                }
                else
                {
                    if (this.AbriSelectionne.Update(Nom, Statut, Description) == 1)
                    {
                        this.Message = LesMessage.SuccesMaj;
                    }

                }
                NouveauAbri();
            }
            catch (Exception ex)
            {
                this.Message = "[Erreur] : " + ex.Message;
            }


        }
        private void SupprimerAbri()
        {
            Message = LesMessage.ErrorSuppression;

            if (this.AbriSelectionne != null)
            {
                try
                {
                    if (Abri.Delete(this.AbriSelectionne) == 1)
                    {
                        AllAbri.DB_Sync();
                        LesAbris.Remove(AbriSelectionne);

                        Message = LesMessage.SuccesSuppression;

                        NouveauAbri();
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
