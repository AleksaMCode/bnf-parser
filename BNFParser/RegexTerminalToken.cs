using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BNFParser
{
    public class RegexTerminalToken : TerminalToken
    {
        private readonly Regex rgxPattern;

        public Regex RgxPattern
        {
            get
            {
                return rgxPattern;
            }
        }

        public RegexTerminalToken() : base() { }

        public RegexTerminalToken(string pattern) : base(pattern)
            => rgxPattern = new Regex(pattern); // ArgumentException & ArgumentNullException     

        //public bool isItAMatch(string rgx)
        //    => rgxPattern.IsMatch(rgx);

        public override int match(string str)
        {
            Match match = rgxPattern.Match(str);
            if (match.Success)
                return /*match.Index +*/ match.Length;
            return -1;
        }

        public List<string> GetCaptureGroups(string rgx)
        {
            List<string> captureGroups = new List<string>();
            Match match = rgxPattern.Match(rgx);
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
                return false;

            return string.Compare(((RegexTerminalToken)o).RgxPattern.ToString(), RgxPattern.ToString()) == 0;
        }
    }
}