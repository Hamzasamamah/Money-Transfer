using System;
using System.Collections.Generic;

namespace Money_Transformer.Models;

public partial class VisaCard
{
    public int Id { get; set; }

    public string CardNum { get; set; } = null!;

    public DateTime ExDate { get; set; }

    public string HolderName { get; set; } = null!;

    public string Cvv { get; set; } = null!;

    public decimal Balance { get; set; }
}
