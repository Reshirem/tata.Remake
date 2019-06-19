using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using cqhttp.Cyan.Messages;
using cqhttp.Cyan.Messages.CQElements;

namespace tata.Remake
{
    public class Global
    {
        public static long botid = 0;
        public static long sbid = 0;
        public static long masterid = 2875726738;
        public static long koiid = 0;
        public static long kusuriid = 0;

        public static string appDir = Directory.GetCurrentDirectory();

        public static bool enabled_ = true;

        public static List<IMessageProcess> processers = new List<IMessageProcess>();

        private static readonly Random rand_ = new Random();

        public static bool msgFilter(Message msg, bool filterAt, Func<string, bool> filterText)
        {
            var foundAt = false;
            var foundText = false;
            foreach (var ele in msg.data)
            {
                if (filterAt && ele is ElementAt)
                {
                    var ea = ele as ElementAt;
                    if (ea.data["qq"] == botid.ToString())
                        foundAt = true;
                }

                if (ele is ElementText)
                {
                    var et = ele as ElementText;
                    if (filterText(et.text))
                        foundText = true;
                }
            }

            return (filterAt ? foundAt : true) && foundText;
        }

        public static string msgText(Message msg)
        {
            var txt = "";
            foreach (var ele in msg.data)
                if (ele is ElementText)
                {
                    var et = ele as ElementText;
                    txt += et.text.Trim();
                }

            return txt;
        }

        public static T randItem<T>(T[] candidates)
        {
            return candidates[rand_.Next(candidates.Length)];
        }
        
        public static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}