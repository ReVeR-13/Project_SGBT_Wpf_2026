using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_App_Pattoon_Animalerie.AccessDB;
using Wpf_App_Pattoon_Animalerie.Modele;

namespace Wpf_App_Pattoon_Animalerie.Service
{
    public static class AllMotifsEntrees
    {
        private static Dictionary<string, MotifEntree> _lesMotifsEntrees;
        private static int _num;

        static AllMotifsEntrees()
        {
            _lesMotifsEntrees = new Dictionary<string, MotifEntree>();
            _num = 0;
        }

        public static int Num
        {
            get
            {
                if (Count > 0)
                {
                    _num = Forma.LastNumero(_lesMotifsEntrees);
                }
                return _num;
            }
        }
        public static int Count
        {
            get { return _lesMotifsEntrees.Count; }
        }
        public static string LesEntrees
        {
            get
            {
                int i = 0;
                string retVal = string.Format($"{"{0,-4} {1,-19} {2,-12} {3,-15} {4,-20}\n"}",
                    "N°", "Id", "Date Crea.", "Libelle", "Details");

                foreach (MotifEntree dm in _lesMotifsEntrees.Values)
                {
                    i++;
                    retVal += string.Format($"{"{0,-4} {1,-19} {2,-12} {3,-15} {4,-20}\n"}",
                    $"{i}°",
                    $"{dm.Id}",
                    $"{dm.DateCreation.ToString("dd-MM-yyyy")}",
                    $"{dm.Libele}",
                    $"{dm.Details}");
                }

                return $"Liste des Motifs Entree [{Count}]\n\n" + retVal;
            }
        }
        public static Dictionary<string, MotifEntree> LesStocks
        {
            get { DB_Sync(); return _lesMotifsEntrees; }
        }

        public static void Add(MotifEntree motifs)
        {
            if (FindById(motifs.Id) != null)
            {
                throw new Exception($"[AllMotifsEntrees] cet type est deja enregistré : {motifs.Libele}");
            }

            if (FindByLibelle(motifs.Libele) != null)
            {
                throw new Exception($"[AllMotifsEntrees] cet type est deja enregistré : {motifs.Libele}");
            }
            _num++;
            _lesMotifsEntrees.Add(motifs.Id, motifs);
        }
        public static MotifEntree FindByLibelle(string libele)
        {
            MotifEntree retval = null;
            string fl = Forma.TrimUpper(libele);
            foreach (MotifEntree m in _lesMotifsEntrees.Values)
            {
                if (m.Libele == fl)
                {
                    retval = m;
                    break;
                }
            }
            return retval;
        }
        public static MotifEntree FindById(string id)
        {
            MotifEntree retval = null;
            string fid = Forma.TrimUpper(id);
            if (_lesMotifsEntrees.TryGetValue(fid, out MotifEntree? value))
            {
                retval = value;
            }
            return retval;
        }
        public static void Remove(MotifEntree motifs)
        {
            if (FindById(motifs.Id) == null)
            {
                throw new Exception($"[AllMotifsEntrees] cet type n'est encore enregistré : {motifs.Libele}");
            }
            _lesMotifsEntrees.Remove(motifs.Id);
        }

        public static int DB_Add(MotifEntree motif)
        {
            int ret = 0;
            if (DB_MotifEntree.UnMotifById(motif) == null)
            {
                ret = DB_MotifEntree.Add(motif);
            }
            return ret;
        }
        public static int DB_Update(MotifEntree motif)
        {
            int ret = 0;
            if (DB_MotifEntree.UnMotifById(motif) != null)
            {
                ret = DB_MotifEntree.Update(motif);
            }
            return ret;
        }
        public static int DB_Delete(MotifEntree motif)
        {
            int ret = 0;
            if (DB_MotifEntree.UnMotifById(motif) != null)
            {
                ret = DB_MotifEntree.Delete(motif);
            }
            return ret;
        }
        public static void DB_Sync()
        {
            _lesMotifsEntrees = new Dictionary<string, MotifEntree>(DB_MotifEntree.All_From_Db());
        }
    }
    public static class AllMotifsSortie
    {
        private static Dictionary<string, MotifSortie> _lesMotifsSorties;
        private static int _num;

        static AllMotifsSortie()
        {
            _lesMotifsSorties = new Dictionary<string, MotifSortie>();
            _num = 0;
        }

        public static int Num
        {
            get
            {
                if (Count > 0)
                {
                    _num = Forma.LastNumero(_lesMotifsSorties);
                }
                return _num;
            }
        }
        public static int Count
        {
            get { return _lesMotifsSorties.Count; }
        }
        public static string LesSorties
        {
            get
            {
                int i = 0;
                string retVal = string.Format($"{"{0,-4} {1,-19} {2,-12} {3,-15} {4,-20}\n"}",
                    "N°", "Id", "Date Crea.", "Libelle", "Details");

                foreach (MotifSortie dm in _lesMotifsSorties.Values)
                {
                    i++;
                    retVal += string.Format($"{"{0,-4} {1,-19} {2,-12} {3,-15} {4,-20}\n"}",
                    $"{i}°",
                    $"{dm.Id}",
                    $"{dm.DateCreation.ToString("dd-MM-yyyy")}",
                    $"{dm.Libele}",
                    $"{dm.Details}");
                }

                return $"Liste des Motifs Sortie [{Count}]\n\n" + retVal;
            }
        }
        public static Dictionary<string, MotifSortie> LesStocks
        {
            get { DB_Sync(); return _lesMotifsSorties; }
        }

        public static void Add(MotifSortie motifs)
        {

            if (FindByLibelle(motifs.Libele) != null)
            {
                throw new Exception($"[AllMotifsEntrees] cet motif est deja enregistré : {motifs.Libele}");
            }
            _num++;
            _lesMotifsSorties.Add(motifs.Id, motifs);
        }
        public static MotifSortie? FindByLibelle(string libele)
        {
            MotifSortie? retval = null;
            string formatlibele = Forma.TrimUpper(libele);
            foreach (MotifSortie l in _lesMotifsSorties.Values)
            {
                if (l.Libele == formatlibele)
                {
                    retval = l; break;
                }
            }

            return retval;
        }
        public static MotifSortie? FindById(string id)
        {
            MotifSortie? retval = null;
            string formatid = Forma.TrimUpper(id);
            if (_lesMotifsSorties.ContainsKey(formatid))
            {
                retval = _lesMotifsSorties[formatid];
            }
            return retval;
        }
        public static void Remove(string id)
        {
            string formatLibele = Forma.TrimUpper(id);
            if (FindById(formatLibele) == null)
            {
                throw new Exception($"[AllMotifsEntrees] cet motif n'est encore enregistré : {formatLibele}");
            }
            _lesMotifsSorties.Remove(formatLibele);
        }

        public static int DB_Add(MotifSortie motif)
        {
            int ret = 0;
            if (DB_MotifSortie.UnMotifById(motif) == null)
            {
                ret = DB_MotifSortie.Add(motif);
            }
            return ret;
        }
        public static int DB_Update(MotifSortie motif)
        {
            int ret = 0;
            if (DB_MotifSortie.UnMotifById(motif) != null)
            {
                ret = DB_MotifSortie.Update(motif);
            }
            return ret;
        }
        public static int DB_Delete(MotifSortie motif)
        {
            int ret = 0;
            if (DB_MotifSortie.UnMotifById(motif) != null)
            {
                ret = DB_MotifSortie.Delete(motif);
            }
            return ret;
        }

        public static void DB_Sync()
        {
            _lesMotifsSorties = new Dictionary<string, MotifSortie>(DB_MotifSortie.All_From_Db());
        }
    }
}
