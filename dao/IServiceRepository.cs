using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_App.dao
{
    internal interface IServiceRepository
    {
        public void CreateCustomer();
        public void CreateProduct();
        public void DeleteProduct();
        public void DeleteCustomer();
        public void AddToCart();
        public void ProductInCart();
        public void PlaceOrder();
        public void GetOrderByCustomer();
    }
}
