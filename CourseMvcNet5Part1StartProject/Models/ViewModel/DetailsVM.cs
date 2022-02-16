using System;
namespace CourseMvcNet5Part1StartProject.Models.ViewModel
{
    public class DetailsVM
    {
        public Product Product { get; set; }
        public bool ExistsIncart { get; set; }

        public DetailsVM()
        {
            Product = new Product();
        }
    }
}
