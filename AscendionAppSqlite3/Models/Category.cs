using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AscendionAppSqlite3.Models;

public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-generate the key
    public int Id { get; set; }

    [Required]
    public String Name { get; set; }
    public int DisplayOrder { get; set; }
}