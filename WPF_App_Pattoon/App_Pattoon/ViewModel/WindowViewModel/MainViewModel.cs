using System.Windows.Input;
using Wpf_App_Pattoon_Animalerie.AccessDB;
using Wpf_App_Pattoon_Animalerie.Commands;
using Wpf_App_Pattoon_Animalerie.Modele;
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

        public ICommand AdoptionCommande { get; }
        public ICommand AccueilCommande { get; }



        //---------------------------------------------------

        public MainViewModel()
        {
            Forma.SyncAllWithDB();

            WindowService windowService = new WindowService();

            AfficherVaccinsCommand = new RelayCommand(_ => VueCourrante = new VaccinViewModel(windowService), _ => true);
            ShowPage1Commande      = new RelayCommand(_ => VueCourrante = new Page1ViewModel(windowService), _ => true);
            ShowPage2Commande      = new RelayCommand(_ => VueCourrante = new Page2ViewModel(windowService), _ => true);
            CompatibiliteCommande  = new RelayCommand(_ => VueCourrante = new CompatibiliteViewModele(windowService), _ => true);
            CouleurCommande        = new RelayCommand(_ => VueCourrante = new CouleurViewModel(windowService), _ => true);
            TypeContactCommande    = new RelayCommand(_ => VueCourrante = new TypeContactViewModel(windowService), _ => true);
            MotifEntreeCommande    = new RelayCommand(_ => VueCourrante = new MotifEntreeViewModel(windowService), _ => true);
            MotifSortieCommande    = new RelayCommand(_ => VueCourrante = new MotifSortieViewModel(windowService), _ => true);

            DemandeCommande        = new RelayCommand(_ => VueCourrante = new DemandeViewModel(windowService), _ => true);
            AnimalCommande         = new RelayCommand(_ => VueCourrante = new AnimalViewModel(windowService), _ => true);
            ContactCommande        = new RelayCommand(_ => VueCourrante = new ContactViewModel(windowService), _ => true);

            EntreeCommande         = new RelayCommand(_ => VueCourrante = new EntreeViewModel(windowService), _ => true);
            SortieCommande         = new RelayCommand(_ => VueCourrante = new SortieViewModel(windowService), _ => true);


            AdoptionCommande       = new RelayCommand(_ => VueCourrante = new AdoptionViewModel(windowService), _ => true);
            AccueilCommande        = new RelayCommand(_ => VueCourrante = new AccueilViewModel(windowService), _ => true);

            VueCourrante = new DemandeViewModel(windowService);
        }


        //---------------------------------------------------

    }
}
