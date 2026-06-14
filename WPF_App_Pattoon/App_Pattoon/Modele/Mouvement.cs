using Wpf_App_Pattoon_Animalerie.Service;

namespace Wpf_App_Pattoon_Animalerie.Modele
{

    public abstract class Mouvement : ITable
    {
        private DateTime _date;
        private string? _details;

        protected Mouvement(string? details)
        {
            Details = details;
            _date = DateTime.Now;
        }

        public abstract string Id
        {
            get;
            set;
        }
        public DateTime DateCreation
        {
            get { return _date; }
            set { _date = Forma.Checked_DateCreation(value); }
        }
        public string? Details
        {
            get { return _details; }
            set { _details = value; }
        }
        public abstract Demande Demande
        {
            get;
            set;
        }
        public abstract EStatutDemande StatutDemande
        {
            get;
        }
        public override string ToString()
        {
            string retVal = Forma.Texta2("Date", $"{DateCreation}") +
                Forma.Texta2("Id Demande", $"{Demande.Id}") +
                Forma.Texta2("Statut Dem.", $"{Demande.Statut}") +
                Forma.Texta2("Id Animal", $"{Demande.Animal.Id}") +
                Forma.Texta2("Animal", $"{Demande.Animal.Nom}") +
                Forma.Texta2("Contact", $"{Demande.Contact.Nom}") +
                Forma.Texta2("Contact Gsm", $"{Demande.Contact.Gsm}") +
                Forma.Texta2("Adresse", $"{Demande.Contact.Adresse}") +
                Forma.Texta2("Details", $"{Details}");

            return retVal;
        }
    }

    public class Sortie : Mouvement, IComparable<Sortie>
    {
        private Demande _demande;
        private MotifSortie _motifs;
        private string _id;
        private Sortie(Demande demande, MotifSortie motifs, string details) : base(details)
        {
            Id = "STE-" + demande.Id;
            Motifs = motifs;
            Demande = demande;
        }

        public override string Id
        {
            get { return _id; }
            set
            {
                _id = Forma.Checked_Id(value);
            }
        }
        public override Demande Demande
        {
            get { return _demande; }
            set
            {
                if (value.Type == ETypeDemande.ENTREE)
                {
                    ExceptionLauncher.New("Sortie Demande", "Cette demande ne peut etre affectée ici...");
                }
                _demande = value;
            }
        }
        public override EStatutDemande StatutDemande
        {
            get { return Demande.Statut; }
        }
        public MotifSortie Motifs
        {
            get { return _motifs; }

            set => _motifs = value;
        }

        public int CompareTo(Sortie sortie)
        {
            return _id.CompareTo(sortie._id);
        }
        public override string ToString()
        {
            string retVal = Id;
               /* Forma.Center($"SORTIE N° [ {Id} ]\n") +
                Forma.Center(new string('-', 90) + $"\n\n") +
                $"{base.ToString()}" +
                Forma.Texta2("Motif", $"{Motifs.Libele}") +
                Forma.Texta2("Motif Details", $"{Motifs.Details}");*/
            return retVal;
        }

        public static Sortie Creer(Demande demande, MotifSortie motifs, string details)
        {
            Forma.ParametreNullTesteur(demande);
            Forma.ParametreNullTesteur(motifs);

            if (demande.Type == ETypeDemande.ENTREE)
            {
                ExceptionLauncher.New("Creer Sortie", "Cet demande est de type entree");
            }
            Sortie retVal = new(demande, motifs, details);

            return retVal;
        }
        public static int Save(Sortie sortie)
        {
            if (sortie.Demande.Statut != EStatutDemande.EN_COURS)
            {
                ExceptionLauncher.New("Creer Sortie", $"Cette demande n'est pas en cours {sortie.Demande.Id}");
            }

            int retVal = 0;
            if (AllSortie.Find(sortie.Id) == null)
            {
                AllSortie.Add(sortie);
                retVal = AllSortie.DB_Add(sortie);
                Sync(sortie);

            }
            return retVal;
        }
        public int Update(MotifSortie motifs, string details)
        {
            Forma.ParametreNullTesteur(motifs);

            int retVal = 0;

            if (AllSortie.Find(Id) != null)
            {
                Motifs = motifs;
                Details = details;
                retVal = AllSortie.DB_Update(this);
            }
            return retVal;
        }
        public static int Delete(Sortie sortie)
        {
            int retVal = 0;
            if (AllSortie.Find(sortie.Id) != null)
            {
                OnDelete(sortie);

                AllSortie.Remove(sortie.Id);
                retVal = AllSortie.DB_Delete(sortie);
            }
            return retVal;
        }

        public static void Sync(Sortie sortie)
        {
            if (sortie.Demande.Type == ETypeDemande.ACCUEIL)
            {
                sortie.Demande.Animal.Update(EStatutAnimal.ACCUEIL);
            }

            if (sortie.Demande.Type == ETypeDemande.ADOPTION)
            {
                sortie.Demande.Animal.Update(EStatutAnimal.ADOPTION);
            }

            if (sortie.Demande.Type == ETypeDemande.SORTIE)
            {
                sortie.Demande.Animal.Update(EStatutAnimal.PROPRIETAIRE);
            }

            if (sortie.Demande.Animal.Abri != null)
            {
                sortie.Demande.Animal.RemoveAbri();
            }
            ;
            sortie.Demande.Update(EStatutDemande.TERMINEE);
            sortie.Demande.Animal.RefuserAllDemandeEncours();
        }
        private static int OnDelete(Sortie sortie)
        {
            if (sortie.Demande.Animal.LastDemande == sortie.Demande)
            {
                sortie.Demande.Update(EStatutDemande.EN_COURS);
                sortie.Demande.Animal.Update(EStatutAnimal.REFUGE);
            }

            return 1;
        }

    }
    public class Entree : Mouvement, IComparable<Entree>
    {
        private MotifEntree _motifs;
        private string _id;
        private Demande _demande;

        private Entree(Demande demande, MotifEntree motifs, string details) : base(details)
        {
            Id = "ENT-" + demande.Id;
            Motifs = motifs;
            Demande = demande;
        }
        public override string Id
        {
            get { return _id; }
            set
            {
                _id = Forma.Checked_Id(value);
            }
        }
        public override Demande Demande
        {
            get { return _demande; }
            set
            {
                if (value.Type != ETypeDemande.ENTREE)
                {
                    ExceptionLauncher.New("Entree Demande", "Cette demande ne peut etre affectée ici...");
                }
                _demande = value;
            }
        }
        public override EStatutDemande StatutDemande
        {
            get { return Demande.Statut; }
        }
        public MotifEntree Motifs
        {
            get { return _motifs; }

            set => _motifs = value;
        }

        public static Entree Creer(Demande demande, MotifEntree motifs, string details)
        {
            Forma.ParametreNullTesteur(demande);
            Forma.ParametreNullTesteur(motifs);

            Entree? retVal = null;
            if (demande.Type != ETypeDemande.ENTREE)
            {
                ExceptionLauncher.New("AllEntree", $"Ceci n est pas un entree : {demande.Type} ");
            }

            return new Entree(demande, motifs, details);

        }
        public int Update(MotifEntree motifs, string details)
        {
            Forma.ParametreNullTesteur(motifs);

            if (Demande.Statut > EStatutDemande.EN_COURS)
            {
                ExceptionLauncher.New("ENTREE", "La demande est n'est pas en cours");
            }

            int retVal = 0;
            if (AllEntree.Find(Id) != null)
            {
                Motifs = motifs;
                Details = details;
                retVal = AllEntree.DB_Update(this);
            }
            return retVal;
        }
        public static int Save(Entree entree)
        {
            if (entree.Demande.Type != ETypeDemande.ENTREE)
            {
                ExceptionLauncher.New("Entree Save", $"Ceci n est pas un entree : {entree.Demande.Type} ");
            }

            if (entree.Demande.Statut != EStatutDemande.EN_COURS)
            {
                ExceptionLauncher.New("Entree Save", "Cette demande n 'est pas valide");
            }

            int retVal = 0;
            if (AllEntree.Find(entree.Id) == null && entree.Demande.Statut == EStatutDemande.EN_COURS)
            {
                OnSave(entree);
                AllEntree.Add(entree);
                retVal = AllEntree.DB_Add(entree);
            }
            return retVal;
        }
        public static int Delete(Entree entree)
        {
            int retVal = 0;
            if (AllEntree.Find(entree.Id) != null)
            {
                OnDelete(entree);

                AllEntree.Remove(entree.Id);
                retVal = AllEntree.DB_Delete(entree);
            }
            return retVal;
        }

        public int CompareTo(Entree entree)
        {
            return _id.CompareTo(entree._id);
        }
        public override string ToString()
        {
            string retVal = Id;
                /*Forma.Center($"ENTREE N° [ {Id} ]\n") +
                Forma.Center(new string('-', 90) + $"\n\n") +
                $"{base.ToString()}" +
                Forma.Texta2("Motif", $"{Motifs.Libele}") +
                Forma.Texta2("Motif Details", $"{Motifs.Details}")*/;
            return retVal;
        }


        private static void OnSave(Entree entree)
        {
            if (entree.Demande.Animal.Statut == EStatutAnimal.ACCUEIL)
            {
                Accueil? accueil = entree.Demande.Animal.LastAccueil;
                accueil.Update(accueil.DateDebut, DateTime.Now);
            }

            if (entree.Demande.Animal.Statut == EStatutAnimal.ADOPTION)
            {
                Adoption? adoption = entree.Demande.Animal.LastAdoption;
                adoption.Update(adoption.DateD, DateTime.Now);
            }

            entree.Demande.Animal.Update(EStatutAnimal.REFUGE);
            entree.Demande.Update(EStatutDemande.TERMINEE);
        }
        private static int OnDelete(Entree entree)
        {
            if (entree.Demande.Animal.LastDemande == entree.Demande)
            {
                entree.Demande.Update(EStatutDemande.EN_COURS);
                entree.Demande.Animal.Update(EStatutAnimal.REFUGE);
            }

            return 1;
        }
    }
}
