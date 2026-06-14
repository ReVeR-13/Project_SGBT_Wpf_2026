using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_App_Pattoon_Animalerie.Modele
{
    public static class ExceptionLauncher
    {
        public static void New(string zone,string message)
        {
            throw new Exception($"[{zone}] {message}");
        }
    }
}
