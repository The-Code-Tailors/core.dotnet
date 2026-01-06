using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;

namespace com.fabioscagliola.Core.Packager
{
    /// <summary>
    /// Exposes static methods to compress and decompress zip packages 
    /// </summary>
    public static class Compressor
    {
        /// <summary>
        /// Compresses files into a zip package and returns the zip package as an array of bytes 
        /// </summary>
        /// <param name="fileList">An array of files to be compressed</param>
        public static byte[] Compress(params FileToBeCompressed[] fileList)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Crc32 crc = new Crc32();

                using (ZipOutputStream zip = new ZipOutputStream(memoryStream))
                {
                    zip.SetLevel(6);

                    foreach (FileToBeCompressed file in fileList)
                    {
                        string name = ZipEntry.CleanName(file.Name);
                        ZipEntry zipEntry = new ZipEntry(name);
                        zipEntry.DateTime = file.LastWriteTimeUtc;
                        crc.Reset();
                        crc.Update(file.Contents);
                        zipEntry.Crc = crc.Value;
                        zip.PutNextEntry(zipEntry);
                        zip.Write(file.Contents, 0, file.Contents.Length);
                    }

                    zip.Finish();
                    zip.Close();
                }

                memoryStream.Close();

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Compresses files into a zip package and returns the zip package as an array of bytes 
        /// </summary>
        /// <param name="pathList">An array of strings containing the full paths to the files to be compressed</param>
        /// <param name="root">The full path to the folder to be used as the root of the zip package (to make paths relative)</param>
        public static byte[] Compress(string[] pathList, string root)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Compress(pathList, root, memoryStream);
                memoryStream.Close();
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Compresses files into a zip package, writes the zip package to a file, and returns the size of the file in bytes 
        /// </summary>
        /// <param name="pathList">An array of strings containing the full paths to the files to be compressed</param>
        /// <param name="root">The full path to the folder to be used as the root of the zip package (to make paths relative)</param>
        /// <param name="path">The full path to the zip package</param>
        public static long Compress(string[] pathList, string root, string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                Compress(pathList, root, fileStream);
                fileStream.Close();
                FileInfo fileInfo = new FileInfo(path);
                return fileInfo.Length;
            }
        }

        /// <summary>
        /// Compresses files into a zip package and writes the zip package to a stream 
        /// </summary>
        /// <param name="pathList">An array of strings containing the full paths to the files to be compressed</param>
        /// <param name="root">The full path to the folder to be used as the root of the zip package (to make paths relative)</param>
        /// <param name="stream">The stream where the zip package is to be written</param>
        public static void Compress(string[] pathList, string root, Stream stream)
        {
            List<FileToBeCompressed> fileList = new List<FileToBeCompressed>();

            foreach (string path in pathList)
            {
                string name = path.Substring(root.Length);

                byte[] contents = null;

                using (FileStream fileStream = File.OpenRead(path))
                {
                    contents = new byte[fileStream.Length];
                    fileStream.Read(contents, 0, contents.Length);
                    fileStream.Close();
                }

                DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(path);

                FileToBeCompressed file = new FileToBeCompressed(name, contents, lastWriteTimeUtc);

                fileList.Add(file);
            }

            byte[] data = Compress(fileList.ToArray());

            stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Extracts a zip package to a folder and returns the number of extracted files 
        /// </summary>
        /// <param name="source">The zip package as an array of bytes</param>
        /// <param name="target">The full path to the target folder</param>
        public static int Decompress(byte[] source, string target)
        {
            using (MemoryStream memoryStream = new MemoryStream(source))
            {
                return Decompress(memoryStream, target);
            }
        }

        /// <summary>
        /// Extracts a zip package to a folder and returns the number of extracted files 
        /// </summary>
        /// <param name="source">The full path to the zip package</param>
        /// <param name="target">The full path to the target folder</param>
        public static int Decompress(string source, string target)
        {
            using (FileStream fileStream = File.OpenRead(source))
            {
                return Decompress(fileStream, target);
            }
        }

        /// <summary>
        /// Extracts a zip package to a folder and returns the number of extracted files 
        /// </summary>
        /// <param name="source">The zip package as a stream</param>
        /// <param name="target">The full path to the target folder</param>
        public static int Decompress(Stream source, string target)
        {
            int result = 0;

            using (ZipInputStream zipInputStream = new ZipInputStream(source))
            {
                ZipEntry zipEntry;

                while ((zipEntry = zipInputStream.GetNextEntry()) != null)
                {
                    string folderName = Path.GetDirectoryName(zipEntry.Name);
                    string folderPath = Path.Combine(target, folderName);

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string fileName = Path.GetFileName(zipEntry.Name);

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        string filePath = Path.Combine(folderPath, fileName);

                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }

                        using (FileStream fileStream = File.OpenWrite(filePath))
                        {
                            if (zipEntry.Size > 0)
                            {
                                byte[] buffer = new byte[2048];

                                while (true)
                                {
                                    int n = zipInputStream.Read(buffer, 0, buffer.Length);

                                    if (n > 0)
                                    {
                                        fileStream.Write(buffer, 0, n);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }

                            fileStream.Close();
                        }

                        File.SetLastWriteTimeUtc(filePath, zipEntry.DateTime);
                    }

                    result++;
                }

                zipInputStream.Close();
            }

            return result;
        }

    }
}

