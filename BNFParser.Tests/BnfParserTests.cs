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
            resourcesDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + Path.DirectorySeparatorChar + "Resources" + Path.DirectorySeparatorChar;
        }

        [TestCase("math_addition")]
        [TestCase("simple_math")]
        [TestCase("combo")]
        [TestCase("email_only")]
        [TestCase("email_only_too_deep_recursive")]
        [TestCase("math_addition_undefined_nonterminal")]
        public void BnfParsing(string testCase)
        {
            Assert.Pass();
        }
    }
}
