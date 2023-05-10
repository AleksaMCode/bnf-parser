using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BNFParser.Tests.Util
{
    public static class TestHelper
    {
        public static string GetBinPath()
        {
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        public static string GetProjectPath()
        {
            string appRoot = GetBinPath();
            return new DirectoryInfo(appRoot).Parent.Parent.Parent.Parent.FullName;
        }
    }
}
