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
    public class TypeContactViewModel : BaseViewModel
    {
        private TypeContact _typeSelectionne;
        private string _id;
        private string _nom;
        private string _description;
        private string _message;

        private ObservableCollection<TypeContact> _lesTypeContact;
        private IWindowService _windowService;

        public ICommand AjouteCommand { get; }
        public ICommand SupprimerCommand { get; }
        public ICommand NouveauCommand { get; }
        public ICommand OuvrirDetailCommand { get; }

        public TypeContactViewModel(IWindowService windowService)
        {
            _windowService = windowService;

            _lesTypeContact = new ObservableCollection<TypeContact>(AllTypeContact.LesStocks.Values);
            _message = string.Empty;
            _id = string.Empty;
            _nom = string.Empty;
            _description = string.Empty;

            NouveauCommand = new RelayCommand(_ => NouveauTypeContact(), _ => true);
            AjouteCommand = new RelayCommand(_ => AjouteOuMettreAJour(), _ => PeutEnregistrer());
            SupprimerCommand = new RelayCommand(_ => SupprimerTypeContact(), _ => this.TypeSelectionne != null);
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
        public TypeContact TypeSelectionne
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

        public ObservableCollection<TypeContact> LesTypes
        {
            get { return _lesTypeContact; }
            set { _lesTypeContact = value; OnPropertyChanged(); }
        }


        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        private void NouveauTypeContact()
        {
            TypeSelectionne = null;
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
                if (this.TypeSelectionne == null)
                {
                    if (Nom != null)
                    {
                        TypeContact nouveau = TypeContact.Creer(Nom, Description);
                        if (TypeContact.Save(nouveau) == 1)
                        {
                            this.Message = LesMessage.SuccesAjout;
                            LesTypes = new ObservableCollection<TypeContact>(AllTypeContact.LesStocks.Values);
                        }

                    }
                }
                else
                {
                    if (this.TypeSelectionne.Update(Nom, Description) == 1)
                    {
                        AllTypeContact.DB_Sync();
                        this.Message = LesMessage.SuccesMaj;
                    }

                }
                NouveauTypeContact();
            }
            catch (Exception ex)
            {
                this.Message = "[Erreur] : " + ex.Message;
            }


        }
        private void SupprimerTypeContact()
        {
            Message = LesMessage.ErrorSuppression;

            if (this.TypeSelectionne != null)
            {
                try
                {
                    if (TypeContact.Delete(this.TypeSelectionne) == 1)
                    {
                        AllTypeContact.DB_Sync();
                        LesTypes.Remove(TypeSelectionne);

                        Message = LesMessage.SuccesSuppression;

                        NouveauTypeContact();
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
            var vm = new TypeContactDetailViewModel(TypeSelectionne);
            _windowService.OuvrirDetail(vm);

        }
    }
}
