using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using FPT_BOOKMVC.Data;
using FPT_BOOKMVC.Utils;
using FPT_BOOKMVC.ModelsCRUD.User;


namespace FPT_BOOKMVC.Areas.Authenticated.Controllers
{
    [Area(SD.AuthenticatedArea)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManger;

        // GET
        public UserController(ApplicationDbContext db, UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
//db: Đối tượng của lớp ApplicationDbContext, cung cấp quyền truy cập đến cơ sở dữ liệu.
//userManager: Đối tượng UserManager<IdentityUser> quản lý các thao tác liên quan đến người dùng, chẳng hạn như tạo, xóa, và quản lý mật khẩu.
//roleManager: Đối tượng RoleManager<IdentityRole> quản lý các vai trò trong hệ thống.
			_db = db;
            _userManager = userManager;
            _roleManger = roleManager;//quản lý vai trò 
		}

        public async Task<IActionResult> AdminIndex()

		{
			// taking current login user id
			var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
//claimsIdentity: Là một đối tượng của lớp ClaimsIdentity, chứa các thông tin liên quan đến người dùng đang đăng nhập.
//claims: Là một đối tượng của lớp Claim, chứa các thông tin chi tiết về người dùng, trong trường hợp này, là NameIdentifier(ID của người dùng).

			//Lấy và gán vai trò cho mỗi người dùng
			// exception itself admin
			var userList = _db.ApplicationUsers.Where(u => u.Id != claims.Value);

            foreach (var user in userList)
			{
                //Lấy danh sách người dùng
				var userTemp = await _userManager.FindByIdAsync(user.Id);
            }
			//Sử dụng vòng lặp để duyệt qua danh sách người dùng.
//Đối với mỗi người dùng, sử dụng await _userManager.FindByIdAsync(user.Id) để lấy thông tin chi tiết về người dùng từ UserManager.Thông tin này sau đó có thể được sử dụng trong View.


			// ReSharper disable once Mvc.ViewNotResolved
			return View(userList.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(AdminIndex));
        }

		[HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
			//Dòng mã var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier); được sử dụng để truy xuất thông tin cụ thể về người dùng từ các claims(quyền) được lưu trữ trong đối tượng ClaimsIdentity.Dưới đây là giải thích chi tiết:

//claimsIdentity: Đối tượng của lớp ClaimsIdentity, đại diện cho danh tính của người dùng. Mỗi người dùng khi đăng nhập sẽ có một danh tính tương ứng với các claims(quyền) của họ.

//claimsIdentity.FindFirst(ClaimTypes.NameIdentifier): Phương thức này được sử dụng để tìm kiếm và trả về claim đầu tiên trong danh sách các claims của người dùng có kiểu là ClaimTypes.NameIdentifier.Trong ngữ cảnh này, ClaimTypes.NameIdentifier thường chứa ID duy nhất của người dùng.

//var claims: Biến này chứa thông tin chi tiết của claim đầu tiên với kiểu là ClaimTypes.NameIdentifier.Thông thường, nó chứa ID duy nhất của người dùng.
			// taking current login user id
			var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var userList = _db.ApplicationUsers
                .Where(u => u.Id != claims.Value)
                .ToList() // Retrieve the list of users from the database//Lấy danh sách người dùng từ cơ sở dữ liệu
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
                .Where(u => u.Id != claims.Value)
                .ToList() // Retrieve the list of users from the database
                .Where(u => _userManager.IsInRoleAsync(u, "StoreOwner").Result); //;lấy danh sách người dùng trả ra kết quả

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
			var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);//Lấy thông tin định danh của người dùng hiện tại:

			// exception itself admin
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

			// exception itself admin
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
			//Kiểm tra xem mô hình (model) của yêu cầu đặt lại mật khẩu có hợp lệ không,
            //tức là dữ liệu được nhập vào đúng định dạng và tuân thủ các quy tắc kiểm định (validation) hay không.
			if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);
                if (user != null)
                {
                    // Set the password without a token
                    var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                    if (removePasswordResult.Succeeded)//Kiểm tra xem việc xóa mật khẩu cũ đã thành công hay không.
					{
                        var addPasswordResult = await _userManager.AddPasswordAsync(user, resetPasswordViewModel.Password);
                        if (addPasswordResult.Succeeded) return RedirectToAction(nameof(AdminIndex)); //Kiểm tra xem việc đặt mật khẩu mới đã thành công hay không.Nếu thành công, chuyển hướng người dùng đến trang quản lý admin.

					}
                }
            }
            return View(resetPasswordViewModel);//Nếu có lỗi hoặc không thành công, trả về view "ResetPassword" với mô hình (model) resetPasswordViewModel để hiển thị thông báo lỗi hoặc thông tin yêu cầu nhập lại mật khẩu.
		}
    }
}
