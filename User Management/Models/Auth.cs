﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace User_Management.Models;

[Table("auth")]
public partial class Auth
{
    [Key]
    [Column("userId")]
    [MaxLength(16)]
    public byte[] UserId { get; set; }

    [Column("password")]
    [StringLength(45)]
    public string Password { get; set; }

    [Required]
    [Column("salt")]
    public byte[] Salt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Auth")]
    public virtual User User { get; set; }
}