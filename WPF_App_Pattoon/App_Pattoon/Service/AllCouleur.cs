using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_App_Pattoon_Animalerie.AccessDB;
using Wpf_App_Pattoon_Animalerie.Modele;

namespace Wpf_App_Pattoon_Animalerie.Service
{
    public static class AllCouleur
    {
        private static Dictionary<string, Couleur> _lesCouleurs;
        private static int _num;

        static AllCouleur()
        {
            _lesCouleurs = [];
            _num = 0;
        }
        public static int Num
        {
            get
            {
                if (Count > 0)
                {
                    _num = Forma.LastNumero(_lesCouleurs);
                }
                return _num;
            }
        }
        public static int Count
        {
            get { return _lesCouleurs.Count; }
        }
        public static string LesCouleurs
        {
            get
            {
                int i = 0;
                string retVal = Forma.Text("N°", "Id", "Date Crea.", "Nom");

                foreach (Couleur dm in _lesCouleurs.Values)
                {
                    i++;
                    retVal += Forma.Text(
                    $"{i}",
                    $"{dm.Id}",
                    $"{dm.DateCreation:dd-MM-yyyy}",
                    $"{dm.Nom}");
                }

                return $"Liste des Couleurs[{i}/{Count}]\n\n" + retVal;
            }
        }
        public static Dictionary<string, Couleur> LesStocks
        {
            get { DB_Sync(); return _lesCouleurs; }
        }
        public static string LesDernieresCouleurs
        {
            get
            {
                int i = 0;
                string retVal = Forma.Text("N°", "Id", "Date Crea.", "Nom");

                foreach (Couleur dm in _lesCouleurs.Values.OrderByDescending(c => c.DateCreation).Take(10))
                {
                    i++;
                    retVal += Forma.Text(
                    $"{i}",
                    $"{dm.Id}",
                    $"{dm.DateCreation:dd-MM-yyyy}",
                    $"{dm.Nom}");
                }

                return $"Liste des Couleurs[{i}/{Count}]\n\n" + retVal;
            }
        }

        public static int Add(Couleur couleur)
        {
            if (couleur == null)
            {
                ExceptionLauncher.New("Couleur Add", "Parametre null");
            }

            if (FindByNom(couleur.Nom) != null)
            {
                ExceptionLauncher.New("Couleur Add", "Cette couleur existe deja");
            }

            _lesCouleurs.Add(couleur.Id, couleur);
            _num++;
            return 1;

        }
        public static int Delete(string id)
        {
            string fid = Forma.TrimUpper(id);

            if (string.IsNullOrEmpty(fid))
            {
                ExceptionLauncher.New("Couleur Delete", "Parametre null");
            }

            if (Find(fid) == null)
            {
                ExceptionLauncher.New("Couleur Delete", "Cette couleur n'existe pas");
            }

            _lesCouleurs.Remove(fid);
            return 1;
        }

        public static Couleur? Find(string id)
        {
            Couleur? couleur = null;
            if (!string.IsNullOrEmpty(id))
            {
                string? id_f = Forma.TrimUpper(id);
                if (_lesCouleurs.TryGetValue(id_f, out Couleur? value))
                {
                    couleur = value;
                }
            }
            return couleur;
        }
        public static Couleur? FindByNom(string nom)
        {
            Couleur? couleur = null;
            if (!string.IsNullOrEmpty(nom))
            {
                string id_f = Forma.TrimUpper(nom);

                foreach (Couleur c in _lesCouleurs.Values)
                {
                    if (c.Nom == id_f)
                    {
                        couleur = c;
                        break;
                    }
                }

            }
            return couleur;
        }

        public static string ManquantsString(Animal animal)
        {
            int i = 0;
            Dictionary<string, AnimalCouleur> coloration = AllAnimalCouleur.FindAllByAnimal(animal);
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Nom");
            foreach (Couleur c in _lesCouleurs.Values)
            {
                bool veri = true;

                if (c == animal.Couleur)
                {
                    veri = false;
                }
                else
                {
                    foreach (AnimalCouleur ca in coloration.Values)
                    {
                        if (ca.Couleur == c)
                        {
                            veri = false;
                        }
                    }
                }


                if (veri)
                {
                    i++;
                    retVal += Forma.Text($"{i}", $"{c.Id}", $"{c.DateCreation:dd-MM-yyyy}", $"{c.Nom}");
                }

            }

            return $"Liste des Couleurs manquants sur - {animal.Nom} - [{i}]\n" + retVal;
        }
        public static Dictionary<string, Couleur> Manquants(Animal animal)
        {
            Dictionary<string, AnimalCouleur> coloration = AllAnimalCouleur.FindAllByAnimal(animal);
            Dictionary<string, Couleur> retVal = [];
            foreach (Couleur c in _lesCouleurs.Values)
            {
                bool veri = true;

                if (c == animal.Couleur)
                {
                    veri = false;
                }
                else
                {
                    foreach (AnimalCouleur ca in coloration.Values)
                    {
                        if (ca.Couleur == c)
                        {
                            veri = false;
                        }
                    }
                }


                if (veri)
                {
                    retVal.Add(c.Id,c);
                }

            }

            return retVal;
        }
        public static IEnumerable<Couleur> Get()
        {
            foreach (Couleur c in _lesCouleurs.Values)
            {
                yield return c;
            }
        }

        public static int DB_Add(Couleur couleur)
        {
            int ret = 0;
            if (DB_Couleur.UnCouleurByNom(couleur.Nom) == null)
            {
                ret = DB_Couleur.Add(couleur);
            }
            return ret;
        }
        public static int DB_Update(Couleur couleur)
        {
            int ret = 0;
            if (DB_Couleur.UnCouleurById(couleur.Id) != null)
            {
                ret = DB_Couleur.Update(couleur);
            }
            return ret;
        }
        public static int DB_Delete(Couleur couleur)
        {
            int ret = 0;
            if (DB_Couleur.UnCouleurById(couleur.Id) != null)
            {
                ret = DB_Couleur.Delete(couleur.Id);
            }
            return ret;
        }

        public static void DB_Sync()
        {
            _lesCouleurs = new(DB_Couleur.All_From_db());
        }

    }
}
