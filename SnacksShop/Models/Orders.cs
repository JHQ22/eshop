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
    
    public partial class Orders
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Orders()
        {
            this.OrdersDetail = new HashSet<OrdersDetail>();
        }
    
        public int OrdersID { get; set; }
        public System.DateTime Orderdate { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<int> AddressID { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public Nullable<int> States { get; set; }
        public string Remark { get; set; }
    
        public virtual Address Address { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdersDetail> OrdersDetail { get; set; }
        public virtual Users Users { get; set; }
    }
}
