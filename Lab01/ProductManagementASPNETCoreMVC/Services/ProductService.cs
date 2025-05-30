using BusinessObjects;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository iProductRepository;
        public ProductService()
        {
            iProductRepository = new ProductRepository();
        }
        public void DeleteProduct(Product p)
        {
            iProductRepository.DeleteProduct(p);
        }

        public Product? GetProductById(int id)
        {
            return iProductRepository.GetProductById(id);
        }

        public List<Product> GetProducts()
        {
            return iProductRepository.GetProducts();
        }

        public void SaveProduct(Product p)
        {
            iProductRepository.SaveProduct(p);
        }

        public async Task<List<Product>> SearchProducts(string? productName, string? categoryName, int currentPage, int pageSize)
        {
            var productList = await iProductRepository.SearchProducts(productName, categoryName);

            var result = productList.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return result;
        }

        public void UpdateProduct(Product p)
        {
            iProductRepository.UpdateProduct(p);
        }
    }
}
