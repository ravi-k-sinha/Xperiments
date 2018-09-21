namespace Xperiments.Service
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Amazon.S3;
    using Amazon.S3.Model;
    using Amazon.S3.Util;
    using Models.S3;

    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _client;

        public S3Service(IAmazonS3 client)
        {
            _client = client;
        }
        
        public async Task<S3Response> CreateBucketAsync(string bucketName)
        {
            try
            {
                var exists = await AmazonS3Util.DoesS3BucketExistAsync(_client, bucketName);

                if (exists)
                {
                    return CreateS3Response(HttpStatusCode.BadRequest, $"[{bucketName}] folder already exists");
                }
                
                var putBucketRequest = new PutBucketRequest
                {
                    BucketName = bucketName,
                    UseClientRegion = true
                };

                var response = await _client.PutBucketAsync(putBucketRequest);

                return CreateS3Response(response.HttpStatusCode, response.ResponseMetadata.RequestId);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return CreateS3Response(e.StatusCode, e.Message);
            }
            catch (Exception e)
            {
                return CreateS3Response(HttpStatusCode.InternalServerError, e.Message);
            }

            return CreateS3Response(HttpStatusCode.InternalServerError, "Something went wrong");
        }

        private static S3Response CreateS3Response(HttpStatusCode status, string message)
        {
            return new S3Response
            {
                Status = status,
                Message = message
            };
        }
    }
}