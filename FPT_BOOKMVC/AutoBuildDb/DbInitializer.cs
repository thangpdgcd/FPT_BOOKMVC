﻿using Microsoft.AspNetCore.Identity;
using FPT_BOOKMVC.Data;
using FPT_BOOKMVC.Models;
using FPT_BOOKMVC.Utils;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace FPT_BOOKMVC.AutoBuildDb
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;//quản lý vai trò 
		}
        public void Initializer()
		{
			// checking update database, if not migration then migrate
			// Kiểm tra xem có bất kỳ bản cập nhậtcó sự thay đổi cơ sở dữ liệu nào chưa được thực
			//a least 1 update await then _db.Database.Migrate() ; have taking (migrate) the db 
			try
			{
				if (_db.Database.GetPendingMigrations().Count() > 0) _db.Database.Migrate();
			}
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            // checking in table Role, if yes then return, if not deploy the codes after these conditions
            if (_db.Roles.Any(r => r.Name == SD.AdminRole)) return;
            if (_db.Roles.Any(r => r.Name == SD.StoreOwnerRole)) return;
            if (_db.Roles.Any(r => r.Name == SD.CustomerRole)) return;
			

		
			// this will deploy if there no have any role yet
			_roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();//display INotifyCompletion: async-await
			_roleManager.CreateAsync(new IdentityRole(SD.StoreOwnerRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.CustomerRole)).GetAwaiter().GetResult();

            // create user admin
            _userManager.CreateAsync(new ApplicationUser
            {
                Email = "admin@gmail.com",
                UserName = "admin@gmail.com",
                EmailConfirmed = true,
                FullName = "Admin",
                Address = "Address of admin"
            }, "Admin123@").GetAwaiter().GetResult();


			// finding the user which is just have created
			//Tìm người dùng vừa được tạo mới trong cơ sở dữ liệu.
			var admin = _db.ApplicationUsers.Where(a => a.Email == "admin@gmail.com").FirstOrDefault();

			// add that user (admin) to admin role
			//: Gán vai trò "Admin" cho người dùng vừa được tạo mới 
			_userManager.AddToRoleAsync(admin, SD.AdminRole).GetAwaiter().GetResult();
        }
    }
}
