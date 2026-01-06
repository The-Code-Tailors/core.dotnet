using System;

namespace com.fabioscagliola.Core.Packager
{
    /// <summary>
    /// Represents a file to be compressed using the <see cref="Compressor"/> class 
    /// </summary>
    public class FileToBeCompressed
    {
        /// <summary>
        /// The name of the file 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The contents of the file 
        /// </summary>
        public byte[] Contents { get; set; }
        /// <summary>
        /// The date and time, in coordinated universal time (UTC), that the file was last written to 
        /// </summary>
        public DateTime LastWriteTimeUtc { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="FileToBeCompressed"/> 
        /// </summary>
        /// <param name="name">The name of the file</param>
        /// <param name="contents">The contents of the file</param>
        /// <param name="lastWriteTimeUtc">The date and time, in coordinated universal time (UTC), that the file was last written to</param>
        public FileToBeCompressed(string name, byte[] contents, DateTime lastWriteTimeUtc)
        {
            Name = name;
            Contents = contents;
            LastWriteTimeUtc = lastWriteTimeUtc;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="FileToBeCompressed"/> using the specified name and contents, 
        /// and sets <see cref="LastWriteTimeUtc"/> to <see cref="DateTime.UtcNow"/> 
        /// </summary>
        /// <param name="name">The name of the file</param>
        /// <param name="contents">The contents of the file</param>
        public FileToBeCompressed(string name, byte[] contents) : this(name, contents, DateTime.UtcNow) { }

    }
}

