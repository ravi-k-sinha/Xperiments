namespace Xperiments.Service
{
    using System.Threading.Tasks;
    using Models.S3;

    public interface IS3Service
    {
        Task<S3Response> CreateBucketAsync(string bucketName);
    }
}