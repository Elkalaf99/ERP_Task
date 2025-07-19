using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Api.DTOs
{

    public class UpdateCityDto
    {
        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z0-9\-_]+$", ErrorMessage = "City code can only contain letters, numbers, hyphens, and underscores.")]
        public string? CityCode { get; set; }
        
        [StringLength(100)]
        public string? CityNameAr { get; set; }
        
        [StringLength(100)]
        public string? CityNameEn { get; set; }
        
        public int? BranchID { get; set; }
    }
} 