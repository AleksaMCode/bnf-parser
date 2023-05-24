namespace BNFParser
{
    public class NonTerminalToken : Token
    {
        public static readonly string leftSymbol = "<", rightSymbol = ">";

        public NonTerminalToken() : base()
        {
        }

        public NonTerminalToken(string name) : base(name)
        {
        }

        public override int match(string str)
        {
            return 0;
        }
    }
}