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
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FreshGoldPractice2.Controllers
{
    public class UploadController : Controller
    {

        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> _userManager;
        private string[] permittedExtensions = { ".txt", ".xls", ".xlsx" };



        public UploadController(ApplicationDbContext context)
        {
            this.context = context;
        }



        #region Show Files

        [Authorize]
        private async Task<UploadViewModel> LoadAllFiles()
        {
            var viewModel = new UploadViewModel();
            viewModel.UploadsOnDatabase = await context.UploadOnDatabase.ToListAsync();
            viewModel.UploadsOnSystem = await context.UploadOnSystem.ToListAsync();
            viewModel.addendumUploads = await context.addendumUploads.ToListAsync();
            return viewModel;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var uploadViewModel = await LoadAllFiles();
            ViewBag.Message = TempData["Message"];
            return View(uploadViewModel);
        }

        private IActionResult GetUploadList(IEnumerable<AddendumUpload> uploadList)
        {
            List<Models.AddendumUpload> uploadLists = new List<Models.AddendumUpload>();
            uploadLists = context.addendumUploads.ToList();
            return View(uploadLists);
        }

        #endregion

        #region Upload Files

        [Authorize(Roles = "FreshGold Employee")]
        [HttpPost]
        public async Task<IActionResult> UploadToFileSystem(List<IFormFile> files, string description)
        {
            foreach (var file in files)
            {
                var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Uploads\\");
                bool basePathExists = System.IO.Directory.Exists(basePath);
                if (!basePathExists) Directory.CreateDirectory(basePath);
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var filePath = Path.Combine(basePath, file.FileName);
                var extension = Path.GetExtension(file.FileName).ToLower();
                var uploadedBy = User.Identity.Name.ToString();

                if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension))
                {
                    //context.SaveChanges();
                    TempData["ExtMessage"] = "Please upload a .txt file.";
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
                    TempData["Message"] = "File successfully uploaded.";

                }
            }

            return RedirectToAction("Index");

        }

        [HttpPost]
        [Authorize(Roles = "FreshGold Employee")]
        public async Task<IActionResult> UploadToDatabase(List<IFormFile> files, string description)
        {
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName).ToLower();
                var uploadedBy = User.Identity.Name.ToString();


                if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension))
                {
                    TempData["ExtMessage"] = "Please upload a .txt, .xls, or .xlsx file.";

                }
                else if (permittedExtensions.Contains(extension))
                {
                    var fileModel = new UploadOnDatabase
                    {
                        CreatedOn = DateTime.Now,
                        FileType = file.ContentType,
                        Extension = extension,
                        Name = fileName,
                        Description = description,
                        UploadedBy = uploadedBy

                    };
                    using (var dataStream = new MemoryStream())
                    {
                        await file.CopyToAsync(dataStream);
                        fileModel.Data = dataStream.ToArray();
                    }
                    context.UploadOnDatabase.Add(fileModel);
                    context.SaveChanges();
                    TempData["Message"] = "File successfully uploaded to Database";
                }

            }
            return RedirectToAction("Index");
        }

        #endregion

        #region View CSV Data


        public async Task<IActionResult> ViewUpload(int id)
        {
            //var file = await context.UploadOnSystem.Where(x => x.Id == id).FirstOrDefaultAsync();
            Models.UploadOnSystem fileId = context.UploadOnSystem.Find(id);
            string filePath = fileId.UploadPath;

            context.addendumUploads.RemoveRange();

            //read file
            string lines = System.IO.File.ReadAllText(filePath);
            var addendumData = new AddendumUpload();

            
            foreach (string line in lines.Split('\n'))
            {
                #region variables
                string recordType = "";
                string locCodeId = "";
                string palletId = "";
                string organisationId = "";
                string country = "";
                string commodityId = "";
                string varietyId = "";
                string pack = "";
                string grade = "";
                string mark = "";
                string sizeCount = "";
                string invCode = "";
                string farmId = "";
                string targetMarket = "";
                string cartonQuant = "";
                string palletQuant = "";
                string intakeDate = "";
                string origDepot = "";
                string origIntakeDate = "";
                string orchard = "";
                string consNo = "";
                string weight = "";
                string targetRegion = "";
                string packhouseCodeId = "";
                string inspDate = "";
                //string line;    //Holds the entire line
                #endregion
                if (!string.IsNullOrEmpty(line) && line.Length > 449)
                {
                    
                    #region substrings
                    recordType = line.Substring(0, 2).Trim();
                    locCodeId = line.Substring(190, 6).Trim();
                    palletId = line.Substring(48, 18).Trim();
                    organisationId = line.Substring(49, 3).Trim();
                    country = line.Substring(41, 2).Trim();
                    commodityId = line.Substring(57, 2).Trim();
                    varietyId = line.Substring(61, 3).Trim();
                    pack = line.Substring(70, 4).Trim();
                    grade = line.Substring(71, 1).Trim();
                    mark = line.Substring(85, 7).Trim();
                    sizeCount = line.Substring(78, 2).Trim();
                    invCode = line.Substring(88, 2).Trim();
                    farmId = line.Substring(101, 5).Trim();
                    targetMarket = line.Substring(129, 2).Trim();
                    cartonQuant = line.Substring(131, 5).Trim();
                    palletQuant = line.Substring(119, 4).Trim();
                    intakeDate = line.Substring(173, 8).Trim();
                    origDepot = line.Substring(140, 6).Trim();
                    inspDate = line.Substring(397, 8).Trim();
                    origIntakeDate = line.Substring(147, 8).Trim();
                    orchard = line.Substring(514, 15).Trim();
                    consNo = line.Substring(23, 10).Trim();
                    weight = line.Substring(270, 8).Trim();
                    targetRegion = line.Substring(487, 3).Trim();
                    packhouseCodeId = line.Substring(421, 6).Trim();
                    #endregion
                    addendumData = new AddendumUpload
                    {
                        //assign variable data to each thing                                              
                        RecordType = recordType,
                        LocCodeId = locCodeId,
                        PalletId = palletId,
                        OrganisationId = organisationId,
                        Country = country,
                        CommodityId = commodityId,
                        VarietyId = varietyId,
                        Pack = pack,
                        Grade = grade,
                        Mark = mark,
                        SizeCount = sizeCount,
                        InvCode = invCode,
                        FarmId = farmId,
                        TargetMarket = targetMarket,
                        CartonQuant = cartonQuant,
                        PalletQuant = palletQuant,
                        IntakeDate = intakeDate,
                        OriginDepot = origDepot,
                        InspectionDate = inspDate,
                        OrigIntakeDate = origIntakeDate,
                        Orchard = orchard,
                        ConsNo = consNo,
                        Weight = weight,
                        TargetRegion = targetRegion,
                        PackhouseCodeId = packhouseCodeId
                    };
                    // uploadDataList.Add(addendumData);

                    //add list type 

                   List<string> PalletList = new List<string>();

                    for (int i = 0; i < 50; i++)
                    {
                        PalletList.Add(context.addendumUploads.Add(addendumData).ToString());
                        //context.addendumUploads.Add(addendumData).ToString();
                       
                    }
                    

                       // context.addendumUploads.Add(addendumData).ToString();
                        //var viewModelList = GetUploadList(uploadDataList);
                       //  return View(viewModelList);
                    

                }

                context.SaveChanges();
               
            }
            return View("ViewUpload", addendumData);

        }

        #endregion

        #region Download files
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

        public async Task<IActionResult> DownloadFileFromDatabase(int id)
        {
            var file = await context.UploadOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            return File(file.Data, file.FileType, file.Name + file.Extension);
        }

        #endregion

        #region Delete File
        [Authorize(Roles = "FreshGold Employee")]
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

        [Authorize(Roles = "FreshGold Employee")]
        public async Task<IActionResult> DeleteFileFromDatabase(int id)
        {
            var file = await context.UploadOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
            context.UploadOnDatabase.Remove(file);
            context.SaveChanges();
            TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from Database.";
            return RedirectToAction("Index");
        }

        #endregion

        #region Search files
        //searches for item in upload database using name of the file or the file description

        /*
        [HttpGet]
        public async Task<IActionResult> Index(string SearchItem)
        {
            var searchquery = from m in context.UploadOnDatabase
                         select m;

            if (!String.IsNullOrEmpty(SearchItem))
            {
                searchquery = searchquery.Where(s => s.Description.Contains(SearchItem));
                searchquery = searchquery.Where(s => s.Name.Contains(SearchItem));
            }


           return View(await searchquery.ToListAsync());
            

        }
        */
        #endregion
    }


}
    


