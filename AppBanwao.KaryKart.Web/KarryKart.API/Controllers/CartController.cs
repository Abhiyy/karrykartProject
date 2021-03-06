﻿
using KarryKart.API.Helpers;
using KarryKart.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KarryKart.API.Controllers
{
    public class CartController : ApiController
    {
        // GET api/<controller>
        CartHelper _cartHelper;

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public CartDetailsModel Get(Guid CartID)
        {
            try
            {
                _cartHelper = new CartHelper();

                return _cartHelper.GetCart(CartID);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // POST api/<controller>
        public CartModel Post(CartModel cart)
        {
           
            try {
                _cartHelper = new CartHelper();
               return _cartHelper.CreateCart(cart);
            }
            catch (Exception ex) {
                return null;
            }
        }

        // PUT api/<controller>/5
        public CartModel Put(CartModel cart)
        {
            try
            {
                _cartHelper = new CartHelper();
                return _cartHelper.UpdateCart(cart);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // DELETE api/<controller>/5
        public CartModel Delete(Guid CartID, Guid ProductID)
        {
            try
            {
                _cartHelper = new CartHelper();
                if (ProductID != Guid.Parse("559B9891-FE58-49DD-A357-B7ADC2CC339A"))
                {
                    return _cartHelper.DeleteCart(CartID, ProductID);
                }
                else {

                    if (_cartHelper.DeleteCart(CartID) != null)
                        return null;
                        
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
    }
}