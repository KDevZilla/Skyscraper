using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skyscraper
{
    public class Utility
    {
        private static Random rnd = new Random();

        public static DateTime RandomDay()
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(rnd.Next(range));

        }
        public static int Random(int Min, int Max)
        {
            return rnd.Next(Min, Max);
        }
        public static string PreGenerateDigitsPath => Utility.CurrentPath + @"\PreGeneratedDigits\";
       
        public static String CurrentPath => AppDomain.CurrentDomain.BaseDirectory + @"\AppInfo\";


    }
}
