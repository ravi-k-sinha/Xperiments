namespace Xperiments.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Prometheus;

    [Route("/")]
    public class DefaultController : Controller
    {

        private Counter sampleCounter = Metrics.CreateCounter("MyCounter", "Another simple counter", new CounterConfiguration
        {
            LabelNames = new[] {"method", "endpoint"}
        });
        
        private Histogram histogram = Metrics.CreateHistogram("my_histogram", "help text", new HistogramConfiguration
        {
            Buckets = new[] { 0, 0.2, 0.4, 0.6, 0.8, 0.9 },
            LabelNames = new[] {"feature"}
        });

        
        /// <summary>
        /// A default endpoint that returns welcome message
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {
            sampleCounter.Inc();
            sampleCounter.Labels("GET", "Get").Inc();
            histogram.Observe(0.01);
            histogram.Labels("Get").Observe(9.0);
            
            return "Welcome to Xperiments!";
        }
    }
}