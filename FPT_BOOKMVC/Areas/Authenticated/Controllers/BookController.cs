using FPT_BOOKMVC.Data;
using FPT_BOOKMVC.Models;
using FPT_BOOKMVC.ModelsCRUD.Book;
using FPT_BOOKMVC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace FPT_BOOKMVC.Areas.Authenticated.Controllers
{
    [Area(SD.AuthenticatedArea)]
    [Authorize(Roles = SD.StoreOwnerRole + "," + SD.CustomerRole)]
    public class BookController : Controller
    {
        string global_image_change_url = "";
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public BookController(ApplicationDbContext context, IWebHostEnvironment webHost)
        {
            this.context = context;
            webHostEnvironment = webHost;
        }
        [HttpGet]
        public async Task<IActionResult> BookIndex()
        {
            var book = await context.Books.Include(_ => _.Category).Include(_ => _.PublishCompany).ToListAsync();
            return View(book);
        }
        [HttpGet]
        public IActionResult CreateBook()
        {
            ViewBag.Category_id = new SelectList(context.Categories, "CategoryId", "Name"); //đưa ra từ create book
            ViewBag.Company_id = new SelectList(context.PublicCompanies, "PublishingCompanyId", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateBook(AddBookViewModel BookModel)
        {
            ViewBag.Category_id = new SelectList(context.Categories, "CategoryId", "Name", BookModel.CategoryId);
            ViewBag.Company_id = new SelectList(context.PublicCompanies, "PublishingCompanyId", "Name", BookModel.PublishCompanyId);
            string uniqueFileName = UploadedFile(BookModel);
            var book = new Book()
            {
                //lay các thuộc tính ra
                Name = BookModel.Name,
                Quantity = BookModel.Quantity,
                Price = BookModel.Price,
                Description = BookModel.Description,
                UpdateDate = BookModel.UpdateDate,
                Author = BookModel.Author,
                Image = uniqueFileName,
                FronImage = BookModel.FronImage,
                CategoryId = BookModel.CategoryId,
                PublishCompanyId = BookModel.PublishCompanyId,
                Category = BookModel.Category,
                PublishCompany = BookModel.PublishCompany
            };

            foreach (var bookitem in context.Books.ToList())
            {
                if (book.Name == bookitem.Name)
                {
                    var NewQuantity = bookitem.Quantity.ToString();
                    var StoredQuantity = book.Quantity.ToString();
                    int ToStoredQuantity = int.Parse(StoredQuantity) + int.Parse(NewQuantity); //số lượng sách
                    bookitem.Quantity = ToStoredQuantity;
                    bookitem.UpdateDate = book.UpdateDate;
                    await context.SaveChangesAsync();
                    return RedirectToAction("BookIndex");
                }
            }

            context.Books.Attach(book); //đính kèm
            context.Entry(book).State = EntityState.Added;
            await context.Books.AddAsync(book);
            await context.SaveChangesAsync();
            return RedirectToAction("BookIndex");
        }
        [HttpGet]
        public async Task<IActionResult> ViewBook(int id)
        {
            ViewBag.Category_id = new SelectList(context.Categories, "CategoryId", "Name");
            ViewBag.Company_id = new SelectList(context.PublicCompanies, "PublishingCompanyId", "Name");
            var book = await context.Books.FirstOrDefaultAsync(x => x.BookId == id);
            if (book != null)
            {
                //get id của sách lấy cacs attribute thuộc tính
                var viewmodel = new UpdateBookView()
                {
                    BookId = book.BookId,
                    Name = book.Name,
                    Quantity = book.Quantity,
                    Price = book.Price,
                    Description = book.Description,
                    UpdateDate = book.UpdateDate,
                    Author = book.Author,
                    Image = book.Image,
                    FronImage = book.FronImage,
                    CategoryId = book.CategoryId,
                    PublishCompanyId = book.PublishCompanyId

                };

                global_image_change_url = book.Image;
                return await Task.Run(() => View("ViewBook", viewmodel));
            }

            return RedirectToAction("BookIndex");
        }
        [HttpPost]
        public async Task<IActionResult> ViewBook(UpdateBookView model)
        {
            ViewBag.Category_id = new SelectList(context.Categories, "CategoryId", "Name", model.CategoryId);
            ViewBag.Company_id = new SelectList(context.PublicCompanies, "PublishingCompanyId", "Name", model.PublishCompanyId);//render du lieu ra view
            var book = await context.Books.FirstOrDefaultAsync(x => x.BookId == model.BookId);
            string change_img = UploadedFile(model);
            if (book != null)
            {
                book.Name = model.Name;
                book.Quantity = model.Quantity;
                book.Price = model.Price;
                book.Description = model.Description;
                book.UpdateDate = model.UpdateDate;
                book.Author = model.Author;
                if (change_img != null)
                {
                    book.Image = change_img;
                }
                book.CategoryId = model.CategoryId;
                book.PublishCompanyId = model.PublishCompanyId;

                await context.SaveChangesAsync();
            }
            else
            {
                return NotFound("Book Not Found");

            }
            await context.SaveChangesAsync();

            return RedirectToAction("BookIndex");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteBook(UpdateBookView model)
        {
            var book = await context.Books.FindAsync(model.BookId);

            if (book != null)
            {
                context.Books.Remove(book);

                await context.SaveChangesAsync();

                return RedirectToAction("BookIndex");
            }

            return RedirectToAction("BookIndex");
        }

        private string UploadedFile(AddBookViewModel model)
        {
            string uniqueFileName = null;

            if (model.FronImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.FronImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.FronImage.CopyTo(fileStream);
                }

            }
            return uniqueFileName;
        }
        private string UploadedFile(UpdateBookView model)
        {
            string uniqueFileName = null;

            if (model.FronImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.FronImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.FronImage.CopyTo(fileStream);
                }

            }
            return uniqueFileName;
        }
        private string UploadedFile(Book model)
        {
            string uniqueFileName = null;

            if (model.FronImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.FronImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.FronImage.CopyTo(fileStream);
                }

            }
            return uniqueFileName;
        }

		/*
        1. Mục đích: Xử lý tệp tin hình ảnh được tải lên từ mô hình Book và lưu trữ nó trong thư mục wwwroot/Images với tên tệp tin duy nhất.
Tham số:
Book model: Đối tượng Book chứa thông tin về hình ảnh cần được xử lý.
        2.Xác định thư mục lưu trữ và tạo tên duy nhất cho tệp tin hình ảnh.
Mô tả:
uploadsFolder: Là đường dẫn đến thư mục wwwroot/Images, nơi tệp tin sẽ được lưu trữ.
uniqueFileName: Tạo tên duy nhất cho tệp tin bằng cách sử dụng Guid.NewGuid().ToString() để tạo một chuỗi ngẫu nhiên và ghép nó với tên gốc của tệp tin từ model.FronImage.FileName.
        3.Mục đích: Lưu trữ tệp tin hình ảnh vào thư mục wwwroot/Images.
Mô tả:
filePath: Đường dẫn đầy đủ đến tệp tin mới được tạo.
using (var fileStream = new FileStream(filePath, FileMode.Create)): Sử dụng FileStream để tạo và mở một luồng để ghi dữ liệu vào tệp tin.
model.FronImage.CopyTo(fileStream): Sao chép dữ liệu từ luồng dữ liệu của model.FronImage (hình ảnh tải lên) vào tệp tin mới được tạo.
        4.Phương thức trả về uniqueFileName, cho phép mã gọi phương thức này sử dụng tên tệp tin đã được tạo trong quá trình xử lý.
        */
		[HttpGet]
        public async Task<IActionResult> BookProduct()
        {
            var book = await context.Books.ToListAsync();
            return View(book);
        }
        public async Task<IActionResult> BookProductDetail(int id)
        {

            var book = await context.Books.FirstOrDefaultAsync(x => x.BookId == id);
            ViewBag.Category_id = new SelectList(context.Categories, "CategoryId", "Name");
            ViewBag.Company_id = new SelectList(context.PublicCompanies, "PublishingCompanyId", "Name");
            if (book != null)
            {
                var viewmodel = new Book()
                {
                    BookId = book.BookId,
                    Name = book.Name,
                    Quantity = book.Quantity,
                    Price = book.Price,
                    Description = book.Description,
                    UpdateDate = book.UpdateDate,
                    Author = book.Author,
                    Image = book.Image,
                    CategoryId = book.CategoryId,
                    PublishCompanyId = book.PublishCompanyId

                };

                return await Task.Run(() => View("BookProductDetail", viewmodel));
            }

            return RedirectToAction("BookProduct");

        }

        public async Task<IActionResult> SearchBook(string Search)
        {
            var search_list = new List<Book>();
            if (Search == null || Search == "")
            {
                return RedirectToAction("BookProduct");
            }
            foreach (var book in context.Books.ToList())
            {
                if (book.Name.Contains(Search))
                {
                    search_list.Add(book);
                }
            }
            return View(search_list);
        }
    }
}

