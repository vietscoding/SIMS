using Microsoft.AspNetCore.Mvc;
using SIMS.Data;
using SIMS.ViewModels;
using SIMS.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace SIMS.Controllers
{
    public class AdminController : Controller
    {
        private readonly DatabaseHelper _db;
        public AdminController(DatabaseHelper db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            return View();
        }

        // New endpoint to create a person via AJAX (expects JSON body)

    }
}