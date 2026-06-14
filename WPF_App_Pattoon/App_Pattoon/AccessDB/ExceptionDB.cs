using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_App_Pattoon_Animalerie.AccessDB
{
    public class ExceptionDB : Exception
    {
        private string Detail {  get; set; }
        public ExceptionDB(string cause , string details): base(cause)
        {
            Detail = details;
        }

        public string getDetails() {  return Detail; }
    }
}
