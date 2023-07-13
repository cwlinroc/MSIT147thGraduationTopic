using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MSIT147.Estore.DataLayer.EFModels;

public partial class Ispan147MidTopicContext : DbContext
{
    public Ispan147MidTopicContext()
    {
    }

    public Ispan147MidTopicContext(DbContextOptions<Ispan147MidTopicContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<CouponOwner> CouponOwners { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Evaluation> Evaluations { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Merchandise> Merchandises { get; set; }

    public virtual DbSet<MerchandiseTag> MerchandiseTags { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderList> OrderLists { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Spec> Specs { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<UsedCoupon> UsedCoupons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=ISpan147MidTopic;Persist Security Info=True;User ID=sa6;Password=sa6;Trust Server Certificate=True;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK_Brand");

            entity.Property(e => e.BrandId).HasColumnName("BrandID");
            entity.Property(e => e.BrandName).HasMaxLength(50);
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");

            entity.HasOne(d => d.Member).WithMany(p => p.Carts)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Carts_Members");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.Property(e => e.CartItemId).HasColumnName("CartItemID");
            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.SpecId).HasColumnName("SpecID");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_Carts");

            entity.HasOne(d => d.Spec).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.SpecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_Specs");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK_Category");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(30);
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.CouponId).HasName("PK_Coupon");

            entity.Property(e => e.CouponId).HasColumnName("CouponID");
            entity.Property(e => e.CouponDiscountCondition).HasColumnType("money");
            entity.Property(e => e.CouponEligibleTagId).HasColumnName("CouponEligibleTagID");
            entity.Property(e => e.CouponEndDate).HasColumnType("date");
            entity.Property(e => e.CouponRebate).HasColumnType("money");
            entity.Property(e => e.CouponStartDate).HasColumnType("date");
            entity.Property(e => e.CouponTypeId).HasColumnName("CouponTypeID");

            entity.HasOne(d => d.CouponEligibleTag).WithMany(p => p.Coupons)
                .HasForeignKey(d => d.CouponEligibleTagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Coupons_Tags");
        });

        modelBuilder.Entity<CouponOwner>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.CouponId).HasColumnName("CouponID");
            entity.Property(e => e.CouponSerialNumber)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.MemberId).HasColumnName("MemberID");

            entity.HasOne(d => d.Coupon).WithMany()
                .HasForeignKey(d => d.CouponId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CouponOwner_Coupons");

            entity.HasOne(d => d.Member).WithMany()
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CouponOwner_Members");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EmployeeAccount).HasMaxLength(20);
            entity.Property(e => e.EmployeeAvatarPath).HasMaxLength(50);
            entity.Property(e => e.EmployeeEmail).HasMaxLength(50);
            entity.Property(e => e.EmployeeName).HasMaxLength(20);
            entity.Property(e => e.EmployeePassword).HasMaxLength(65);
            entity.Property(e => e.EmployeePhone)
                .HasMaxLength(20)
                .IsFixedLength();
        });

        modelBuilder.Entity<Evaluation>(entity =>
        {
            entity.Property(e => e.EvaluationId).HasColumnName("EvaluationID");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(50)
                .HasColumnName("ImageURL");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.MerchandiseId).HasColumnName("MerchandiseID");

            entity.HasOne(d => d.Member).WithMany(p => p.Evaluations)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Evaluations_Members");

            entity.HasOne(d => d.Merchandise).WithMany(p => p.Evaluations)
                .HasForeignKey(d => d.MerchandiseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Evaluations_Merchandises");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK_Member");

            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.Account)
                .HasMaxLength(15)
                .IsFixedLength();
            entity.Property(e => e.Address).HasMaxLength(30);
            entity.Property(e => e.Avatar).HasMaxLength(50);
            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsFixedLength();
            entity.Property(e => e.MemberName).HasMaxLength(30);
            entity.Property(e => e.NickName).HasMaxLength(30);
            entity.Property(e => e.Password)
                .HasMaxLength(65)
                .IsFixedLength();
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsFixedLength();
        });

        modelBuilder.Entity<Merchandise>(entity =>
        {
            entity.HasKey(e => e.MerchandiseId).HasName("PK_Merchandise");

            entity.Property(e => e.MerchandiseId).HasColumnName("MerchandiseID");
            entity.Property(e => e.BrandId).HasColumnName("BrandID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(50)
                .HasColumnName("ImageURL");
            entity.Property(e => e.MerchandiseName).HasMaxLength(30);

            entity.HasOne(d => d.Brand).WithMany(p => p.Merchandises)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Merchandises_Brands");

            entity.HasOne(d => d.Category).WithMany(p => p.Merchandises)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Merchandise_Category");
        });

        modelBuilder.Entity<MerchandiseTag>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.MerchandiseId).HasColumnName("MerchandiseID");
            entity.Property(e => e.TagId).HasColumnName("TagID");

            entity.HasOne(d => d.Merchandise).WithMany()
                .HasForeignKey(d => d.MerchandiseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MerchandiseTags_Merchandises");

            entity.HasOne(d => d.Tag).WithMany()
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MerchandiseTags_Tags");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ContactPhoneNumber)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.DeliveryAddress).HasMaxLength(100);
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.PaymentMethodId).HasColumnName("PaymentMethodID");
            entity.Property(e => e.PurchaseTime).HasColumnType("datetime");
            entity.Property(e => e.Remark).HasMaxLength(200);

            entity.HasOne(d => d.Member).WithMany(p => p.Orders)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Member");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_PaymentMethods");
        });

        modelBuilder.Entity<OrderList>(entity =>
        {
            entity.HasKey(e => e.OrderListId).HasName("PK_OrderList");

            entity.Property(e => e.OrderListId).HasColumnName("OrderListID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.SpecId).HasColumnName("SpecID");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderLists)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderList_Orders");

            entity.HasOne(d => d.Spec).WithMany(p => p.OrderLists)
                .HasForeignKey(d => d.SpecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderLists_Specs");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.Property(e => e.PaymentMethodId)
                .ValueGeneratedNever()
                .HasColumnName("PaymentMethodID");
            entity.Property(e => e.PaymentMethodName).HasMaxLength(50);
        });

        modelBuilder.Entity<Spec>(entity =>
        {
            entity.HasKey(e => e.SpecId).HasName("PK_ProductName");

            entity.Property(e => e.SpecId).HasColumnName("SpecID");
            entity.Property(e => e.MerchandiseId).HasColumnName("MerchandiseID");
            entity.Property(e => e.SpecName).HasMaxLength(50);

            entity.HasOne(d => d.Merchandise).WithMany(p => p.Specs)
                .HasForeignKey(d => d.MerchandiseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Specs_Merchandises");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.Property(e => e.TagId).HasColumnName("TagID");
            entity.Property(e => e.TagName).HasMaxLength(50);
        });

        modelBuilder.Entity<UsedCoupon>(entity =>
        {
            entity.Property(e => e.UsedCouponId).HasColumnName("UsedCouponID");
            entity.Property(e => e.CouponId).HasColumnName("CouponID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");

            entity.HasOne(d => d.Coupon).WithMany(p => p.UsedCoupons)
                .HasForeignKey(d => d.CouponId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsedCoupons_Coupons");

            entity.HasOne(d => d.Order).WithMany(p => p.UsedCoupons)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsedCoupons_Orders");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
