using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ProductBLL
{
    public interface IProductService
    {
        IEnumerable<ProductModel> GetAll();
        IEnumerable<ProductModel> GetAll_Anonimus(List<int> allProducts);
        IEnumerable<ProductModel> OrderBYDate(List<ProductModel> list);
        IEnumerable<ProductModel> OrderBYTitle(List<ProductModel> list);
        Task RemoveProductFromData(List<int> productList);
        void RemoveProductFromCart(ProductModel product);
        //async Task RemoveProductFromDataa(List<int> productList);
        void AddProductToData(ProductModel product , int id);
        void AddProductToCart(ProductModel product, UserModel userModel);
        IEnumerable<ProductModel> GetAllProductsInCart(string userName);
        IEnumerable<ProductModel> GetAllProductsInCartById_AndState(List<int> productsInAnnonymusCart);
        ProductModel ProductDetails(ProductModel product);
        ProductModel GetProductModelById(int productId);
    }
}
