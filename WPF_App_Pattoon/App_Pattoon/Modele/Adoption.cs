using Wpf_App_Pattoon_Animalerie.Service;

namespace Wpf_App_Pattoon_Animalerie.Modele
{
    public class Adoption :ITable
    {
        private string _id;
        private Demande _demande;
        private EStatutValidation _statut;
        private DateTime _date;
        private DateTime? _dateD;
        private DateTime? _dateF;
        private string? _raisonRefus;
        private string _infos;

        private Adoption(Demande demande,string info) 
        {
            _id = "ADO-" + demande.Id;
            _date = DateTime.Now;
            Demande = demande;
            Statut = EStatutValidation.EN_COURS;
            DateD = null;
            DateF = null;
            _raisonRefus = null;
            Info = info;

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
        public Demande Demande 
        { 
            get { return _demande; } 
            set 
            {
                if (value.Type != ETypeDemande.ADOPTION )
                {
                    ExceptionLauncher.New("Adoption", $"Demande invalide : Statut {value.Statut} - GetUnType {value.Type}");
                }
                _demande = value; 
            }
        }
        public EStatutValidation Statut
        {
            get
            {
                return _statut;
            }
            set
            {
                _statut = value;
            }
        }
        public DateTime? DateD 
        {
            get { return _dateD; }
            set { _dateD = value; }
        }
        public DateTime? DateF 
        {
            get { return _dateF; }
            set { _dateF = value; }
        }
        public string Info 
        {
            get { return _infos; }
            set { _infos = value; }
        }
        public string? RaisonRefus
        {
            get { return _raisonRefus; }
            set { _raisonRefus = value; }
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
                Forma.Center($"FICHE D'ADOPTION N° [ {Id} ]\n")+
                Forma.Center(new string('-',90) + $"\n") +

                info +

                Forma.Texta2("Date", $"{DateCreation:dd-MM-yyyy}") +
                Forma.Texta2("ID", Id) +
                Forma.Texta2("ID Demande", Demande.Id) + "\n" +

                Forma.Texta2("Contact", $"{Demande.Contact.Nom} {Demande.Contact.Prenom}") +
                Forma.Texta2("Gsm", Demande.Contact.Gsm) + "\n" +

                Forma.Texta2("Id Animal", $"{Demande.Animal.Id}") +
                Forma.Texta2("Nom", Demande.Animal.Nom) + "\n" +

                Forma.Texta2("Date Debut", DateD == null ? "--" : DateD?.ToString("dd-MM-yyyy")) +
                Forma.Texta2("Date Fin", DateF == null ? "--" : DateF?.ToString("dd-MM-yyyy")) + "\n" +

                Forma.Texta2("Statut", $"{Statut}") +
                Forma.Texta2("Raison Refus", RaisonRefus ?? "--") +
                Forma.Texta2("Infos", Info);

            return retVal;
        }

        public int Update(Demande demande, string info)
        {
            if (Sortie != null)
            {
                ExceptionLauncher.New("Adoption Update", "La Sortie est deja creer");
            }

            int ret = 0;
            if (AllAdoption.Find(Id) != null)
            {
                Demande = demande;
                Info = info;
                ret = AllAdoption.DB_Update(this);
            }
            return ret;
        }
        public int Update (EStatutValidation statut)
        {
            if (Sortie != null)
            {
                ExceptionLauncher.New("Adoption Update", "La Sortie est deja creer");
            }
            int ret = 0;
            if (AllAdoption.Find(Id) != null)
            {
                Statut = statut;
                DateD = DateTime.Now;
                ret = AllAdoption.DB_Update(this);
                Sync(this);
            }
            return ret;
        }
        public int Update(DateTime? D, DateTime? F)
        {
            int ret = 0;
            if (AllAdoption.Find(Id) != null)
            {
                DateD = D;
                DateF = F;
                ret = AllAdoption.DB_Update(this);
            }
            return ret;
        }
        public int Update(EStatutValidation statut,string refus)
        {
            if (Sortie != null)
            {
                ExceptionLauncher.New("Adoption Update", "La Sortie est deja creer");
            }
            int ret = 0;
            if (AllAdoption.Find(Id) != null)
            {
                Statut = statut;
                RaisonRefus = refus;
                DateF = DateTime.Now;
                ret = AllAdoption.DB_Update(this);
                Sync(this);
            }
            return ret;
        }

        public Adoption Accepter()
        {
            if (Sortie != null)
            {
                ExceptionLauncher.New("Adoption Accepter", "La Sortie est deja creer");
            }

            if (Demande.Statut == EStatutDemande.TERMINEE || Demande.Statut == EStatutDemande.CLOTUREE)
            {
                ExceptionLauncher.New("Adoption Accepter", "Cette demande est Terminee");
            }
            if (AllAdoption.Find(Id) == null)
            {
                ExceptionLauncher.New("Adoption Accepter", "Cette adoption n'est pas enregistré");
            }

            Statut = EStatutValidation.ACCEPTEE;
            DateD = DateTime.Now;
            Demande.Update(EStatutDemande.EN_COURS);
            AllAdoption.DB_Update(this);

            return this;
        }
        public Adoption Refuser(string? refus)
        {
            if (Sortie != null)
            {
                ExceptionLauncher.New("Adoption Refuser", "La Sortie est deja creer");
            }

            if (Demande.Statut == EStatutDemande.TERMINEE || Demande.Statut == EStatutDemande.CLOTUREE)
            {
                ExceptionLauncher.New("Adoption Accepter", "Cette demande est Terminee");
            }

            if (AllAdoption.Find(Id) == null)
            {
                ExceptionLauncher.New("Adoption Accepter", "Cette adoption n'est pas enregistré");
            }

            Statut = EStatutValidation.REFUSEE;
            DateF = DateTime.Now;
            RaisonRefus = refus;
            Demande.Update(EStatutDemande.TERMINEE);
            AllAdoption.DB_Update(this);

            return this;
        }
        public Adoption Indecis()
        {
            if (Sortie != null)
            {
                ExceptionLauncher.New("Adoption Indecis", "La Sortie est deja creer");
            }

            if (Demande.Statut == EStatutDemande.TERMINEE || Demande.Statut == EStatutDemande.CLOTUREE)
            {
                ExceptionLauncher.New("Adoption Accepter", "Cette demande est Terminee");
            }

            if (AllAdoption.Find(Id) == null)
            {
                ExceptionLauncher.New("Adoption Accepter", "Cette adoption n'est pas enregistré");
            }

            Statut = EStatutValidation.EN_COURS;
            DateD = null;
            DateF = null;
            RaisonRefus = null;
            Demande.Update(EStatutDemande.VALIDATION);
            AllAdoption.DB_Update(this);

            return this;
        }

        public static Adoption Creer(Demande demande, string info) 
        {
            if (demande == null )
            {
                ExceptionLauncher.New("Adoption Creer", $"Cette demande est null ");
            }
            Adoption adoption = new Adoption(demande, info);
            return adoption;
        }  
        public static int Save(Adoption adoption)
        {
            int retVal = 0;
            if (AllAdoption.Find(adoption.Id) == null && adoption.Demande.Statut == EStatutDemande.EXAMINATION)
            {
                    AllAdoption.Add(adoption);
                    retVal = AllAdoption.DB_Add(adoption);
                    if (retVal == 1)
                    {
                        Sync(adoption);
                    }
            }
            return retVal;
        } 
        public static int Delete(Adoption adoption)
        {
            int ret = 0;
            if (AllAdoption.Find(adoption.Id) != null)
            { 
                OnDelete(adoption);
                AllAdoption.Remove(adoption.Id);
                ret = AllAdoption.DB_Delete(adoption);
            }
            return ret;
        }

        private static int Sync(Adoption adoption)
        {
            int ret;
            if (adoption.Statut == EStatutValidation.REFUSEE)
            {
                ret = adoption.Demande.Update(EStatutDemande.TERMINEE);

            }else if (adoption.Statut == EStatutValidation.EN_COURS) 
            {
                ret = adoption.Demande.Update(EStatutDemande.VALIDATION);
            }
            else
            {
                ret = adoption.Demande.Update(EStatutDemande.EN_COURS);
            }
            return ret;

        }
        private static int OnDelete(Adoption adoption)
        {
            if (adoption.Sortie != null)
            {
                Sortie.Delete(adoption.Sortie);
            }

            return 1;
        }

    }
}
