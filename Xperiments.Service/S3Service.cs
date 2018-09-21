namespace Xperiments.Service
{
    using System.Threading.Tasks;
    using Models.S3;

    class S3Service : IS3Service
    {
        public Task<S3Response> CreateBucketAsync(string bucketName)
        {
            throw new System.NotImplementedException();
        }
    }
}