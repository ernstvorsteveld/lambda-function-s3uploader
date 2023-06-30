using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using S3Uploader;

// https://github.com/WestDiscGolf/MinimalApiFunctions

namespace S3UploaderFunction
{
    public class S3UploaderFunction
    {
        private static IS3Uploader _s3Uploader;

        public S3UploaderFunction(IS3Uploader iS3Uploader)
        {
            _s3Uploader = iS3Uploader;
        }

        [FunctionName("S3UploaderFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "upload")]
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("S3UploaderFunction C# HTTP trigger function processed a request");
            try
            {
                await _s3Uploader.Write(req.Body);
                return new OkResult();
            }
            catch (AbstractS3Exception e)
            {
                log.LogError("Error while handling S3 request: {Msg}", e.Get());
                return new BadRequestObjectResult(e.Get());
            }
            catch (Exception e)
            {
                log.LogError("Other exception {Msg}", e.Message);
                return new BadRequestObjectResult(
                    new Error("LOGO00001", "Other Error, contact system administrator"));
            }
        }
    }
}