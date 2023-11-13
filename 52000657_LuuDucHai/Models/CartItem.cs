using System;
using System.Collections.Generic;
using _52000657_LuuDucHai.Models;
using System.Linq;
using System.Web;

namespace _52000657_LuuDucHai.Models
{
    [Serializable]
    public class CartItem
    {
        public product product { set; get; }
        public int quantity { set; get; }
    }
}