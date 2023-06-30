using System.IO;
using System.Threading.Tasks;
using S3Uploader;

namespace S3UploaderFunction.Tests
{
    public class FilesystemIoMock : IFilesystemIo
    {
        public MemoryStream? Bytes { set; get; }
        
        public Task<BucketResponse> Write(FilesystemData filesystemData)
        {
            throw new System.NotImplementedException();
        }
    }
}