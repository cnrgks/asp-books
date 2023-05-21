using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using books.Models.Entities;
using books.Models.ViewModels;

public class YazarlarViewComponent : ViewComponent
{
    private readonly KitapDbContext db = new KitapDbContext(); // dependency injection nesnesi
    public YazarlarViewComponent(KitapDbContext _db) // Dep'i parametre olarak ekledik.
    {
        db = _db; // dependency injection yaptık. 
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var yazarlar = (from x in db.Yazarlars
                        select new YazarListVM
                        {
                            id = x.Id,
                            adi = x.Adi + " " + x.Soyadi,
                            kitapSayisi = (from k in db.Kitaplars
                                           where k.YazarId == x.Id
                                           select x).Count()

                        }).OrderByDescending(a => a.kitapSayisi).Take(10).ToList();

        return View(yazarlar);
    }

}
