using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNFParser
{
    public class TerminalToken : Token
    {
        public TerminalToken() : base() { }
        public TerminalToken(string name) : base(name) { }

        public override bool equals(object o)
        {
            if (o == null || !(o is TerminalToken))
                return false;

            return string.Compare(((TerminalToken)o).Name, Name) == 0;
        }

        public override int match(string str)
        {
            if (str.Length < Name.Length)
                return -1;

            if (Name.CompareTo(str.Substring(0, Name.Length)) == 0)
                return Name.Length;

            return 0;
        }
    }
}