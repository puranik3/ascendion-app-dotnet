using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AscendionApp.Models
{
	public class Category
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-generate the key
        public int Id { get; set; }

        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(24, ErrorMessage = "Category Name cannot exceed 24 characters")]
        [DisplayName("Category Name")]
        public String Name { get; set; }

        [Required(ErrorMessage = "Display Order is required")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1 - 100")]
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }
    }
}

