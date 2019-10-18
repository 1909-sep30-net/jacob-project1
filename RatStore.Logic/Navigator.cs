using System;
using System.Collections.Generic;
using System.Text;
using RatStore.Data;
using System.Linq;

namespace RatStore.Logic
{
    public class Navigator
    {
        List<OrderDetails> _cart;
        public RatStore CurrentStore { get; set; }

        public Customer CurrentCustomer { get; set; }

        public Navigator()
        {
            _cart = new List<OrderDetails>();
        }

        public decimal Subtotal
        {
            get
            {
                decimal sum = 0;
                foreach (OrderDetails cartItem in _cart)
                {
                    sum += cartItem.Product.Cost * cartItem.Quantity;
                }

                return sum;
            }
        }

        /// <summary>
        /// Changes the CurrentStore to the Location with the given Id.
        /// </summary>
        /// <param name="targetStoreId"></param>
        public void GoToStore(int targetStoreId)
        {
            CurrentStore.ChangeLocation(targetStoreId);
        }

        /// <summary>
        /// Verifies that the CurrentStore can fulfill the product request and then adds it to the Cart.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        public void AddProductToCart(int productId, int quantity)
        {
            List<Product> availableProducts = CurrentStore.GetAvailableProducts();
            if (productId > availableProducts.Count || productId < 0)
                throw new Exception("Invalid product id");

            Product product = availableProducts[productId];
            OrderDetails cartItem;

            if (!_cart.Exists(item => item.Product.ProductId == product.ProductId))
            {
                cartItem = new OrderDetails 
                { 
                    Product = product, 
                    Quantity = quantity 
                };

                _cart.Add(cartItem);
            }
            else
            {
                cartItem = _cart.Find(item => item.Product.ProductId == product.ProductId);
                cartItem.Quantity += quantity;
            }

            if (!CurrentStore.CanFulfillProductQty(cartItem))
            {
                cartItem.Quantity -= quantity;
                if (cartItem.Quantity == 0)
                    _cart.Remove(cartItem);

                throw new Exception($"Inventory cannot fulfill quantity: {product.Name} x {quantity}");
            }
        }

        /// <summary>
        /// Removes all items from the Cart.
        /// </summary>
        public void ClearCart()
        {
            _cart.Clear();
        }

        /// <summary>
        /// Passes the Cart - a list of OrderDetails - to the CurrentStore for submission.
        /// </summary>
        public void SubmitCart()
        {
            CurrentStore.SubmitOrder(CurrentCustomer, _cart);
            ClearCart();
        }
    }
}
