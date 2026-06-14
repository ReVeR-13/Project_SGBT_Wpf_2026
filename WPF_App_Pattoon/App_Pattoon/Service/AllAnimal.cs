using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_App_Pattoon_Animalerie.AccessDB;
using Wpf_App_Pattoon_Animalerie.Modele;

namespace Wpf_App_Pattoon_Animalerie.Service
{
    public static class AllAnimal
    {
        private static readonly Dictionary<string, Animal> _lesAnimaux;
        private static int _numAnimaux;

        static AllAnimal()
        {
            _lesAnimaux = new Dictionary<string, Animal>();
            _numAnimaux = 0;
        }

        public static int NumAnimaux
        {
            get
            {
                if (Count > 0)
                {
                    _numAnimaux = Forma.LastNumero(_lesAnimaux);
                }
                return _numAnimaux;
            }
        }
        public static int Count
        {
            get
            {
                return _lesAnimaux.Count;
            }
        }
        public static string LesAnimaux
        {
            get
            {
                int i = 0;
                string retVal = Forma.Text("N°", "Id", "Date Crea.", "Nom", "Type", "Statut");
                foreach (Animal a in _lesAnimaux.Values)
                {
                    i++;
                    retVal += Forma.Text(
                    $"{i}",
                    $"{a.Id}",
                    $"{a.DateCreation:dd-MM-yyyy}",
                    $"{a.Nom}",
                    $"{a.Type.Nom}",
                    $"{a.Statut}");
                }
                return $"Liste des Animaux [{i}/{Count}]\n\n" + retVal;
            }
        }
        public static string LesAnimauxDerniers()
        {
            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Nom", "Type", "Statut");
            foreach (Animal a in _lesAnimaux.Values.OrderByDescending(ob => ob.DateCreation).Take(10))
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Nom}",
                $"{a.Type.Nom}",
                $"{a.Statut}");
            }

            return $"Liste des Top 10 derniers animaux encore au refuge [{i}/{Count}]\n\n" + retVal;
        }

        public static string ListeByStatut()
        {
            return LesAnimaux;
        }
        public static string ListeByStatut(EStatutAnimal eStatut)
        {
            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Nom", "Type", "Statut");
            foreach (Animal a in _lesAnimaux.Values.Where(a => a.Statut == eStatut))
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Nom}",
                $"{a.Type.Nom}",
                $"{a.Statut}");
            }
            return $"Liste des Animaux [{i}/{Count}]\n\n" + retVal;
        }
        public static string ListeByStatut(EStatutAnimal eStatut, EStatutAnimal yStatut)
        {
            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Nom", "Type", "Statut");
            foreach (Animal a in _lesAnimaux.Values.Where(a => a.Statut == eStatut || a.Statut == yStatut))
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Nom}",
                $"{a.Type.Nom}",
                $"{a.Statut}");
            }
            return $"Liste des Animaux [{i}/{Count}]\n\n" + retVal;
        }
        public static string ListeByStatutNot(EStatutAnimal eStatut)
        {
            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Nom", "Type", "Statut");
            foreach (Animal a in _lesAnimaux.Values.Where(a => a.Statut != eStatut))
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Nom}",
                $"{a.Type.Nom}",
                $"{a.Statut}");
            }
            return $"Liste des Animaux [{i}/{Count} ]\n\n\n" + retVal;
        }
        public static string ListeByStatutNot(EStatutAnimal eStatut, EStatutAnimal yStatut)
        {
            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Nom", "Type", "Statut");
            foreach (Animal a in _lesAnimaux.Values.Where(a => a.Statut != eStatut && a.Statut != yStatut))
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Nom}",
                $"{a.Type.Nom}",
                $"{a.Statut}");
            }
            return $"Liste des Animaux [{i}/{Count}]\n\n" + retVal;
        }

        public static IEnumerable<Animal> Get()
        {
            foreach (Animal animal in _lesAnimaux.Values)
            {
                yield return animal;
            }
        }
        public static IEnumerable<Animal> Get(EStatutAnimal eStatut)
        {
            foreach (Animal animal in _lesAnimaux.Values)
            {
                if (animal.Statut.Equals(eStatut))
                {
                    yield return animal;
                }
            }
        }
        public static IEnumerable<Animal> Get(TypeAnimal type)
        {
            foreach (Animal animal in _lesAnimaux.Values.Where(a => a.Type == type))
            {
                yield return animal;
            }
        }
        public static Animal Rechercher(string idAmi)
        {
            Animal? retVal = null;
            string fid = Forma.TrimUpper(idAmi);
            if (_lesAnimaux.TryGetValue(fid, out Animal? value))
            {
                retVal = value;
            }
            return retVal;
        }

        public static void Add(Animal ami)
        {
            if (Rechercher(ami.Id) != null)
            {
                throw new Exception($"[Groupe Animaux] Cet identifiant existe deja :{ami.Id}");
            }
            _numAnimaux++;
            _lesAnimaux.Add(ami.Id, ami);
        }
        public static void Supprimer(string idami)
        {
            if (Rechercher(idami) == null)
            {
                throw new Exception($"[ Groupe Animaux] Cet Animal est deja supprimé :{idami}");
            }
            _lesAnimaux.Remove(idami);
        }

        public static int DB_Add(Animal ami)
        {
            int retVal = 0;
            if (DB_Animal.UnAnimalById(ami.Id) == null)
            {
                retVal = DB_Animal.Add(ami);
            }
            return retVal;
        }
        public static int DB_Update(Animal ami)
        {
            int retVal = 0;
            if (DB_Animal.UnAnimalById(ami.Id) != null)
            {
                retVal = DB_Animal.Update(ami);
            }
            return retVal;
        }
        public static int DB_UpdateStatut(Animal ami)
        {
            int retVal = 0;
            if (DB_Animal.UnAnimalById(ami.Id) != null)
            {
                retVal = DB_Animal.UpdateStatut(ami);
            }
            return retVal;
        }
        public static int DB_UpdateAbri(Animal ami)
        {
            int retVal = 0;
            if (DB_Animal.UnAnimalById(ami.Id) != null)
            {
                retVal = DB_Animal.UpdateAbri(ami);
            }
            return retVal;
        }
        public static int DB_Delete(Animal animal)
        {
            int retVal = 0;
            if (DB_Animal.UnAnimalById(animal.Id) != null)
            {
                retVal = DB_Animal.Delete(animal.Id);
            }
            return retVal;
        }

    }
}
