using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStoreSharedLibrary.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required, Range(1, 99999)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Required, DisplayName("Product image")]
        public string? Base64Img { get; set; }
        public int Quantity { get; set; }
        public bool Featured { get; set; } = false;
        public DateTime DateUploaded { get; set; } = DateTime.Now;

        // Relationship Many-to-One


    }
}
