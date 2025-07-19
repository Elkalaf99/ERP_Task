namespace Task_ERP_Bar.Models
{
    public class CreateSubAccountRequest
    {
        public string SubAccountNumber { get; set; } = string.Empty;
        public string SubAccountNameAr { get; set; } = string.Empty;
        public string SubAccountNameEn { get; set; } = string.Empty;
        public int? BranchID { get; set; }
        public int? LevelID { get; set; }
        public int? ParentID { get; set; }
        public int? SubAccountTypeID { get; set; }
        public bool IsActive { get; set; } = true;
    }
} 