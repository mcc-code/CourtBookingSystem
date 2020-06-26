using CourtBookingApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourtBookingApp.Models
{
    public class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(serviceProvider.GetRequiredService<
               DbContextOptions<ApplicationDbContext>>());

            if (context.BookingStatus.Any()) { }
            else
            {
                await context.BookingStatus.AddAsync(new BookingStatus { Status = "Pending" });
                await context.BookingStatus.AddAsync(new BookingStatus { Status = "Approved" });
                await context.BookingStatus.AddAsync(new BookingStatus { Status = "Denied" });
                await context.SaveChangesAsync();
            }

            if (context.Court.Any()) { }
            else
            {
                await context.Court.AddAsync(new Court { CourtName = "Badminton Court 1" });
                await context.Court.AddAsync(new Court { CourtName = "Badminton Court 2" });
                await context.SaveChangesAsync();
            }

            if (context.Users.Any())
            {
                return;
            }
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            string[] roles = new string[] { "admin", "customer" };
            foreach (string role in roles)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            var user = new ApplicationUser
            {
                UserName = "admin1"
            };

            await userManager.CreateAsync(user, "admin1"); // set password same as username

            await userManager.AddToRoleAsync(user, "admin"); // add admin role 
        }
    }
}
