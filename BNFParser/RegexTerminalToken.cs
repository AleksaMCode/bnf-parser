using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BNFParser
{
    public class RegexTerminalToken : TerminalToken
    {
        private readonly Regex _rgxPattern;

        public Regex RgxPattern
        {
            get
            {
                return _rgxPattern;
            }
        }

        public RegexTerminalToken() : base()
        {
        }

        public RegexTerminalToken(string pattern) : base(pattern)
        {
            _rgxPattern = new Regex(pattern); // ArgumentException & ArgumentNullException     
        }

        //public bool isItAMatch(string rgx)
        //    => rgxPattern.IsMatch(rgx);

        public override int match(string str)
        {
            Match match = _rgxPattern.Match(str);
            if (match.Success)
            {
                return /*match.Index +*/ match.Length;
            }
            return -1;
        }

        public List<string> GetCaptureGroups(string rgx)
        {
            List<string> captureGroups = new List<string>();
            Match match = _rgxPattern.Match(rgx);
            while (match.Success)
            {
                captureGroups.Add(match.ToString());
                match = match.NextMatch();
            }

            return captureGroups;
        }

        public override bool equals(object o)
        {
            if (o == null || !(o is RegexTerminalToken))
            {
                return false;
            }

            return string.Compare(((RegexTerminalToken)o).RgxPattern.ToString(), RgxPattern.ToString()) == 0;
        }
    }
}