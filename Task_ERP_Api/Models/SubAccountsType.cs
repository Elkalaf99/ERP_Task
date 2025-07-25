﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Task_ERP_Api.Models;

public partial class SubAccountsType
{
    [Key]
    public int SubAccountTypeID { get; set; }

    [Required]
    [StringLength(100)]
    public string SubAccountTypeNameAr { get; set; }

    [StringLength(100)]
    public string SubAccountTypeNameEn { get; set; }

    public int? BranchID { get; set; }

    [InverseProperty("SubAccountType")]
    public virtual ICollection<SubAccount> SubAccounts { get; set; } = new List<SubAccount>();
}