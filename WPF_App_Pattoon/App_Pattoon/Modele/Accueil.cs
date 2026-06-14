using Wpf_App_Pattoon_Animalerie.Service;

namespace Wpf_App_Pattoon_Animalerie.Modele
{

    public class Accueil : ITable, IComparable<Accueil>
    {
        private string _id;
        private DateTime _date;
        private DateTime? _dateFin;
        private DateTime? _dateDebut;
        private EStatutValidation _statut;
        private string? _infos;
        private string? _raisonAnnulation;
        private Demande _demande;
        private Accueil(Demande demande, string? infos)
        {
            Id = "ACC-" + demande.Id;
            DateCreation = DateTime.Now;
            DateDebut = null;
            DateFin = null;
            Statut = EStatutValidation.EN_COURS;
            Info = infos;
            Demande = demande;

            RaisonAnullation = null;
        }

        public string Id
        {
            get { return _id; }
            set { _id = Forma.Checked_Id(value); }
        }
        public DateTime DateCreation
        {
            get { return _date; }
            set { _date = Forma.Checked_DateCreation(value); }
        }
        public DateTime? DateFin
        {
            get { return _dateFin; }
            set
            {
                if (value < DateCreation)
                {
                    ExceptionLauncher.New("Accueil Date Fin", "La date de fin n'est pas valide.");
                }
                _dateFin = value;
            }
        }
        public DateTime? DateDebut
        {
            get { return _dateDebut; }
            set
            {
                if (value < DateCreation)
                {
                    ExceptionLauncher.New("Accueil Date Debut", "La date debut n'est pas valide.");
                }
                _dateDebut = value;
            }
        }
        public EStatutValidation Statut
        {
            get { return _statut; }
            set { _statut = value; }
        }
        public Demande Demande
        {
            get { return _demande; }
            set
            {
                if (value.Type != ETypeDemande.ACCUEIL)
                {
                    ExceptionLauncher.New("Accueil Demande", $"La demande n'est pas valide. - Statut : {value.Statut} - MyType : {value.Type}");
                }
                _demande = value;
            }
        }
        public string? Info
        {
            get { return _infos; }
            set { _infos = value; }
        }
        public string? RaisonAnullation
        {
            get { return _raisonAnnulation; }
            set { _raisonAnnulation = value; }
        }
        public Sortie? Sortie
        {
            get
            {
                Sortie? ret = null;
                if (AllSortie.Find(Demande) != null)
                {
                    ret = AllSortie.Find(Demande);
                }
                return ret;
            }
        }
        public override string ToString()
        {
            string? info = null;
            if (Statut > EStatutValidation.EN_COURS)
            {
                info = $"- [ {Statut} ] -";

                if (Sortie == null)
                {
                    info += $" SORTIE À CRÈER";
                }
                else
                {
                    info += $" SORTIE CRÈER !!!";
                }
                info = Forma.Center(info + "\n\n", 100);
            }

            string retVal =
                Forma.Center($"FICHE D'ACCUEIL N° [ {Id} ]\n") +
                Forma.Center(new string('-', 90) + $"\n") +

                info +

                Forma.Texta2("Date", DateCreation.ToString("dd-MM-yyyy")) +
                Forma.Texta2("Id", Id + "\n") +
                Forma.Texta2("Animal Id", Demande.Animal.Id) +
                Forma.Texta2("Nom animal", Demande.Animal.Nom) +
                Forma.Texta2("GetUnType animal", Demande.Animal.Type.Nom + "\n") +
                Forma.Texta2("Demande id", Demande.Id) +
                Forma.Texta2("Contact", Demande.Contact.Nom) +
                Forma.Texta2("Statut", Statut.ToString() + "\n") +
                Forma.Texta2("Date Debut", DateDebut == null ? "--" : DateDebut.ToString()) +
                Forma.Texta2("Date Fin.", DateFin == null ? "--" : DateFin.ToString()) +
                Forma.Texta2("Raison", DateFin == null ? "--" : RaisonAnullation);
            return retVal;
        }

        public static Accueil? Creer(Demande demande, string? infos)
        {
            if (demande == null || demande.Type != ETypeDemande.ACCUEIL)
            {
                ExceptionLauncher.New("Accueil Creer", $"La demande n'est pas de type Accueil - type {demande.Type}");
            }

            Accueil dem = new(demande, infos);

            return dem;
        }
        public static int Save(Accueil accueil)
        {
            int ret = 0;
            if (AllAccueil.Find(accueil.Id) == null && accueil.Demande.Statut == EStatutDemande.EXAMINATION)
            {
                AllAccueil.Add(accueil);
                ret = AllAccueil.DB_Add(accueil);
                Sync(accueil);
            }
            return ret;
        }

        public Accueil? Accepter()
        {
            if (Sortie != null)
            {
                ExceptionLauncher.New("Accueil Accepter", "La Sortie est deja creer");
            }

            if (Demande.Statut == EStatutDemande.TERMINEE || Demande.Statut == EStatutDemande.CLOTUREE)
            {
                ExceptionLauncher.New("Accueil Accepter", "Cette demande est Terminee");
            }

            if (AllAccueil.Find(Id) == null)
            {
                ExceptionLauncher.New("Accueil Accepter", "Cette accueil n'est pas enregistré");
            }


            Statut = EStatutValidation.ACCEPTEE;
            DateDebut = DateTime.Now;
            Demande.Update(EStatutDemande.EN_COURS);
            AllAccueil.DB_Update(this);

            return this;
        }
        public Accueil? Refuser(string? motif)
        {
            if (Sortie != null)
            {
                ExceptionLauncher.New("Accueil Refuser", "La Sortie est deja creer");
            }

            if (Demande.Statut == EStatutDemande.TERMINEE || Demande.Statut == EStatutDemande.CLOTUREE)
            {
                ExceptionLauncher.New("Accueil Refuser", "Cette demande est Terminee");
            }

            if (AllAdoption.Find(Id) == null)
            {
                ExceptionLauncher.New("Accueil Refuser", "Cette accueil n'est pas enregistré");
            }

            Statut = EStatutValidation.REFUSEE;
            DateFin = DateTime.Now;
            RaisonAnullation = motif;
            Demande.Update(EStatutDemande.TERMINEE);
            AllAccueil.DB_Update(this);

            return this;
        }
        public Accueil? Indecis()
        {
            if (Sortie != null)
            {
                ExceptionLauncher.New("Accueil Indecis", "La Sortie est deja creer");
            }

            if (Demande.Statut == EStatutDemande.TERMINEE || Demande.Statut == EStatutDemande.CLOTUREE)
            {
                ExceptionLauncher.New("Accueil Indecis", "Cette demande est Terminee");
            }

            if (AllAdoption.Find(Id) == null)
            {
                ExceptionLauncher.New("Accueil Indecis", "Cette accueil n'est pas enregistré");
            }

            Statut = EStatutValidation.EN_COURS;
            DateDebut = null;
            DateFin = null;
            RaisonAnullation = null;
            Demande.Update(EStatutDemande.VALIDATION);
            AllAccueil.DB_Update(this);

            return this;
        }

        public int Update(Demande demande, string? infos)
        {
            if (Sortie != null)
            {
                ExceptionLauncher.New("Accueil Update", "La Sortie est deja creer");
            }

            int ret = 0;
            if (AllAccueil.Find(Id) != null)
            {
                Demande = demande;
                Info = infos;

                ret = AllAccueil.DB_Update(this);
            }
            return ret;
        }
        public int Update(DateTime? dteD, DateTime? dteF)
        {
            int ret = 0;
            if (AllAccueil.Find(Id) != null)
            {
                DateDebut = dteD;
                DateFin = dteF;

                ret = AllAccueil.DB_Update(this);
            }
            return ret;
        }

        public static int Delete(Accueil accueil)
        {
            int ret = 0;
            if (AllAccueil.Find(accueil.Id) != null)
            {
                OnDelete(accueil);
                AllAccueil.Remove(accueil.Id);
                ret = AllAccueil.DB_Delete(accueil);
            }
            return ret;
        }
        public int CompareTo(Accueil accueil)
        {
            return CompareTo(accueil);
        }

        private static int Sync(Accueil accueil)
        {
            int ret;
            if (accueil.Statut == EStatutValidation.REFUSEE)
            {
                ret = accueil.Demande.Update(EStatutDemande.TERMINEE);

            }
            else if (accueil.Statut == EStatutValidation.EN_COURS)
            {
                ret = accueil.Demande.Update(EStatutDemande.VALIDATION);
            }
            else
            {
                ret = accueil.Demande.Update(EStatutDemande.EN_COURS);
            }
            return ret;
        }
        private static int OnDelete(Accueil accueil)
        {
            if (accueil.Sortie != null)
            {
                Sortie.Delete(accueil.Sortie);
            }

            return 1;
        }


    }
}
