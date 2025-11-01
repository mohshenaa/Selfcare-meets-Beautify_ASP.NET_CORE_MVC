using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Selfcare_meets_Beautify.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [DisplayName("Product Name")]
        public string Name { get; set; } = default!;


        [DataType(DataType.ImageUrl)]
        public string? ImageUrl { get; set; }

        [NotMapped, DisplayName("Image")]
        public IFormFile? ImageFile { get; set; }


        [Required]
        public string SkinType { get; set; } = default!;


        [Required]
        public string? Description { get; set; }


        [Required]
        public string Size { get; set; } = default!;


        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        [Column(TypeName = "decimal(18, 2)")] 
        public decimal? Price { get; set; }


        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Production Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ProductionDate { get; set; }


        [Required]
        [DisplayName("Expiry Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ExpiryDate { get; set; }


        public string Origin { get; set; } = default!;

       
        [Required]
        [DisplayName("Brand")]
        public int BrandId { get; set; }

        [ForeignKey("BrandId")]
        public Brand? Brand { get; set; }

       
        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; } 
    }
}