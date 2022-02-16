using System;
using System.ComponentModel.DataAnnotations;

namespace CourseMvcNet5Part1StartProject.Models
{
    public class Application
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

    }
}
