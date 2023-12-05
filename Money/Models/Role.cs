using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Money_Transformer.Models;

public partial class Role
{
    public int Id { get; set; }
    [Required]
    public string Rolename { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
