using System;

namespace ChallengeCalculator
{
    public class ChallengeCalculator
    {
       
        public static int sumCommaDelimitedString(string input)
        {
            var split = splitInput(input);

            int sum = 0;
            foreach (var segment in split)
            {
                sum += interpretAsInteger(segment);
            }

            return sum;
        }

        // split an input line into substrings
        static string[] splitInput(string line)
        {
            return line.Split(',');
        }

        // interpret the string as an int, or return 0 on a failure.
        static int interpretAsInteger(string s)
        {
            if(!int.TryParse(s.Trim(), out int n))
            {
                return 0;
            }
            return n;
        }

        static void Main(string[] args)
        {
            Console.WriteLine(sumCommaDelimitedString(Console.ReadLine()));
        }
    }
}
