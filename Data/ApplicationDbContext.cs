using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshGoldPractice2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

       // public DbSet<FreshGoldPractice2.Models.UploadModel> Upload { get; set; }
        public DbSet<FreshGoldPractice2.Models.UploadOnDatabase> UploadOnDatabase { get; set; }
        public DbSet<FreshGoldPractice2.Models.UploadOnSystem> UploadOnSystem { get; set; }

        public DbSet<FreshGoldPractice2.Models.AddendumUpload> addendumUploads { get; set; }
       
        public DbSet<FreshGoldPractice2.Models.UploadOnSystem> uploadOnSystems { get; set; }
        public DbSet<FreshGoldPractice2.Models.UploadOnDatabase> uploadOnDatabases { get; set; }

    }
}
