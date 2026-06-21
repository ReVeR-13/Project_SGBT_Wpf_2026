using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_App_Pattoon_Animalerie.AccessDB;
using Wpf_App_Pattoon_Animalerie.Modele;

namespace Wpf_App_Pattoon_Animalerie.Service
{
    public static class AllAbri
    {
        private static Dictionary<string, Abri> _lesAbris;
        private static int _num;
        static AllAbri()
        {
            _lesAbris = new Dictionary<string, Abri>();
            _num = 0;
        }
        public static void Add(Abri abri)
        {
            if (FindbyNom(abri.Libelle) != null)
            {
                ExceptionLauncher.New("Add AllAbri", "Cet nom d'abri existe deja");
            }
            _num++;
            _lesAbris.Add(abri.Id, abri);
        }
        //---------------------------------------
        public static int DB_Add(Abri abri)
        {
            int ret = 0;
            if (FindbyNom(abri.Libelle) == null)
            {
                ret = DB_Abri.Add(abri);
            }
            return ret;
        }
        public static int DB_Update(Abri abri)
        {
            return DB_Abri.Update(abri);
        }
        public static int DB_Delete(Abri abri)
        {
            int ret = 0;
            if (DB_Abri.UnAbriId_db(abri.Id) != null)
            {
                ret = DB_Abri.Delete(abri.Id);
            }
            return ret;
        }

        public static void DB_Sync()
        {
            _lesAbris = new Dictionary<string, Abri>(DB_Abri.All_From_db());
        }
        //----------------------------------------
        public static void Remove(string id)
        {
            string f_id = Forma.TrimUpper(id);
            if (Find(f_id) == null)
            {
                ExceptionLauncher.New("Delete All_From_db", "Cet statut n'est pas en liste");
            }
            DB_Abri.Delete(f_id);
            _lesAbris.Remove(f_id);
        }
        public static Abri? Find(string id)
        {
            Abri abri = null;
            string f_id = Forma.TrimUpper(id);
            if (_lesAbris.ContainsKey(f_id))
            {
                abri = _lesAbris[f_id];
            }
            return abri;
        }
        public static Abri? FindbyNom(string libelle)
        {
            Abri? abri = null;
            string f_libelle = Forma.TrimUpper(libelle);
            foreach (Abri ab in _lesAbris.Values)
            {
                if (ab.Libelle == f_libelle)
                {
                    abri = ab;
                }
            }

            return abri;
        }
        public static string LesAbris
        {
            get
            {
                int i = 0;
                string retVal = $"Liste des Abris [{Count}]\n\n" +
                    Forma.Text("N°", "Id", "Date Crea.", "Libelle", "Decision", "Desciption");
                foreach (Abri a in _lesAbris.Values)
                {
                    i++;
                    retVal += Forma.Text(
                    $"{i}",
                    $"{a.Id}",
                    $"{a.DateCreation:dd-MM-yyyy}",
                    $"{a.Libelle}",
                    $"{a.Statut}",
                    $"{a.Description}");
                }
                return retVal;
            }
        }
        public static Dictionary<string, Abri> LesStockAbris
        {
            get 
            {
                DB_Sync();
                return _lesAbris; 
            }
        }
        public static int Count
        {
            get
            {
                return _lesAbris.Count;
            }
        }
        public static int Num
        {
            get
            {
                if (Count > 0)
                {
                    _num = Forma.LastNumero(_lesAbris);
                }
                return _num;
            }
        }
        public static IEnumerable<Abri> Get()
        {
            foreach (Abri sta in _lesAbris.Values)
            {
                yield return sta;
            }
        }

    }
}
