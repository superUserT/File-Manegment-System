using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace FreshGoldPractice2.Models
{
    public class FileOnFileSystem : ReUploadModel
    {
        //for reupload
        public string FilePath { get; set; }
    }
}
