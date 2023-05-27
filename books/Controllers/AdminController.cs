using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using books.Models;
using books.Models.Entities;
using books.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using books.Models.AdminViewModels;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace books.Controllers.Admin;

[Authorize]
public class AdminController : Controller
{
    private readonly KitapDbContext db = new KitapDbContext(); // dependency injection nesnesi
    public AdminController(KitapDbContext _db) // Dep'i parametre olarak ekledik.
    {
        db = _db; // dependency injection yaptık. 
    }

    public IActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(UsersVM postedData)
    {
        if (!ModelState.IsValid)
        {
            return View(postedData);
        }

        var user = (from x in db.Users
                    where x.Username == postedData.Username && x.Password == postedData.Password
                    select x
                    ).FirstOrDefault();

        if (user != null)
        {
            var claims = new List<Claim>{
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("user",user.Id.ToString()),
                new Claim("role","admin")
            };

            var claimsIdendity = new ClaimsIdentity(claims, "Cookies", "user", "role");
            var claimsPrinciple = new ClaimsPrincipal(claimsIdendity);

            await HttpContext.SignInAsync(claimsPrinciple);

            return Redirect("/Admin/Index");
        }

        return View();
    }

    [Route("/Admin/Logout")]
    public async Task<IActionResult> Signout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/Admin");
    }

    // TÜRLER

    [Route("/Admin/Turler")]
    public IActionResult Turler()
    {
        List<TurlerVM> TurListesi = (from x in db.Turlers
                                     select new TurlerVM
                                     {
                                         Id = x.Id,
                                         Sira = x.Sira,
                                         TurAdi = x.TurAdi
                                     }).ToList();

        db.Dispose();
        return View(TurListesi);
    }

    [HttpGet]
    public IActionResult TurForm(int? id)
    {
        if (id != null)
        {
            TurlerVM duzenlenecekTur = (from x in db.Turlers
                                        where x.Id == id
                                        select new TurlerVM
                                        {
                                            Id = x.Id,
                                            Sira = x.Sira,
                                            TurAdi = x.TurAdi
                                        }).FirstOrDefault();

            ViewBag.PageTitle = "Tür Düzenle";
            ViewBag.ButtonRenk = "btn-primary";
            ViewBag.ButtonText = "Kaydet";
            return View(duzenlenecekTur);
        }
        else if (id == null)
        {
            ViewBag.PageTitle = "Tür Ekle";
            ViewBag.ButtonRenk = "btn-success";
            ViewBag.ButtonText = "Ekle";
            return View();
        }
        else
        {
            return View();
        }

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TurForm(TurlerVM gelenData)
    {
        if (!ModelState.IsValid)
        {
            return View(gelenData);
        }

        if (gelenData.Id != 0)
        {
            Turler duzenelenecekTur = db.Turlers.Find(gelenData.Id);
            if (duzenelenecekTur != null)
            {
                duzenelenecekTur.Sira = gelenData.Sira;
                duzenelenecekTur.TurAdi = gelenData.TurAdi;
            }
        }
        else if (gelenData.Id == 0)
        {
            Turler yeniTur = new Turler
            {
                TurAdi = gelenData.TurAdi,
                Sira = gelenData.Sira
            };
            await db.AddAsync(yeniTur);
        }
        await db.SaveChangesAsync();

        return Redirect("/Admin/Turler");
    }


    public async Task<IActionResult> TurSil(int id)
    {
        Turler silinecekTur = db.Turlers.Find(id);
        if (silinecekTur != null)
        {
            db.Turlers.Remove(silinecekTur);
            await db.SaveChangesAsync();
        }

        return Redirect("/Admin/Turler");
    }

    [Route("/Admin/Diller")]
    public IActionResult Diller()
    {
        List<DillerVM> DilListesi = (from x in db.Dillers
                                     select new DillerVM
                                     {
                                         Id = x.Id,
                                         DilAdi = x.DilAdi
                                     }).ToList();

        db.Dispose();
        return View(DilListesi);
    }

    [HttpGet]
    public IActionResult DilForm(int? id)
    {
        if (id != null)
        {
            DillerVM duzenlenecekDil = (from x in db.Dillers
                                        where x.Id == id
                                        select new DillerVM
                                        {
                                            Id = x.Id,
                                            DilAdi = x.DilAdi
                                        }).FirstOrDefault();

            ViewBag.PageTitle = "Dil Düzenle";
            ViewBag.ButtonRenk = "btn-primary";
            ViewBag.ButtonText = "Kaydet";
            return View(duzenlenecekDil);
        }
        else if (id == null)
        {
            ViewBag.PageTitle = "Dil Ekle";
            ViewBag.ButtonRenk = "btn-success";
            ViewBag.ButtonText = "Ekle";
            return View();
        }
        else
        {
            return View();
        }

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DilForm(DillerVM gelenData)
    {
        if (!ModelState.IsValid)
        {
            return View(gelenData);
        }

        if (gelenData.Id != 0)
        {
            Diller duzenelenecekDil = db.Dillers.Find(gelenData.Id);
            if (duzenelenecekDil != null)
            {
                duzenelenecekDil.DilAdi = gelenData.DilAdi;
            }
        }
        else if (gelenData.Id == 0)
        {
            Diller yeniDil = new Diller
            {
                DilAdi = gelenData.DilAdi,
            };
            await db.AddAsync(yeniDil);
        }
        await db.SaveChangesAsync();

        return Redirect("/Admin/Diller");
    }

    public async Task<IActionResult> DilSil(int id)
    {
        Diller silinecekDil = db.Dillers.Find(id);
        if (silinecekDil != null)
        {
            db.Dillers.Remove(silinecekDil);
            await db.SaveChangesAsync();
        }

        return Redirect("/Admin/Diller");
    }

    [Route("/Admin/Users")]
    public IActionResult Users()
    {
        List<UsersVM> UserListesi = (from x in db.Users
                                     select new UsersVM
                                     {
                                         Id = x.Id,
                                         Username = x.Username,
                                         Password = x.Password
                                     }).ToList();

        db.Dispose();
        return View(UserListesi);
    }

    [HttpGet]
    public IActionResult UserForm(int? id)
    {
        if (id != null)
        {
            UsersVM duzenlenecekUser = (from x in db.Users
                                        where x.Id == id
                                        select new UsersVM
                                        {
                                            Id = x.Id,
                                            Username = x.Username,
                                            Password = x.Password
                                        }).FirstOrDefault();

            ViewBag.PageTitle = "Kullanıcı Düzenle";
            ViewBag.ButtonRenk = "btn-primary";
            ViewBag.ButtonText = "Kaydet";
            return View(duzenlenecekUser);
        }
        else if (id == null)
        {
            ViewBag.PageTitle = "Kullanıcı Ekle";
            ViewBag.ButtonRenk = "btn-success";
            ViewBag.ButtonText = "Ekle";
            return View();
        }
        else
        {
            return View();
        }

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UserForm(UsersVM gelenData)
    {
        if (!ModelState.IsValid)
        {
            return View(gelenData);
        }

        if (gelenData.Id != 0)
        {
            User duzenlenecekUser = db.Users.Find(gelenData.Id);
            if (duzenlenecekUser != null)
            {
                duzenlenecekUser.Username = gelenData.Username;
                duzenlenecekUser.Password = gelenData.Password;
            }
        }
        else if (gelenData.Id == 0)
        {
            User yeniUser = new User
            {
                Username = gelenData.Username,
                Password = gelenData.Password
            };
            await db.AddAsync(yeniUser);
        }
        await db.SaveChangesAsync();

        return Redirect("/Admin/Users");
    }

    public async Task<IActionResult> UserSil(int id)
    {
        User silinecekUser = db.Users.Find(id);
        if (silinecekUser != null)
        {
            db.Users.Remove(silinecekUser);
            await db.SaveChangesAsync();
        }

        return Redirect("/Admin/Users");
    }

    [Route("/Admin/Yazarlar")]
    public IActionResult Yazarlar()
    {
        List<YazarlarVM> YazarListesi = (from x in db.Yazarlars
                                         select new YazarlarVM
                                         {
                                             Id = x.Id,
                                             Adi = x.Adi,
                                             Soyadi = x.Soyadi,
                                             DogumTarihi = x.DogumTarihi,
                                             DogumYeri = x.DogumYeri,
                                             Cinsiyeti = x.Cinsiyeti
                                         }).ToList();

        db.Dispose();
        return View(YazarListesi);
    }

    [HttpGet]
    public IActionResult YazarForm(int? id)
    {
        if (id != null)
        {
            YazarlarVM duzenlenecekYazar = (from x in db.Yazarlars
                                            where x.Id == id
                                            select new YazarlarVM
                                            {
                                                Id = x.Id,
                                                Adi = x.Adi,
                                                Soyadi = x.Soyadi,
                                                DogumTarihi = x.DogumTarihi,
                                                DogumYeri = x.DogumYeri,
                                                Cinsiyeti = x.Cinsiyeti
                                            }).FirstOrDefault();

            ViewBag.PageTitle = "Yazar Düzenle";
            ViewBag.ButtonRenk = "btn-primary";
            ViewBag.ButtonText = "Kaydet";
            return View(duzenlenecekYazar);
        }
        else if (id == null)
        {
            ViewBag.PageTitle = "Yazar Ekle";
            ViewBag.ButtonRenk = "btn-success";
            ViewBag.ButtonText = "Ekle";
            return View();
        }
        else
        {
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> YazarForm(YazarlarVM gelenData)
    {
        if (!ModelState.IsValid)
        {
            return View(gelenData);
        }

        if (gelenData.Id != 0)
        {
            Yazarlar duzenlenecekYazar = db.Yazarlars.Find(gelenData.Id);
            if (duzenlenecekYazar != null)
            {
                duzenlenecekYazar.Adi = gelenData.Adi;
                duzenlenecekYazar.Soyadi = gelenData.Soyadi;
                duzenlenecekYazar.DogumTarihi = gelenData.DogumTarihi;
                duzenlenecekYazar.DogumYeri = gelenData.DogumYeri;
                duzenlenecekYazar.Cinsiyeti = gelenData.Cinsiyeti;
            }
        }
        else if (gelenData.Id == 0)
        {
            Yazarlar yeniYazar = new Yazarlar
            {
                Adi = gelenData.Adi,
                Soyadi = gelenData.Soyadi,
                DogumTarihi = gelenData.DogumTarihi,
                DogumYeri = gelenData.DogumYeri,
                Cinsiyeti = gelenData.Cinsiyeti
            };
            await db.AddAsync(yeniYazar);
        }
        await db.SaveChangesAsync();

        return Redirect("/Admin/Yazarlar");
    }

    public async Task<IActionResult> YazarSil(int id)
    {
        Yazarlar silinecekYazar = db.Yazarlars.Find(id);
        if (silinecekYazar != null)
        {
            db.Yazarlars.Remove(silinecekYazar);
            await db.SaveChangesAsync();
        }

        return Redirect("/Admin/Yazarlar");
    }
}