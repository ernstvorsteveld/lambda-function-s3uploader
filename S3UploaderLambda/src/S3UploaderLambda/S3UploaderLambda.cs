using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using HttpMultipartParser;
using Microsoft.AspNetCore.Mvc;
using S3Uploader;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace S3UploaderLambda
{
    public class S3UploaderLambda
    {
        private static S3Uploader.S3Uploader _s3Uploader;

        public S3UploaderLambda(S3Uploader.S3Uploader s3Uploader)
        {
            _s3Uploader = s3Uploader;
        }

        public string FunctionHandler(APIGatewayHttpApiV2ProxyRequest requestBody, ILambdaContext context)
        {
            try
            {
                var parser = MultipartFormDataParser.Parse(
                    new MemoryStream(Convert.FromBase64String(requestBody.Body)));
                var file = parser.Files.First();
                _s3Uploader.Write(file.Data);

                // MyModel extractedData = new MyModel
                // {
                //     FirstName = Convert.ToString(parser.GetParameterValue("FirstName")),  //STRING
                //     LastName = Convert.ToString(parser.GetParameterValue("LastName")),   //STRING
                //     VoiceFile = audioFile   //byte[]
                // };
                // return extractedData;
                return "OK";
            }
            catch (AbstractS3Exception e)
            {
                LambdaLogger.Log($"Error while handling S3 request: {e.Get()}");
                return JsonSerializer.Serialize(new BadRequestObjectResult(e.Get()));
            }
            catch (Exception e)
            {
                LambdaLogger.Log($"Other exception {e.Message}");
                return JsonSerializer.Serialize(new BadRequestObjectResult(
                    new Error("LOGO00001", "Other Error, contact system administrator")));
            }
        }
    }
}