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
    public class TypeAnimalDetailViewModel:BaseViewModel,ICloseable
    {
        private TypeAnimal _typeSelectionne;
        private string _id;
        private string _nom;
        private string _description;
        private string _message;
        private string _title;

        private ObservableCollection<TypeAnimal> _lesTypeAnimal;

        public event Action CloseFenetre;

        public ICommand AjouteCommand { get; }
        public ICommand SupprimerCommand { get; }
        public ICommand NouveauCommand { get; }
        public TypeAnimalDetailViewModel(TypeAnimal type)
        {
            _lesTypeAnimal = [];
            _typeSelectionne = type;

            _id = type == null ? string.Empty : type.Id;
            _nom = type == null ? string.Empty : type.Nom;
            _description = type == null ? string.Empty : type.Description;
            _message = string.Empty;

            NouveauCommand = new RelayCommand(_ => NouveauTypeAnimal(), _ => true);
            AjouteCommand = new RelayCommand(_ => AjouteOuMettreAJour(), _ => PeutEnregistrer());
            SupprimerCommand = new RelayCommand(_ => SupprimerTypeAnimal(), _ => this.TypeSelectionne != null);

            if (type != null)
            {
                _lesTypeAnimal.Add(type);
                _title = $"FICHE TYPE-CONTACT N° [ {_id} ]";
            }
            else
            {
                _lesTypeAnimal = new ObservableCollection<TypeAnimal>(AllTypeAnimal.LesStocks.Values);
                _title = $"NOUVEAU TYPE";
            }

        }

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
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
                    Title = $"FICHE TYPE-CONTACT N° [ {value.Id} ]";
                }
            }
        }

        public ObservableCollection<TypeAnimal> LesTypes
        {
            get { return _lesTypeAnimal; }
            set { _lesTypeAnimal = value; OnPropertyChanged(); }
        }


        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        private void NouveauTypeAnimal()
        {
            LesTypes = new ObservableCollection<TypeAnimal>(AllTypeAnimal.LesStocks.Values);
            TypeSelectionne = null;
            Nom = string.Empty;
            Description = string.Empty;
            Id = string.Empty;
            Title = $"NOUVEAU TYPE";
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
                            Forma.SyncAllWithDB();
                            this.Message = LesMessage.SuccesAjout;
                            LesTypes.Add(nouveau);
                        }

                    }
                }
                else
                {
                    if (this.TypeSelectionne.Update(Nom, Description) == 1)
                    {
                        Forma.SyncAllWithDB();
                        this.Message = LesMessage.SuccesMaj;
                    }

                }
                NouveauTypeAnimal();
            }
            catch (Exception ex)
            {
                this.Message = "[Erreur] : " + ex.Message;
            }
        }
        private void SupprimerTypeAnimal()
        {
            Message = LesMessage.ErrorSuppression;

            if (this.TypeSelectionne != null)
            {
                try
                {
                    if (TypeAnimal.Delete(this.TypeSelectionne) == 1)
                    {
                        Forma.SyncAllWithDB();
                        LesTypes.Remove(TypeSelectionne);

                        Message = LesMessage.SuccesSuppression;

                        NouveauTypeAnimal();
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
