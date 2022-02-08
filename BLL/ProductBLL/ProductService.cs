using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace BLL.ProductBLL
{
    public class ProductService : IProductService
    {
        private MyContext _context;
        public ProductService(MyContext context)
        {
            _context = context;
        }
        
        public void AddProductToCart(ProductModel product ,UserModel userName)
        {
            var prod = _context.Products.Find(product.Id);
            prod.ProductState = State.InCart;
            prod.User = userName;
            _context.Update(prod);
            _context.SaveChanges();
        }

        public void AddProductToData(ProductModel product  , int id)
        {
            _context.Products.Add(product);
            UserModel user = _context.Users.FirstOrDefault(x => x.Id == id);
            product.Owner = user;
            _context.SaveChanges();
        }

        public IEnumerable<ProductModel> GetAll()
        {
            return _context.Products.Include(p=>p.User).Include(p =>p.Owner).ToList();
        }
        public IEnumerable<ProductModel> GetAll_Anonimus(List<int> allProducts)//get all the products that without userId 
        {
            return _context.Products.Where(p => !allProducts.Contains(p.Id)).ToList();
        }

        public IEnumerable<ProductModel> GetAllProductsInCart(string userName)
        {
            return GetAll().Where(x => x.ProductState == State.InCart && x.User?.UserName == userName);
        }
        public IEnumerable<ProductModel> GetAllProductsInCartById_AndState(List<int> productsInAnnonymusCart)// get all products in cart of anonimus user
        {
            return GetAll().Where(p => p.ProductState == State.UnSold && productsInAnnonymusCart.Contains(p.Id)).ToList();
        }

        public IEnumerable<ProductModel> OrderBYDate(List<ProductModel> list)
        {
            return list.OrderBy(x => x.Date);
        }

        public IEnumerable<ProductModel> OrderBYTitle(List<ProductModel> list)
        {
            return list.OrderBy(x => x.Title);
        }

        public void RemoveProductFromCart(ProductModel product)
        {
            var prod = _context.Products.Find(product.Id);
            prod.ProductState = State.UnSold;
            _context.Update(prod);
            _context.SaveChanges();
        }

        public async Task RemoveProductFromData(List<int> productList)
        {
            await _context.Products.Where(p => productList.Contains(p.Id)).ForEachAsync(p =>
            {
                p.ProductState = State.Sold;
                _context.Update(p);
            });
             _context.SaveChanges();
        }
        public ProductModel ProductDetails(ProductModel product)
        {
            return _context.Products.Find(product.Id);
        }
        public ProductModel GetProductModelById(int productId)
        {
            ProductModel prod = _context.Products.Include(p => p.Owner).FirstOrDefault(x => x.Id == productId);
            return prod;
        }










        
    }
}
