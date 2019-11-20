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

    public class ChallengeCalculatorOptions
    {
        public bool runOnce;// = false;
        public List<string> delimiters = new List<string>() { ",", "\n" };
        public bool allowNegative;// = false;
        public int upperBound = 1000;

        public ChallengeCalculatorOptions(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "--run-once":
                        runOnce = true;
                        break;

                    case "--alternate-delimiter":
                        delimiters.Remove( "\n" );
                        delimiters.Add( args[++i][0].ToString() );
                        break;

                    case "--allow-negative":
                        allowNegative = true;
                        break;

                    case "--upper-bound":
                        upperBound = Int32.Parse(args[++i]);
                        break;

                    default:
                        break;
                }
            }
        }

        
    }

    public class ChallengeCalculator
    {
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
        public static string checkForAndSetCustomDelimiters(string input, ChallengeCalculatorOptions options){
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
                options.delimiters.AddRange(Regex.Matches(delimiterString, "(?<=\\[).+?(?=\\])").Cast<Match>().Select( m=> m.Value).ToList());

                // return without the first for chars that specify the delimiter
                return input.Substring(indexOfFirstNewLine + 1, input.Length - indexOfFirstNewLine - 1);
            } else { // single char delimiter

                // find the delimiter at this char index
                // //x\n
                // 0123

                options.delimiters.Add( $"{input[2]}" );
                return input.Substring(4, input.Length - 4);
            }
        }

        public static CalculatorResult sumString(string input, ChallengeCalculatorOptions options)
        {
            var split = splitInput(input, options);

            // maintain a list of encountered negative numbers for the
            // exception message
            var negatives = new List<int>();

            int sum = 0;
            var formulaStringBuilder = new StringBuilder();

            foreach (var segment in split)
            {
                var integer = interpretAsValidInteger(segment, options);
                sum += integer;

                formulaStringBuilder.Append(integer);
                formulaStringBuilder.Append("+");

                if ( integer < 0 && !options.allowNegative) {
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
        static string[] splitInput(string line, ChallengeCalculatorOptions options)
        {
            return line.Split(options.delimiters.ToArray(), StringSplitOptions.None);
        }

        // interpret the string as an int less then or equal to 1000,
        // or return 0 on a failure.
        static int interpretAsValidInteger(string s, ChallengeCalculatorOptions options)
        {
            if(!int.TryParse(s.Trim(), out int n))
            {
                return 0;
            }
            return n > options.upperBound ? 0 : n;
        }

        public static void Main(string[] args)
        {
            var options = new ChallengeCalculatorOptions(args);

            // process custom delimiters on first run
            var userInput = collectUserInput();
            var processedInput = checkForAndSetCustomDelimiters(userInput, options);
            var result = sumString(processedInput, options);
            Console.WriteLine( $"{result.formula} = {result.sum}");

            // run once option (used in unit tests)
            if(options.runOnce == true){
                return;
            }

            while(true){
                // do no process custom delimiters on subsequent runs
                userInput = collectUserInput();
                result = sumString(processedInput, options);
                Console.WriteLine( $"{result.formula} = {result.sum}");
            }
        }
    }
}
