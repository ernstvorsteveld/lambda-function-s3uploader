using System.IO;
using System.Threading.Tasks;

namespace S3Uploader
{
    public interface IFilesystemIo
    {
        Task<BucketResponse> Write(Stream filePart);
    }
}