using Wpf_App_Pattoon_Animalerie.Service;

namespace Wpf_App_Pattoon_Animalerie.Modele
{
    public abstract class Motifs :ITable 
    {
        private DateTime _date;  
        private string _libele;
        private string _Details;
        protected Motifs(string libele,string details)
        {
            DateCreation = DateTime.Now;
            Libele = libele;
            Details = details;
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
        public string Libele
        {
            get { return _libele; }
            set
            {
                if (string.IsNullOrEmpty(value.Trim()) || value.Trim().Length < 3)
                {
                    throw new Exception($"[Motifs] Cet libele n'est pas valide : {value}");
                }
                _libele = value.Trim().ToUpper();
            }
        }
        public string Details
        {
            get { return _Details; }
            set
            {
                string val = value.Trim();
                if (string.IsNullOrEmpty(value.Trim()))
                {
                    val="Pas de Detail";
                }
                _Details = val;
            }
        }
        public override string ToString()
        {
            string retVal = Forma.Texta2("Date", DateCreation.ToString("dd-MM-yyyy")) +
                Forma.Texta2("Nom", Libele) +
                Forma.Texta2("Description", Details);
            
            return retVal;
        }

    }

    public class MotifEntree : Motifs
    {
        private string _id;
        private MotifEntree(string libele, string details):base(libele, details)
        {
            _id = Forma.SimpleId("MOE", AllMotifsEntrees.Num + 1);
        }
        public override string Id
        {
            get { return _id; }
            set { _id = Forma.Checked_Id(value); }
        }
        public static MotifEntree Creer(string libele, string description)
        {
            MotifEntree retVal = null;
            string FormatLibele = libele.Trim().ToUpper();
            if (string.IsNullOrEmpty(libele))
            {
                throw new Exception($"[Motif Entree] Les parametres sont invalides");
            }
            retVal = new MotifEntree(FormatLibele, description);
            return retVal;
        }
        public static int Save(MotifEntree motifEntree)
        {
            int retVal = 0;
            if (AllMotifsEntrees.FindById(motifEntree.Id) == null)
            {
                AllMotifsEntrees.Add(motifEntree);
                retVal = AllMotifsEntrees.DB_Add(motifEntree);
            }
            return retVal;
        }
        public int Update (string libele, string description)
        {
            int retVal = 0;
            if (AllMotifsEntrees.FindByLibelle(libele) == null)
            {
                Libele = libele;
                Details = description;
                retVal = AllMotifsEntrees.DB_Update(this);
            }
            return retVal;
        }
        public static int Delete(MotifEntree motif)
        {
            int retVal = 0;
            if (AllMotifsEntrees.FindById(motif.Id) != null)
            {
                AllMotifsEntrees.Remove(motif);
                retVal = AllMotifsEntrees.DB_Delete(motif);
            }
            return retVal;
        }
        public override string ToString()
        {
            string retVal = Id;//Forma.Texta2("Id", Id)+ base.ToString();
            return retVal;
        }
    }
    public class MotifSortie : Motifs
    {
        private string _id;
        private MotifSortie(string libele, string details) : base(libele, details)
        {
            _id = Forma.SimpleId("MOS", AllMotifsSortie.Num + 1);
        }
        public override string Id
        {
            get { return _id; }
            set { _id = Forma.Checked_Id(value); }
        }
        public static MotifSortie Creer(string libele, string description)
        {
            MotifSortie retVal = null;
            string FormatLibele = libele.Trim().ToUpper();
            if (string.IsNullOrEmpty(libele))
            {
                throw new Exception($"[Motif Sortie] Les parametres sont invalides");
            }
            retVal = new MotifSortie(FormatLibele, description);
            return retVal;
        }
        public static int Save(MotifSortie motif)
        {
            int retVal = 0;
            if (AllMotifsSortie.FindById(motif.Id) == null)
            {
                AllMotifsSortie.Add(motif);
                retVal = AllMotifsSortie.DB_Add(motif);
            }
            return retVal;
        }
        public int Update(string libele, string description)
        {
            int retVal = 0;
            if (AllMotifsSortie.FindByLibelle(libele) == null)
            {
                Libele = Forma.TrimUpper(libele);
                Details = description;
                retVal = AllMotifsSortie.DB_Update(this);
            }
            return retVal;
        }
        public static int Delete(MotifSortie motif)
        {
            int retVal = 0;
            if (AllMotifsSortie.FindById(motif.Id) != null)
            {
                AllMotifsSortie.Remove(motif.Id);
                retVal = AllMotifsSortie.DB_Delete(motif);
            }
            return retVal;
        }
        public override string ToString()
        {
            string retVal = Id;//Forma.Texta2("Id", Id) + base.ToString();
            return retVal;
        }
    }
}
