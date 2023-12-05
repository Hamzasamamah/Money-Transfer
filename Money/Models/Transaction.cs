using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Money_Transformer.Models;

public partial class Transaction
{
    public int Id { get; set; }

    public string SenderIban { get; set; } = null!;

    public string ReceiverIban { get; set; } = null!;

    public decimal Amount { get; set; }

    public decimal Fees { get; set; }

    public DateTime Date { get; set; }
    

    public virtual Wallet? ReceiverIbanNavigation { get; set; }
    
    public virtual Wallet? SenderIbanNavigation { get; set; } 
}
