﻿using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IProductRepository
    {
        void SaveProduct(Product p);
        void DeleteProduct(Product p);
        void UpdateProduct(Product p);
        void CreateProduct(Product p);
        List<Product> GetProducts();
        Task<List<Product>> SearchProducts(string? productName, string? categoryName);
        Product? GetProductById(int id);
    }
}
