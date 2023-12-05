using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Money_Transformer.Models;

public partial class Bank
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!; 

    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
}
