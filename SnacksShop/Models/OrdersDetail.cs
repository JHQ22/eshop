//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SnacksShop.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrdersDetail
    {
        public int DetailID { get; set; }
        public int OrdersID { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Nullable<int> States { get; set; }
    
        public virtual Orders Orders { get; set; }
        public virtual Product Product { get; set; }
    }
}
