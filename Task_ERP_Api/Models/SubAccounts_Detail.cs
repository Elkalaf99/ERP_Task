﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Task_ERP_Api.Models;

public partial class SubAccounts_Detail
{
    [Key]
    public int SubAccountDetailID { get; set; }

    public int SubAccountID { get; set; }

    public int AccountID { get; set; }

    public int? BranchID { get; set; }

    [ForeignKey("AccountID")]
    [InverseProperty("SubAccounts_Details")]
    public virtual Account Account { get; set; }

    [ForeignKey("BranchID")]
    [InverseProperty("SubAccounts_Details")]
    public virtual Branch Branch { get; set; }

    [ForeignKey("SubAccountID")]
    [InverseProperty("SubAccounts_Details")]
    public virtual SubAccount SubAccount { get; set; }
}