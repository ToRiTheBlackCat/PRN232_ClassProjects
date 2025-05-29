using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class ProductDAO
    {
        public static List<Product> GetProducts()
        {
            var listPoducts = new List<Product>();
            try
            {
                using var db = new MyStoreDBContext();
                listPoducts = db.Products.Include(f => f.Category).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listPoducts;
        }

        public static void SaveProduct(Product p)
        {
            try
            {
                using var context = new MyStoreDBContext();
                context.Products.Add(p);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void CreateProduct(Product p)
        {
            try
            {
                using var context = new MyStoreDBContext();
                context.Products.Add(p);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateProduct(Product p)
        {
            try
            {
                using var context = new MyStoreDBContext();
                context.Entry<Product>(p).State
                    = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void DeleteProduct(Product p)
        {
            try
            {
                using var context = new MyStoreDBContext();
                var foundProduct =
                    context.Products.SingleOrDefault(c => c.ProductId == p.ProductId);

                if (foundProduct != null)
                    context.Products.Remove(foundProduct);

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static Product? GetProductById(int id)
        {
            using var db = new MyStoreDBContext();
            return db.Products.Include(x => x.Category).FirstOrDefault(c => c.ProductId.Equals(id));
        }
    }
}
