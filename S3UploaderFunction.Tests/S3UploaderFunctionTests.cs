using System;
using System.Buffers;
using FluentAssertions;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using S3Uploader;
using Xunit;

namespace S3UploaderFunction.Tests
{
    public class S3UploaderFunctionTests
    {
        private static string TheImageContextBytes => "The image context bytes";
        private readonly ILogger _logger = TestFactory.CreateLogger();

        private S3UploaderFunction _s3UploaderFunction;

        public S3UploaderFunctionTests()
        {
            IFilesystemIo filesystemIoMock = new FilesystemIoMock();
            IPropertyGetter propertyGetterMock = new PropertyGetterMock();
            IS3Uploader iS3Uploader = new S3Uploader.S3Uploader(filesystemIoMock, propertyGetterMock);
            _s3UploaderFunction = new S3UploaderFunction(iS3Uploader);
        }

        [Fact]
        public async void should_successful_upload_file()
        {
            var logger = (ListLogger) TestFactory.CreateLogger(LoggerTypes.List);
            var request = TestFactory.CreateHttpRequest(TheImageContextBytes);
            var response = (OkResult) await _s3UploaderFunction.Run(request, logger);
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            logger.Logs[0].Should().Be("S3UploaderFunction C# HTTP trigger function processed a request");
        }

        [Fact]
        public async void should_return_error_missing_image()
        {
            var request = TestFactory.CreateHttpRequest("");
            var response = (BadRequestObjectResult) await _s3UploaderFunction.Run(request, _logger);
            StatusCodes.Status400BadRequest.Should().Be(response.StatusCode);
            "LOGO00001".Should().Be(((Error) response.Value).Code);
            "Other Error, contact system administrator".Should().Be(((Error) response.Value).Description);
        }
    }
}