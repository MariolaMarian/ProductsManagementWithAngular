using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductsMngmtAPI.Models;

namespace ProductsMngmtAPI.Data
{
    public class DataContext : IdentityDbContext<User,Role,string, IdentityUserClaim<string>,
    UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public DataContext(DbContextOptions<DataContext> options):base(options){}

        public DbSet<Product> Products {get;set;}
        public DbSet<ExpirationDate> ExpirationDates {get;set;}
        public DbSet<Category> Categories {get;set;}
        public DbSet<UserCategory> UserCategories {get; set;}
        
        protected override void OnModelCreating(ModelBuilder builder){
            base.OnModelCreating(builder);

            builder.Entity<UserRole>(userRole => {

                userRole.HasKey(ur => new { ur.UserId, ur.RoleId});

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<UserCategory>(userCategory => {

                userCategory.HasKey(uc => new { uc.UserId, uc.CategoryId});

                userCategory.HasOne(uc => uc.Category)
                    .WithMany(c => c.CategoryUsers)
                    .HasForeignKey(ec => ec.CategoryId);

                userCategory.HasOne(uc => uc.User)
                    .WithMany(u => u.UserCategories)
                    .HasForeignKey(uc => uc.UserId);
            });

            builder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
        }
    }
}