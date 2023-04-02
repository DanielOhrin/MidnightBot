using System.Text;

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

        public static string AddNumbers(string numberOne, string numberTwo)
        {
            int decimals = (int.Parse(numberOne.Substring(numberOne.IndexOf(".") + 1)) + int.Parse(numberTwo.Substring(numberTwo.IndexOf(".") + 1))) % 100;
            int carry = decimals / 100;

            numberOne = numberOne.Substring(0, numberOne.IndexOf("."));
            numberTwo = numberTwo.Substring(0, numberTwo.IndexOf("."));
            numberOne = string.Join("", numberOne.Reverse());
            numberTwo = string.Join("", numberTwo.Reverse());

            string result = string.Empty;

            int i = 0;

            while (i < Math.Max(numberOne.Length, numberTwo.Length))
            {
                int tempSum = carry;

                if (i < numberOne.Length)
                {
                    tempSum += int.Parse(numberOne[i].ToString());
                }

                if (i < numberTwo.Length)
                {
                    tempSum += int.Parse(numberTwo[i].ToString());
                }

                carry = tempSum / 10;
                result = result.Insert(0, (tempSum % 10).ToString());
                i++;
            }
            
            return (carry > 0 ? result.Insert(0, carry.ToString()) : result) + "." + decimals;
        }
    }
}
