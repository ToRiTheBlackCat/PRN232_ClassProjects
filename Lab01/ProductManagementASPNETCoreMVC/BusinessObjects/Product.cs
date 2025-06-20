﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects;

public partial class Product
{
    [Required]
    public int ProductId { get; set; }
    [Required]
    public string ProductName { get; set; }
    [Required]
    public int CategoryId { get; set; }
    [Required]
    public short? UnitsInStock { get; set; }
    [Required]
    public decimal? UnitPrice { get; set; }

    public virtual Category Category { get; set; }
}