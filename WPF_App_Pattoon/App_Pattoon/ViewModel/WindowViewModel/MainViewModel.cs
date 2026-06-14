using System.Windows.Input;
using Wpf_App_Pattoon_Animalerie.AccessDB;
using Wpf_App_Pattoon_Animalerie.Commands;
using Wpf_App_Pattoon_Animalerie.ViewModel.UserControlViewModel;

namespace Wpf_App_Pattoon_Animalerie.ViewModel.WindowViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private object _vueCourrante;
        public object VueCourrante
        {
            get => _vueCourrante;
            set
            {
                _vueCourrante = value;
                OnPropertyChanged();
            }
        }

        //---------------------------------------------------ICommande
        public ICommand AfficherVaccinsCommand { get; }
        public ICommand ShowPage1Commande { get; }
        public ICommand ShowPage2Commande { get; }
        public ICommand CompatibiliteCommande { get; }
        public ICommand CouleurCommande { get; }
        public ICommand TypeContactCommande { get; }
        public ICommand MotifEntreeCommande { get; }
        public ICommand MotifSortieCommande { get; }


        public ICommand DemandeCommande { get; }
        public ICommand AnimalCommande { get; }
        public ICommand ContactCommande { get; }
        public ICommand OptionCommande { get; }


        public ICommand EntreeCommande { get; }
        public ICommand SortieCommande { get; }



        //---------------------------------------------------

        public MainViewModel()
        {
            FullDBSync();

            WindowService windowService = new WindowService();

            AfficherVaccinsCommand = new RelayCommand(_ => VueCourrante = new VaccinViewModel(), _ => true);
            ShowPage1Commande      = new RelayCommand(_ => VueCourrante = new Page1ViewModel(), _ => true);
            ShowPage2Commande      = new RelayCommand(_ => VueCourrante = new Page2ViewModel(), _ => true);
            CompatibiliteCommande  = new RelayCommand(_ => VueCourrante = new CompatibiliteViewModele(), _ => true);
            CouleurCommande        = new RelayCommand(_ => VueCourrante = new CouleurViewModel(), _ => true);
            TypeContactCommande    = new RelayCommand(_ => VueCourrante = new TypeContactViewModel(), _ => true);
            MotifEntreeCommande    = new RelayCommand(_ => VueCourrante = new MotifEntreeViewModel(), _ => true);
            MotifSortieCommande    = new RelayCommand(_ => VueCourrante = new MotifSortieViewModel(), _ => true);

            DemandeCommande        = new RelayCommand(_ => VueCourrante = new DemandeViewModel(), _ => true);
            AnimalCommande         = new RelayCommand(_ => VueCourrante = new AnimalViewModel(), _ => true);
            ContactCommande        = new RelayCommand(_ => VueCourrante = new ContactViewModel(windowService), _ => true);

            EntreeCommande         = new RelayCommand(_ => VueCourrante = new EntreeViewModel(), _ => true);
            SortieCommande         = new RelayCommand(_ => VueCourrante = new SortieViewModel(), _ => true);

            VueCourrante = new DemandeViewModel();
        }


        //---------------------------------------------------

        private void FullDBSync()
        {
            DB_Couleur.All_From_db();
            DB_Abri.All_From_db();
            DB_TypeAnimal.AllTypesAnimal();
            DB_Animal.Db_listeAnimaux();

            DB_TypeContact.All_From_Db();
            DB_Contact.All_From_Db();
            DB_TypeCnt_Contact.AllRoles();
            DB_AnimalCouleur.All_From_Db();

            DB_Vaccin.All_From_Db();
            DB_Vaccination.All_From_Db();

            DB_Compatibilite.All_From_Db();
            DB_AnimalCompatibilité.All_From_Db();

            DB_Demande.All_From_Db();

            DB_MotifEntree.All_From_Db();
            DB_MotifSortie.All_From_Db();

            DB_Entree.All_From_Db();
            DB_Sortie.All_From_Db();

            DB_Adoption.All_From_Db();
            DB_Accueil.All_From_Db();
            DB_User.All_From_Db();
        }
    }
}
