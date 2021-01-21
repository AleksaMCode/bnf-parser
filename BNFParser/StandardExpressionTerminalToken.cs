using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BNFParser
{
    public class StandardExpressionTerminalToken : TerminalToken
    {
        private readonly Regex rgxPattern;

        public Regex RgxPattern
        {
            get
            {
                return rgxPattern;
            }
        }

        public StandardExpressionTerminalToken() : base() { }

        public StandardExpressionTerminalToken(string pattern) : base(pattern)
            => rgxPattern = new Regex(pattern);

        public bool IsItAMatch(string rgx)
            => rgxPattern.IsMatch(rgx);

        public override int match(string str)
        {
            if (rgxPattern.IsMatch(str))
                return str.Length;
            return -1;
        }


        public override bool equals(object o)
        {
            if (o == null || !(o is StandardExpressionTerminalToken))
                return false;

            return string.Compare(((StandardExpressionTerminalToken)o).RgxPattern.ToString(), RgxPattern.ToString()) == 0;
        }
    }
}