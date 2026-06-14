using Wpf_App_Pattoon_Animalerie.Service;
using System.Text.RegularExpressions;


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
    }
}
