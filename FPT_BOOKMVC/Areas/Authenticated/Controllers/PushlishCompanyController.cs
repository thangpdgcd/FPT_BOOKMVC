using FPT_BOOKMVC.Data;
using FPT_BOOKMVC.Models;
using FPT_BOOKMVC.ModelsCRUD.PushlishCompany;
using FPT_BOOKMVC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FPT_BOOKMVC.Areas.Authenticated.Controllers
{
	[Area(SD.AuthenticatedArea)]
	[Authorize(Roles = SD.StoreOwnerRole)]
	public class PublishCompanyController : Controller
	{
		private readonly ApplicationDbContext context;
		public PublishCompanyController(ApplicationDbContext context)
		{
			this.context = context;
		}
		[HttpGet]
		public async Task<IActionResult> CompanyIndex()
		{
			var company = await context.PublicCompanies.ToListAsync();
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
			var company = new PublishCompany()
			{
				Name = CompanyModel.Name,
				Adress = CompanyModel.Adress,


			};

			await context.PublicCompanies.AddAsync(company);
			await context.SaveChangesAsync();
			return RedirectToAction("CompanyIndex");
		}
		[HttpGet]
		public async Task<IActionResult> ViewCompany(int id)
		{
			var company = await context.PublicCompanies.FirstOrDefaultAsync(x => x.PublishingCompanyId == id);

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
			var company = await context.PublicCompanies.FindAsync(model.PublishingCompanyId);
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
			var company = await context.PublicCompanies.FindAsync(model.PublishingCompanyId);

			if (company != null)
			{
				context.PublicCompanies.Remove(company);


				await context.SaveChangesAsync();

				return RedirectToAction("CompanyIndex");
			}

			return RedirectToAction("CompanyIndex");
		}
	}
}
