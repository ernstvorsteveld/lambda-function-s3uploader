namespace S3Uploader
{
    public interface IPropertyGetter
    {
        int GetMaxSize();
        string GetAccessKey();
        string GetSecretKey();
        string GetRegion();
        string GetFolder();
        string GetBucketName();
    }
}