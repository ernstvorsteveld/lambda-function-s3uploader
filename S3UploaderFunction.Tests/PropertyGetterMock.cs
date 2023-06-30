using S3Uploader;

namespace S3UploaderFunction.Tests
{
    public class PropertyGetterMock : IPropertyGetter
    {
        public int GetMaxSize()
        {
            return 100;
        }

        public string GetAccessKey()
        {
            return "key";
        }

        public string GetSecretKey()
        {
            return "secret";
        }

        public string GetRegion()
        {
            return "region";
        }

        public string GetFolder()
        {
            return "folder";
        }

        public string GetBucketName()
        {
            return "bucket";
        }
    }
}