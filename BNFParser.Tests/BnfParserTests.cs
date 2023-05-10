using BNFParser.Tests.Util;
using NUnit.Framework;
using System;
using System.IO;

namespace BNFParser.Tests
{
    public class BnfParserTests
    {
        private string resourcesDirectory;

        [SetUp]
        public void Setup()
        {
            resourcesDirectory = TestHelper.GetProjectPath() + Path.DirectorySeparatorChar + "Resources" + Path.DirectorySeparatorChar;
        }

        [TestCase("math_addition")]
        [TestCase("simple_math")]
        [TestCase("combo")]
        [TestCase("email_only")]
        public void BnfParsing(string testCase)
        {
            var filePath = resourcesDirectory + testCase;
            var outputfilePath = $"{testCase}_output.xml";
            new BnfParsingEngine($"{filePath}_grammar.bnf", $"{filePath}_input.txt", outputfilePath).Parse();

            Assert.That(FileHasher.GetFileHash(outputfilePath), Is.EqualTo(FileHasher.GetFileHash($"{filePath}_expected_output.xml")));
        }

        [TestCase("email_only_too_deep_recursive")]
        public void BnfParsingTooDeepRecursion(string testCase)
        {
            var filePath = resourcesDirectory + testCase;
            var outputfilePath = $"{testCase}_output.xml";

            var ex = Assert.Throws<Exception>(() => new BnfParsingEngine($"{filePath}_grammar.bnf", $"{filePath}_input.txt", outputfilePath).Parse());
            StringAssert.Contains("Max. number of recursion steps reached. Error line", ex.Message.ToString());
        }


        [TestCase("math_addition_undefined_nonterminal")]
        public void BnfParsingGrammarError(string testCase)
        {
            var filePath = resourcesDirectory + testCase;
            var outputfilePath = $"{testCase}_output.xml";

            var ex = Assert.Throws<Exception>(() => new BnfParsingEngine($"{filePath}_grammar.bnf", $"{filePath}_input.txt", outputfilePath).Parse());
            StringAssert.Contains("Invalid Grammar Exception", ex.Message.ToString());
        }
    }
}
