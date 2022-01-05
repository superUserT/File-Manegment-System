using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FreshGoldPractice2.Models
{
    public class AddendumUpload
    {
        
        [Key]
        [Required]
        [StringLength(50)]
        public int Id { get; set; }

        public string RecordType { get; set; }

        public string LocCodeId { get; set; }

        [StringLength(18)]
        public string PalletId { get; set; }

        public string OrganisationId { get; set; }

        public string Country { get; set; }

        [StringLength(50)]
        public string CommodityId { get; set; }

        [StringLength(50)]
        public string VarietyId { get; set; }

        public string Pack { get; set; }

        public string Grade { get; set; }

        public string Mark { get; set; }

        public string SizeCount { get; set; }

        public string InvCode { get; set; }

        [StringLength(50)]
        public string FarmId { get; set; }

        public string TargetMarket { get; set; }

        public string CartonQuant { get; set; }

        [StringLength(50)]
        public string PalletQuant { get; set; }

        public string IntakeDate { get; set; }

        public string OriginDepot { get; set; }

        public string InspectionDate { get; set; }

        public string OrigIntakeDate { get; set; }

        public string Orchard { get; set; }

        public string ConsNo { get; set; }

        public string Weight { get; set; }

        public string TargetRegion { get; set; }

        [StringLength(100)]
        public string PackhouseCodeId { get; set; }







    }

}
