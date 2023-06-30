using System.IO;
using System.Threading.Tasks;

namespace S3Uploader
{
    public interface IS3Uploader
    {
        public Task Write(Stream stream);
    }
}