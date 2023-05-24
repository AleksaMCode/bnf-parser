using System;

namespace BNFParser
{
    public abstract class Token
    {
        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public Token()
        {
            _name = "";
        }

        public Token(string name)
        {
            _name = name;
        }

        public virtual bool equals(Object o)
        {
            if (o == null || !(o is Token))
            {
                return false;
            }

            return string.Compare(((Token)o).Name, _name) == 0;
        }

        public abstract int match(string str);
    }
}