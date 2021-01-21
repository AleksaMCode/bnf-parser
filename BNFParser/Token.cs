using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNFParser
{
    public abstract class Token
    {
        private string name;

        public string Name
        {
            get
            {
                return name;
            }
            set
                => name = value;            
        }

        public Token()
            => name = "";

        public Token(string name)
            => this.name = name;

        public virtual bool equals(Object o)
        {
            if (o == null || !(o is Token))
                return false;

            return string.Compare(((Token)o).Name, name) == 0;
        }

        public abstract int match(string str);
    }
}