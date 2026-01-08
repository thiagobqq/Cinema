using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infra.Seeder
{
    public class UserSeeder
    {   
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
                using var scope = serviceProvider.CreateScope();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                var adminEmail = "admin@gmail.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    adminUser = new AppUser
                    {
                        UserName = "admin",
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(adminUser, "123456");

                    if (result.Succeeded)
                    {
                        var roleResult = await userManager.AddToRoleAsync(adminUser, "Admin");

                        if (!roleResult.Succeeded)
                        {
                            Console.WriteLine($"Erro ao adicionar role: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                        }
                        else
                        {
                            Console.WriteLine("Usuário admin criado e role 'Admin' atribuída com sucesso!");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Erro ao criar admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    Console.WriteLine("Usuário admin já existe.");
                }
        }    
    }
}