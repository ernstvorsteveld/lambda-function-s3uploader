using System;

namespace S3Uploader
{
    public abstract class AbstractS3Exception : Exception
    {
        public abstract Error Get();
        public abstract string GetMessage();
    }

    public class S3OperationFailedS3Exception : AbstractS3Exception
    {
        private readonly BucketResponse _response;

        public S3OperationFailedS3Exception(BucketResponse response)
        {
            _response = response;
        }

        public override Error Get()
        {
            return new Error("LOGO00005", GetMessage());
        }

        public override string GetMessage()
        {
            return "Failure storing logo";
        }
    }

    public class FileSizeTooLargeS3Exception : AbstractS3Exception
    {
        public override Error Get()
        {
            return new Error("LOGO00004", GetMessage());
        }

        public override string GetMessage()
        {
            return "File too large";
        }
    }

    public class ContentTypeNotSupportedS3Exception : AbstractS3Exception
    {
        public override Error Get()
        {
            return new Error("LOGO00003", GetMessage());
        }

        public override string GetMessage()
        {
            return "Content type not supported";
        }
    }

    public class LogoNotPresentS3Exception : AbstractS3Exception
    {
        public override Error Get()
        {
            return new Error("LOGO00002", GetMessage());
        }

        public override string GetMessage()
        {
            return "Missing logo file";
        }
    }
}