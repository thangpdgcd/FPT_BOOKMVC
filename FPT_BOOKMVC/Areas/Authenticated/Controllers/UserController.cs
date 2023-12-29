using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using FPT_BOOKMVC.Data;
using FPT_BOOKMVC.Utils;
using FPT_BOOKMVC.ModelsCRUD.User;
using FPT_BOOKMVC.Models;


namespace FPT_BOOKMVC.Areas.Authenticated.Controllers
{
    [Area(SD.AuthenticatedArea)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManger;

		// GET - ClaimsIdentity-xacthucthongtinuser
		public UserController(ApplicationDbContext db, UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
        {
			_db = db;
			_userManager = userManager;
            _roleManger = roleManager;
		}
		
		public async Task<IActionResult> AdminIndex()
		{
			// taking current login user id -> claim identity of System.Security.Claims;
			var claimsIdentity = (ClaimsIdentity)User.Identity;

			//search claim contain id user information - attribute claimtype
			var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier); 

			//get userlist of db, exclude(-) user current
			var userList = _db.ApplicationUsers.Where(u => u.Id != claims.Value);
			foreach (var user in userList)
			{
				var userTemp = await _userManager.FindByIdAsync(user.Id);
            }
			return View(userList.ToList());
		}

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            await _userManager.DeleteAsync(user);
            return RedirectToAction("AdminIndex");
        }

		[HttpGet]
		public async Task<IActionResult> GetCustomers()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			var userList = _db.ApplicationUsers
				.Where(u => u.Id != claims.Value)
				.ToList() 
				.Where(u => _userManager.IsInRoleAsync(u, "Customer").Result);

			foreach (var user in userList)
			{
				var userTemp = await _userManager.FindByIdAsync(user.Id);

			}
			return View(userList.ToList());
		}
	[HttpGet]

	public async Task<IActionResult> GetStoreOwners()
	{
		// taking current login user id
		var claimsIdentity = (ClaimsIdentity)User.Identity;
		var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
		var userList = _db.ApplicationUsers
			.Where(u => u.Id != claims.Value)// id!= value curent
			.ToList() 
			.Where(u => _userManager.IsInRoleAsync(u, "StoreOwner").Result); 

		foreach (var user in userList)
		{
			var userTemp = await _userManager.FindByIdAsync(user.Id);
		}

		return View(userList.ToList());

	}
	[HttpGet]
	public async Task<IActionResult> ProfileUser()
	{
			// taking current login user id
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


			var userList = _db.ApplicationUsers.Where(u => u.Id == claims.Value);

			foreach (var user in userList)
			{
				var userTemp = await _userManager.FindByIdAsync(user.Id);

			}
			return View(userList.ToList());

		}

	public async Task<IActionResult> HelpUser()
	{
		// taking current login user id
		var claimsIdentity = (ClaimsIdentity)User.Identity;
		var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
		var userList = _db.ApplicationUsers.Where(u => u.Id == claims.Value);

		foreach (var user in userList)
		{
			var userTemp = await _userManager.FindByIdAsync(user.Id);

		}
		return View(userList.ToList());
	}

	[HttpGet]
	[Authorize(Roles = SD.AdminRole)]
	public async Task<IActionResult> ResetPassword()
	{
		var resetPasswordViewModel = new ResetPasswordViewModel();
		return View(resetPasswordViewModel);
	}

	[HttpPost]
	[Authorize(Roles = SD.AdminRole)]
	public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
	{

		if (ModelState.IsValid)
		{
			var user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);
			if (user != null)
			{
				// Set the password without a token
				var removePasswordResult = await _userManager.RemovePasswordAsync(user);
				if (removePasswordResult.Succeeded)
				{
						var addPasswordResult = await _userManager.AddPasswordAsync(user, resetPasswordViewModel.Password);
						if (addPasswordResult.Succeeded) return RedirectToAction("AdminIndex");
				}
			}
		}
			return View(resetPasswordViewModel);
	}	
}
}