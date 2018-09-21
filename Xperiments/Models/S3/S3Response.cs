namespace Xperiments.Models.S3
{
    using System.Net;

    public class S3Response
    {
        public HttpStatusCode Status { get; set; }
        
        public string Message { get; set; }
    }
}