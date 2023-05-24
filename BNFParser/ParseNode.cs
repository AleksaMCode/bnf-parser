using System.Collections.Generic;

namespace BNFParser
{
    public class ParseNode
    {
        /// <summary>
        /// List of children in this node.
        /// </summary>
        private List<ParseNode> children = null;

        /// <summary>
        /// Token and it's value, if it has any.
        /// </summary>
        private string token, value; // Dictonary

        public string Token
        {
            get
            {
                return token;
            }
            set
            {
                token = value;
            }
        }

        public string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// Creating empty parse node.
        /// </summary>
        public ParseNode()
        {
            children = new List<ParseNode>();
        }

        public ParseNode(string token) : this()
        {
            this.token = token;
        }

        public List<ParseNode> GetChildren()
        {
            if (children != null)
            {
                List<ParseNode> nodes = new List<ParseNode>(children.Count);
                nodes.AddRange(children);
                return nodes;
            }
            return new List<ParseNode>(0);
        }

        public void AddChild(ParseNode child)
        {
            children.Add(child);
        }

        /// <summary>
        /// Size of parsing tree.
        /// </summary>
        /// <returns> Number of nodes. </returns>
        public int GetSize()
        {
            int size = 1;
            foreach (ParseNode node in children)
            {
                size += node.GetSize();
            }
            return size;
        }
    }
}