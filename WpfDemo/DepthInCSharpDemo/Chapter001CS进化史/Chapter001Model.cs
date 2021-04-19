using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepthInCSharpDemo
{
    public class Product
    {
        public string Name { get; private set; }
        public decimal Price { get; private set; }

        public Product(string nm, decimal pr)
        {
            Name = nm;
            Price = pr;
        }
        public Product() { }
        public static List<Product> GetSampleProducts()
        {
            return new List<Product>
            {
                new Product{Name="Weet Side Story",Price=9.99m },
                new Product{Name="Assassine",Price=14.99m },
                new Product{Name="Frogs",Price=13.99m },
                new Product{Name="Sweeney Todd",Price=10.99m },
            };
        }
        public override string ToString()
        {
            return string.Format("{0}:{1}", Name, Price);
        }
    }
    public class ProductNameComparer : IComparer<Product>
    {
        public int Compare(Product x, Product y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }
}
