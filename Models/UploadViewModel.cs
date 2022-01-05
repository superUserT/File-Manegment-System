using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshGoldPractice2.Models
{
    public class UploadViewModel
    {
        public List<UploadOnSystem> UploadsOnSystem { get; set; }
        public List<UploadOnDatabase> UploadsOnDatabase { get; set; }

        public List<AddendumUpload> addendumUploads { get; set; }
    }
}
