using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProjectBookStores.EF;

public partial class ProjectBookStoreContext : DbContext
{
    public ProjectBookStoreContext()
    {
    }

    public ProjectBookStoreContext(DbContextOptions<ProjectBookStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookDiscount> BookDiscounts { get; set; }

    public virtual DbSet<BookFaverite> BookFaverites { get; set; }

    public virtual DbSet<BookNews> BookNews { get; set; }

    public virtual DbSet<BookRank> BookRanks { get; set; }

    public virtual DbSet<BooksGenre> BooksGenres { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerDiscount> CustomerDiscounts { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Ward> Wards { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-ICOEV4N;Initial Catalog=ProjectBookStore;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
            entity.Property(e => e.DetailAddress).HasMaxLength(250);
            entity.Property(e => e.DistrictId)
                .HasMaxLength(5)
                .HasColumnName("District_ID");
            entity.Property(e => e.ProvinceId)
                .HasMaxLength(2)
                .HasColumnName("Province_ID");
            entity.Property(e => e.WardId)
                .HasMaxLength(10)
                .HasColumnName("Ward_ID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Addresses_Customers");

            entity.HasOne(d => d.District).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.DistrictId)
                .HasConstraintName("FK_Addresses_Districts");

            entity.HasOne(d => d.Province).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.ProvinceId)
                .HasConstraintName("FK_Addresses_Provinces");

            entity.HasOne(d => d.Ward).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.WardId)
                .HasConstraintName("FK_Addresses_Wards");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Admin");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(155)
                .IsUnicode(false);
            entity.Property(e => e.PassWord)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PermissionId).HasColumnName("Permission_ID");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Permission).WithMany(p => p.Admins)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("FK_Admins_Permissions");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AuthorName).HasMaxLength(250);
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GenreId).HasColumnName("Genre_ID");
            entity.Property(e => e.Images)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.IntialPrice).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Language).HasMaxLength(50);
            entity.Property(e => e.MoreImages).HasColumnType("xml");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.PublisherId).HasColumnName("Publisher_ID");
            entity.Property(e => e.Status).HasDefaultValue(true);

            entity.HasOne(d => d.Genre).WithMany(p => p.Books)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK_Books_Books_Genres");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Books)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("FK_Books_Publisher");
        });

        modelBuilder.Entity<BookDiscount>(entity =>
        {
            entity.ToTable("Book_Discounts");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BookId).HasColumnName("Book_ID");
            entity.Property(e => e.DiscountId).HasColumnName("Discount_ID");

            entity.HasOne(d => d.Book).WithMany(p => p.BookDiscounts)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Book_Discounts_Books");

            entity.HasOne(d => d.Discount).WithMany(p => p.BookDiscounts)
                .HasForeignKey(d => d.DiscountId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Book_Discounts_Discounts");
        });

        modelBuilder.Entity<BookFaverite>(entity =>
        {
            entity.ToTable("Book_Faverites");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BookId).HasColumnName("Book_ID");
            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
            entity.Property(e => e.FaverDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Book).WithMany(p => p.BookFaverites)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Book_Faverites_Books");

            entity.HasOne(d => d.Customer).WithMany(p => p.BookFaverites)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Book_Faverites_Customers");
        });

        modelBuilder.Entity<BookNews>(entity =>
        {
            entity.ToTable("Book_News");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BookId).HasColumnName("Book_ID");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Images)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.Title).HasMaxLength(250);

            entity.HasOne(d => d.Book).WithMany(p => p.BookNews)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Book_News_Books");
        });

        modelBuilder.Entity<BookRank>(entity =>
        {
            entity.ToTable("Book_Ranks");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Banner)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.BookId).HasColumnName("Book_ID");

            entity.HasOne(d => d.Book).WithMany(p => p.BookRanks)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Book_Ranks_Books");
        });

        modelBuilder.Entity<BooksGenre>(entity =>
        {
            entity.ToTable("Books_Genres");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateOfBirth)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(250);
            entity.Property(e => e.Images)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.LastName).HasMaxLength(250);
            entity.Property(e => e.PassWord)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PermissionId).HasColumnName("Permission_ID");
            entity.Property(e => e.ResetToken)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ResetTokenExpiration)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Permission).WithMany(p => p.Customers)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("FK_Customers_Permissions");
        });

        modelBuilder.Entity<CustomerDiscount>(entity =>
        {
            entity.ToTable("Customer_Discounts");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
            entity.Property(e => e.DiscountId).HasColumnName("Discount_ID");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerDiscounts)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Customer_Discounts_Customers");

            entity.HasOne(d => d.Discount).WithMany(p => p.CustomerDiscounts)
                .HasForeignKey(d => d.DiscountId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Customer_Discounts_Discounts");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.DiscountPercent).HasColumnType("decimal(5, 2)");
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PK_districts");

            entity.Property(e => e.Code).HasMaxLength(5);
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.ProvinceCode)
                .HasMaxLength(2)
                .HasColumnName("Province_code");

            entity.HasOne(d => d.ProvinceCodeNavigation).WithMany(p => p.Districts)
                .HasForeignKey(d => d.ProvinceCode)
                .HasConstraintName("FK_Districts_Provinces");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
            entity.Property(e => e.Note).HasMaxLength(100);
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OrderDelivery)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Orders_Customers");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusId)
                .HasConstraintName("FK_Orders_OrderStatus");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("Order_Details");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BookId).HasColumnName("Book_ID");
            entity.Property(e => e.OrderId).HasColumnName("Order_ID");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Book).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Order_Details_Books");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Order_Details_Orders");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.ToTable("OrderStatus");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PK_provinces");

            entity.Property(e => e.Code).HasMaxLength(2);
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.ToTable("Publisher");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PublisherName)
                .HasMaxLength(250)
                .HasColumnName("Publisher_Name");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BookId).HasColumnName("Book_ID");
            entity.Property(e => e.CommentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
            entity.Property(e => e.NewId).HasColumnName("New_ID");
            entity.Property(e => e.Rating1).HasColumnName("Rating");

            entity.HasOne(d => d.Book).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Ratings_Books");

            entity.HasOne(d => d.Customer).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Ratings_Customers");

            entity.HasOne(d => d.New).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.NewId)
                .HasConstraintName("FK_Ratings_Book_News");
        });

        modelBuilder.Entity<Ward>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PK_wards");

            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.DistrictCode)
                .HasMaxLength(5)
                .HasColumnName("District_code");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");

            entity.HasOne(d => d.DistrictCodeNavigation).WithMany(p => p.Wards)
                .HasForeignKey(d => d.DistrictCode)
                .HasConstraintName("FK_Wards_Districts");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
