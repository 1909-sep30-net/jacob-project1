using System;
using System.Collections.Generic;
using System.Text;

namespace RatStore.Logic
{
    public class Cart
    {
        List<OrderDetails> _cart;

        public List<OrderDetails> OrderDetails {  get { return _cart;  } }

        public Cart()
        {
            _cart = new List<OrderDetails>();
        }

        public override string ToString()
        {
            if (_cart.Count == 1)
                return "Cart (1 item)";
            else return $"Cart ({_cart.Count} items)";
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
        /// Verifies that the given location can fulfill the entire order, including the requested product, and then adds it to the Cart.
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
        }

        /// <summary>
        /// Removes all items from the Cart.
        /// </summary>
        public void ClearCart()
        {
            _cart.Clear();
        }

        /// <summary>
        /// Passes the Cart - a list of OrderDetails - to the given store for submission.
        /// </summary>
        public void SubmitCart(Location location, Customer customer)
        {
            location.SubmitOrder(customer, _cart);
            ClearCart();
        }
    }
}
