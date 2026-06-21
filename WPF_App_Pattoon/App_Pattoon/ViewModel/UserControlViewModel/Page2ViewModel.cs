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
using Wpf_App_Pattoon_Animalerie.ViewModel.UserControlViewModel.Details;

namespace Wpf_App_Pattoon_Animalerie.ViewModel.UserControlViewModel
{
    class Page2ViewModel : BaseViewModel
    {
        private TypeAnimal _typeSelectionne;
        private string _id;
        private string _nom;
        private string _description;
        private string _message;

        private ObservableCollection<TypeAnimal> _lesTypes;
        IWindowService _windowService;
        public ICommand AjouteCommand { get; }
        public ICommand SupprimerCommand { get; }
        public ICommand NouveauCommand { get; }
        public ICommand OuvrirDetailCommand { get; }

        public Page2ViewModel(IWindowService windowService)
        {
            _windowService = windowService;
            _lesTypes = new ObservableCollection<TypeAnimal>(AllTypeAnimal.LesStocks.Values);

            _message = string.Empty;
            _description = string.Empty;
            _id = string.Empty;
            _nom = string.Empty;

            NouveauCommand = new RelayCommand(_ => NouveauType(), _ => true);
            AjouteCommand = new RelayCommand(_ => AjouteOuMettreAJour(), _ => PeutEnregistrer());
            SupprimerCommand = new RelayCommand(_ => SupprimerType(), _ => this.TypeSelectionne != null);
            OuvrirDetailCommand = new RelayCommand(_ => OuvrirDetail(), _ => this.TypeSelectionne != null);

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
        public TypeAnimal TypeSelectionne
        {
            get { return _typeSelectionne; }
            set
            {
                _typeSelectionne = value;
                OnPropertyChanged();

                if (value != null)
                {
                    Id = value.Id;
                    Nom = value.Nom;
                    Description = value.Description;
                    Message = string.Empty;
                }
            }
        }

        public ObservableCollection<TypeAnimal> LesTypes
        {
            get { return _lesTypes; }
        }

        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        private void NouveauType()
        {
            TypeSelectionne = null;
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
                if (this.TypeSelectionne == null)
                {
                    if (Nom != null)
                    {
                        TypeAnimal nouveau = TypeAnimal.Creer(Nom, Description);
                        if (TypeAnimal.Save(nouveau) == 1)
                        {
                            AllTypeAnimal.DB_Sync();
                            this.Message = LesMessage.SuccesAjout;
                            LesTypes.Add(nouveau);
                        }

                    }
                }
                else
                {
                    if (this.TypeSelectionne.Update(Nom, Description) == 1)
                    {
                        AllTypeAnimal.DB_Sync();
                        this.Message = LesMessage.SuccesMaj;
                    }

                }
                NouveauType();
            }
            catch (Exception ex)
            {
                this.Message = "[Erreur] : " + ex.Message;
            }


        }
        private void SupprimerType()
        {
            Message = LesMessage.ErrorSuppression;

            if (this.TypeSelectionne != null)
            {
                try
                {
                    if (TypeAnimal.Delete(this.TypeSelectionne) == 1)
                    {
                        AllTypeAnimal.DB_Sync();
                        LesTypes.Remove(TypeSelectionne);

                        Message = LesMessage.SuccesSuppression;

                        NouveauType();
                    }
                }
                catch (Exception ex)
                {
                    this.Message = "[Erreur] : " + ex.Message;
                }

            }
        }

        private void OuvrirDetail()
        {
            var vm = new TypeAnimalDetailViewModel(TypeSelectionne);
            _windowService.OuvrirDetail(vm);

        }
    }
}
