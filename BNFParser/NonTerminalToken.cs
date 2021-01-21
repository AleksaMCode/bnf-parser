using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNFParser
{
    public class NonTerminalToken : Token
    {
        public static readonly string leftSymbol = "<", rightSymbol = ">";

        public NonTerminalToken() : base() { }
        public NonTerminalToken(string name) : base(name) { }

        public override int match(string str)
        {
            return 0;
        }
    }
}