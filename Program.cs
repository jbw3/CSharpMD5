using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace md5
{
    class Program
    {
        public static void PrintBytes(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; ++i)
            {
                Console.Write($"{bytes[i]:x2}");
            }
        }

        public static bool CompareArrays(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; ++i)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }

            return true;
        }

        static void Main(string[] args)
        {
            IEnumerable<string> testStrings = new List<string>
            {
                "",
                "Hello World!",
                "The quick brown fox jumps over the lazy dog",
                "The quick brown fox jumps over the lazy dog.",
                // 200
                "01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789",
            }.Concat(Enumerable.Range(0, 65).Select(i => new String('x', i)));

            MD5 systemHasher = MD5.Create();
            CustomMD5 customHasher = new CustomMD5();

            foreach (string str in testStrings)
            {
                byte[] input = Encoding.ASCII.GetBytes(str);

                byte[] systemHash = systemHasher.ComputeHash(input);
                byte[] customHash = customHasher.ComputeHash(input);

                bool equal = CompareArrays(systemHash, customHash);

                PrintBytes(systemHash);
                Console.Write("  ");
                PrintBytes(customHash);
                Console.WriteLine("  {0}", equal ? "OK" : "BAD!!!");
            }
        }
    }
}
