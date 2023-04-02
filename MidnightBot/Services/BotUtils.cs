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

            numberOne.Reverse();
            numberTwo.Reverse();

            string result = string.Empty;

            int i = 0;

            while (i < Math.Max(numberOne.Length, numberTwo.Length) - 1)
            {
                int tempSum = carry;

                if (i < numberOne.Length)
                {
                    tempSum += Convert.ToInt32(numberOne[i]);
                }

                if (i < numberTwo.Length)
                {
                    tempSum += Convert.ToInt32(numberTwo[i]);
                }

                carry = tempSum / 10;
                result.Insert(0, (tempSum % 10).ToString());
                i++;
            }

            return result + decimals;
        }
    }
}
