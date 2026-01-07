using Auth.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infra.Data
{
    internal class AuthDbContext(DbContextOptions<AuthDbContext> options) : IdentityDbContext<AppUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("auth");

            builder.Entity<AppUser>().ToTable("Users", "auth");
            builder.Entity<IdentityRole>().ToTable("Roles", "auth");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "auth");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "auth");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "auth");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "auth");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "auth");
        }
    }
}