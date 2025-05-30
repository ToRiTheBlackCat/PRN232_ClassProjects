using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IProductService
    {
        void SaveProduct(Product p);
        void DeleteProduct(Product p);
        void UpdateProduct(Product p);
        List<Product> GetProducts();
        Task<List<Product>> SearchProducts(string? productName, string? categoryName, int currentPage, int pageSize);
        Product? GetProductById(int id);
    }
}
