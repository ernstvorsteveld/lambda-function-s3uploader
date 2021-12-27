namespace S3Uploader
{
    public class Error
    {
        public Error(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public string Code { get; set; }
        public string Description { get; set; }
    }
}