using System.ComponentModel.DataAnnotations;

namespace mvvmsample.model
{
    public class ProductModel
    {
        [Key]
        public int O_id { get; set; }
        public int pr_id { get; set; }
        public long U_id { get; set; }
        public string pr_name { get; set; }
        public int pr_cost { get; set; }
        public string Status { get; set; }
        public string username { get; set; }
    }
}
