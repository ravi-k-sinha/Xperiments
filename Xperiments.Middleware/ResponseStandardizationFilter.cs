using LendFoundry.Foundation.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Xperiments.Middleware
{
    public class ResponseStandardizationFilter : IResultFilter
    {

        private readonly ILogger _logger;

        public ResponseStandardizationFilter(ILogger logger)
        {
            _logger = logger;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            _logger.Debug("OnResultExecuted()");
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            //_logger.Debug("OnResultExecuting() - " + context.Result.GetType().ToString());

            //_logger.Debug("ResultFilter, HTTP Method is ", context.HttpContext.Request.Method);
            var sw = Stopwatch.StartNew();

            if (context.Result is ObjectResult result)
            {
                var response = new StandardResponse(result.Value, _logger);
                var responseJson = JsonConvert.SerializeObject(response);

                if (IsJsonContentTypeSpecified(context))
                {
                    // If JSON is specified then, platform mapper will take care of converting the object to its JSON format
                    result.Value = response;
                }
                else
                {
                    // Otherwise, send JSON back
                    result.Value = responseJson;
                }
            }

            
            sw.Stop();
            _logger.Debug("Response standardization filter, Elapsed time : " + sw.ElapsedMilliseconds);
        }

        private bool IsJsonContentTypeSpecified(ResultExecutingContext context)
        {

            var contentType = context.HttpContext.Response.ContentType;

            if (contentType != null && contentType.StartsWith("application/json"))
            {
                return true;
            }

            foreach (var entry in context.Filters)
            {
                //_logger.Debug($"Type is : {entry.GetType()}");

                if (entry.GetType().Equals(new ProducesAttribute("application/text").GetType()))
                {
                    ProducesAttribute pa = (ProducesAttribute)entry;

                    if (pa.ContentTypes.IndexOf("application/json") != -1)
                    {
                        return true;
                    }

                    //_logger.Debug($"We have a Produces Attribute");
                }

                //_logger.Debug(entry.ToString());
            }

            return false;
        }
    }

    class StandardResponse
    {
        public string status;
        public int code;
        public string message;
        public object data;
        
        public StandardResponse(object data, ILogger logger)
        {
            if (data is Array)
            {
                var count = ((Array)data).Length;
                this.data = new
                {
                    count,
                    result = data,
                };
            }
            else if (data is IList)
            {
                var myList = (IList)data;
                var count = ((IList)data).Count;
                this.data = new
                {
                    count,
                    result = data
                };

                foreach (var obj in myList)
                {
                    logger.Debug("Iterating a list: " + obj);
                }

            }
            else
            {
                this.data = data;
            }
            
            this.code = 200;
            this.message = "Some dummy message";
            this.status = "success";
        }

        public bool IsList(object o)
        {
            return o is IList &&
               o.GetType().IsGenericType &&
               o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }
    }
}
