﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Task_ERP_Api.Models;

public partial class SubAccounts_Level
{
    [Key]
    public int LevelID { get; set; }

    public int Width { get; set; }

    public int? BranchID { get; set; }

    [ForeignKey("BranchID")]
    [InverseProperty("SubAccounts_Levels")]
    public virtual Branch Branch { get; set; }

    [InverseProperty("Level")]
    public virtual ICollection<SubAccount> SubAccounts { get; set; } = new List<SubAccount>();
}