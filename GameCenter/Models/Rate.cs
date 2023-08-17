﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCenter.Models;
[Table("Rates")]
public class Rate
{
    public Guid Id { get; set; }
    public int GameRate { get; set; }
    public Game Game { get; set; }
    public Guid GId { get; set; }
    public IdentityUser User { get; set; }
    public Guid UId { get; set; }

}
