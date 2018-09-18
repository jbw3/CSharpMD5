using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace md5
{
    class Program
    {
        static void PrintBytes(byte[] bytes)
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
                "Hello world!",
                "The quick brown fox jumps over the lazy dog",
                "The quick brown fox jumps over the lazy dog.",
            };

            MD5 systemHasher = MD5.Create();

            foreach (string str in testStrings)
            {
                byte[] input = Encoding.ASCII.GetBytes(str);

                byte[] systemHash = systemHasher.ComputeHash(input);

                PrintBytes(systemHash);
                Console.WriteLine();
            }
        }
    }
}
