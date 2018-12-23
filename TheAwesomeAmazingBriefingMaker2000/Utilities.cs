using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAwesomeAmazingBriefingMaker2000
{
    static class Utilities
    {
        /// <summary>
        /// Creates a string of numbers from the current system date and time.
        /// </summary>
        /// <returns></returns>
        public static string CreateDatetimeSeed()
        {
            string datetime = DateTime.Now.ToString();
            string datetimeSeed = "";

            // Remove any special characters.
            foreach (char c in datetime)
            {
                if (char.IsLetterOrDigit(c) == true)
                    datetimeSeed += c;
            }

            return datetimeSeed;
        }
    }
}
