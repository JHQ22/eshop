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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SnacksShopDBEntities : DbContext
    {
        public SnacksShopDBEntities()
            : base("name=SnacksShopDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<AdminUser> AdminUser { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<OrdersDetail> OrdersDetail { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Category> Category { get; set; }
    }
}
