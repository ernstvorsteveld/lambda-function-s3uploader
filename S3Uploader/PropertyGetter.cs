using System;
using Microsoft.Extensions.Configuration;

namespace S3Uploader
{
    public class PropertyGetter : IPropertyGetter
    {
        private readonly IConfiguration _configuration;

        public PropertyGetter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int GetMaxSize()
        {
            try
            {
                return int.Parse(_configuration["MAX_LOGO_SIZE"]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 10000;
            }
        }

        public string GetAccessKey()
        {
            return _configuration["ACCESS_KEY"];
        }

        public string GetSecretKey()
        {
            return _configuration["SECRET_KEY"];
        }

        public string GetRegion()
        {
            return _configuration["REGION"];
        }

        public string GetFolder()
        {
            return _configuration["FOLDER"];
        }

        public string GetBucketName()
        {
            return _configuration["BUCKET_NAME"];
        }
    }
}