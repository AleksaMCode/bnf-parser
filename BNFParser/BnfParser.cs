using System;
using System.Collections.Generic;
using System.IO;

namespace BNFParser
{
    public class BnfParser
    {
        private List<BnfRule> rules = new List<BnfRule>();
        private BnfRule startRule;
        /// <summary>
        /// Upper bound that prevents parser from entering an infinite recursion.
        /// </summary>
        private readonly int maxRecursionSteps = 40;

        public BnfParser()
        {
        }

        public BnfParser(StreamReader stream) : this()
        {
            SetGrammar(stream);
        }

        public void SetGrammar(StreamReader grammar)
        {
            List<BnfRule> rules = GetRules(grammar);
            AddRules(rules);
        }

        private BnfRule GetRule(string ruleName)
        {
            foreach (BnfRule rule in rules)
            {
                if (string.Compare(ruleName, rule.LeftHandSide.Name) == 0)
                {
                    return rule;
                }
            }
            return null;
        }

        private BnfRule GetRule(Token token)
        {
            if (token == null)
            {
                return null;
            }
            foreach (BnfRule rule in rules)
            {
                if (rule.LeftHandSide != null && string.Compare(rule.LeftHandSide.Name, token.Name) == 0)
                {
                    return rule;
                }
            }
            return null;
        }

        public List<BnfRule> GetRules(StreamReader grammar)
        {
            string line;
            List<BnfRule> rulesList = new List<BnfRule>();
            // TODO: fix the problem with if condition
            //if (grammar.ReadLine() == null)
            //  throw new Exception("Invalid Grammar Exception - Grammar is empty!");

            while ((line = grammar.ReadLine()) != null)
            {
                int index;
                // Skipping comments; comment start with '#'
                if ((index = line.IndexOf('#')) != -1)
                {
                    line = line.Substring(0, index);
                }

                // Remove all leading and trailing white-space characters from the current string object
                line = line.Trim();
                if (String.IsNullOrEmpty(line) || line.StartsWith("#"))
                {
                    continue;
                }

                try
                {
                    BnfRule newRule = BnfRule.ParseRule(line);
                    rulesList.Add(newRule);
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format(e.Message + " + Invalid Grammar Exception - line: {0}", line));
                }
            }

            //if (!string.IsNullOrEmpty(lineForError))
            //    throw new Exception("Error parsing rule : " + lineForError);
            return rulesList;
        }

        public void AddRule(BnfRule rule)
        {
            NonTerminalToken lhs = rule.LeftHandSide;
            foreach (BnfRule ruleIn in rules)
            {
                if (lhs.equals(ruleIn.LeftHandSide))
                {
                    ruleIn.AddPossibilities(rule.GetPossibilities());
                    break;
                }
            }
            this.rules.Add(rule); // No rule with the same LHS found
        }

        public void AddRules(List<BnfRule> rules)
        {
            foreach (BnfRule rule in rules)
            {
                AddRule(rule);
            }
        }

        //public void SetStartRule(NonTerminalToken token)
        //    => startRule = GetRule(token);

        //public void SetStartRule(string tokenName)    
        //    => SetStartRule(new NonTerminalToken(tokenName));


        public ParseNode Parse(string input, int lineNum)
        {
            if (startRule == null)
            {
                if (rules.Count == 0)
                {
                    throw new Exception("No start rule found!\n");
                }
                // The first rule is the start rule!
                startRule = rules[0];
            }
            return Parse(startRule, ref input, 0, lineNum);
        }

        public ParseNode Parse(BnfRule rule, ref string input, int recursionStep, int lineNum)
        {
            // When the maximum recursion depth is reached program throws exception.
            if (recursionStep > maxRecursionSteps)
            {
                throw new Exception(string.Format("Max. number of recursion steps reached. Error line : {0}", lineNum));
            }

            ParseNode node = null;
            bool wrongToken = true;
            string inputCpy = $"{input}";
            foreach (TokenList poss in rule.GetPossibilities())
            {
                node = new ParseNode();
                node.Token = node.Value = rule.LeftHandSide.Name;
                TokenList newPossib = poss.getCopy();
                IEnumerator<Token> possibIterator = newPossib.GetEnumerator();
                wrongToken = false;
                inputCpy = $"{input}";
                while (possibIterator.MoveNext() && !wrongToken) // checks if it has next token
                {
                    inputCpy = inputCpy.Trim();
                    Token possToken = possibIterator.Current;
                    if (possToken is TerminalToken)
                    {
                        if (string.IsNullOrEmpty(inputCpy))
                        {
                            wrongToken = true;
                            break;
                        }

                        int limit = possToken.match(inputCpy);
                        if (limit > 0)
                        {
                            ParseNode child = new ParseNode();
                            string inputSubstring = inputCpy.Substring(0, limit);
                            inputCpy = inputCpy.Remove(0, inputSubstring.Length);

                            if (possToken is RegexTerminalToken)
                            {
                                child = AddRegexToken(child, (RegexTerminalToken)possToken, inputSubstring);
                            }

                            child.Token = inputSubstring;
                            node.AddChild(child);
                        }
                        //else if (limit == -1)
                        //{
                        //    if (possToken is StandardExpressionTerminalToken && !((StandardExpressionTerminalToken)possToken).IsItAMatch(inputCpy))
                        //        throw new Exception("Regex not mached [" + possToken.Name + "] -> " + inputCpy + "\n");

                        //    else if (possToken is BigCityTerminalToken && !((BigCityTerminalToken)possToken).IsItAMatch(inputCpy))
                        //        throw new Exception("City not mached! City: " + inputCpy + "\n");
                        //}
                        else // No match; rule expects token
                        {
                            wrongToken = true;
                            node = null;
                            break;
                        }
                    }
                    else // Parse non-terminal token
                    {
                        ParseNode child = null;
                        BnfRule newRule = GetRule(possToken);
                        if (newRule == null)
                        {
                            throw new Exception("There is rule missing for the non-terminal token '" + possToken.Name + "'!\n");
                        }

                        child = Parse(newRule, ref inputCpy, recursionStep + 1, lineNum);
                        if (child == null) // Parsing failed!
                        {
                            wrongToken = true;
                            node = null;
                            break;
                        }
                        node.AddChild(child);
                    }
                }
                if (!wrongToken)
                {
                    if (!possibIterator.MoveNext())
                    {
                        if (recursionStep > 0 || (recursionStep == 0 && inputCpy.Trim().Length == 0))
                        {
                            break;
                        }
                    }
                    else
                    {
                        wrongToken = true;
                        inputCpy = "";
                        inputCpy += input;
                        break;
                    }
                }
            }
            int consumedLength = input.Length - inputCpy.Length;
            if (wrongToken || consumedLength == 0)
            {
                return null;
            }

            input = input.Substring(consumedLength);
            if (recursionStep == 0 && !string.IsNullOrEmpty(input))
            {
                return null;
            }

            return node;
        }

        private static ParseNode AddRegexToken(ParseNode node, RegexTerminalToken token, string str)
        {
            List<string> matches = token.GetCaptureGroups(str);
            foreach (string match in matches)
            {
                node.AddChild(new ParseNode(match));
            }

            return node;
        }
    }
}
