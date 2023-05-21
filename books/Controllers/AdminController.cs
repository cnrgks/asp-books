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
    public IActionResult TurEkle()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TurEkle(TurlerVM gelenData)
    {
        if (!ModelState.IsValid)
        {
            return View(gelenData);
        }

        Turler yeniTur = new Turler
        {
            TurAdi = gelenData.TurAdi,
            Sira = gelenData.Sira
        };

        await db.AddAsync(yeniTur);
        await db.SaveChangesAsync();

        return Redirect("/Admin/Turler");
    }

    [HttpGet]
    public IActionResult TurDuzenle(int id)
    {
        ViewBag.TurInfo = (from x in db.Turlers
                           where x.Id == id
                           select new TurlerVM
                           {
                               Id = x.Id,
                               TurAdi = x.TurAdi,
                               Sira = x.Sira
                           }).FirstOrDefault();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TurDuzenle(TurlerVM gelenData)
    {
        var tur = await db.Turlers.FirstOrDefaultAsync(x => x.Id == gelenData.Id);

        if (!ModelState.IsValid)
        {
            return View(gelenData);
        }

        tur.Id = gelenData.Id;
        tur.TurAdi = gelenData.TurAdi;
        tur.Sira = gelenData.Sira;

        db.Turlers.Update(tur);
        await db.SaveChangesAsync();

        return Redirect("/Admin/Turler");
    }

    [HttpGet]
    public IActionResult TurSil(int? id)
    {
        ViewBag.TurInfo = (from x in db.Turlers
                           where x.Id == id
                           select new TurlerVM
                           {
                               Id = x.Id,
                               TurAdi = x.TurAdi,
                               Sira = x.Sira
                           }).FirstOrDefault();

        if (id == null)
        {
            return NotFound();
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TurSilOnay(int id)
    {
        var tur = await db.Turlers.FindAsync(id);

        db.Turlers.Remove(tur);
        await db.SaveChangesAsync();

        return RedirectToAction("Turler");
    }

    // DİLLER

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
    public IActionResult DilEkle()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DilEkle(DillerVM gelenData)
    {
        if (!ModelState.IsValid)
        {
            return View(gelenData);
        }

        Diller yeniDil = new Diller
        {
            DilAdi = gelenData.DilAdi
        };

        await db.AddAsync(yeniDil);
        await db.SaveChangesAsync();

        return Redirect("/Admin/Diller");
    }

    [HttpGet]
    public IActionResult DilDuzenle(int id)
    {
        ViewBag.DilInfo = (from x in db.Dillers
                           where x.Id == id
                           select new DillerVM
                           {
                               Id = x.Id,
                               DilAdi = x.DilAdi,
                           }).FirstOrDefault();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DilDuzenle(DillerVM gelendata)
    {
        var dil = await db.Dillers.FirstOrDefaultAsync(x => x.Id == gelendata.Id);

        if (!ModelState.IsValid)
        {
            return View(gelendata);
        }

        dil.Id = gelendata.Id;
        dil.DilAdi = gelendata.DilAdi;

        db.Dillers.Update(dil);
        await db.SaveChangesAsync();

        return Redirect("/Admin/Diller");
    }

    [HttpGet]
    public IActionResult DilSil(int? id)
    {
        ViewBag.DilInfo = (from x in db.Dillers
                           where x.Id == id
                           select new DillerVM
                           {
                               Id = x.Id,
                               DilAdi = x.DilAdi,
                           }).FirstOrDefault();

        if (id == null)
        {
            return NotFound();
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DilSilOnay(int id)
    {
        var dil = await db.Dillers.FindAsync(id);

        db.Dillers.Remove(dil);
        await db.SaveChangesAsync();

        return RedirectToAction("Diller");
    }

    // USERS

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
    public IActionResult UserEkle()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UserEkle(UsersVM gelenData)
    {
        if (!ModelState.IsValid)
        {
            return View(gelenData);
        }

        User yeniUser = new User
        {
            Id = gelenData.Id,
            Username = gelenData.Username,
            Password = gelenData.Password
        };

        await db.AddAsync(yeniUser);
        await db.SaveChangesAsync();

        return Redirect("/Admin/Users");
    }

    [HttpGet]
    public IActionResult UserDuzenle(int id)
    {
        ViewBag.UserInfo = (from x in db.Users
                            where x.Id == id
                            select new UsersVM
                            {
                                Id = x.Id,
                                Username = x.Username,
                                Password = x.Password
                            }).FirstOrDefault();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UserDuzenle(UsersVM gelendata)
    {
        var user = await db.Users.FirstOrDefaultAsync(x => x.Id == gelendata.Id);

        if (!ModelState.IsValid)
        {
            return View(gelendata);
        }

        user.Id = gelendata.Id;
        user.Username = gelendata.Username;
        user.Password = gelendata.Password;

        db.Users.Update(user);
        await db.SaveChangesAsync();

        return Redirect("/Admin/Users");
    }

    [HttpGet]
    public IActionResult UserSil(int? id)
    {
        ViewBag.UserInfo = (from x in db.Users
                            where x.Id == id
                            select new UsersVM
                            {
                                Id = x.Id,
                                Username = x.Username,
                                Password = x.Password
                            }).FirstOrDefault();

        if (id == null)
        {
            return NotFound();
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UserSilOnay(int id)
    {
        var user = await db.Users.FindAsync(id);

        db.Users.Remove(user);
        await db.SaveChangesAsync();

        return RedirectToAction("Users");
    }

}