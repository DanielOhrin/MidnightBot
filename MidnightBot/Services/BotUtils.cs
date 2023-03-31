using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidnightBot.Services
{
    public static class BotUtils
    {
        public static string FormatNumber(string numbers)
        {
            string decimals = string.Empty;

            int indexOfDecimal = numbers.IndexOf(".");
            
            if (indexOfDecimal != -1)
            {
                decimals = numbers.Substring(indexOfDecimal);
                numbers = numbers.Substring(0, indexOfDecimal);
            }

            int numLength = numbers.Length;
            for (int i = numLength - 3; i > 0; i-= 3) 
            {
                numbers = numbers.Insert(i, ",");
            }

            return numbers + decimals;
        }
    }
}
