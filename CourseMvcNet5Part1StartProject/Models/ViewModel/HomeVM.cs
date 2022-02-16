using System;
using System.Collections.Generic;

namespace CourseMvcNet5Part1StartProject.Models.ViewModel
{
    public class HomeVM
    {
        public IEnumerable<Product> Prodcuts { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
