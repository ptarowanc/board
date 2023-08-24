using MHLab.PATCH.Compression.ZIP;
using MHLab.PATCH.Compression.TAR;

namespace MHLab.PATCH.Compression
{
    public enum CompressionType
    {
        ZIP,
        TAR,
        TARGZ
    }

    internal class Compressor
    {
        public static void Compress(string folderToCompress, string outputFile, CompressionType type, string password)
        {
            switch(type)
            {
                case CompressionType.ZIP:
                    ZIPCompressor.ZipFolder(outputFile, password, folderToCompress);
                    break;
                case CompressionType.TAR:
                    TARGZCompressor.ArchiveFolder(outputFile, folderToCompress, false);
                    break;
                case CompressionType.TARGZ:
                    TARGZCompressor.ArchiveFolder(outputFile, folderToCompress, true);
                    break;
            }
        }

        public static void Decompress(string folderWhereDecompress, string inputFile, CompressionType type, string password)
        {
            switch (type)
            {
                case CompressionType.ZIP:
                    ZIPCompressor.ExtractZipFile(inputFile, password, folderWhereDecompress);
                    break;
                case CompressionType.TAR:
                    TARGZCompressor.ExtractTAR(inputFile, folderWhereDecompress);
                    break;
                case CompressionType.TARGZ:
                    TARGZCompressor.ExtractTGZ(inputFile, folderWhereDecompress);
                    break;
            }
        }
    }
}