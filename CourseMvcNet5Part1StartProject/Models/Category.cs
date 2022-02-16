using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace CourseMvcNet5Part1StartProject.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [DisplayName("Display Order")]
        [Required]
        [Range(1,int.MaxValue,ErrorMessage ="Display Order For Category Must Be Greater Than Zero")]
        public int DisplayOrder { get; set; }
    }
}
