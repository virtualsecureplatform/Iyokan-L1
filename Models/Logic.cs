namespace Iyokan_L1.Models
{
    public abstract class Logic
    {
        public int id { get; set; }
        public string type { get; set; }
        
        public abstract void Serialize();
    }
}