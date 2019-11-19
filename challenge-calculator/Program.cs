using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace ChallengeCalculator
{
    public struct CalculatorResult
    {
        public CalculatorResult(int sum, string formula)
        {
            this.sum = sum;
            this.formula = formula;
        }

        public int sum { get; }
        public string formula { get; }

        public static implicit operator int(CalculatorResult cr) => cr.sum;
    }

    public class ChallengeCalculator
    {
        static List<string> delimiters = new List<string>() { ",", "\n" };

        public static string collectUserInput(){
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

            // shave off the last 2 \n's that was appended
            inputStringBuilder.Remove(inputStringBuilder.Length - 2, 2);
            return inputStringBuilder.ToString();
        }

        // Check the input for custom delimiters, and return the remaining string
        public static string checkForAndSetCustomDelimiters(string input){
            // delimiter specifier has format //{delimiter}\n for a
            // single char and //[{delimiter}]\n for a multi char delimiter
            if ( input.Substring(0,2) != "//")
                return input;

            if ( input[2] == '[') { // multi char delimiter
                var indexOfFirstNewLine = input.IndexOf('\n');

                // find the delimiters at these char indexes
                // //[delimiter1][delimiter2]\n
                // 012.......................^ = first newline

                var delimiterString = input.Substring(2, indexOfFirstNewLine - 2);
                delimiters.AddRange(Regex.Matches(delimiterString, "(?<=\\[).+?(?=\\])").Cast<Match>().Select( m=> m.Value).ToList());

                // return without the first for chars that specify the delimiter
                return input.Substring(indexOfFirstNewLine + 1, input.Length - indexOfFirstNewLine - 1);
            } else { // single char delimiter

                // find the delimiter at these char indexes
                // //x\n
                // 0123

                delimiters.Add( $"{input[2]}" );
                return input.Substring(4, input.Length - 4);
            }
        }

        public static CalculatorResult sumString(string input)
        {
            var split = splitInput(input);

            // maintain a list of encountered negative numbers for the
            // exception message
            var negatives = new List<int>();

            int sum = 0;
            var formulaStringBuilder = new StringBuilder();

            foreach (var segment in split)
            {
                var integer = interpretAsValidInteger(segment);
                sum += integer;

                formulaStringBuilder.Append(integer);
                formulaStringBuilder.Append("+");

                if ( integer < 0) {
                    negatives.Add(integer);
                }
            }

            if(negatives.Count > 0){
                throw new ArgumentException($"Found these negatives: {String.Join(", ", negatives)}" );
            }

            // shave off the last '+'
            formulaStringBuilder.Remove(formulaStringBuilder.Length - 1, 1);

            return new CalculatorResult(sum, formulaStringBuilder.ToString());
        }

        // split an input line into substrings
        static string[] splitInput(string line)
        {
            return line.Split( delimiters.ToArray(), StringSplitOptions.None);
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

        public static void Main(string[] args)
        {
            var userInput = collectUserInput();
            var processedInput = checkForAndSetCustomDelimiters(userInput);

            var result = sumString(processedInput);

            Console.WriteLine( $"{result.formula} = {result.sum}");
        }
    }
}
