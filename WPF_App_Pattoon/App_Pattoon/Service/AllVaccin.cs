using System.Runtime.CompilerServices;
using Wpf_App_Pattoon_Animalerie.AccessDB;
using Wpf_App_Pattoon_Animalerie.Modele;


namespace Wpf_App_Pattoon_Animalerie.Service
{
    public static class AllVaccin
    {
        private static Dictionary<string, Vaccin> _lesVaccins;
        private static int _numVacc;

        static AllVaccin()
        {
            _lesVaccins = new Dictionary<string, Vaccin>();
            _numVacc = 0;
        }
        public static int Num
        {
            get
            {
                if (Count > 0)
                {
                    _numVacc = Forma.LastNumero(_lesVaccins);
                }
                return _numVacc;
            }
        }
        public static int Count
        {
            get { return _lesVaccins.Count; }
        }
        public static string LesVaccins
        {
            get
            {
                int i = 0;
                string retVal = $"Liste des Vaccins [{Count}]\n" +
                    string.Format($"{"{0,-4} {1,-16} {2,-11} {3,-10} {4,-10}\n"}",
                                     "N°", "Id", "Date Crea.", "Nom", "Descrip.");
                foreach (Vaccin vc in _lesVaccins.Values)
                {
                    i++;
                    retVal += string.Format($"{"{0,-4} {1,-16} {2,-11} {3,-10} {4,-10}\n"}",
                             $"{i}", $"{vc.Id}", $"{vc.DateCreation.ToString("dd-MM-yyyy")}", $"{vc.Nom}", $"{vc.Description}");
                }

                return retVal;
            }
        }
        public static Dictionary<string, Vaccin> StockVaccins
        {
           get 
            {
                if (_lesVaccins.Count == 0)
                {
                    DB_Sync();
                }
                return _lesVaccins; 
            } 
        }
        
        public static string ManquantsString(Animal ami)
        {
            int i = 0;
            Dictionary<string, Vaccination> vaccination = Vaccination.ByAnimal(ami);
            string retVal =
            Forma.Text("N°", "Id", "Date Crea.", "Nom", "Descrip.");
            foreach (Vaccin vc in _lesVaccins.Values)
            {
                bool veri = true;

                foreach (Vaccination va in vaccination.Values)
                {
                    if (va.Vaccin == vc)
                    {
                        veri = false;
                    }
                }

                if (veri)
                {
                    i++;
                    retVal += Forma.Text($"{i}", $"{vc.Id}", $"{vc.DateCreation:dd-MM-yyyy}", $"{vc.Nom}", $"{vc.Description}");
                }

            }

            return Forma.Center($"Liste des Vaccins non effectués sur - {ami.Nom} - [{i}]\n") + retVal;
        }
        public static Dictionary<string, Vaccin> Manquants(Animal ami)
        {
            Dictionary<string, Vaccination> vaccination = Vaccination.ByAnimal(ami);
            Dictionary<string, Vaccin> retVal = [];
            foreach (Vaccin vc in _lesVaccins.Values)
            {
                bool veri = true;

                foreach (Vaccination va in vaccination.Values)
                {
                    if (va.Vaccin == vc)
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
        public static IEnumerable<Vaccin> Get()
        {
            foreach (Vaccin vacc in _lesVaccins.Values)
            {
                yield return vacc;
            }
        }

        public static Vaccin Find(string id)
        {
            string f_id = Forma.TrimUpper(id);
            Vaccin vc = null;
            if (_lesVaccins.ContainsKey(f_id))
            {
                vc = _lesVaccins[f_id];
            }
            return vc;
        }
        public static Vaccin FindByNom(string nom)
        {
            Vaccin vc = null;
            string f_nom = Forma.TrimUpper(nom);
            foreach (Vaccin v in _lesVaccins.Values)
            {
                if (v.Nom == f_nom)
                {
                    vc = v;
                    break;
                }
            }
            return vc;
        }

        public static void Add(Vaccin vaccin)
        {
            if (FindByNom(vaccin.Nom) != null)
            {
                ExceptionLauncher.New("Add AllVaccin", "Cet Vaccin existe deja dans la table des vaccin");
            }
            _numVacc++;
            _lesVaccins.Add(vaccin.Id, vaccin);
        }
        public static void Remove(string id)
        {
            string f_id = Forma.TrimUpper(id);
            if (!_lesVaccins.ContainsKey(f_id))
            {
                ExceptionLauncher.New("RemoveVaccin AllVaccin", "Cet Vaccin n'existe pas dans la table des vaccin");
            }
            _lesVaccins.Remove(f_id);
        }

        public static Dictionary<string, Vaccin> DB_Sync()
        {
            _lesVaccins = new Dictionary<string, Vaccin>(DB_Vaccin.All_From_Db());
            return _lesVaccins;
        }
        public static int DB_Add(Vaccin vaccin)
        {
            int ret = 0;
            if (DB_Vaccin.UnVaccinById(vaccin.Id) == null)
            {
                ret = DB_Vaccin.Add(vaccin);
            }
            return ret;
        }
        public static int DB_Update(Vaccin vaccin)
        {
             return DB_Vaccin.Update(vaccin);  
        }
        public static int DB_Delete(Vaccin vaccin)
        {
            int ret = 0;
            if (DB_Vaccin.UnVaccinById(vaccin.Id) != null)
            {
                ret = DB_Vaccin.Delete(vaccin.Id);
            }
            return ret;
        }

    }
}
