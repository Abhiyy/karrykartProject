﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AppBanwao.KarryKart.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class karrykartEntities : DbContext
    {
        public karrykartEntities()
            : base("name=karrykartEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<CouponImage> CouponImages { get; set; }
        public DbSet<CouponType> CouponTypes { get; set; }
        public DbSet<CouponValue> CouponValues { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<DeliverySlot> DeliverySlots { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<OTPHolder> OTPHolders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductPrice> ProductPrices { get; set; }
        public DbSet<ProductShipping> ProductShippings { get; set; }
        public DbSet<ProductSizeMapping> ProductSizeMappings { get; set; }
        public DbSet<refCity> refCities { get; set; }
        public DbSet<refSaluation> refSaluations { get; set; }
        public DbSet<refState> refStates { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<SizeType> SizeTypes { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<UserAddressDetail> UserAddressDetails { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<GuestUserDetail> GuestUserDetails { get; set; }
        public DbSet<ImportantValue> ImportantValues { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public DbSet<Panel> Panels { get; set; }
        public DbSet<PanelItem> PanelItems { get; set; }
        public DbSet<KKExpense> KKExpenses { get; set; }
        public DbSet<TredingProduct> TredingProducts { get; set; }
        public DbSet<VideoLog> VideoLogs { get; set; }
        public DbSet<OrderJourney> OrderJourneys { get; set; }
        public DbSet<UserAlert> UserAlerts { get; set; }
    
        public virtual int UpdatePageHit()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdatePageHit");
        }
    
        public virtual ObjectResult<GetDashBoardCards_Result> GetDashBoardCards()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetDashBoardCards_Result>("GetDashBoardCards");
        }
    
        public virtual ObjectResult<GetSalesRevenue_Result> GetSalesRevenue(string @for)
        {
            var forParameter = @for != null ?
                new ObjectParameter("for", @for) :
                new ObjectParameter("for", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetSalesRevenue_Result>("GetSalesRevenue", forParameter);
        }
    
        public virtual ObjectResult<GetAllOrders_Result> GetAllOrders()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAllOrders_Result>("GetAllOrders");
        }
    }
}
