using System;
using System.Collections.Generic;

namespace books.Models.Entities;

public partial class Iletisim
{
    public int Id { get; set; }

    public string Isim { get; set; } = null!;

    public string Eposta { get; set; } = null!;

    public string Konu { get; set; } = null!;

    public string Mesaj { get; set; } = null!;
}
