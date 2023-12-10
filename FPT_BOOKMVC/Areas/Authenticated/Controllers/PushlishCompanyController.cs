using FPT_BOOKMVC.Data;
using FPT_BOOKMVC.Models;
using FPT_BOOKMVC.ModelsCRUD.PushlishCompany;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FPT_BOOKMVC.Areas.Authenticated.Controllers
{
    public class PushlishCompanyController : Controller
	{
			private readonly ApplicationDbContext context;

			public PushlishCompanyController(ApplicationDbContext context)
			{
				this.context = context;
			}
			[HttpGet]
			public async Task<IActionResult> CompanyIndex()
			{
				var company = await context.PushlishCompanies.ToListAsync();
				return View(company);
			}
			[HttpGet]
			public IActionResult CreatePublishCompany()
			{
				return View();
			}
			[HttpPost]
			public async Task<IActionResult> CreatePublishCompany(AddCompanyViewModel CompanyModel)
			{
				var company = new PushlishCompany()
				{
					Name = CompanyModel.Name,
					Adress = CompanyModel.Adress,
				};
				await context.PushlishCompanies.AddAsync(company);
				await context.SaveChangesAsync();
				return RedirectToAction("CompanyIndex");
			} 
			[HttpGet]
			public async Task<IActionResult> ViewCompany(int id)
			{
			/**///để truy vấn công ty từ cơ sở dữ liệu dựa trên id được truyền vào.
				//Phương thức FirstOrDefaultAsync trả về phần tử đầu tiên hoặc mặc định
				//(null nếu không tìm thấy) theo điều kiện đưa ra.
				
			var company = await context.PushlishCompanies.FirstOrDefaultAsync(x => x.PublishingCompanyId == id);

				if (company != null)
				{
					var viewmodel = new UpdateCompany()
					{
						PublishingCompanyId = company.PublishingCompanyId,
						Name = company.Name,
						Adress = company.Adress,
					};

					return await Task.Run(() => View("ViewCompany", viewmodel));
				}

				return RedirectToAction("CompanyIndex");
			}
			[HttpPost]
			public async Task<IActionResult> ViewCompany(UpdateCompany model)
			{
			//get id của sách lấy cacs attribute thuộc tính
			var company = await context.PushlishCompanies.FindAsync(model.PublishingCompanyId);
				if (company != null)
				{
					company.Name = model.Name;
					company.Adress = model.Adress;

					await context.SaveChangesAsync();

					return RedirectToAction("CompanyIndex");
				}

				return RedirectToAction("CompanyIndex");
			}
			[HttpPost]
			public async Task<IActionResult> DeleteCompany(UpdateCompany model)
			{
				var company = await context.PushlishCompanies.FindAsync(model.PublishingCompanyId);

				if (company != null)
				{
					context.PushlishCompanies.Remove(company);


					await context.SaveChangesAsync();

					return RedirectToAction("CompanyIndex");
				}

				return RedirectToAction("CompanyIndex");
			}
		
	}
}