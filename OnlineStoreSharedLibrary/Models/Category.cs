using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnlineStoreSharedLibrary.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string? Name { get; set; }
        [MaxLength(75)]
        public string? Slug { get; set; }
        public string? IconCSS { get; set; }
        public string? CategoryItemCSS { get; set; }
        [Required, DisplayName("Category image")]
        public string? ImageURL { get; set; }
        public bool ShowOnMenuNavBar { get; set; }

        [JsonIgnore]
        // Relationship : One to Many
        public List<Product>? Products { get; set; }
    }
}
