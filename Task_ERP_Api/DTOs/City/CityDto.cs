using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Api.DTOs
{
    public class CityDto
    {
        public int CityID { get; set; }
        [StringLength(50)]
        public string? CityCode { get; set; }
        [StringLength(100)]
        public string? CityNameAr { get; set; }
        [StringLength(100)]
        public string? CityNameEn { get; set; }
        public int? BranchID { get; set; }
    }
} 