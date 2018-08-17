namespace Xperiments.Models
{
    public interface IAddress
    {
        string Street { get; set; }
        
        string City { get; set; }
        
        string State { get; set; }
        
        string Pincode { get; set; }
    }
}