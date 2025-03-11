using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public static class StreamExtensions
    {
        public static string ReadString(this StreamReader reader)
        {
            return reader.ReadLine() ?? string.Empty; // Return empty string if null
        }

        public static int ReadInteger(this StreamReader reader)
        {
            string line = reader.ReadLine();
            return int.TryParse(line, out int result) ? result : throw new FormatException("Invalid integer format");
        }

        public static float ReadFloat(this StreamReader reader)
        {
            string line = reader.ReadLine();
            return float.TryParse(line, out float result) ? result : throw new FormatException("Invalid float format");
        }
    }
}
