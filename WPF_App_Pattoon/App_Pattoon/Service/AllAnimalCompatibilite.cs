
using Wpf_App_Pattoon_Animalerie.AccessDB;
using Wpf_App_Pattoon_Animalerie.Modele;

namespace Wpf_App_Pattoon_Animalerie.Service
{
    public static class AnimalCompatibilitéService
    {
        private static readonly Dictionary<string, AnimalCompatibilité> _lesCompatibilite;
        static AnimalCompatibilitéService()
        {
            _lesCompatibilite = new Dictionary<string, AnimalCompatibilité>();
        }
        public static int Count
        {
            get { return _lesCompatibilite.Count; }
        }
        public static string LesCompatibilites
        {
            get
            {
                int i = 0;
                string retVal = $"Liste de toutes les Compatibilités [{Count}]\n" +
                    string.Format($"{"{0,-4} {1,-30} {2,-11} {3,-14} {4,-14} {5,-10} {6,-10}\n"}",
                                     "N°", "Id", "Date Crea.", "Animal", "Nom Compat.", "Compatible", "Remarque");
                foreach (AnimalCompatibilité vc in _lesCompatibilite.Values)
                {
                    i++;
                    retVal += string.Format($"{"{0,-4} {1,-30} {2,-11} {3,-14} {4,-14} {5,-10} {6,-10}\n"}",
                             $"{i}",
                             $"{vc.Id}",
                             $"{vc.DateCreation.ToString("dd-MM-yyyy")}",
                             $"{vc.Animal.Nom}",
                             $"{vc.Compatibilite.Nom}",
                             vc.Compatible == true ? "Oui" : "Non",
                             vc.Remaque ?? "--");
                }

                return retVal;
            }
        }
        public static string LesDerniersCompatibilites
        {
            get
            {
                int i = 0;
                string retVal = Forma.Text("N°", "Id", "Date Crea.", "Animal", "Nom Compat.", "Compatible", "Remarque");
                foreach (AnimalCompatibilité vc in _lesCompatibilite.Values.OrderByDescending(a => a.DateCreation).Take(10))
                {
                    i++;
                    retVal += Forma.Text(
                             $"{i}",
                             $"{vc.Id}",
                             $"{vc.DateCreation:dd-MM-yyyy}",
                             $"{vc.Animal.Nom}",
                             $"{vc.Compatibilite.Nom}",
                             vc.Compatible == true ? "Oui" : "Non",
                             vc.Remaque ?? "--");
                }

                return $"Liste des Compatibilités testés [{i}/{Count}]\n" + retVal;
            }
        }
        public static void Add(AnimalCompatibilité acomp)
        {
            if (_lesCompatibilite.ContainsKey(acomp.Id))
            {
                ExceptionLauncher.New("AnimalCompatibilité", "Cet Animal possede deja cet compatibilité");
            }
            _lesCompatibilite.Add(acomp.Id, acomp);
        }
        public static void Remove(string id)
        {
            if (!_lesCompatibilite.ContainsKey(id))
            {
                ExceptionLauncher.New("AnimalCompatibilité", "Cet Animal ne possede pas cet compatibilité");
            }
            _lesCompatibilite.Remove(id);
        }
        public static void RemoveAllbyAnimal(Animal animal)
        {

            if (animal == null)
            {
                ExceptionLauncher.New("AnimalCompatibilité", "Cet Animal est null");
            }

            foreach (AnimalCompatibilité c in FindAllByAnimal(animal).Values)
            {
                Remove(c.Id);
            }

        }

        public static AnimalCompatibilité Find(string id)
        {
            AnimalCompatibilité? retval = null;
            string f_id = Forma.TrimUpper(id);
            if (_lesCompatibilite.ContainsKey(f_id))
            {
                retval = _lesCompatibilite[f_id];
            }
            return retval;
        }
        public static AnimalCompatibilité Find(Animal animal, Compatibilite compatibilite)
        {
            AnimalCompatibilité? retval = null;
            foreach (AnimalCompatibilité c in _lesCompatibilite.Values)
            {
                if (c.Animal == animal && c.Compatibilite == compatibilite)
                {
                    retval = c; break;
                }
            }

            return retval;
        }
        public static string ListeByAnimal(string idAnimal)
        {
            string f_id = Forma.TrimUpper(idAnimal);
            int i = 0;
            string retval = string.Format($"{"{0,-4} {1,-16} {2,-11} {3,-10} {4,-10} {5,-10\n"}",
                                     "N°", "Id", "Date Crea.", "MyType", "Compatible", "Remarque");
            foreach (AnimalCompatibilité av in _lesCompatibilite.Values)
            {
                if (av.Animal.Id == f_id)
                {
                    i++;
                    retval += string.Format($"{"{0,-4} {1,-16} {2,-11} {3,-10} {4,-10} {5,-10\n"}",
                             $"{i}",
                             $"{av.Id}",
                             $"{av.DateCreation.ToString("dd-MM-yyyy")}",
                             $"{av.Compatibilite.Nom}",
                             av.Compatible == true ? "Oui" : "Non",
                             $"{av.Remaque}"); ;
                }
            }
            return $"Liste des Compatibilités de {AllAnimal.Rechercher(f_id).Nom} [{i}]\n" + retval;
        }
        public static string ListeByCompatibilite(string idcomp)
        {
            string f_id = Forma.TrimUpper(idcomp);
            int i = 0;
            string retval = string.Format($"{"{0,-4} {1,-30} {2,-11} {3,-14} {4,-10} {5,-10} {6,-10}\n"}",
                                     "N°", "Id", "Date Crea.", "Animal", "MyType", "Compatible", "Remarque");
            foreach (AnimalCompatibilité av in _lesCompatibilite.Values)
            {
                if (av.Compatibilite.Id == f_id)
                {
                    i++;
                    retval += string.Format($"{"{0,-4} {1,-30} {2,-11} {3,-14} {4,-10} {5,-10} {6,-10}\n"}",
                             $"{i}",
                             $"{av.Id}",
                             $"{av.DateCreation.ToString("dd-MM-yyyy")}",
                             $"{av.Animal.Nom}",
                             $"{av.Compatibilite.Nom}",
                             av.Compatible == true ? "Oui" : "Non",
                             $"{av.Remaque}");
                }
            }
            return $"Liste des animaux ayant la compatibilité de type: {AllCompatibilite.Find(f_id).Nom} [{i}]\n" + retval;
        }

        public static Dictionary<string, AnimalCompatibilité> FindAllByAnimal(Animal animal)
        {
            Dictionary<string, AnimalCompatibilité> retval = new Dictionary<string, AnimalCompatibilité>();

            foreach (AnimalCompatibilité av in _lesCompatibilite.Values.Where(a => a.Animal == animal))
            {

                retval.Add(av.Id, av);

            }
            return retval;
        }
        public static Dictionary<string, AnimalCompatibilité> FindAllByCompatibilite(Compatibilite comp)
        {
            Dictionary<string, AnimalCompatibilité> retval = new Dictionary<string, AnimalCompatibilité>();

            foreach (AnimalCompatibilité av in _lesCompatibilite.Values.Where(a => a.Compatibilite == comp))
            {
                retval.Add(av.Id, av);
            }
            return retval;
        }
        public static Dictionary<string, AnimalCompatibilité> FindAllByCompatibilite(string idcomp, bool compatible)
        {
            Dictionary<string, AnimalCompatibilité> retval = new Dictionary<string, AnimalCompatibilité>();
            string f_id = Forma.TrimUpper(idcomp);

            foreach (AnimalCompatibilité av in _lesCompatibilite.Values)
            {
                if (av.Compatibilite.Id == f_id && av.Compatible == compatible)
                {
                    retval.Add(av.Id, av);
                }
            }
            return retval;
        }

        public static int DB_Add(AnimalCompatibilité animalCompatibilité)
        {
            int ret = 0;
            if (DB_AnimalCompatibilité.UnCompatibiliteById(animalCompatibilité.Id) == null)
            {
                ret = DB_AnimalCompatibilité.Add(animalCompatibilité);
            }
            return ret;
        }
        public static int DB_Update(AnimalCompatibilité animalCompatibilité)
        {
            int ret = 0;
            if (DB_AnimalCompatibilité.UnCompatibiliteById(animalCompatibilité.Id) != null)
            {
                ret = DB_AnimalCompatibilité.Update(animalCompatibilité);
            }
            return ret;
        }
        public static int DB_Delete(AnimalCompatibilité animalCompatibilité)
        {
            int ret = 0;
            if (DB_AnimalCompatibilité.UnCompatibiliteById(animalCompatibilité.Id) != null)
            {
                ret = DB_AnimalCompatibilité.Delete(animalCompatibilité.Id);
            }
            return ret;
        }
    }
}
