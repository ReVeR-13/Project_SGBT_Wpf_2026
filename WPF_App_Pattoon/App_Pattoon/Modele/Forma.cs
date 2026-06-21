using System.Text.RegularExpressions;
using System.Windows;
using Wpf_App_Pattoon_Animalerie.AccessDB;
using Wpf_App_Pattoon_Animalerie.Service;


namespace Wpf_App_Pattoon_Animalerie.Modele
{
    public static class Forma
    {
        public static int tailleLignes = 100;

        public static string Center(string text)
        {
            int lenght = tailleLignes / 2 - text.Length / 2;
            if (lenght <= 0)
            {
                lenght = 2;
            }
            string ret = new(' ', lenght);

            return ret + text;
        }
        public static string Center(string text, int taille)
        {
            int lenght = taille / 2 - text.Length / 2;
            if (lenght <= 0)
            {
                lenght = 2;
            }
            string ret = new(' ', lenght);

            return ret + text;
        }
        public static string Section(string titre)
        {
            string ret = "\n" + Padding(Center(titre, 35)) + "\n" +
                         Padding(new string('-', 35)) + "\n";
            return ret;
        }
        public static string Padding(string text)
        {
            string ret = new(' ', 5);

            return ret + text;
        }
        public static string Padding(string text, int pixel)
        {
            string ret = new(' ', pixel);

            return ret + text;
        }


        public static string? TrimUpper(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Trim().ToUpper();
            }
            return str;
        }
        public static string SimpleId(string prefixe, int numero)
        {
            return $"{prefixe}{DateTime.Now:yyMMdd}-{numero:D5}";
        }
        public static int LastNumero<T>(Dictionary<string, T> valuePairs) where T : ITable
        {
            return valuePairs.Values.Where(a => a.DateCreation.Date == DateTime.Today).Count();
        }

        public static string IdBuilder_Animal()
        {
            DateTime dte = DateTime.Today;
            int n = AllAnimal.NumAnimaux + 1;
            string retVal = $"{dte:yyyyMMdd}{n:D5}";
            return retVal;

        }

        public static string Texta2(string clef, string valeur)
        {
            return Padding(string.Format("{0 ,-15} : {1,-100}\n", clef, valeur));
        }

        public static string Text(string a1, string a2, string a3, string a4, string a5, string a6, string a7, string a8, string a9, string a10)
        {

            return string.Format($"{"{0,-4} | {1,-30} | {2,-10} | {3,-15} | {4,-15} | {5,-15} | {6,-20} | {7,-20} | {8,-20} | {9,-20}\n"}",
                        $"{a1}",
                        $"{a2}",
                        $"{a3}",
                        $"{a4}",
                        $"{a5}",
                        $"{a6}",
                        $"{a7}",
                        $"{a8}",
                        $"{a9}",
                        $"{a10}");
        }
        public static string Text(string a1, string a2, string a3, string a4, string a5, string a6, string a7, string a8, string a9)
        {
            return string.Format($"{"{0,-4} | {1,-30} | {2,-10} | {3,-15} | {4,-15} | {5,-20} | {6,-20} | {7,-20} | {8,-20}\n"}",
                        $"{a1}",
                        $"{a2}",
                        $"{a3}",
                        $"{a4}",
                        $"{a5}",
                        $"{a6}",
                        $"{a7}",
                        $"{a8}",
                        $"{a9}");
        }
        public static string Text(string a1, string a2, string a3, string a4, string a5, string a6, string a7, string a8)
        {
            return string.Format($"{"{0,-4} | {1,-30} | {2,-10} | {3,-15} | {4,-20} | {5,-20} | {6,-20} | {7,-20}\n"}",
                        $"{a1}",
                        $"{a2}",
                        $"{a3}",
                        $"{a4}",
                        $"{a5}",
                        $"{a6}",
                        $"{a7}",
                        $"{a8}");
        }
        public static string Text(string a1, string a2, string a3, string a4, string a5, string a6, string a7)
        {
            return string.Format($"{"{0,-4} | {1,-30} | {2,-10} | {3,-15} | {4,-20} | {5,-20} | {6,-20}\n"}",
                        $"{a1}",
                        $"{a2}",
                        $"{a3}",
                        $"{a4}",
                        $"{a5}",
                        $"{a6}",
                        $"{a7}");
        }
        public static string Text(string a1, string a2, string a3, string a4, string a5, string a6)
        {
            return string.Format($"{"{0,-4} | {1,-30} | {2,-10} | {3,-15} | {4,-15} | {5,-20}\n"}",
                        $"{a1}",
                        $"{a2}",
                        $"{a3}",
                        $"{a4}",
                        $"{a5}",
                        $"{a6}");
        }
        public static string Text(string a1, string a2, string a3, string a4, string a5)
        {
            return string.Format($"{"{0,-4} | {1,-30} | {2,-10} | {3,-15} | {4,-20}\n"}",
                        $"{a1}",
                        $"{a2}",
                        $"{a3}",
                        $"{a4}",
                        $"{a5}");
        }
        public static string Text(string a1, string a2, string a3, string a4)
        {
            return string.Format($"{"{0,-4} | {1,-30} | {2,-10} | {3,-15}\n"}",
                        $"{a1}",
                        $"{a2}",
                        $"{a3}",
                        $"{a4}");
        }

        public static bool IsNumeric(string value)
        {
            return value.All(char.IsDigit);
        }
        public static bool IsCaractere(string value)
        {
            return value.All(char.IsLetter);
        }
        public static bool IsNum(string num)
        {
            bool retVal = false;
            Regex regex = new Regex(@"[0-9]{10}");

            if (!string.IsNullOrEmpty(num.Trim()) || regex.Match(num.Trim()).Success)
            {
                retVal = true;
            }
            return retVal;
        }
        public static bool IsMail(string email)
        {
            bool retval = false;
            if (!string.IsNullOrEmpty(email.Trim()) || email.Trim().IndexOf("@") == 1)
            {
                retval = true;
            }
            return retval;
        }

        public static string Checked_Id(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ExceptionLauncher.New("ID", "Donnees invalide");
            }
            return TrimUpper(id);
        }
        public static DateTime Checked_DateCreation(DateTime date)
        {
            if (date > DateTime.Now)
            {
                ExceptionLauncher.New("Date Creation", "La Date entree n est pas valide");
            }
            return date;
        }
        public static void ParametreNullTesteur(object obj)
        {
            if (obj == null)
            {
                ExceptionLauncher.New("Parametre Null Testeur", "Parametre null");
            }
        }

        public static void MsgInfo(string titre, string msg)
        {
            _ = MessageBox.Show
                    (
                        $"{msg}",
                        $"{titre}",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
        }
        public static MessageBoxResult MsgValidation(string titre, string msg)
        {
            MessageBoxResult msgBox = MessageBox.Show
                    (
                        $"{msg}",
                        $"{titre}",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question
                    );
            return msgBox;
        }

        public static string InfoDemande(Demande? demande)
        {
            string ret = $"[ Etape 1/5 ] Selectionner / Créer une demande";
            if (demande != null)
            {
                switch (demande.Statut)
                {
                    case EStatutDemande.EXAMINATION:
                        {
                            ret = $"[ Etape 2/5 ] {demande.Type} : a créé pour cette demande n° [ {demande.Id} ]";
                        }
                        break;
                    case EStatutDemande.VALIDATION:
                        {
                            ret = $"[ Etape 3/5 ] Cette Demande est en attente de Decision : voir [ {demande.Type} ] ";
                        }
                        break;
                    case EStatutDemande.EN_COURS:
                        {

                            ret = $"[ Etape 4/5 ] Une Sortie/Entrée doit etre créé pour cette demande n° {demande.Id}";

                        }
                        break;
                    case EStatutDemande.TERMINEE:
                        {
                            ret = $"[ Etape 5/5 ] Cette demande est Terminée {demande.Id}";
                        }
                        break;
                    case EStatutDemande.ANNULEE:
                        {
                            ret = $"[ Etape X/5 ] Cette demande est Annulléé {demande.Id}";
                        }
                        break;

                }
            }

            return ret;
        }

        public static void SyncAllWithDB()
        {
            DB_Couleur.All_From_db();
            DB_Abri.All_From_db();
            DB_TypeAnimal.AllTypesAnimal();
            DB_Animal.Db_listeAnimaux();

            DB_TypeContact.All_From_Db();
            DB_Contact.All_From_Db();
            DB_TypeCnt_Contact.AllRoles();
            DB_AnimalCouleur.All_From_Db();

            DB_Vaccin.All_From_Db();
            DB_Vaccination.All_From_Db();

            DB_Compatibilite.All_From_Db();
            DB_AnimalCompatibilité.All_From_Db();

            DB_Demande.All_From_Db();

            DB_MotifEntree.All_From_Db();
            DB_MotifSortie.All_From_Db();

            DB_Entree.All_From_Db();
            DB_Sortie.All_From_Db();

            DB_Adoption.All_From_Db();
            DB_Accueil.All_From_Db();
            DB_User.All_From_Db();
        }
    }
}
