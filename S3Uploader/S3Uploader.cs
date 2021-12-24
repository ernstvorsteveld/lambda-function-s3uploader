﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HttpMultipartParser;

namespace S3Uploader
{
    public class S3Uploader : IFilesystemIo
    {
        private readonly S3FilesystemIo _s3FilesystemIo;
        private readonly PropertyGetter _propertyGetter;

        public S3Uploader(S3FilesystemIo s3FilesystemIo, PropertyGetter propertyGetter)
        {
            _s3FilesystemIo = s3FilesystemIo;
            _propertyGetter = propertyGetter;
        }

        public async Task<BucketResponse> Write(Stream stream)
        {
            MultipartFormDataParser bodyParser = await MultipartFormDataParser.ParseAsync(stream);
            FilePart? file = bodyParser.Files.FirstOrDefault(x => x.Name == "logo");
            Validate(file);
            string id = bodyParser.GetParameterValue("id");
            return await _s3FilesystemIo.Write(id, file);
        }

        private void Validate(FilePart? file)
        {
            if (file == null)
            {
                throw new LogoNotPresentException();
            }

            if (!file.ContentType.Contains("image"))
            {
                throw new ContentTypeNotSupportedException();
            }

            if (file.Data.Length > _propertyGetter.GetMaxSize())
            {
                throw new FileSizeTooLargeException();
            }
        }
    }

    public class FileSizeTooLargeException : Exception
    {
    }

    public class ContentTypeNotSupportedException : Exception
    {
    }

    public class LogoNotPresentException : Exception
    {
    }
}