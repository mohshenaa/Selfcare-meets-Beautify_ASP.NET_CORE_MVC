using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Selfcare_meets_Beautify.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Category Type")]
        public string Type { get; set; } = default!;

        public IList<Product> Products { get; set; } = new List<Product>();
    }
}