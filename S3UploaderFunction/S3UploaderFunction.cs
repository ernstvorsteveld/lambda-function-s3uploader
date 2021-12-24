using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using S3Uploader;


// https://github.com/WestDiscGolf/MinimalApiFunctions

namespace S3UploaderFunction
{
    public class S3UploaderFunction
    {
        private static S3Uploader.S3Uploader _s3Uploader;

        public S3UploaderFunction(S3Uploader.S3Uploader s3Uploader)
        {
            _s3Uploader = s3Uploader;
        }

        [FunctionName("S3UploaderFunction")]
        public async Task<IActionResult> Run(
            [Microsoft.Azure.Functions.Worker.HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "upload")]
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("S3UploaderFunction C# HTTP trigger function processed a request");
            try
            {
                BucketResponse bucketResponse = await _s3Uploader.Write(req.Body);
                if (!bucketResponse.IsSuccess())
                {
                    return new BadRequestObjectResult(
                        new Error
                        {
                            Description = "Failure storing logo",
                            Code = "LOGO00005"
                        });
                }

                return new OkResult();
            }
            catch (LogoNotPresentException e)
            {
                log.LogError("Missing logo file");
                return new BadRequestObjectResult(
                    new Error
                    {
                        Description = "Missing logo file",
                        Code = "LOGO00002"
                    });
            }
            catch (UnsupportedContentTypeException e)
            {
                log.LogError("Content type not supported");
                return new BadRequestObjectResult(
                    new Error
                    {
                        Description = "Content type not supported",
                        Code = "LOGO00003"
                    });
            }
            catch (FileSizeTooLargeException e)
            {
                log.LogError("File too large");
                return new BadRequestObjectResult(
                    new Error
                    {
                        Description = "File too large",
                        Code = "LOGO00004"
                    });
            }
            catch (Exception e)
            {
                var msg = e.Message;
                log.LogError("Other exception {Msg}", msg);
                return new BadRequestObjectResult(
                    new Error
                    {
                        Description = "Other Error, contact system administrator",
                        Code = "LOGO00001"
                    });
                ;
            }
        }
    }
}