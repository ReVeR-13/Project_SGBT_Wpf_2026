using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_App_Pattoon_Animalerie.AccessDB;
using Wpf_App_Pattoon_Animalerie.Modele;

namespace Wpf_App_Pattoon_Animalerie.Service
{
    public static class AllAnimalCouleur
    {
        private static readonly Dictionary<string, AnimalCouleur> _lesAnimalCouleurs;

        static AllAnimalCouleur()
        {
            _lesAnimalCouleurs = [];
        }

        public static int Count
        {
            get { return _lesAnimalCouleurs.Count; }
        }
        public static string Liste
        {
            get
            {
                int i = 0;
                string retVal = Forma.Text("N°", "Id", "Date Crea.", "Id Animal", "Id Couleur");

                foreach (AnimalCouleur dm in _lesAnimalCouleurs.Values)
                {
                    i++;
                    retVal += Forma.Text(
                    $"{i}",
                    $"{dm.Id}",
                    $"{dm.DateCreation:dd-MM-yyyy}",
                    $"{dm.Animal.Id}",
                    $"{dm.Couleur.Id}");
                }

                return $"Liste des Couleurs Appliqués [{Count}]\n\n" + retVal;
            }
        }
        public static string DerniersListe
        {
            get
            {
                int i = 0;
                string retVal = Forma.Text("N°", "Id", "Date Crea.", "Id Animal", "Id Couleur");

                foreach (AnimalCouleur dm in _lesAnimalCouleurs.Values.OrderByDescending(a => a.DateCreation).Take(10))
                {
                    i++;
                    retVal += Forma.Text(
                    $"{i}",
                    $"{dm.Id}",
                    $"{dm.DateCreation:dd-MM-yyyy}",
                    $"{dm.Animal.Id}",
                    $"{dm.Couleur.Id}");
                }

                return $"Liste des Couleurs Appliqués [{i}/{Count}]\n\n" + retVal;
            }
        }


        public static int Add(AnimalCouleur couleur)
        {
            if (Find(couleur.Id) != null)
            {
                ExceptionLauncher.New("AllAnimalCouleur Add", "cette application de couleur existe deja");
            }
            _lesAnimalCouleurs.Add(couleur.Id, couleur);
            return 1;
        }
        public static int Delete(string id)
        {
            string fid = Forma.TrimUpper(id);
            if (Find(fid) == null)
            {
                ExceptionLauncher.New("AllAnimalCouleur Delete", "cette application de couleur n 'existe pas");
            }

            _lesAnimalCouleurs.Remove(fid);
            return 1;
        }

        public static AnimalCouleur? Find(string id)
        {
            AnimalCouleur? an = null;
            string fid = Forma.TrimUpper(id);
            if (_lesAnimalCouleurs.TryGetValue(fid, out AnimalCouleur? value))
            {
                an = value;
            }
            return an;
        }
        public static Dictionary<string, AnimalCouleur> FindAllByAnimal(Animal animal)
        {
            Forma.ParametreNullTesteur(animal);

            Dictionary<string, AnimalCouleur> retval = [];

            if (Count > 0)
            {
                foreach (AnimalCouleur c in _lesAnimalCouleurs.Values)
                {
                    if (c.Animal == animal) retval.Add(c.Id, c);
                }

            }
            return retval;
        }
        public static Dictionary<string, AnimalCouleur> FindAllByCouleur(Couleur couleur)
        {
            Forma.ParametreNullTesteur(couleur);

            Dictionary<string, AnimalCouleur> retval = [];

            if (Count > 0)
            {
                foreach (AnimalCouleur c in _lesAnimalCouleurs.Values)
                {
                    if (c.Couleur == couleur) retval.Add(c.Id, c);
                }

            }
            return retval;
        }

        public static int DB_Add(AnimalCouleur an)
        {
            Forma.ParametreNullTesteur(an);
            int retval = 0;
            if (DB_AnimalCouleur.UneColorationById(an.Id) == null)
            {
                retval = DB_AnimalCouleur.Add(an);
            }
            return retval;
        }
        public static int DB_Delete(AnimalCouleur an)
        {
            Forma.ParametreNullTesteur(an);
            int retval = 0;
            if (DB_AnimalCouleur.UneColorationById(an.Id) != null)
            {
                retval = DB_AnimalCouleur.Delete(an.Id);
            }
            return retval;
        }

    }
}
