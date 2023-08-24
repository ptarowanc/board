using System;
using System.IO;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.GZip;

namespace MHLab.PATCH.Compression.TAR
{
    internal class TARGZCompressor
    {
        public static void ArchiveFolder(string outPathname, string folderName, bool compress)
        {

            // Create an output stream. Does not have to be disk, could be MemoryStream etc.
            string tarOutFn = outPathname;
            Stream outStream = File.Create(tarOutFn);

            // If you wish to create a .Tar.GZ (.tgz):
            // - set the filename above to a ".tar.gz",
            // - create a GZipOutputStream here
            // - change "new TarOutputStream(outStream)" to "new TarOutputStream(gzoStream)"
            // Stream gzoStream = new GZipOutputStream(outStream);
            // gzoStream.SetLevel(3); // 1 - 9, 1 is best speed, 9 is best compression

            TarOutputStream tarOutputStream;

            if (compress)
            {
                GZipOutputStream gzoStream = new GZipOutputStream(outStream);
                gzoStream.SetLevel(9); // 1 - 9, 1 is best speed, 9 is best compression
                tarOutputStream = new TarOutputStream(gzoStream);
            }
            else
            {
                tarOutputStream = new TarOutputStream(outStream);
            }

            CreateTarManually(tarOutputStream, folderName, folderName);

            // Closing the archive also closes the underlying stream.
            // If you don't want this (e.g. writing to memorystream), set tarOutputStream.IsStreamOwner = false
            tarOutputStream.Close();
        }

        private static void CreateTarManually(TarOutputStream tarOutputStream, string sourceDirectory, string rootDirectory)
        {

            // Optionally, write an entry for the directory itself.
            //
            //TarEntry tarEntry = TarEntry.CreateEntryFromFile(sourceDirectory);
            //tarOutputStream.PutNextEntry(tarEntry);

            // Write each file to the tar.
            //
            string[] filenames = Directory.GetFiles(sourceDirectory);

            foreach (string filename in filenames)
            {

                // You might replace these 3 lines with your own stream code

                using (Stream inputStream = File.OpenRead(filename))
                {

                    string tarName = filename.Substring(rootDirectory.Length + (rootDirectory.EndsWith(Path.DirectorySeparatorChar.ToString()) ? 0 : 1)); 

                    long fileSize = inputStream.Length;

                    // Create a tar entry named as appropriate. You can set the name to anything,
                    // but avoid names starting with drive or UNC.

                    TarEntry entry = TarEntry.CreateTarEntry(tarName);

                    // Must set size, otherwise TarOutputStream will fail when output exceeds.
                    entry.Size = fileSize;

                    // Add the entry to the tar stream, before writing the data.
                    tarOutputStream.PutNextEntry(entry);

                    // this is copied from TarArchive.WriteEntryCore
                    byte[] localBuffer = new byte[32 * 1024];
                    while (true)
                    {
                        int numRead = inputStream.Read(localBuffer, 0, localBuffer.Length);
                        if (numRead <= 0)
                        {
                            break;
                        }
                        tarOutputStream.Write(localBuffer, 0, numRead);
                    }
                }
                tarOutputStream.CloseEntry();
            }


            // Recurse. Delete this if unwanted.

            string[] directories = Directory.GetDirectories(sourceDirectory);
            foreach (string directory in directories)
            {
                CreateTarManually(tarOutputStream, directory, rootDirectory);
            }
        }

        public static void ExtractTAR(String tarFileName, String destFolder)
        {

            Stream inStream = File.OpenRead(tarFileName);

            TarArchive tarArchive = TarArchive.CreateInputTarArchive(inStream);
            tarArchive.ExtractContents(destFolder);
            tarArchive.Close();

            inStream.Close();
        }

        public static void ExtractTGZ(String gzArchiveName, String destFolder)
        {

            Stream inStream = File.OpenRead(gzArchiveName);
            Stream gzipStream = new GZipInputStream(inStream);

            TarArchive tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
            tarArchive.ExtractContents(destFolder);
            tarArchive.Close();

            gzipStream.Close();
            inStream.Close();
        }
    }
}
