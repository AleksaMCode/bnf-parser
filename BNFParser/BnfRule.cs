using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BNFParser
{
    public class BnfRule
    {
        /// <summary>
        /// Single non-terminal symbol
        /// </summary>
        private NonTerminalToken leftHandSide;

        /// <summary>
        /// List of all possible cases of the rule
        /// </summary>
        private List<TokenList> possibilities = new List<TokenList>();
        private static string[] rgxPattern =
            { @"^[[+]387]{0,1}0{0,1}6[1-6]{1}[\/]*\d{3}[-]{0,1}\d{3}$"/*@"^[[+]+387]*6[1-6]+[\/]+\d{3}[-]+\d{3}$"*/,
            @"^\w+([-+.']\w+)*@\w+([-.]\\w+)*\.\w+([-.]\w+)*$",
            @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&\/\/=]*)",
            @"^\d+(\.\d+)?$" //@"^[0-9]*$"
        };

        public NonTerminalToken LeftHandSide
        {
            get
            {
                return leftHandSide;
            }
            set
            {
                leftHandSide = value;
            }
        }

        public BnfRule()
        {
        }

        public static BnfRule ParseRule(string input)
        {
            BnfRule rule = new BnfRule();
            string[] leftRightSplit = Regex.Split(input, @"\s*::=\s*"); // \s* matches any whitespace character (0 or more) - \s*::=\s*

            if (leftRightSplit.Length != 2)
            {
                throw new Exception("Cannot find left-hand and right-hand side of BNF rule!\n");
            }

            string lhs = leftRightSplit[0].Trim();
            rule.LeftHandSide = new NonTerminalToken(lhs);
            //leftRightSplit[1] = leftRightSplit[1].Trim();

            if (string.Compare(leftRightSplit[1], "phone_number") == 0)
            {
                TokenList addMe = new TokenList();
                addMe.Add(new StandardExpressionTerminalToken(rgxPattern[0]));
                rule.possibilities.Add(addMe);
            }
            else if (string.Compare(leftRightSplit[1], "email_address") == 0)
            {
                TokenList addMe = new TokenList();
                addMe.Add(new StandardExpressionTerminalToken(rgxPattern[1]));
                rule.possibilities.Add(addMe);
            }
            else if (string.Compare(leftRightSplit[1], "web_link") == 0)
            {
                TokenList addMe = new TokenList();
                addMe.Add(new StandardExpressionTerminalToken(rgxPattern[2]));
                rule.possibilities.Add(addMe);
            }
            else if (string.Compare(leftRightSplit[1], "number_constant") == 0)
            {
                TokenList addMe = new TokenList();
                addMe.Add(new StandardExpressionTerminalToken(rgxPattern[3]));
                rule.possibilities.Add(addMe);
            }
            else if (string.Compare(leftRightSplit[1], "big_city") == 0)
            {
                TokenList addMe = new TokenList();
                addMe.Add(new BigCityTerminalToken(leftRightSplit[1]));
                rule.possibilities.Add(addMe);
            }
            else if (Regex.IsMatch(leftRightSplit[1], @"regex\((.+?)\)"))  //else if (leftRightSplit[1].StartsWith("regex(") && leftRightSplit[1].EndsWith(")"))
            {
                string[] split = Regex.Split(leftRightSplit[1], @"regex\((.+?)\)");
                TokenList addMe = new TokenList();
                addMe.Add(new RegexTerminalToken(split[1])); // split[1] is a regex expression that was inside () -> regex(rgx_expr)
                rule.possibilities.Add(addMe);
            }
            else // other cases
            {
                string[] parts = Regex.Split(leftRightSplit[1], @"\s+\|\|\s+");
                string[] altNotLast = Regex.Split(parts[0], @"\s+\|\s+"),
                    altLast = new string[0];

                if (parts.Length > 1)
                {
                    altLast = Regex.Split(parts[1], @"\s+\|\s+");
                }

                if (altNotLast.Length == 0 && altLast.Length == 0/* || parts.Length == 0*/)
                {
                    throw new Exception("Right-hand side of BNF rule is empty!\n");
                }

                ProcessPossibilities(rule, altNotLast, false);
                ProcessPossibilities(rule, altLast, true);
            }
            return rule;
        }

        private static void ProcessPossibilities(BnfRule rule, string[] possibilities, bool isIt)
        {
            foreach (string poss in possibilities)
            {
                TokenList addMe = new TokenList();
                addMe.ToLastOrNotToLast = isIt;
                string[] splits = poss.Split(' ');
                if (splits.Length == 0)
                {
                    throw new Exception("Possibilties of BNF rule are empty!");
                }

                foreach (string split in splits)
                {
                    string splitTrim = split.Trim();
                    if (splitTrim.Contains("<") && !splitTrim.StartsWith("<"))
                    {
                        throw new Exception("The expression '" + splitTrim + "' contains tokens that are not separated by space/s.!\n"); // |<div> | <sub>
                    }
                    if (splitTrim.StartsWith("<")) // Adding non-terminal token
                    {
                        Token addMeToken = new NonTerminalToken(splitTrim);
                        addMe.Add(addMeToken);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(splitTrim))
                        {
                            throw new Exception("Creating an empty terminal token is not allowed!\n");
                        }

                        addMe.Add(new TerminalToken(splitTrim)); // literal token - this is a token e.q. 1 | 2 | 3 -> 1 is a literal token
                    }
                    rule.possibilities.Add(addMe);
                }
            }
        }

        public void AddPossibilities(List<TokenList> list)
        {
            possibilities.AddRange(list);
        }

        public List<TokenList> GetPossibilities()
        {
            List<TokenList> list = new List<TokenList>();
            List<TokenList> listOfLastElements = new List<TokenList>();
            foreach (TokenList token in possibilities)
            {
                if (token.ToLastOrNotToLast)
                    listOfLastElements.Add(token);
                else
                    list.Add(token);
            }
            list.AddRange(listOfLastElements);
            return list;
        }

        public List<TerminalToken> GetTerminalTokens()
        {
            List<TerminalToken> list = new List<TerminalToken>();
            foreach (TokenList tokenList in possibilities)
            {
                list.AddRange(tokenList.getTerminalTokens());
            }
            return list;
        }

    }
}