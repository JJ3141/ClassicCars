using System;
using ClassicCars.Data;
using ClassicCars.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ProfileController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProfileController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("Login", "Login");

        var user = _context.Users
            .Include(u => u.Cars)
            .FirstOrDefault(u => u.Id == userId);

        if (user == null) return NotFound();

        return View(user);
    }
    [HttpPost]
    public IActionResult Create(Car car)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        //Console.WriteLine($"PROFILE PAGE UserId = {userId}");
        //Console.WriteLine($"SESSION UserId = {userId}");
        //Console.WriteLine($"BEFORE SET car.UserId = {car.UserId}");
        if (userId == null) return RedirectToAction("Login", "Login");


        if (ModelState.IsValid)
        {
            car.UserId = userId.Value;
            //Console.WriteLine($"AFTER SET car.UserId = {car.UserId}");
            _context.Cars.Add(car);
            //Console.WriteLine($"UserId = {car.UserId}");
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction("Index", "Profile");



    }
}
