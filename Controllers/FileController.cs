using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreshGoldPractice2.Data;
using FreshGoldPractice2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ClosedXML;
using ClosedXML.Excel;
using Microsoft.Data.SqlClient;
using Aspose.Pdf;
using Aspose.Pdf.Text;
using System.Data;
using Microsoft.Extensions.Configuration;


namespace FreshGoldPractice2.Controllers
{
    public class FileController : Controller
    { 
        

        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> _userManager;
        private string[] permittedExtensions = { ".txt", ".xls", ".xlsx" };

        public FileController(ApplicationDbContext context)
        {
            this.context = context;
        }

        #region Show Files
        [Authorize]
        private async Task<FileUploadViewModel> LoadAllFiles()
        {
            var viewModel = new FileUploadViewModel();
            viewModel.UploadOnSystems = await context.uploadOnSystems.ToListAsync();
            viewModel.uploadOnDatabases = await context.uploadOnDatabases.ToListAsync();
            return viewModel;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            //for reUpload
            var fileuploadViewModel = await LoadAllFiles();
            ViewBag.Message = TempData["Message"];
            return View(fileuploadViewModel);
        }
        #endregion

        #region Upload Files

        [HttpPost]
        [Authorize(Roles = "Shipping Employee")]
        public async Task<IActionResult> UploadToFileSystem(List<IFormFile> files, string description)
        {
            foreach (var file in files)
            {
                var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Files\\");
                bool basePathExists = System.IO.Directory.Exists(basePath);
                if (!basePathExists) Directory.CreateDirectory(basePath);
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var filePath = Path.Combine(basePath, file.FileName);
                var extension = Path.GetExtension(file.FileName);
                var uploadedBy = User.Identity.Name.ToString();


                // permitted extensions 

                if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension))
                {
                    //context.SaveChanges();
                    TempData["ExtMessage"] = "Please upload an .xls or .xlsx file.";
                }
                else if (!System.IO.File.Exists(filePath) && permittedExtensions.Contains(extension))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    var fileModel = new UploadOnSystem
                    {
                        CreatedOn = DateTime.Now,
                        FileType = file.ContentType,
                        Extension = extension,
                        Name = fileName,
                        Description = description,
                        UploadedBy = uploadedBy,
                        UploadPath = filePath
                    };
                    context.UploadOnSystem.Add(fileModel);
                    context.SaveChanges();
                }
            }

            TempData["Message"] = "File successfully uploaded.";
            return RedirectToAction("Index");
        }

        #endregion

        #region Download Files
        [Authorize(Roles = "Shipping Employee")]
        public async Task<IActionResult> DownloadFileFromFileSystem(int id)
        {

            var file = await context.UploadOnSystem.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            var memory = new MemoryStream();
            using (var stream = new FileStream(file.UploadPath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, file.FileType, file.Name + file.Extension);
        }

        #endregion

        #region Delete Files
        [Authorize(Roles = "Shipping Employee")]
        public async Task<IActionResult> DeleteFileFromFileSystem(int id)
        {

            var file = await context.UploadOnSystem.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            if (System.IO.File.Exists(file.UploadPath))
            {
                System.IO.File.Delete(file.UploadPath);
            }
            context.UploadOnSystem.Remove(file);
            context.SaveChanges();
            TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from File System.";
            return RedirectToAction("Index");
        }

        #endregion

        #region Export files
        public IActionResult ExportExcel()
        {
            using (var workbook = new XLWorkbook())
            {
                //Sheet Name
                var ws = workbook.AddWorksheet("AddendumReport");
                ws.Range("A1:H1").Merge();
                ws.Cell(1, 1).Value = "FreshGold Addendum Report";
                ws.Cell(1, 1).Style.Font.Bold = true;
                ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(1, 1).Style.Font.FontSize = 25;

                //Styling Headers for the table.

                ws.Range("A2:Y2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("A2:Y2").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Range("A2:Y2").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Range("A2:Y2").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Range("A2:Y2").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Range("A2:Y2").Style.Font.FontSize = 12;


                //Style table 
                ws.Range("A3:Y29").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("A3:Y29").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Range("A3:Y29").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Range("A3:Y29").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Range("A3:Y29").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Range("A3:Y29").Style.Font.FontSize = 12;



                ws.Cell(2, 1).Value = "RecordType";
                ws.Cell(2, 2).Value = "LocCodeId";
                ws.Cell(2, 3).Value = "ContainerID";
                ws.Cell(2, 4).Value = "PalletId";
                ws.Cell(2, 5).Value = "OrganisationId";
                ws.Cell(2, 6).Value = "Country";
                ws.Cell(2, 7).Value = "CommodityId";
                ws.Cell(2, 8).Value = "VarietyId";
                ws.Cell(2, 9).Value = "Pack";
                ws.Cell(2, 10).Value = "Grade";
                ws.Cell(2, 11).Value = "Mark";
                ws.Cell(2, 12).Value = "SizeCount";
                ws.Cell(2, 13).Value = "InvCode";
                ws.Cell(2, 14).Value = "FarmId";
                ws.Cell(2, 15).Value = "TargetMarket";
                ws.Cell(2, 16).Value = "CartonQuant";
                ws.Cell(2, 17).Value = "PalletQuant";
                ws.Cell(2, 18).Value = "IntakeDate";
                ws.Cell(2, 19).Value = "OriginDepot";
                ws.Cell(2, 20).Value = "InspectionDate";
                ws.Cell(2, 21).Value = "OrigIntakeDate";
                ws.Cell(2, 22).Value = "Orchard";
                ws.Cell(2, 23).Value = "ConsNo";
                ws.Cell(2, 24).Value = "Weight";
                ws.Cell(2, 25).Value = "TargetRegion";
                ws.Cell(2, 25).Value = "PackhouseCodeId";

                //Connect to Sql
                System.Data.DataTable dt = new System.Data.DataTable();
                SqlConnection con = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=aspnet-FreshGoldPractice2-586E9FDE-5996-4D87-B9ED-4D71307E6EF6;Trusted_Connection=True;MultipleActiveResultSets=true");
                SqlDataAdapter ad = new SqlDataAdapter("Select * from addendumUploads", con);
                ad.Fill(dt);


                //Adds Addendum file data into the excel file, iterating through the string.
                for (int i = 3; i < 30; i++)
                {
                    foreach (System.Data.DataRow row in dt.Rows)
                    {

                        ws.Cell(i, 1).Value = row[1].ToString();
                        ws.Cell(i, 2).Value = row[2].ToString();


                        ws.Cell(i, 3).Value = row[2].ToString();


                        ws.Cell(i, 4).Value = row[3].ToString();
                        ws.Cell(i, 5).Value = row[4].ToString();
                        ws.Cell(i, 6).Value = row[5].ToString();
                        ws.Cell(i, 7).Value = row[6].ToString();
                        ws.Cell(i, 8).Value = row[7].ToString();
                        ws.Cell(i, 9).Value = row[8].ToString();
                        ws.Cell(i, 10).Value = row[9].ToString();
                        ws.Cell(i, 11).Value = row[10].ToString();
                        ws.Cell(i, 12).Value = row[11].ToString();
                        ws.Cell(i, 13).Value = row[12].ToString();
                        ws.Cell(i, 14).Value = row[13].ToString();
                        ws.Cell(i, 15).Value = row[14].ToString();
                        ws.Cell(i, 16).Value = row[15].ToString();
                        ws.Cell(i, 17).Value = row[16].ToString();
                        ws.Cell(i, 18).Value = row[17].ToString();
                        ws.Cell(i, 19).Value = row[19].ToString();
                        ws.Cell(i, 20).Value = row[20].ToString();
                        ws.Cell(i, 21).Value = row[21].ToString();
                        ws.Cell(i, 22).Value = row[22].ToString();
                        ws.Cell(i, 23).Value = row[23].ToString();
                        ws.Cell(i, 24).Value = row[24].ToString();
                        ws.Cell(i, 25).Value = row[25].ToString();

                    }



                }

                //Saves newly created workbook into local file.
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument-spreadsheetml", "AddendumReport.xlsx");
                }
            }
        }


        #endregion

        #region Search files
        //searches for item in upload database using name of the file or the file description


        //[HttpGet]
        //public async Task<IActionResult> Index(string SearchItem)
        //{
        //    ViewData["GetFileDetails"] = SearchItem;


        //    var searchquery = from x in context.uploadOnDatabases
        //                      select x;
        //    if (!String.IsNullOrEmpty(SearchItem))
        //    {
        //        searchquery = searchquery.Where(x => x.Name.Contains(SearchItem) || x.Description.Contains(SearchItem));
        //    }
        //    return View(await searchquery.ToListAsync());
        //    //  return View(searchquery);
        //}


        #endregion

        #region Export to Pdf report
        public IActionResult ExportPDF()
        {


            //Connect to Sql
            System.Data.DataTable dt1 = new System.Data.DataTable();
            SqlConnection con = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=aspnet-FreshGoldPractice2-586E9FDE-5996-4D87-B9ED-4D71307E6EF6;Trusted_Connection=True;MultipleActiveResultSets=true");
            SqlDataAdapter ad = new SqlDataAdapter("Select * from addendumUploads", con);
            ad.Fill(dt1);


            // Create new a PDF document
            var document = new Document
            {
                PageInfo = new PageInfo { Margin = new(28, 28, 28, 40) }
            };


            var pdfPage = document.Pages.Add();

            // Initializes a new instance of the TextFragment for report's title
            var textFragment = new TextFragment("AddendumReport");



            Table table = new Table()
            {
                // Set column widths of the table
                ColumnWidths = "25% 25% 25% 25%",
                // Set cell padding
                DefaultCellPadding = new MarginInfo(10, 5, 10, 5),
                // Set the table border color as black
                // Set the border for table cells as Black
                Border = new BorderInfo(BorderSide.All, .5f, Color.Black),
                DefaultCellBorder = new BorderInfo(BorderSide.All, .2f, Color.Black),
            };



            //document.Pages[1].Paragraphs.Add(table);

            table.ImportDataTable(dt1, true, 0, 0);
            Aspose.Pdf.Row row = table.Rows.Add();
            for (int rowCount = 1; rowCount < 50; rowCount++)
            {
                row.Cells.Add("(cell,rowCount)").ToString();
            }

            /*
            for (int rowCount = 1;  rowCount >= 30; rowCount++)
            {
                
                // foreach (System.Data.DataRow row in dt1.Rows)
                // { 
                var row1 = table.Rows.Add();
                  //  row1.Cells.Add("(cell,rowCount)").ToString();
                  
                    row1.Cells.Add($"Cell ({rowCount})").ToString();
                    //row1.Cells.Add("Column (" + rowCount + ", 1");
               // }
            }
             */





            pdfPage.Paragraphs.Add(textFragment);
            pdfPage.Paragraphs.Add(table);



            document.Pages[1].Paragraphs.Add(table);



            using (var streamOut = new MemoryStream())
            {
                document.Save(streamOut);
                return new FileContentResult(streamOut.ToArray(), "application/pdf")
                {
                    FileDownloadName = "AddendumReport.pdf"
                };


            }
        }
        #endregion
    }


}
   

