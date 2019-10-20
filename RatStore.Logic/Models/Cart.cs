using System;
using System.Collections.Generic;
using System.Text;

namespace RatStore.Logic
{
    public class Cart
    {
        List<OrderDetails> _cart;

        public Cart()
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
        /// Verifies that the CurrentStore can fulfill the product request and then adds it to the Cart.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        public void AddProductToCart(Customer customer, Location location, Product product, int quantity)
        {
            Dictionary<Product, int> availableProducts = location.GetAvailableProducts();
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

            if (!location.CanFulfillOrder(_cart))
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
        public void SubmitCart(Location location, Customer customer)
        {
            location.SubmitOrder(customer, _cart);
            ClearCart();
        }
    }
}
