using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Api.DTOs
{
    public class SubAccounts_LevelDto
    {
        public int LevelID { get; set; }
        public int Width { get; set; }
        public int? BranchID { get; set; }
    }
} 