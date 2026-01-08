using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infra.Seeder
{
    public class RoleSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] { "Admin", "User" };

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new IdentityRole(roleName);
                    var result = await roleManager.CreateAsync(role);

                    if (result.Succeeded)
                    {
                        Console.WriteLine($"Role '{roleName}' created successfully!");
                    }
                    else
                    {
                        Console.WriteLine($"Erro creating role '{roleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    Console.WriteLine($"Role '{roleName}' already exists.");
                }
            }
        }
        
    }
}