using System.Collections.Generic;

namespace Xperiments.Persistence.Common
{
    public class MultilingualText
    {
        public string Text { get; set;}
        public IDictionary<string, string> translations { get; set; } = new Dictionary<string, string>();
    }
}