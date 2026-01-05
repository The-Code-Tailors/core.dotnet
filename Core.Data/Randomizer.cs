using System;
using System.Text;

namespace com.fabioscagliola.Core.Data
{
    public class Randomizer
    {
        protected Random random;

        public Randomizer()
        {
            random = new Random();
        }

        public bool GetBoolean()
        {
            return random.Next() % 2 == 0;

        }

        public byte GetByte()
        {
            return (byte)random.Next(byte.MinValue, byte.MaxValue);
        }

        public DateTime GetDateTime()
        {
            return new DateTime(random.Next());
        }

        public double GetDouble()
        {
            return random.NextDouble();
        }

        public double GetDouble(double minValue, double maxValue)
        {
            return random.NextDouble() * (maxValue - minValue) + minValue;
        }

        public T GetEnum<T>()
        {
            T[] values = (T[])Enum.GetValues(typeof(T));
            return values[random.Next(0, values.Length)];
        }

        public string GetHexString(int numberOfBytes)
        {
            StringBuilder sb = new StringBuilder();
            for (int index = 0; index < numberOfBytes; index++)
            {
                sb.Append(GetByte().ToString("X2"));
            }
            return sb.ToString();
        }

        public int GetInt32(int minValue = int.MinValue, int maxValue = int.MaxValue)
        {
            return random.Next(minValue, maxValue);
        }

        public string GetString(int length = 16, bool digitsOnly = false)
        {
            string result = null;

            string source = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            if (digitsOnly)
            {
                source = "0123456789";
            }

            for (int i = 0; i < length; i++)
            {
                result += source[random.Next(0, source.Length)];
            }

            return result;
        }

        public TimeSpan GetTimeSpan()
        {
            return new TimeSpan(random.Next());
        }

        public ushort GetUInt16(ushort minValue = ushort.MinValue, ushort maxValue = ushort.MaxValue)
        {
            return (ushort)random.Next(minValue, maxValue);
        }

        public Version GetVersion()
        {
            return new Version(GetByte(), GetByte());
        }

    }
}

