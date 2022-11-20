using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SnacksShop.ViewModel
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        public string Title { get; set; }

        public int CateId { get; set; }

        public decimal MarketPrice { get; set; }

        public decimal Price { get; set; }

        public string Content { get; set; }

        public DateTime? PostTime { get; set; }
        public int Stock { get; set; }
        public string Size { get; set; }

        public string Photo { get; set; }
        public string CategoryName { get; set; }
    }
}