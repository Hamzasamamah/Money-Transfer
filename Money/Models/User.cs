using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Money_Transformer.Models;

public partial class User
{
    public int Id { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Username { get; set; } 
                 
    public string? Password { get; set; } 
                 
    public string? Email { get; set; } 

    public int RoleId { get; set; }

    public string? ImagePath { get; set; } 

    [NotMapped]
    public virtual IFormFile? ImageFile { get; set; }

    public virtual Role? Role { get; set; } 

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();

    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
}