using System;
using System.Collections.Generic;

namespace Money_Transformer.Models;

public partial class Wallet
{
   

    public int Id { get; set; }
    
    public string? Iban { get; set; } 

    public int UserId { get; set; }

    public int BankId { get; set; }

    public decimal? Balance { get; set; }

    public bool? Active { get; set; }

    public virtual Bank? Bank { get; set; } 

    public virtual ICollection<Transaction> TransactionReceiverIbanNavigations { get; set; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionSenderIbanNavigations { get; set; } = new List<Transaction>();

    public virtual User? User { get; set; }
}
