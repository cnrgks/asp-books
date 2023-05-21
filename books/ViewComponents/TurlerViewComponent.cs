using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using books.Models.Entities;
using books.Models.ViewModels;

public class TurlerViewComponent : ViewComponent
{
    private readonly KitapDbContext db = new KitapDbContext(); // dependency injection nesnesi
    public TurlerViewComponent(KitapDbContext _db) // Dep'i parametre olarak ekledik.
    {
        db = _db; // dependency injection yaptık. 
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var turler = (from x in db.Turlers
                      select new TurListVM
                      {
                          id = x.Id,
                          tur = x.TurAdi,
                          kitapSayisi = (from k in db.Turlertokitaplars
                                         where k.TurId == x.Id
                                         select x).Count()

                      }).ToList();

        return View(turler);
    }

}
