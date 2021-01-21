using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Threading;

namespace BNFParser
{
    class Program
    {
        static Dictionary<string, Tuple<string, string>> test = new Dictionary<string, Tuple<string, string>>
        {
        /*0*/    { "math_simple.xml", Tuple.Create( "math_simple.bnf", "math_simple.txt" ) },
        /*1*/    { "add_math.xml", Tuple.Create( "add_math.bnf", "add_math.txt" ) },
        /*2*/     { "email_only.xml", Tuple.Create( "email_only.bnf", "email_only.txt" ) },
        /*3*/     { "extraBNF.xml", Tuple.Create( "extraBNF.bnf", "extraBNF.txt" ) },
        /*4*/     { "combo_test.xml", Tuple.Create( "combo_test.bnf", "combo_test.txt" ) },
        /*5*/     { "add_math _undefined_nonterminal.xml", Tuple.Create( "add_math _undefined_nonterminal.bnf", "add_math _undefined_nonterminal.txt" ) },
        /*6*/     { "math_simple_not_reachable.xml", Tuple.Create( "math_simple_not_reachable.bnf", "math_simple_not_reachable.txt" ) },
        /*7*/     { "email_only_direct_recursive.xml", Tuple.Create( "email_only_direct_recursive.bnf", "email_only_direct_recursive.txt" ) },
        /*8*/     { "config.xml", Tuple.Create( "config.bnf", "config.txt" ) }
        };

        static int element = 8;

        static public void XmlWrite(XmlWriter writer, ParseNode node)
        {
            if (!node.Token.Contains("<") && !node.Token.Contains(">"))
            {
                writer.WriteElementString("Token", node.Token);
                return;
            }

            writer.WriteStartElement(node.Token.Replace("<", "").Replace(">", ""));

            foreach (ParseNode children in node.GetChildren())
                XmlWrite(writer, children);
            writer.WriteEndElement();
        }

        static public void Loading(string loadingMessage)
        {
            Console.Write(loadingMessage);
            using (var progress = new ProgressBar())
            {
                for (int i = 0; i <= 100; i++)
                {
                    progress.Report((double)i / 100);
                    Thread.Sleep(20);
                }
            }
            Console.WriteLine("Done.");
        }

        static void Main(string[] args)
        {
            var stringRead = new List<String>();
            using (StreamReader file = new StreamReader(test[test.Keys.ElementAt(element)].Item2))
            {
                String temp;
                Task taskLoading = Task.Factory.StartNew(() => Loading(String.Format("Reading {0}... ", test[test.Keys.ElementAt(element)].Item2)));
                Task taskReadInput = Task.Factory.StartNew(
                    () =>
                    {
                        while ((temp = file.ReadLine()) != null)
                            stringRead.Add(temp);
                    }
                    );
                Task.WaitAll(taskLoading, taskReadInput);
            }

            try
            {

                List<ParseNode> parseForest = new List<ParseNode>();

                using (StreamReader file = new StreamReader(test[test.Keys.ElementAt(element)].Item1))
                {
                    int lineNum = 0;
                    try
                    {
                        BnfParser parser = new BnfParser(file);
                        Task taskLoading = Task.Factory.StartNew(() => Loading(String.Format("Parsing using BNF grammar... ")));
                        Task taskParsing = Task.Factory.StartNew(
                            () =>
                            {
                                stringRead.ToList().ForEach(
                                    str =>
                                    {
                                        try
                                        {
                                            ParseNode parseTree = parser.Parse(str, lineNum);
                                            if (parseTree != null)
                                                parseForest.Add(parseTree);
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine("\n" + e.Message);
                                            throw e;
                                        }
                                    }
                                    );
                            }
                        );
                        Task.WaitAll(taskLoading, taskParsing);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }

                using (XmlWriter writer = XmlWriter.Create(test.Keys.ElementAt(element)))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("parsetree");
                    Task taskLoading = Task.Factory.StartNew(() => Loading("Creating XML file... "));
                    Task taskXml = Task.Factory.StartNew(
                        () =>
                        {
                            foreach (ParseNode parseTree in parseForest)
                                XmlWrite(writer, parseTree);
                        }
                        );
                    Task.WaitAll(taskLoading, taskXml);
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }
    }
}