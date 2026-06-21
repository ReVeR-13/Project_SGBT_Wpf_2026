using Wpf_App_Pattoon_Animalerie.AccessDB;
using Wpf_App_Pattoon_Animalerie.Modele;

namespace Wpf_App_Pattoon_Animalerie.Service
{
    public static class AllCompatibilite
    {
        private static Dictionary<string, Compatibilite> _lesCompatibilites;
        private static int _num;

        static AllCompatibilite()
        {
            _num = 0;
            _lesCompatibilites = [];
        }

        public static int Num
        {
            get
            {
                if (Count > 0)
                {
                    _num = Forma.LastNumero(_lesCompatibilites);
                }
                return _num;
            }
        }
        public static int Count
        {
            get { return _lesCompatibilites.Count; }
        }
        public static string LesCompatibilites
        {
            get
            {
                int i = 0;
                string retVal =
                    Forma.Text("N°", "ID", "Date Crea.", "Nom", "Details");
                foreach (Compatibilite vc in _lesCompatibilites.Values)
                {
                    i++;
                    retVal += Forma.Text(
                             $"{i}",
                             $"{vc.Id}",
                             $"{vc.DateCreation.ToString("dd-MM-yyyy")}",
                             $"{vc.Nom}",
                             $"{vc.Details}");
                }
                return $"Liste des Compatibilités [{i}/{Count}]\n" + retVal;
            }
        }
        public static Dictionary<string, Compatibilite> LesStocks
        {
            get { DB_Sync(); return _lesCompatibilites; }
        }

        public static string ManquantsString(Animal ami)
        {
            int i = 0;
            string retVal = Forma.Padding(new string('-', 90)) + "\n" +
                Forma.Padding(Forma.Text("N°", "Date Crea.", "Nom", "Details"));

            foreach (Compatibilite vc in _lesCompatibilites.Values)
            {
                bool veri = true;

                foreach (AnimalCompatibilité va in AnimalCompatibilitéService.FindAllByAnimal(ami).Values)
                {
                    if (va.Compatibilite == vc)
                    {
                        veri = false;
                    }
                }

                if (veri)
                {
                    i++;
                    retVal += Forma.Padding(Forma.Text(
                             $"{i}",
                             $"{vc.DateCreation:dd-MM-yyyy}",
                             $"{vc.Nom}",
                             $"{vc.Details}"));
                }

            }

            return Forma.Center($"Liste des Compatibilités manquants [{i}/{Count}]\n") + retVal;
        }
        public static Dictionary<string, Compatibilite> Manquants(Animal ami)
        {
            Dictionary<string, Compatibilite> retVal = [];

            foreach (Compatibilite vc in _lesCompatibilites.Values)
            {
                bool veri = true;

                foreach (AnimalCompatibilité va in AnimalCompatibilitéService.FindAllByAnimal(ami).Values)
                {
                    if (va.Compatibilite.Id == vc.Id)
                    {
                        veri = false;
                    }
                }

                if (veri)
                {
                   
                    retVal.Add(vc.Id,vc);
                }

            }

            return retVal;
        }
        public static IEnumerable<Compatibilite> Get()
        {
            foreach (Compatibilite cont in _lesCompatibilites.Values)
            {
                yield return cont;
            }
        }
        public static Compatibilite Find(string id)
        {
            string f_id = Forma.TrimUpper(id);
            Compatibilite vc = null;
            if (_lesCompatibilites.ContainsKey(f_id))
            {
                vc = _lesCompatibilites[f_id];
            }
            return vc;
        }
        public static Compatibilite FindByNom(string nom)
        {
            string f_nom = Forma.TrimUpper(nom);
            Compatibilite vc = null;
            foreach (Compatibilite c in _lesCompatibilites.Values)
            {
                if (c.Nom == f_nom)
                {
                    vc = c;
                }
            }

            return vc;
        }
        public static void Add(Compatibilite comp)
        {
            if (Find(comp.Id) != null)
            {
                ExceptionLauncher.New("Add Compatibilite", "Cette compatibilé existe deja dans la table");
            }
            _num++;
            _lesCompatibilites.Add(Forma.TrimUpper(comp.Id), comp);
        }
        public static void Remove(Compatibilite compatibilite)
        {
            if (Find(compatibilite.Id) == null)
            {
                ExceptionLauncher.New("RemoveVaccin AllCompatibilite", "Cette compatibilité n'existe pas dans la table");
            }
            _lesCompatibilites.Remove(compatibilite.Id);
        }

        public static int DB_Add(Compatibilite comp)
        {
            int ret = 0;
            if (DB_Compatibilite.UnCompatibiliteById(comp.Id) == null)
            {
                ret = DB_Compatibilite.Add(comp);
            }
            return ret;
        }
        public static int DB_Update(Compatibilite comp)
        {
            return DB_Compatibilite.Update(comp);
        }
        public static int DB_Delete(Compatibilite comp)
        {
            int ret = 0;
            if (DB_Compatibilite.UnCompatibiliteById(comp.Id) != null)
            {
                ret = DB_Compatibilite.Delete(comp.Id);
            }
            return ret;
        }

        public static void DB_Sync()
        {
            _lesCompatibilites = new Dictionary<string, Compatibilite>(DB_Compatibilite.All_From_Db());
        }

    }
}
