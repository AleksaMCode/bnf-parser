using System.Collections.Generic;

namespace BNFParser
{
    public class TokenList : List<Token>
    {
        //public List<Token> list;
        private bool toLastOrNotToLast = false;

        /// <summary>
        /// Should this element be tried last when parsing.
        /// <param> Value set to true if it should be tried first. </param>
        /// </summary>
        public bool ToLastOrNotToLast
        {
            get
            {
                return toLastOrNotToLast;
            }            
            set
            {
                toLastOrNotToLast = value;
            }
        }

        public TokenList() : base() { }

        public List<TerminalToken> getTerminalTokens()
        {
            List<TerminalToken> retList = new List<TerminalToken>();
            foreach (Token token in this)
            {
                if (token is TerminalToken)
                {
                    retList.Add((TerminalToken)token);
                }
            }

            return retList;
        }

        public TokenList getCopy()
        {
            TokenList copyList = new TokenList();
            copyList.ToLastOrNotToLast = toLastOrNotToLast;
            copyList.AddRange(this);
            return copyList;
        }
    }
}
