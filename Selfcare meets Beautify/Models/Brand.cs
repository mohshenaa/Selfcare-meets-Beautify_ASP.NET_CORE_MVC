using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Selfcare_meets_Beautify.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Brand Name")]
        public string Name { get; set; } = default!;

        [DataType(DataType.ImageUrl)]
        [DisplayName("Logo URL")]
        public string? LogoUrl { get; set; } 

        [NotMapped, DisplayName("Logo")]
        public IFormFile? LogoFile { get; set; }

       
        public IList<Product> Products { get; set; } = new List<Product>();
    }
}
