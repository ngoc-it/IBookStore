using AspNetCore.Reporting;
using BookStore.Models.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using System.Data;
using System.Text;
using System.IO.Packaging;
namespace BookStore.Controllers
{
    public class ReportController : Controller
    {
        private readonly IBookService _bookService;


        public ReportController(IBookService bookService)
        {
            _bookService = bookService;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        /*public IActionResult Index()
        {
            return View();
        }*/
        public IActionResult ExportBookReport()
        {
            var format = Request.Query["format"].ToString();
            var categoryId = Request.Query["categoryId"].ToString();

            // Lọc sách theo danh mục nếu có
            var books = string.IsNullOrEmpty(categoryId) || categoryId == "0"
                ? _bookService.GetAllBooks() // Tất cả sách
                : _bookService.GetAllBooks().Where(x => x.CategoryId.ToString() == categoryId).ToList();
            // Khởi tạo DataTable để chứa dữ liệu báo cáo
            DataTable dataTable = new DataTable("dsSach");
            /*dataTable.Columns.Add("BookImage", typeof(string));*/
            dataTable.Columns.Add("BookId", typeof(string));
            dataTable.Columns.Add("BookName", typeof(string));
            dataTable.Columns.Add("CategoryName", typeof(string));
            dataTable.Columns.Add("Quantity", typeof(int));
            dataTable.Columns.Add("SoldQuantity", typeof(int));
            dataTable.Columns.Add("Price", typeof(decimal));
            dataTable.Columns.Add("PriceDiscount", typeof(decimal));
            /*            dataTable.Columns.Add("IsActive", typeof(bool));*/

            foreach (var book in books)
            {
                dataTable.Rows.Add(
                    /*book.BookImage,*/
                    book.BookId,
                    book.BookName,
                    book.CategoryName,
                    book.Quantity,
                    book.SoldQuantity,
                    book.Price,
                    book.PriceDiscount
                /*                    book.IsActive*/
                );
            }

            // Đường dẫn tới file RDLC
            string rdlcPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports", "BookRPT.rdlc");

            // Khởi tạo LocalReport với AspNetCore.Reporting
            var report = new LocalReport(rdlcPath);

            // Thêm nguồn dữ liệu vào báo cáo với đúng tên "dsSach"
            report.AddDataSource("dsSach", dataTable);

            /* // Xuất báo cáo ra PDF
             var result = report.Execute(RenderType.Pdf, 1);

             // Trả về file PDF
             return File(result.MainStream, "application/pdf", "BookReport.pdf");*/
            byte[] result = null;
            string fileName = "BookReport";

            // Kiểm tra định dạng yêu cầu và xuất báo cáo tương ứng
            switch (format?.ToLower())
            {
                case "pdf":
                    result = report.Execute(RenderType.Pdf, 1).MainStream;
                    fileName = "BookReport.pdf";
                    return File(result, "application/pdf", fileName);

                case "excel":
                    result = report.Execute(RenderType.ExcelOpenXml, 1).MainStream;
                    fileName = "BookReport.xlsx";
                    return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

                case "word":
                    result = report.Execute(RenderType.WordOpenXml, 1).MainStream;
                    fileName = "BookReport.docx";
                    return File(result, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);

                default:
                    return BadRequest("Invalid format specified. Supported formats are: PDF, Excel, Word.");
            }
        }

    }
}
