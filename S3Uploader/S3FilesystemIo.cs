using System.Net;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using HttpMultipartParser;
using Microsoft.Extensions.Configuration;

namespace S3Uploader
{
    public class S3FilesystemIo 
    {
        private PropertyGetter _getter;
        private AmazonS3Client _s3Client;

        public S3FilesystemIo(IConfiguration configuration)
        {
            _getter = new PropertyGetter(configuration);
            _s3Client = new AmazonS3Client(
                _getter.GetAccessKey(), 
                _getter.GetSecretKey(),
                Amazon.RegionEndpoint.GetBySystemName(_getter.GetRegion()));
        }

        public async Task<BucketResponse> Write(string name, FilePart filePart)
        {
            PutObjectRequest request = new()
            {
                BucketName = _getter.GetBucketName(),
                ContentType = "image/*",
                InputStream = filePart.Data,
                Key = $"{_getter.GetFolder()}/{name}",
                CannedACL = S3CannedACL.PublicRead,
                StorageClass = S3StorageClass.Standard
            };

            PutObjectResponse putObjectResponse = await _s3Client.PutObjectAsync(request);
            return new BucketResponse(putObjectResponse);
        }
    }

    public class BucketResponse
    {
        private readonly PutObjectResponse _putObjectResponse;

        public BucketResponse(PutObjectResponse putObjectResponse)
        {
            _putObjectResponse = putObjectResponse;
        }

        public bool IsSuccess()
        {
            return _putObjectResponse.HttpStatusCode == HttpStatusCode.OK;
        }

        public string? GetError()
        {
            return _putObjectResponse.ToString();
        }
    }
}