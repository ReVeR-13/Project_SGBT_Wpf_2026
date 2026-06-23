using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Wpf_App_Pattoon_Animalerie.Commands;
using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;
using Wpf_App_Pattoon_Animalerie.ViewModel.WindowViewModel;

namespace Wpf_App_Pattoon_Animalerie.ViewModel.UserControlViewModel.Details
{
    internal class AnimalDetailViewModel:BaseViewModel
    {
        private Animal? _animal;
        private string? _idAnimal;
        private DateTime _date;
        private string? _nom;
        private TypeAnimal? _type;
        private DateTime? _dateNaissance;
        private DateTime? _dateDeces;
        private ESexe _sexe;
        private Couleur? _couleur;
        private bool _sterile;
        private DateTime? _dateSterile;
        private string? _description;
        private string? _particularite;
        private EStatutAnimal? _statue;
        private Abri? _abri;

        private string _title;
        private string _message;

        private ObservableCollection<Couleur> _lesCouleurs;

        private ObservableCollection<AnimalCouleur> _colorations;
        private AnimalCouleur? _colorationSelectionne;

        private ObservableCollection<SCompatible> _lesCompatibles;
        private SCompatible? _compatibilitésSelectionne;

        private ObservableCollection<Vaccination> _lesVaccinations;
        private Vaccination? _vaccinationSelectionne;
        private ObservableCollection<Abri> _lesAbrisDisponible;
        private ObservableCollection<TypeAnimal> _lesTypes;


        private IWindowService _windowService;
        public ICommand NouveauCommand { get; }
        public ICommand EnregistreCommand { get; }
        public ICommand DeleteCommand { get; }

        public ICommand AddVaccinationCommande { get; }
        public ICommand RemoveVaccinationCommande { get; }

        public ICommand AddColorationCommande { get; }
        public ICommand RemoveColorationCommande { get; }

        public ICommand AddCompatibliteCommande { get; }
        public ICommand RemoveCompatibliteCommande { get; }


        public ICommand AbrisCommand { get; }
        public ICommand CouleursCommand { get; }
        public ICommand TypeAnimalCommand { get; }
        public ICommand HistoriqueCommand { get; }

        public ICommand EtapeSuivantCommand { get; }

        private readonly Action<Animal>? _onSaved;
        private readonly Action<Animal>? _onCreated;
        private readonly Action<Animal>? _onDelete;

        public AnimalDetailViewModel(Animal animal, IWindowService windowService, Action<Animal>? onSaved, Action<Animal>? onCreated, Action<Animal>? onDelete)
        {
            _onSaved = onSaved;
            _onCreated = onCreated;
            _onDelete = onDelete;
            _windowService = windowService;
            _animal = animal;
            Id = animal?.Id;
            DateCreation = animal == null ? DateTime.Now : animal.DateCreation;
            Nom = animal?.Nom;
            TypeSelectionne = animal?.Type;
            DateNaissance = animal == null ? DateTime.Now : animal.DateNaissance;
            SexeSelectionne = animal == null ? ESexe.M : animal.Sexe;
            CouleurSelectionne = animal?.Couleur;
            _dateDeces = animal?.DateDeces;
            Sterile = animal != null ? animal.Sterile : false;
            DateSterilisation = animal?.DateSterilisation;
            Description = animal?.Description;
            Particularite = animal?.Particularite;
            AbriSelectionne = animal?.Abri;
            _message = string.Empty;
            _lesTypes = [];
            _lesCouleurs = [];
            _colorationSelectionne = null;
            _vaccinationSelectionne = null;
            _compatibilitésSelectionne = null;

            StatutSelectionne = animal != null ? animal.Statut : EStatutAnimal.EXAMINATION;
            Title = animal != null ? $"FICHE DE L ANIMAL N° [ {_idAnimal} ]" : "CREATION ANIMAL";

            _colorations = animal != null ? new ObservableCollection<AnimalCouleur>(AllAnimalCouleur.FindAllByAnimal(animal).Values) : [];
            _lesCompatibles = animal != null ? new ObservableCollection<SCompatible>(Animal.TraitementSCompatible(animal)) : [];
            _lesVaccinations = animal != null ? new ObservableCollection<Vaccination>(animal.EtatVaccinations().Values) : [];
            _lesAbrisDisponible = animal != null ? new ObservableCollection<Abri>(AllAbri.LesStockAbris.Values.Where(a => a.Statut == EStatutAbri.DISPONIBLE)) : [];

            if (animal != null)
            {
                _lesTypes.Add(animal.Type);
                _lesCouleurs.Add(animal.Couleur);

            }
            else
            {
                Nouveau();
            }



            NouveauCommand = new RelayCommand(_ => Nouveau(), _ => AnimalSelectionne != null );
            EnregistreCommand = new RelayCommand(_ => EnregistrerAnimal(), _ => PeutEnregistrer());
            DeleteCommand = new RelayCommand(_ => DeleteAnimal(), _ => AnimalSelectionne != null);

            TypeAnimalCommand = new RelayCommand(_ => OuvrirTypeAnimal(), _ => true);
            CouleursCommand = new RelayCommand(_ => OuvrirCouleur(), _ => true);
            AbrisCommand = new RelayCommand(_ => OuvrirAbri(), _ => true);
            EtapeSuivantCommand = new RelayCommand(_ => OuvrirDemande(), _ => AnimalSelectionne != null);

            AddVaccinationCommande = new RelayCommand(_ => AddVaccinationAnimal(), _ => AnimalSelectionne != null && AnimalSelectionne.Statut == EStatutAnimal.REFUGE);
            RemoveVaccinationCommande = new RelayCommand(_ => RemoveVaccinationAnimal(), _ => AnimalSelectionne != null && AnimalSelectionne.Statut == EStatutAnimal.REFUGE && VaccinationSelectionne != null);

            AddColorationCommande = new RelayCommand(_ => AddColorationAnimal(), _ => AnimalSelectionne != null && AnimalSelectionne.Statut == EStatutAnimal.REFUGE);
            RemoveColorationCommande = new RelayCommand(_ => RemoveColorationAnimal(), _ => AnimalSelectionne != null && AnimalSelectionne.Statut == EStatutAnimal.REFUGE && ColorationSelectionne != null);

            AddCompatibliteCommande = new RelayCommand(_ => AddcompatAnimal(), _ => AnimalSelectionne != null && AnimalSelectionne.Statut == EStatutAnimal.REFUGE);
            RemoveCompatibliteCommande = new RelayCommand(_ => RemoveCompatAnimal(), _ => AnimalSelectionne != null && AnimalSelectionne.Statut == EStatutAnimal.REFUGE && CompatibilitésSelectionne != null && peutSuppCompat());
            HistoriqueCommand = new RelayCommand(_ => HistoriqueAnimal(), _ => AnimalSelectionne != null);
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
        public string? Id
        {
            get { return _idAnimal; }
            set { _idAnimal = value;  OnPropertyChanged(); }
        }
        public DateTime DateCreation
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(); }
        }
        public TypeAnimal? TypeSelectionne
        {
            get { return _type; }
            set
            {
                _type = value; OnPropertyChanged();
            }
        }
        public EStatutAnimal? StatutSelectionne
        {
            get
            {
                return _statue;
            }
            set
            {
                _statue = value; OnPropertyChanged();
            }
        }

        public string? Nom
        {
            get { return _nom; }
            set
            {
                if (value?.Length < 2)
                {
                    Message= $"[Animal] Le nom n'est pas valide : {value}";
                }
                _nom = value?.Trim().ToUpper(); OnPropertyChanged();
            }
        }
        public Couleur? CouleurSelectionne
        {
            get { return _couleur; }
            set
            {
                _couleur = value; OnPropertyChanged();
            }
        }
        public DateTime? DateNaissance
        {
            get { return _dateNaissance; }
            set
            {
                if (value > DateTime.Now)
                {
                    Message = "Animal - Date de naissance... invalide";
                }
                _dateNaissance = value; OnPropertyChanged();
            }
        }
        public ESexe SexeSelectionne
        {
            get { return _sexe; }
            set
            {
                if (!Enum.IsDefined(typeof(ESexe), value))
                {
                    Message = "[Animal] Sexe invalide : {value}";
                }
                _sexe = value; OnPropertyChanged();
            }
        }
        public bool Sterile
        {
            get { return _sterile; }
            set { _sterile = value; OnPropertyChanged(); }
        }
        public DateTime? DateSterilisation
        {
            get { return _dateSterile; }
            set
            {

                _dateSterile = value; OnPropertyChanged();
            }
        }
        public string? Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value; OnPropertyChanged();
            }
        }
        public string? Particularite
        {
            get
            {
                return _particularite;
            }
            set
            {
                _particularite = value; OnPropertyChanged();
            }
        }
        public Abri? AbriSelectionne
        {
            get
            {
                return _abri;
            }
            set
            {
                _abri = value; OnPropertyChanged();
            }
        }

        public DateTime? DateDeces
        {
            get
            {
                return _dateDeces;
            }
            set
            {
                if (value != null && value < DateNaissance)
                {
                    Message = $"Animal - La date de deces n'est pas valide : {value}"; 
                    return;
                }
                _dateDeces = value; OnPropertyChanged();
            }
        }

        public Animal? AnimalSelectionne
        {
            get { return _animal; }
            set { _animal = value; OnPropertyChanged(); }   
        }

        public Array LesSexes
        {
            get { return Enum.GetValues(typeof(ESexe)); }
        }
        public ObservableCollection<TypeAnimal> LesTypesAnimal
        {
            get { return _lesTypes; }
            set { _lesTypes = value; OnPropertyChanged(); }
        }
        public ObservableCollection<AnimalCouleur> LesColorations
        {
            get { return _colorations; }
            set { _colorations = value; OnPropertyChanged(); }
        }
        public AnimalCouleur ColorationSelectionne
        {
            get { return _colorationSelectionne; }
            set { _colorationSelectionne = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Couleur> LesCouleurs
        {
            get { return _lesCouleurs; }
            set { _lesCouleurs = value; OnPropertyChanged(); }
        }
        public ObservableCollection<SCompatible> LesCompatibilités
        {
            get { return _lesCompatibles; }
            set { _lesCompatibles = value; OnPropertyChanged(); }
        }
        public SCompatible? CompatibilitésSelectionne
        {
            get
            {
                return _compatibilitésSelectionne;
            }
            set { _compatibilitésSelectionne = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Vaccination> LesVaccination
        {
            get { return _lesVaccinations; }
            set { _lesVaccinations = value; OnPropertyChanged(); }
        }
        public Vaccination? VaccinationSelectionne
        {
            get { return _vaccinationSelectionne; }
            set { _vaccinationSelectionne = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Abri> LesAbrisDisponibles
        {
            get { return _lesAbrisDisponible; }
            set { _lesAbrisDisponible = value; OnPropertyChanged(); }
        }

        private void Nouveau()
        {
            AnimalSelectionne = null;
            Id = string.Empty;
            DateCreation = DateTime.Now;
            TypeSelectionne = null;
            StatutSelectionne = EStatutAnimal.EXAMINATION;
            Nom = string.Empty ;
            CouleurSelectionne = null;
            DateNaissance = DateTime.Now;
            SexeSelectionne = ESexe.M;
            Sterile = false ;
            DateSterilisation = null;
            Description = string.Empty ;
            Particularite = string.Empty ;
            AbriSelectionne = null;
            Message = string.Empty ;

            Title = "CREATION ANIMAL";

            LesTypesAnimal = new ObservableCollection<TypeAnimal>(AllTypeAnimal.LesStocks.Values);
            LesCouleurs = new ObservableCollection<Couleur>(AllCouleur.LesStocks.Values);
            LesColorations = [];
            LesCompatibilités = new ObservableCollection<SCompatible>(Animal.TraitementSCompatible(AnimalSelectionne));
            LesVaccination = [];
            LesAbrisDisponibles = new ObservableCollection<Abri>(AllAbri.LesStockAbris.Values.Where(a => a.Statut == EStatutAbri.DISPONIBLE));

        }

        private bool PeutEnregistrer()
        {
            return TypeSelectionne != null
                && !string.IsNullOrEmpty(Nom)
                && CouleurSelectionne != null;
        }
        private void EnregistrerAnimal()
        {

            try
            {
                if (!Sterile)
                {
                    DateSterilisation = null;
                }


                if (AnimalSelectionne == null)
                {
                    Animal animal = Animal.Creer(Nom, TypeSelectionne.Nom, DateNaissance, SexeSelectionne.ToString(), CouleurSelectionne, Sterile, DateSterilisation, Description, Particularite);

                    if (animal != null)
                    {
                        int x = animal.Save();

                        if (x == 1)
                        {
                            Forma.MsgInfo("Animal", $"Animal creer: {animal?.Nom}");
                            AnimalSelectionne = animal;
                            _onCreated?.Invoke( animal );
                            MessageBoxResult result = Forma.MsgValidation("Message", "Voulez-vous faire l'entree de l' animal? ");

                            if (result == MessageBoxResult.Yes)
                            {
                                OuvrirDemande();
                            }
                        }
                    }
                }
                else
                {
                    Message = AnimalSelectionne.Update(TypeSelectionne,Nom,SexeSelectionne,DateNaissance,Sterile,DateSterilisation,Description, Particularite,AbriSelectionne);
                    _onSaved?.Invoke( AnimalSelectionne );
                }

            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            
        }
        private void DeleteAnimal()
        {
            try
            {
                if (AnimalSelectionne != null)
                {
                    MessageBoxResult result = Forma.MsgValidation("Message", "Voulez-vous supprimer cette element? ");

                    if (result == MessageBoxResult.Yes)
                    {
                        int x = Animal.Delete(AnimalSelectionne);
                        if (x == 1)
                        {
                            _onDelete?.Invoke( AnimalSelectionne );
                            Forma.MsgInfo("Animal", $"Animal Supprimer");
                            Nouveau();
                        }
                    }
                    
                }
            }catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        private void AddVaccinationAnimal()
        {
            try
            {
                if (AnimalSelectionne == null || AnimalSelectionne.Statut != EStatutAnimal.REFUGE)
                {
                    Message = $"Cet animal n est pas encore créé...";
                    return;
                }

                IEnumerable<Vaccin> manquants = AllVaccin.Manquants(AnimalSelectionne).Values;

                if (!manquants.Any())
                {
                    Message = $"{AnimalSelectionne.Nom} possede tous les vaccins disponibles";
                    return;
                }

                var selector = new SelectionViewModel<Vaccin>(manquants);
                Vaccin selectedRole = _windowService.OuvrirPopup(selector);

                if (selectedRole != null)
                {
                    Vaccination vaccination = Vaccination.Creer(AnimalSelectionne, selectedRole, "--");
                    if (Vaccination.Save(vaccination) == 1)
                    {
                        LesVaccination.Add(vaccination);
                        Message = $"Role Ajouté : {selectedRole.Nom}";
                    }

                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            
        }
        private void RemoveVaccinationAnimal()
        {
            try
            {
                if (VaccinationSelectionne == null)
                {
                    Message = $"Veuillez sectionner un element à supprimer";
                    return;
                }

                if (Vaccination.Delete(VaccinationSelectionne) == 1)
                {
                    LesVaccination.Remove(VaccinationSelectionne);
                }
            }
            catch ( Exception ex)
            {
                Message = ex.Message;
            }
            
            
        }
        private void AddColorationAnimal()
        {
            try
            {
                if (AnimalSelectionne == null || AnimalSelectionne.Statut != EStatutAnimal.REFUGE)
                {
                    Message = $"Cet animal n est pas au  refuge...";
                    return;
                }

                IEnumerable<Couleur> manquants = AllCouleur.Manquants(AnimalSelectionne).Values;

                if (!manquants.Any())
                {
                    Message = $"{AnimalSelectionne.Nom} possede tous les couleurs disponibles";
                    return;
                }

                var selector = new SelectionViewModel<Couleur>(manquants);
                Couleur selectedRole = _windowService.OuvrirPopup(selector);

                if (selectedRole != null)
                {
                    AnimalCouleur animalCouleur = AnimalCouleur.Creer(AnimalSelectionne, selectedRole);
                    if (AnimalCouleur.Save(animalCouleur) == 1)
                    {
                        LesColorations.Add(animalCouleur);
                        Message = $"Role Ajouté : {selectedRole.Nom}";
                    }

                }
            }catch ( Exception ex )
            {
                Message = ex.Message;
            }
            
        }
        private void RemoveColorationAnimal()
        {
            try
            {
                if (ColorationSelectionne == null)
                {
                    Message = $"Veuillez selecctionner un element à supprimer";
                    return;
                }

                if (ColorationSelectionne.Delete() == 1)
                {
                    LesColorations.Remove(ColorationSelectionne);
                }
            }catch( Exception ex )
            {
                Message = ex.Message;
            }
            
            
        }

        private bool peutSuppCompat()
        {
            AnimalCompatibilité cmpSelected = AnimalSelectionne.Compatible(CompatibilitésSelectionne.Value._compatibilite);
            return cmpSelected != null;
        }
        private void AddcompatAnimal()
        {
            try
            {
                if (AnimalSelectionne == null || AnimalSelectionne.Statut != EStatutAnimal.REFUGE )
                {
                    Message = "Cet animal n'est pas au refuge";
                    return;
                }
                IEnumerable<Compatibilite> manquants = AllCompatibilite.Manquants(AnimalSelectionne).Values;

                if (!manquants.Any())
                {
                    Message = $"Tous les compatibilités disponibles de {AnimalSelectionne.Nom} ont éte verifiés ";
                    return;
                }

                var selector = new SelectionViewModel<Compatibilite>(manquants);
                Compatibilite selectedRole = _windowService.OuvrirPopup(selector);

                if (selectedRole != null)
                {

                    MessageBoxResult result = Forma.MsgValidation("Compatibilité", $"{AnimalSelectionne.Nom} est-il compatible avec : {selectedRole.Nom}");

                    bool compa = result == MessageBoxResult.Yes;
                    AnimalCompatibilité animalCompatibilité = AnimalCompatibilité.Creer(AnimalSelectionne, selectedRole, compa,"--");
                    if (AnimalCompatibilité.Save(animalCompatibilité) == 1)
                    {

                        LesCompatibilités = new ObservableCollection<SCompatible>(Animal.TraitementSCompatible(AnimalSelectionne));
                        Message = $"Compatibilité Ajoutée : {selectedRole.Nom}";
                    }

                }

            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }
        private void RemoveCompatAnimal()
        {
            try
            {
                if (CompatibilitésSelectionne == null)
                {
                    Message = $"Veuillez selectionner un element";
                    return;
                }

                AnimalCompatibilité cmpSelected = AnimalSelectionne.Compatible( CompatibilitésSelectionne.Value._compatibilite);
                if (cmpSelected == null)
                {
                    Message = $"cette compatibilité n'est pas verifier";
                    return;
                }

                if (AnimalCompatibilité.Delete(cmpSelected) == 1)
                {
                    LesCompatibilités = new ObservableCollection<SCompatible>(Animal.TraitementSCompatible(AnimalSelectionne));
                    Message = $"Compatibilité retirée : {CompatibilitésSelectionne.Value._compatibilite.Nom}";
                }

            }
            catch (Exception ex)
            {
                Message=ex.Message;
            }
        }


        private void OuvrirTypeAnimal()
        {
            var vm = new Page2ViewModel(_windowService);
            _windowService.OuvrirDetail(vm);
        }
        private void OuvrirCouleur()
        {
            var vm = new CouleurViewModel(_windowService);
            _windowService.OuvrirDetail(vm);
        }
        private void OuvrirAbri()
        {
            var vm = new Page1ViewModel(_windowService);
            _windowService.OuvrirDetail(vm);
        }
        private void OuvrirDemande()
        {
            var vm = new DemandeDetailViewModel(null,null,AnimalSelectionne,_windowService,null,null);
            _windowService.OuvrirDetail(vm);
        }

       private void HistoriqueAnimal()
       {
            var vm = new HistoriqueAnimalViewModel(AnimalSelectionne);
            _windowService.OuvrirDetail(vm);
        }

    }
}
