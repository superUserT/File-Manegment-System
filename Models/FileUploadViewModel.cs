using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;


namespace FreshGoldPractice2.Models
{
    public class FileUploadViewModel
    {
        //For reUpload
        public List<UploadOnSystem> UploadOnSystems { get; set; }
        public List<UploadOnDatabase> uploadOnDatabases { get; set; }
    }
}
