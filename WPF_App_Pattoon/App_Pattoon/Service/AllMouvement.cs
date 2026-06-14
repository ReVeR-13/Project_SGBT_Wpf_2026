using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_App_Pattoon_Animalerie.AccessDB;
using Wpf_App_Pattoon_Animalerie.Modele;

namespace Wpf_App_Pattoon_Animalerie.Service
{
    public static class AllEntree
    {
        private static Dictionary<string, Entree> _lesEntres;

        static AllEntree()
        {
            _lesEntres = new Dictionary<string, Entree>();
        }
        public static int Count
        {
            get { return _lesEntres.Count; }
        }
        public static string LesEntrees
        {
            get
            {
                int i = 0;
                string retVal = $"Liste des Entrees [{Count}]\n" +
                Forma.Text("N°", "Id", "Date Crea.", "Motifs", "Id Demande", "Contact", "Id Animal", "Nom Animal");
                foreach (Entree vc in _lesEntres.Values.OrderByDescending(a => a.DateCreation))
                {
                    i++;
                    retVal += Forma.Text(
                        $"{i}",
                        $"{vc.Id}",
                        $"{vc.DateCreation.ToString("dd-MM-yyyy")}",
                        $"{vc.Motifs.Libele}",
                        $"{vc.Demande.Id}",
                        $"{vc.Demande.Contact.Nom} {vc.Demande.Contact.Prenom}",
                        $"{vc.Demande.Animal.Id}",
                        $"{vc.Demande.Animal.Nom}");
                }

                return retVal;
            }
        }
        public static string LesEntreesDerniers
        {
            get
            {
                int i = 0;
                string retVal = $"Liste des Entrees [{Count}]\n" +
                Forma.Text("N°", "Id", "Date Crea.", "Motifs", "Id Demande", "Contact", "Id Animal", "Nom Animal");
                foreach (Entree vc in _lesEntres.Values.OrderByDescending(ob => ob.DateCreation).Take(10))
                {
                    i++;
                    retVal += Forma.Text(
                        $"{i}",
                        $"{vc.Id}",
                        $"{vc.DateCreation.ToString("dd-MM-yyyy")}",
                        $"{vc.Motifs.Libele}",
                        $"{vc.Demande.Id}",
                        $"{vc.Demande.Contact.Nom} {vc.Demande.Contact.Prenom}",
                        $"{vc.Demande.Animal.Id}",
                        $"{vc.Demande.Animal.Nom}");
                }

                return retVal;
            }
        }

        public static Dictionary<string, Entree> LesStocks
        {
            get { DB_Sync(); return _lesEntres; }
        }

        public static IEnumerable<Entree> Get()
        {
            foreach (Entree ent in _lesEntres.Values)
            {
                yield return ent;
            }
        }
        public static IEnumerable<Entree> Get(Animal animal)
        {
            foreach (Entree en in Get().Where(a => a.Demande.Animal == animal).OrderByDescending(v => v.DateCreation))
            {
                yield return en;
            }
        }
        public static IEnumerable<Entree> Get(Contact contact)
        {
            foreach (Entree en in Get().Where(a => a.Demande.Contact == contact).OrderByDescending(v => v.DateCreation))
            {
                yield return en;
            }
        }

        public static Entree? Find(string key)
        {
            Forma.ParametreNullTesteur(key);

            Entree? entree = null;
            string F_key = Forma.TrimUpper(key);
            if (_lesEntres.ContainsKey(F_key))
            {
                entree = _lesEntres[F_key];
            }
            return entree;
        }
        public static Entree? Find(Demande demande)
        {

            Entree? entree = null;
            foreach (Entree e in _lesEntres.Values)
            {
                if (e.Demande == demande)
                {
                    entree = e;
                    break;
                }
            }

            return entree;
        }
        public static void Add(Entree entree)
        {
            if (entree.Demande.Type != ETypeDemande.ENTREE)
            {
                ExceptionLauncher.New("AllEntree", $"Ceci n est pas un entree : {entree.Demande.Type} ");
            }
            if (Find(entree.Id) != null)
            {
                ExceptionLauncher.New("AllEntree", $"Cet Id est deja dans la liste : {entree.Id} ");
            }
            _lesEntres.Add(entree.Id, entree);
        }
        public static void Remove(string id)
        {
            string formatId = id.Trim().ToUpper();
            if (Find(formatId) == null)
            {
                ExceptionLauncher.New("AllEntree", $"Cet Id n'est plus dans la liste : {formatId} ");
            }
            _lesEntres.Remove(formatId);
        }

        public static int DB_Add(Entree entree)
        {
            if (entree.Demande.Type != ETypeDemande.ENTREE)
            {
                ExceptionLauncher.New("AllEntree", $"Ceci n est pas un entree : {entree.Demande.Type} ");
            }

            int retVal = 0;
            if (DB_Entree.UnEntreeById(entree) == null)
            {
                retVal = DB_Entree.Add(entree);
            }
            return retVal;
        }
        public static int DB_Update(Entree entree)
        {
            if (entree.Demande.Type != ETypeDemande.ENTREE)
            {
                ExceptionLauncher.New("AllEntree", $"Ceci n est pas un entree : {entree.Demande.Type} ");
            }
            int retVal = 0;
            if (DB_Entree.UnEntreeById(entree) != null)
            {
                retVal = DB_Entree.Update(entree);
            }
            return retVal;
        }
        public static int DB_Delete(Entree entree)
        {
            int retVal = 0;
            if (DB_Entree.UnEntreeById(entree) != null)
            {
                retVal = DB_Entree.Delete(entree);
            }
            return retVal;
        }

        public static void DB_Sync()
        {
            _lesEntres = new Dictionary<string, Entree>(DB_Entree.All_From_Db());
        }

    }
    public static class AllSortie
    {
        private static Dictionary<string, Sortie> _lesSorties;

        static AllSortie()
        {
            _lesSorties = new Dictionary<string, Sortie>();
        }

        public static int Count
        {
            get { return _lesSorties.Count; }
        }
        public static string LesSorties
        {

            get
            {
                int i = 0;
                string retVal = $"Liste des Sorties [{Count}]\n" +
                Forma.Text("N°", "Id", "Date Crea.", "Motifs", "Id Demande", "Contact", "Id Animal", "Nom Animal");
                foreach (Sortie vc in _lesSorties.Values)
                {
                    i++;
                    retVal += Forma.Text(
                        $"{i}",
                        $"{vc.Id}",
                        $"{vc.DateCreation:dd-MM-yyyy}",
                        $"{vc.Motifs.Libele}",
                        $"{vc.Demande.Id}",
                        $"{vc.Demande.Contact.Nom} {vc.Demande.Contact.Prenom}",
                        $"{vc.Demande.Animal.Id}",
                        $"{vc.Demande.Animal.Nom}");
                }

                return retVal;
            }

        }
        public static string LesSortiesDernieres
        {

            get
            {
                int i = 0;
                string retVal = $"Liste des 10 dernières Sorties [{Count}]\n" +
                Forma.Text("N°", "Id", "Date Crea.", "Motifs", "Id Demande", "Contact", "Id Animal", "Nom Animal");
                foreach (Sortie vc in _lesSorties.Values.OrderByDescending(ob => ob.DateCreation).Take(10))
                {
                    i++;
                    retVal += Forma.Text(
                        $"{i}",
                        $"{vc.Id}",
                        $"{vc.DateCreation:dd-MM-yyyy}",
                        $"{vc.Motifs.Libele}",
                        $"{vc.Demande.Id}",
                        $"{vc.Demande.Contact.Nom} {vc.Demande.Contact.Prenom}",
                        $"{vc.Demande.Animal.Id}",
                        $"{vc.Demande.Animal.Nom}");
                }

                return retVal;
            }

        }

        public static Dictionary<string, Sortie> LesStocks
        {
            get { DB_Sync(); return _lesSorties; }
        }

        public static IEnumerable<Sortie> Get()
        {
            foreach (Sortie sort in _lesSorties.Values)
            {
                yield return sort;
            }
        }
        public static IEnumerable<Sortie> Get(Animal animal)
        {
            foreach (Sortie sort in Get().Where(a => a.Demande.Animal == animal).OrderByDescending(v => v.DateCreation))
            {
                yield return sort;
            }
        }
        public static IEnumerable<Sortie> Get(Contact contact)
        {
            foreach (Sortie sort in Get().Where(a => a.Demande.Contact == contact).OrderByDescending(v => v.DateCreation))
            {
                yield return sort;
            }
        }

        public static Sortie Find(string key)
        {
            Sortie sortie = null;
            string F_key = Forma.TrimUpper(key);
            if (_lesSorties.ContainsKey(F_key))
            {
                sortie = _lesSorties[F_key];
            }
            return sortie;
        }
        public static Sortie Find(Demande demande)
        {
            Sortie? sortie = null;
            foreach (Sortie s in _lesSorties.Values)
            {
                if (s.Demande == demande)
                {
                    sortie = s;
                    break;
                }
            }

            return sortie;
        }
        public static void Add(Sortie sortie)
        {
            if (Find(sortie.Id) != null)
            {
                ExceptionLauncher.New("AllSortie", $"Cet Id est deja dans la liste : {sortie.Id} ");
            }
            _lesSorties.Add(sortie.Id, sortie);
        }
        public static void Remove(string id)
        {
            string formatId = id.Trim().ToUpper();
            if (Find(formatId) == null)
            {
                ExceptionLauncher.New("AllSortie", $"Cet Id n'est plus dans la liste : {formatId} ");
            }
            _lesSorties.Remove(formatId);
        }

        public static int DB_Add(Sortie sortie)
        {
            int retVal = 0;
            if (DB_Sortie.UnSortieById(sortie) == null)
            {
                retVal = DB_Sortie.Add(sortie);
            }
            return retVal;
        }
        public static int DB_Update(Sortie sortie)
        {
            int retVal = 0;
            if (DB_Sortie.UnSortieById(sortie) != null)
            {
                retVal = DB_Sortie.Update(sortie);
            }
            return retVal;
        }
        public static int DB_Delete(Sortie sortie)
        {
            int retVal = 0;
            if (DB_Sortie.UnSortieById(sortie) != null)
            {
                retVal = DB_Sortie.Delete(sortie);
            }
            return retVal;
        }

        public static void DB_Sync()
        {
            _lesSorties = new Dictionary<string, Sortie>(DB_Sortie.All_From_Db());
        }

    }
}
