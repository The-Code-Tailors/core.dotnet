using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace com.fabioscagliola.Core.Data
{
    /// <summary>
    /// Reads data from a CSV file whose first line contains field names.
    /// </summary>
    public class CsvReader : IDisposable
    {
        protected string path;
        protected char[] separator;
        protected bool convertEmptyStringToNull;
        protected Encoding encoding;

        protected StreamReader streamReader;

        protected string line = null;

        protected List<string> keys;
        protected List<string> values;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="path">The full path to the CSV file.</param>
        /// <param name="separator">The character(s) used to delimit field names and values.</param>
        public CsvReader(string path, params char[] separator)
        {
            this.path = path;
            this.separator = separator;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="path">The full path to the CSV file.</param>
        /// <param name="convertEmptyStringToNull">A Boolean value indicating if empty strings are returned as null references.</param>
        /// <param name="separator">The character(s) used to delimit field names and values.</param>
        public CsvReader(string path, bool convertEmptyStringToNull, params char[] separator) : this(path, separator)
        {
            this.convertEmptyStringToNull = convertEmptyStringToNull;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="path">The full path to the CSV file.</param>
        /// <param name="convertEmptyStringToNull">A Boolean value indicating if empty strings are returned as null references.</param>
        /// <param name="encoding">The encoding to be used when reading the CSV file.</param>
        /// <param name="separator">The character(s) used to delimit field names and values.</param>
        public CsvReader(string path, bool convertEmptyStringToNull, Encoding encoding, params char[] separator) : this(path, convertEmptyStringToNull, separator)
        {
            this.encoding = encoding;
        }

        /// <summary>
        /// Closes the underlying stream reader.
        /// </summary>
        public void Close()
        {
            if (streamReader != null)
            {
                streamReader.Close();
            }
        }

        /// <summary>
        /// Disposes of the underlying stream reader.
        /// </summary>
        public void Dispose()
        {
            if (streamReader != null)
            {
                streamReader.Dispose();
            }
        }

        /// <summary>
        /// Returns the value of a bool field.
        /// </summary>
        /// <param name="key">The name of the field.</param>
        public bool GetBoolValue(string key)
        {
            string value = GetStringValue(key);
            bool result;
            bool.TryParse(value, out result);
            return result;
        }

        /// <summary>
        /// Returns the value of a date and time field.
        /// </summary>
        /// <param name="key">The name of the field.</param>
        /// <param name="format">The format of the field.</param>
        public DateTime GetDateTimeValue(string key, string format)
        {
            string value = GetStringValue(key);
            DateTime result;
            DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
            return result;
        }

        /// <summary>
        /// Returns the value of a string field.
        /// </summary>
        /// <param name="key">The name of the field.</param>
        public string GetStringValue(string key)
        {
            if (values == null)
            {
                throw new ApplicationException("Values not read. Did you invoke the ReadLine method?");
            }
            int index = keys.IndexOf(key);
            if (index == -1)
            {
                throw new ApplicationException(string.Format("Key '{0}' not found.", key));
            }
            string value = values[index].Trim();
            if (value == string.Empty && convertEmptyStringToNull)
            {
                value = null;
            }
            return value;
        }

        /// <summary>
        /// The names of the fields 
        /// </summary>
        public List<string> Keys
        {
            get
            {
                return keys;
            }
        }

        /// <summary>
        /// The raw contents of the current line: 
        /// (1) null before invoking the <see cref="Load"/> method, 
        /// (2) the raw contents of the first line (containing field names) after invoking the <see cref="Load"/> method and before invoking the <see cref="ReadLine"/> method, 
        /// and (3) the raw contents of the current line after each subsequent invocation of the <see cref="ReadLine"/> method 
        /// </summary>
        public string Line
        {
            get
            {
                return line;
            }
        }

        /// <summary>
        /// Loads the CSV file and reads the field names from the first line.
        /// </summary>
        public virtual void Load()
        {
            streamReader = new StreamReader(path, encoding ?? Encoding.Default);  // Duplicated line {F9A0216B-4C7F-465A-93D0-FB86202BB0E3} 
            if (!ReadLineTo(out keys))
            {
                throw new ApplicationException("Cannot read field names. Is the file empty?");
            }
        }

        /// <summary>
        /// Loads the CSV file inferring the specified field names.
        /// </summary>
        /// <param name="keys">The names of the fields.</param>
        public virtual void Load(List<string> keys)
        {
            streamReader = new StreamReader(path, encoding ?? Encoding.Default);  // Duplicated line {F9A0216B-4C7F-465A-93D0-FB86202BB0E3} 
            this.keys = keys;
        }

        /// <summary>
        /// Reads the next line of values from the CSV file.
        /// </summary>
        /// <returns>Returns true if the line exists or false if the end of the file has been reached.</returns>
        public bool ReadLine()
        {
            bool read = ReadLineTo(out values);
            if (read && keys != null && keys.Count != values.Count)
            {
                throw new ApplicationException("The number of values does not match the number of keys.");
            }
            return read;
        }

        /// <summary>
        /// Reads the next line of values from the CSV file to the specified list of strings.
        /// </summary>
        /// <param name="target">The list of strings to store values.</param>
        /// <returns>Returns true if the line exists or false if the end of the file has been reached.</returns>
        protected virtual bool ReadLineTo(out List<string> target)
        {
            if (streamReader == null)
            {
                throw new ApplicationException("The underlying stream reader is not initialized. Did you invoke the Load method?");
            }
            bool read = false;
            target = new List<string>();
            line = streamReader.ReadLine();
            if (line != null && !string.IsNullOrWhiteSpace(line))
            {
                read = true;
                target.AddRange(line.Split(separator));
            }
            return read;
        }

    }
}

