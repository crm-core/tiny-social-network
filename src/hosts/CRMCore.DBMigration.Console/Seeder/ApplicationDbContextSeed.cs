﻿using CRMCore.Framework.Entities.Identity;
using CRMCore.Module.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCore.DBMigration.Console.Seeder
{
    public class ApplicationDbContextSeed
    {
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher = new PasswordHasher<ApplicationUser>();

        public async Task SeedAsync(ApplicationDbContext context, IHostingEnvironment env, ILogger<ApplicationDbContextSeed> logger)
        {
            logger.LogInformation("Begin Seed data - Application DB context");
            try
            {
                if (!context.Users.Any())
                {
                    await context.Users.AddRangeAsync(GetDefaultUser());
                }

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, $"There is an error migrating data for ApplicationDbContext");
            }
        }

        private IEnumerable<ApplicationUser> GetDefaultUser()
        {
            var user = new ApplicationUser()
            {
                Email = "demouser@nomail.com",
                LastName = "DemoLastName",
                FirstName = "DemoUser",
                PhoneNumber = "1234567890",
                UserName = "demouser@nomail.com",
                NormalizedEmail = "DEMOUSER@NOMAIL.COM",
                NormalizedUserName = "DEMOUSER@NOMAIL.COM",
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, "P@ssw0rd");

            return new List<ApplicationUser>()
            {
                user
            };
        }
    }
}
