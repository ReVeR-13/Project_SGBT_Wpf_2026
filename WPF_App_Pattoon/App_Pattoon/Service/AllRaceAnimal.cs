using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_App_Pattoon_Animalerie.Modele;

namespace Wpf_App_Pattoon_Animalerie.Service
{
    public static class AllRaceAnimal
    {
        private static readonly Dictionary<string, RaceAnimal> _lesRaces;
        private static int _num;

        static AllRaceAnimal()
        {
            _lesRaces = [];
            _num = 0;
        }

        public static Dictionary<string, RaceAnimal> LesRaces
        {
            get { return _lesRaces; }
        }
        public static int Num
        {
            get
            {
                if (Count > 0)
                {
                    _num = Forma.LastNumero(_lesRaces);
                }
                return _num;
            }
        }
        public static int Count
        {
            get { return LesRaces.Count; }
        }

        public static string LesRacesToString()
        {
            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Type", "Nom");
            foreach (RaceAnimal a in LesRaces.Values)
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Type.Nom}",
                $"{a.Nom}");
            }
            return Forma.Center($"Liste des Races Animal [{i}/{Count}]\n\n") + retVal;
        }
        public static string LesRacesToString(TypeAnimal type)
        {
            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Type", "Nom");
            foreach (RaceAnimal a in Find(type))
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Type.Nom}",
                $"{a.Nom}");
            }
            return Forma.Center($"Liste des Races Animal [{i}/{Count}]\n\n") + retVal;
        }

        public static void Add(RaceAnimal raceAnimal)
        {
            if (Find(raceAnimal.Id) != null)
            {
                ExceptionLauncher.New("AllRaceAnimal Add", "Cet Id existe deja");
            }
            if (Find(raceAnimal.Type, raceAnimal.Nom) != null)
            {
                ExceptionLauncher.New("AllRaceAnimal Add", $"Il Existe deja ce nom [{raceAnimal.Nom}] pour ce type[{raceAnimal.Type.Nom}]");
            }
            _num++;
            _lesRaces.Add(raceAnimal.Id, raceAnimal);
        }
        public static void Delete(RaceAnimal raceAnimal)
        {
            if (Find(raceAnimal.Id) == null)
            {
                ExceptionLauncher.New("AllRaceAnimal Delete", "Cet Id n'existe pas");
            }
            _lesRaces.Remove(raceAnimal.Id);
        }

        public static RaceAnimal? Find(string id)
        {
            Forma.ParametreNullTesteur(id);

            RaceAnimal? race = null;
            if (_lesRaces.ContainsKey(Forma.TrimUpper(id)))
            {
                race = _lesRaces[Forma.TrimUpper(id)];
            }
            return race;
        }
        public static RaceAnimal? Find(TypeAnimal type, string nom)
        {
            Forma.ParametreNullTesteur(nom);

            RaceAnimal? race = (RaceAnimal?)_lesRaces.Values.Select(a => a.Nom == Forma.TrimUpper(nom) && a.Type == type);

            return race;
        }
        public static IEnumerable<RaceAnimal> FindByNom(string nom)
        {
            Forma.ParametreNullTesteur(nom);

            RaceAnimal? race = null;
            foreach (RaceAnimal v in _lesRaces.Values.Where(a => a.Nom == Forma.TrimUpper(nom)))
            {
                yield return v;

            }
        }
        public static IEnumerable<RaceAnimal> Find(TypeAnimal type)
        {
            Forma.ParametreNullTesteur(type);

            RaceAnimal? race = null;
            foreach (RaceAnimal v in _lesRaces.Values.Where(a => a.Type == type))
            {
                yield return v;

            }
        }


    }
}
