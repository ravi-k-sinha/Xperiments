namespace Xperiments.Models
{
    public class Cat : ICat
    {
        public string TenantId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int NumOfLegs { get; set; }
    }
}