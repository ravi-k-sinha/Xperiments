namespace Xperiments.Service
{
    using System.Threading.Tasks;

    public interface IS3Service
    {
        Task CreateBucketAsync(string bucketName);
    }
}