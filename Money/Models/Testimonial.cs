using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Money_Transformer.Models;

public partial class Testimonial
{
    public int Id { get; set; }

    public int UserId { get; set; }
    [Required]
    public string Content { get; set; } = null!;
    [Required]
    public bool? Status { get; set; }

    public virtual User? User { get; set; }
}
