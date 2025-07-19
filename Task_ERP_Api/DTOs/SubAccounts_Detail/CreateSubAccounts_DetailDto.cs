using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Api.DTOs
{

    public class CreateSubAccounts_DetailDto
    {
        public int SubAccountID { get; set; }
        
        public int AccountID { get; set; }
        
        public int? BranchID { get; set; }
    }
} 