using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;

namespace Test
{
    public class Program
    {

        static string[] fulllines;
        static string[] fullconfig;
        static List<Token> tokens = new List<Token>();

        static void Main()
        {
            fullconfig = System.IO.File.ReadAllLines(@"config.bnf");
            fulllines = System.IO.File.ReadAllLines(@"input.txt");
            //fulllines = System.IO.File.ReadAllLines(@"D:\ETF\Formalne metode u softverskom inženjerstvu\Project2019\input.txt"); //  fulllines = System.IO.File.ReadAllLines(args[1]);

            Read(fulllines, fullconfig); // Upisuje linije iz inputa i config,bnf u program

            foreach (Token token in tokens) //Definiše sve one tokene koji nisu final
            {
                if (!token.Final)
                    Define(token.Token_name); // Šaljemo u funkciju define svaki token koji nema final
            }

            string temp;
            foreach (Token token in tokens) // Objedinjuje sve tokene za početak i kraj, i nakon ovoga su svi tokeni definisani i tekst je spreman za parsiranje
            {
                temp = @"^(" + token.Final_expression + @")$";
                token.Final_expression = temp;
            }

            Parser();

            using (XmlWriter writer = XmlWriter.Create("XMLout.xml"))  //using (XmlWriter writer = XmlWriter.Create("out.xml")) // using (XmlWriter writer = XmlWriter.Create(args[2]))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("ROOT");
                foreach (Elements element in forXML)
                {
                    writer.WriteStartElement(element.Name.ToString());
                    writer.WriteElementString(element.Name.ToString(), element.Value.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            Console.ReadKey();
        }

        static List<string> expressions = new List<string>();
        static List<string> expr_names = new List<string>();
        static List<string> lines = new List<string>(); // iz input.txt
        static List<string> config = new List<string>(); // iz config.bnf
        static List<string> tempList = new List<string>();
        static List<Elements> forXML = new List<Elements>(); // FInalna lista koju na kraju ispisuješ
        static Dictionary<string, string> dictionary = new Dictionary<string, string>(); // Veza između lines i config


        public static void Read(string[] sent_lines, string[] sent_config)
        {
            for (int i = 0; i < sent_lines.Length; i++) lines.Add(sent_lines[i]);
            for (int i = 0; i < sent_config.Length; i++) config.Add(sent_config[i]);

            foreach (string array in config)
            {
                if (String.IsNullOrWhiteSpace(array)) throw new Exception("Prazna linija u config.bnf!");
                int index = array.IndexOf('>'); // Upisuje ime tokena u listu
                string name = array.Substring(1, index - 1);
                expr_names.Add(name);

                index = array.IndexOf('=') + 2; // Upisuje expression tokena u listu               
                string expression = array.Remove(0, index);
                expressions.Add(expression);
                dictionary.Add(name, expression);

                Token temp = new Token(name, expression);
                tokens.Add(temp);
            }
        }

        static public string Define(string expr_name)
        {
            string temp;
            if (dictionary.ContainsKey(expr_name)) // Gleda ima li tog tokena 
            {
                string expression;
                if (dictionary.TryGetValue(expr_name, out expression)) // Ako ima ona pokupi expression od tog tokena
                {
                    string[] particions = expression.Split('|'); // Dijeli expression
                    int index;
                    for (index = 0; index < tokens.Count; index++)
                        if (tokens[index].Token_name == expr_name) break; // Petlja da nam kaže o kom broju tokena se tačno radi
                    if (tokens[index].Final) return tokens[index].Final_expression;
                    if (expression.StartsWith("regex")) // Ako je token definisan preko regexa onda samo ubaci expression u Final expression i preskoči particije
                    {
                        int f_index = expression.IndexOf('(') + 3;
                        int l_index = expression.LastIndexOf(')') - 1;
                        int new_len = l_index - f_index;

                        string temp_expr = expression.Substring(f_index, new_len);
                        tokens[index].Final_expression = temp_expr;
                    }
                    else
                        foreach (string part in particions)
                        {
                            if (!String.IsNullOrEmpty(tokens[index].Final_expression)) // Ako FinalExpression nije prazan onda dodaj "ili"  zbog particija |
                            { tokens[index].Final_expression += "|"; }

                            char sign1 = part[0];
                            char sign2 = part[part.Length - 1];
                            string temp_expr = part.Substring(1, part.Length - 2);

                            switch (sign1)
                            {
                                case '<':
                                    {
                                        if (part[part.Length - 1] == '>')
                                        {
                                            int first_index = part.IndexOf(">");
                                            int last_index = part.LastIndexOf(">");

                                            if (first_index != last_index) // U OVO SLUČAJU TEMP_EXPR NIŠTA NE ZNAČI 
                                            {
                                                string[] second_particions = part.Split('>'); // Ovo je onaj slučaj kad ima više ovih <a><b><c>
                                                foreach (string second_part in second_particions)
                                                {

                                                    if (String.IsNullOrEmpty(second_part)) break;
                                                    string second_name = second_part.Substring(1, second_part.Length - 1);
                                                    if (String.Equals(second_name, expr_name))
                                                    {
                                                        temp = "((" + tokens[index].Final_expression + ")+)";
                                                        tokens[index].Final_expression = temp;

                                                    }
                                                    else
                                                    {
                                                        //Console.WriteLine("Second part>"+second_part);
                                                        //string second_name = second_part.Substring(1, second_part.Length - 1);
                                                        //Console.WriteLine("Second particions broj"+second_particions.Length);
                                                        string second_expr = Define(second_name);
                                                        if (String.IsNullOrEmpty(tokens[index].Final_expression))
                                                        {
                                                            // tokens[index].Final_expression += @"(";
                                                        }
                                                        else
                                                        {
                                                            int num = tokens[index].Final_expression.Length;
                                                            // if (tokens[index].Final_expression[num-1] != '+')                                                            
                                                            //tokens[index].Final_expression += "|";
                                                        }
                                                        tokens[index].Final_expression += second_expr;
                                                    }

                                                }

                                            }
                                            else
                                            {
                                                if (String.Equals(temp_expr, expr_name)) // za slučaj kad je <a> ::= ...| <a>
                                                {
                                                    temp = "((" + tokens[index].Final_expression + ")+)";
                                                    tokens[index].Final_expression = temp;
                                                }
                                                else // za slučaj kad je <a> ::= ...| <b><c> pa onda ide svaki posebno i definiše se
                                                {
                                                    string temp_place = Define(temp_expr);
                                                    if (!String.IsNullOrEmpty(tokens[index].Final_expression))
                                                    {
                                                        int num = tokens[index].Final_expression.Length; // Da ako nije prazan da ovo | izostavi ako je iza + da se ne bi preskočio
                                                        if (tokens[index].Final_expression[num - 1] != '+')
                                                            tokens[index].Final_expression += "|";
                                                    }
                                                    tokens[index].Final_expression += temp_place;
                                                }
                                            }
                                        }
                                        else throw new Exception("Greska nedostaje > !");
                                    }
                                    break;
                                default:
                                    {
                                        if (ToExclude(temp_expr)) tokens[index].Final_expression += "\\";
                                        tokens[index].Final_expression += temp_expr; // Ubacuje ono što je između "" i sve ostalo
                                    }
                                    break;

                            }
                        }

                    tokens[index].Final = true;
                    temp = @"(" + tokens[index].Final_expression + ")";
                    tokens[index].Final_expression = temp;

                    return tokens[index].Final_expression;
                }
            }
            else throw new Exception("Token <" + expr_name + " >  nije definisan.");

            return "\n Greska! Ne postoji definicija za token";
        }

        static public void Parser()
        {
            foreach (Token token in tokens)
            {
                if (token.Final)
                {
                    foreach (string line in lines)
                    {
                        MatchCollection mc = Regex.Matches(line, token.Final_expression);
                        foreach (Match m in mc)
                        {
                            string temp_line = m.ToString();
                            if (!String.IsNullOrWhiteSpace(temp_line))
                            {
                                string name = token.Token_name;
                                string temp_name = token.Token_name.ToString();
                                string temp_expr = token.Final_expression.ToString();
                                if (name == "veliki_grad")
                                {
                                    bool result = false;
                                    string[] cities = System.IO.File.ReadAllLines(@"cities200.txt");
                                    foreach (string city in cities)
                                    {
                                        result = city.Equals(temp_line);
                                        if (result) break;
                                    }
                                    if (result)
                                    {
                                        Elements temp = new Elements(temp_name, temp_line, temp_expr);
                                        forXML.Add(temp);
                                    }

                                }
                                else
                                {
                                    Elements temp = new Elements(temp_name, temp_line, temp_expr);
                                    forXML.Add(temp);
                                }

                            }

                        }
                    }
                }
                else throw new Exception("Token <" + token.Token_name + " >  nije zavrsen.");


            }
        }

        class Token
        {
            string token_name;
            string final_expression = "";
            string start_expr;
            string line;
            Boolean final = false;

            public Token(string old_name, string old_expression)
            {
                token_name = old_name; start_expr = old_expression;
            }

            public string Token_name { get { return token_name; } set => token_name = value; }
            public string Start_expression { get { return start_expr; } set => start_expr = value; }
            public string Final_expression { get { return final_expression; } set => final_expression = value; }
            public bool Final { get { return final; } set => final = value; }
            public string Line { get { return line; } set => line = value; }

        }

        public static bool ToExclude(string temp_expr) //.^$*+?()[{\|
        {
            if (String.Equals(temp_expr, ".") ||
                String.Equals(temp_expr, "^") ||
                String.Equals(temp_expr, "$") ||
                String.Equals(temp_expr, "*") ||
                String.Equals(temp_expr, "+") ||
                String.Equals(temp_expr, "?") ||
                String.Equals(temp_expr, "(") ||
                String.Equals(temp_expr, ")") ||
                String.Equals(temp_expr, "/") ||
                String.Equals(temp_expr, "[") ||
                String.Equals(temp_expr, "{") ||
                String.Equals(temp_expr, "\\") ||
                String.Equals(temp_expr, "|")

                )
                return true;
            else return false;
        }

        class Elements
        {
            string name, evalue, expr;
            public Elements(string ime, string vrijednost, string izraz) { name = ime; evalue = vrijednost; expr = izraz; }
            public string Name { get { return name; } set => name = value; }
            public string Value { get { return evalue; } set => evalue = value; }
            public string Expr { get { return expr; } set => expr = value; }
        }
    }
}
