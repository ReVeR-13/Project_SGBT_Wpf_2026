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
    public class AbriDetailViewModel : BaseViewModel, ICloseable
    {
        private Abri? _abriSelectionne;
        private string? _id;
        private string? _nom;
        private string? _description;
        private string? _message;
        private EStatutAbri? _statutSelectionne;
        private string? _title;
        private DateTime? _date;
        

        public event Action CloseFenetre;

        public ICommand AjouteCommand { get; }
        public ICommand SupprimerCommand { get; }
        public ICommand NouveauCommand { get; }

        

        public AbriDetailViewModel(Abri abri)
        {
            _statutSelectionne = abri == null ? EStatutAbri.DISPONIBLE : abri.Statut;
            _abriSelectionne = abri ?? null;
            _message = string.Empty;
            _description = abri == null ? string.Empty : abri.Description;
            _id = abri == null ?  string.Empty : abri.Id;
            _nom = abri == null ? string.Empty : abri.Libelle;
            _date = abri == null ? DateTime.Now : abri.DateCreation;

            NouveauCommand = new RelayCommand(_ => NouveauAbri(), _ => true);
            AjouteCommand = new RelayCommand(_ => AjouteOuMettreAJour(), _ => PeutEnregistrer());
            SupprimerCommand = new RelayCommand(_ => SupprimerAbri(), _ => this.AbriSelectionne != null);

            _title = abri == null? "Creation Abri": $"FICHE ABRI N° [ {abri.Id} ]";

        }

        public string? Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }
        public string? Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }
        public string? Nom
        {
            get { return _nom; }
            set { _nom = value; OnPropertyChanged(); }
        }
        public DateTime? DateCreation
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(); }
        }
        public string? Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }
        public EStatutAbri? StatutSelectionne
        {
            get { return _statutSelectionne; }
            set { _statutSelectionne = value; OnPropertyChanged(); }
        }
        public Abri? AbriSelectionne
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
                    StatutSelectionne = EStatutAbri.DISPONIBLE;
                    Message = string.Empty;
                }
            }
        }

        public Array LesStatutAbris => Enum.GetValues(typeof(EStatutAbri));

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
            StatutSelectionne = EStatutAbri.DISPONIBLE;
            Title = $"CREATION ABRI";
            Message = string.Empty;
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
                if (this.AbriSelectionne == null)
                {
                    if (Nom != null)
                    {
                        Abri nouveau = Abri.Creer(Nom, Description);
                        if (Abri.Save(nouveau) == 1)
                        {
                            AllAbri.Add(nouveau);
                            this.Message = LesMessage.SuccesAjout;
                            
                        }

                    }
                }
                else
                {
                    if (this.AbriSelectionne.Update(Nom, StatutSelectionne, Description) == 1)
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
                        AllAbri.Remove(AbriSelectionne.Id);
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
