using System.Text.RegularExpressions;

namespace BNFParser
{
    public class StandardExpressionTerminalToken : TerminalToken
    {
        private readonly Regex _rgxPattern;

        public Regex RgxPattern
        {
            get
            {
                return _rgxPattern;
            }
        }

        public StandardExpressionTerminalToken() : base()
        {
        }

        public StandardExpressionTerminalToken(string pattern) : base(pattern)
        {
            _rgxPattern = new Regex(pattern);
        }

        public bool IsItAMatch(string rgx)
        {
            return _rgxPattern.IsMatch(rgx);
        }

        public override int match(string str)
        {
            if (_rgxPattern.IsMatch(str))
            {
                return str.Length;
            }
            return -1;
        }

        public override bool equals(object o)
        {
            if (o == null || !(o is StandardExpressionTerminalToken))
            {
                return false;
            }

            return string.Compare(((StandardExpressionTerminalToken)o).RgxPattern.ToString(), RgxPattern.ToString()) == 0;
        }
    }
}