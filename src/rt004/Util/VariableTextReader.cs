using System.Text;
using System.Text.RegularExpressions;

namespace rt004.Util
{
    internal class VariableTextReader : TextReader
    {
        Dictionary<string, string> variables = new Dictionary<string, string>();

        string initialString = "";
        int currentIndexInInitialString = 0;


        public VariableTextReader(Stream stream)
        {
            InitializeVariables(new StreamReader(stream));
            int start = initialString.IndexOf("<variables>");
            int end = initialString.IndexOf("</variables>") + "<variables>\n".Length + 1;

            if (start > 0 && end > 0)
            {
                initialString = initialString.Remove(start, end - start);
            }
        }


        public override int Read()
        {
            if (currentIndexInInitialString + 1 < initialString.Length)
                return initialString[++currentIndexInInitialString];
            return base.Read();
        }

        public override int Peek()
        {
            if(currentIndexInInitialString < initialString.Length)
                return initialString[currentIndexInInitialString];
            return base.Peek();
        }

        private void InitializeVariables(TextReader reader)
        {
            StringBuilder initialString = new StringBuilder();
            string? str = reader.ReadLine();
            bool hasEndOfVariable = false;

            while (str is not null)
            {
                initialString.AppendLine(str);
                if (hasEndOfVariable)
                {
                    break;
                }
                
                str = reader.ReadLine();
                hasEndOfVariable = str.Contains("</variables>");
            }
            this.initialString = initialString.ToString();
            variables = ParseVariables(this.initialString);
        }


        private Dictionary<string, string> ParseVariables(string parseFrom)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            string xmlTagPattern = @"\s*\b(\w+)\b.+\1\b";

            int startIndex = parseFrom.IndexOf("<variables>") + "<variables>\n".Length + 1;
            int endIndex = parseFrom.IndexOf("</variables>");

            string variableDefinitions = parseFrom.Substring(startIndex, endIndex - startIndex);

            foreach (Match match in Regex.Matches(variableDefinitions, xmlTagPattern, RegexOptions.Singleline))
            {
                var variableName = match.Groups[1].Value;
                string variableContentPattern = $"{variableName}>" + @"\s*(.*)\s*" + $"</{variableName}";
                
                //Match variable content
                Match variableContentMatch = Regex.Match(match.Groups[0].Value, variableContentPattern, RegexOptions.Singleline);
                if (variableContentMatch.Success)
                {
                    result.Add(variableName, variableContentMatch.Groups[1].Value);
                }
                else
                {
                    throw new ArgumentException("variable content not found");
                }
            }
            return result;
        }
    }
}
