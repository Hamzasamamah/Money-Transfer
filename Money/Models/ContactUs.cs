using System;
using System.Collections.Generic;

namespace Money_Transformer.Models;

public partial class ContactUs
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string PhoneNum { get; set; } = null!;

    public string Message { get; set; } = null!;
}
