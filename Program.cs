using System;
using System.Collections.Generic;
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

        static void Main(string[] args)
        {
            List<string> testStrings = new List<string>
            {
                "",
                "Hello World!",
                "The quick brown fox jumps over the lazy dog",
                "The quick brown fox jumps over the lazy dog.",
                // 200
                "01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789",
            };

            MD5 systemHasher = MD5.Create();
            CustomMD5 customHasher = new CustomMD5();

            foreach (string str in testStrings)
            {
                byte[] input = Encoding.ASCII.GetBytes(str);

                byte[] systemHash = systemHasher.ComputeHash(input);
                byte[] customHash = customHasher.ComputeHash(input);

                PrintBytes(systemHash);
                Console.Write("  ");
                PrintBytes(customHash);
                Console.WriteLine();
            }
        }
    }
}
