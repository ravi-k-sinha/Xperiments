namespace Xperiments.Persistence.Common.Multilingual
{
    using System;
    
    public class TranslationTargetException : Exception
    {
        public TranslationTargetException(string description, Exception innerException) : base(description,
            innerException)
        {
            
        }
    }
}