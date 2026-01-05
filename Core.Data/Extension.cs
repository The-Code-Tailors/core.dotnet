using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace com.fabioscagliola.Core.Data
{
    public static class Extension
    {
        /// <summary>
        /// Returns the byte array as an hexadecimal string.
        /// </summary>
        public static string ToHexString(this byte[] _this)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte value in _this)
            {
                stringBuilder.Append(value.ToString("x2"));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Truncates the string at the specified length.
        /// </summary>
        /// <param name="length">The number of characters to be returned.</param>
        /// <reremarks>If <paramref name="length" /> is lower than zero or greater than the length of the string, then the string is returned unaltered.</reremarks>
        public static string Truncate(this string s, int length)
        {
            // s == null prevents null reference exception
            if (s == null || length < 0 || length > s.Length)
            {
                return s;
            }
            else
            {
                return s.Substring(0, length);
            }
        }

        /// <summary>
        /// Returns true if the IDataRecord contains the specified column.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <returns></returns>
        public static bool ContainsColumn(this IDataRecord dataRecord, string columnName)
        {
            for (int i = 0; i < dataRecord.FieldCount; i++)
            {
                if (dataRecord.GetName(i) == columnName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Randomly shuffles the list using the Fisher–Yates shuffle algorithm.
        /// </summary>
        public static void Shuffle<T>(this IList<T> iList)
        {
            Random random = new Random();
            int i = iList.Count;
            while (i-- > 1)
            {
                int j = random.Next(i + 1);
                T value = iList[j];
                iList[j] = iList[i];
                iList[i] = value;
            }
        }

    }
}

