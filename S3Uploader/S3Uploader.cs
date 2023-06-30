using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HttpMultipartParser;

namespace S3Uploader
{
    public class S3Uploader : IS3Uploader
    {
        private readonly IFilesystemIo _filesystemIo;
        private readonly IPropertyGetter _propertyGetter;

        public S3Uploader(IFilesystemIo filesystemIo, IPropertyGetter propertyGetter)
        {
            _filesystemIo = filesystemIo;
        }

        public async Task Write(Stream stream)
        {
            BucketResponse response = await _filesystemIo.Write(await GetFilesystemData(stream));
            if (!response.IsSuccess())
            {
                throw new S3OperationFailedS3Exception(response);
            }
        }

        private async Task<FilesystemData> GetFilesystemData(Stream stream)
        {
            MultipartFormDataParser? bodyParser = null;
            try
            {
                bodyParser = await MultipartFormDataParser.ParseAsync(stream);
            }
            catch (MultipartParseException e)
            {
                throw new NoDataFoundException();
            }

            FilePart? file = bodyParser?.Files.FirstOrDefault(x => x.Name == "logo");
            Validate(file);
            return new FilesystemDataBuilder()
                .Name(bodyParser.GetParameterValue("id"))
                .Stream(file?.Data)
                .Build();
        }

        private void Validate(FilePart? file)
        {
            if (file == null)
            {
                throw new LogoNotPresentS3Exception();
            }

            if (!file.ContentType.Contains("image"))
            {
                throw new ContentTypeNotSupportedS3Exception();
            }

            if (file.Data.Length > _propertyGetter.GetMaxSize())
            {
                throw new FileSizeTooLargeS3Exception();
            }
        }
    }
}