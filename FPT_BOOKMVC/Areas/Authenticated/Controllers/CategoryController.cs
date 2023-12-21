using FPT_BOOKMVC.Data;
using FPT_BOOKMVC.Models;
using FPT_BOOKMVC.ModelsCRUD.Category;
using FPT_BOOKMVC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FPT_BOOKMVC.Areas.Authenticated.Controllers
{
    //area
    [Area(SD.AuthenticatedArea)]
    [Authorize(Roles = SD.StoreOwnerRole + "," + SD.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext context;

        public CategoryController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<IActionResult> CategoryIndex()
        {

            List <Category> category = await context.Categories.Where(x => x.IsApproved).ToListAsync();//để lưu trữ danh sách các danh mục đã được phê duyệt.
																									   //Phương thức này được																				   // *gọi để chuyển đổi kết quả của truy vấn thành một danh sách(List)
			return View(category);
        }
       
        [HttpGet] //gửi yêu cầu qua URL.
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost] //dữ liệu từ máy khách(client) lên máy chủ(server) để xử lý.

        public async Task<IActionResult> CreateCategory(AddCategoryViewModel CategoryModel) //lấy dữ liệumodel
        {
            var category = new Category() //tạo cat để nhập dl từ add
            {
                Name = CategoryModel.Name,//gán
				Description = CategoryModel.Description
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            return RedirectToAction("CategoryIndex");
        }
        [HttpGet]// hiển thị trang cập nhật
		public async Task<IActionResult> ViewCategory(int id)//: Định danh duy nhất của danh mục cần cập nhật.
		{
            var category = await context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id); //trùng với tham số id,  search danh mục


			if (category != null) //lấy thông tin từ cate gán model
            {
                var viewmodel = new UpdateCategoryView() //truyền dữ liệu cập nhật
                {
                    CategoryId = category.CategoryId,
                    Name = category.Name,
                    Description = category.Description
                };

                return await Task.Run(() => View("ViewCategory", viewmodel)); //truyền đối tượng viewmodel //trả về trang cập nhật với dữ liệu đã được chuẩn bị.

			}
            return RedirectToAction("CategoryIndex");
        }
        [HttpPost]
        public async Task<IActionResult> ViewCategory(UpdateCategoryView model)
        {
            var category = await context.Categories.FindAsync(model.CategoryId); //Lấy danh mục cần cập nhật từ cơ sở dữ liệu. lấy từ cat để dựa trên id từ model
			if (category != null)
            {
                category.Name = model.Name;
                category.Description = model.Description;

                await context.SaveChangesAsync();

                return RedirectToAction("CategoryIndex");
            }

            return RedirectToAction("CategoryIndex");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(UpdateCategoryView model)
        {
            var category = await context.Categories.FindAsync(model.CategoryId);

            if (category != null)
            {
                context.Categories.Remove(category);

                await context.SaveChangesAsync();

                return RedirectToAction("CategoryIndex");
            }

            return RedirectToAction("CategoryIndex");
        }
		[HttpGet]
		public async Task<IActionResult> CategoryApproved()
		{
			var category = await context.Categories.ToListAsync();// danh sách đã được phê duyệt
			return View(category);
		}
		[HttpPost("{id}/approve")] //xác định đường dẫn này để xử lý, id là 1 tham số đường đẫn
        public async Task<IActionResult> Approve(int id)
        {
            var category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            category.IsApproved = true;//phê duyệt một danh mục bằng cách đặt thuộc tính
			context.SaveChanges();

            // code to notify store owner of category approval here

            return RedirectToAction(nameof(CategoryApproved));
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            var category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            context.Categories.Remove(category);
            context.SaveChanges();

            // code to notify store owner of category rejection here

            return RedirectToAction(nameof(CategoryApproved));
        }
    }
}
