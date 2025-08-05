
using Domains.Entities;
using Domains.Entities.Identity;
using Domains.Entities.Product;
using Domains.Identity;
using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.ApplicationContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser , Domains.Entities.Identity.Role, string>
    {
        private readonly IEncryptionProvider _encryptionProvider;

        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Photo> Photo { get; set; }
        public  DbSet<Address> Address { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<UserRefreshToken> UserRefreshToken { get; set; }



        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            _encryptionProvider = new GenerateEncryptionProvider("7376406c78014aa894a4daeee1a32276");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.UseEncryption(_encryptionProvider);
            //modelBuilder.Entity<VwBook>(entity =>
            //{
            //    entity.HasNoKey();
            //    entity.ToView("VwBook");
            //});

            //modelBuilder.Entity<VwUserProfile>(entity =>
            //{
            //    entity.HasNoKey();
            //    entity.ToView("VwUserProfile");
            //});

            //modelBuilder.Entity<TbRefreshToken>(entity =>
            //{
            //    entity.HasKey(e => e.Id);

            //    entity.Property(e => e.Token)
            //          .IsRequired();

            //    entity.Property(e => e.ExpiresAt)
            //          .IsRequired();

            //    entity.Property(e => e.CurrentState)
            //          .HasDefaultValue(1);


            //});

        }
    }

}
