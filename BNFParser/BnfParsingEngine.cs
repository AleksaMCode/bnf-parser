﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace BNFParser
{
    public class BnfParsingEngine
    {
        private string _grammarPath;
        private string GrammarPath
        {
            get
            {
                return _grammarPath;
            }

            set
            {
                _grammarPath = value;
                InputLoad();
            }
        }
        private string InputPath { get; set; }
        private string OutputPath { get; set; }
        private List<string> inputLines = new List<string>();
        public BnfParsingEngine(string grammarPath, string inputPath, string outputPath)
        {
            GrammarPath = grammarPath;
            InputPath = inputPath;
            OutputPath = outputPath;
            InputLoad();
        }

        public BnfParsingEngine() { }


        private void InputLoad()
        {
            using (StreamReader file = new StreamReader(InputPath))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    inputLines.Add(line);
                }
            }
        }

        public void Parse()
        {
            try
            {
                List<ParseNode> parseForest = new List<ParseNode>();

                using (StreamReader file = new StreamReader(GrammarPath))
                {
                    int lineNum = 0;
                    BnfParser parser = new BnfParser(file);

                    inputLines.ForEach(
                        str =>
                        {
                            ParseNode parseTree = parser.Parse(str, lineNum);
                            if (parseTree != null)
                                parseForest.Add(parseTree);
                        }
                        );
                }
                WriteOutput(parseForest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private void XmlWrite(XmlWriter writer, ParseNode node)
        {
            if (!node.Token.Contains("<") && !node.Token.Contains(">"))
            {
                writer.WriteElementString("Token", node.Token);
                return;
            }

            writer.WriteStartElement(node.Token.Replace("<", "").Replace(">", ""));

            foreach (ParseNode children in node.GetChildren())
            {
                XmlWrite(writer, children);
            }
            writer.WriteEndElement();
        }

        private void WriteOutput(List<ParseNode> parseForest)
        {
            using (XmlWriter writer = XmlWriter.Create(OutputPath))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("parsetree");

                foreach (ParseNode parseTree in parseForest)
                {
                    XmlWrite(writer, parseTree);
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}