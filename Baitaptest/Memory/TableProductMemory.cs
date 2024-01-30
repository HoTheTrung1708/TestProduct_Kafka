namespace Baitaptest.Memory
{
    public class TableProductMemory
    {

        public Dictionary<string, Product> Memory { get; set; }

        public TableProductMemory()
        {
            Memory = new Dictionary<string, Product>();
        }
    }
}
