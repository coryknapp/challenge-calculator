using System;
using System.Text;
using System.Collections.Generic;

namespace ChallengeCalculator
{
    public class ChallengeCalculator
    {
       
        public static int sumCommaDelimitedString(string input)
        {
            var split = splitInput(input);

            // maintain a list of encountered negative numbers for the
            // exception message
            var negatives = new List<int>();

            int sum = 0;
            foreach (var segment in split)
            {
                var integer = interpretAsValidInteger(segment);
                sum += integer;
                
                if( integer < 0) {
                    negatives.Add(integer);
                }
            }

            if(negatives.Count > 0){
                throw new ArgumentException($"Found these negatives: {String.Join(", ", negatives)}" );
            }

            return sum;
        }

        // split an input line into substrings
        static string[] splitInput(string line)
        {
            return line.Split(new char[] { ',', '\n' });
        }

        // interpret the string as an int less then or equal to 1000,
        // or return 0 on a failure.
        static int interpretAsValidInteger(string s)
        {
            if(!int.TryParse(s.Trim(), out int n))
            {
                return 0;
            }
            return n > 1000 ? 0 : n;
        }

        static void Main(string[] args)
        {
            // collect input until the user hits enter twice, as we're now
            // supporting /n as a delimiter
            var inputStringBuilder = new StringBuilder();
            string lastLine = "";

            do
            {
                var line = Console.ReadLine();
                if( line.Length == 0 && lastLine.Length == 0 ){
                    break;
                } else {
                    inputStringBuilder.Append(line);
                    inputStringBuilder.Append('\n');
                    lastLine = line;
                }
            } while (lastLine.Length > 0);

            // shave off the last \n that was appended
            inputStringBuilder.Remove(inputStringBuilder.Length - 2, 1);

            Console.WriteLine( sumCommaDelimitedString( inputStringBuilder.ToString() ));
        }
    }
}
