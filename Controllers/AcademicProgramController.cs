using Microsoft.AspNetCore.Mvc;
using SIMS.Data;
using SIMS.ViewModels;
using SIMS.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace SIMS.Controllers
{
    public class AcademicProgramController : Controller
    {
        private readonly DatabaseHelper _db;
        public AcademicProgramController(DatabaseHelper db)
        {
            _db = db;
        }
        public IActionResult AcademicProgram()
        {
            var academicProgramList = new AcademicProgramViewModel
            {
                AcademicPrograms = _db.GetAllAcademicPrograms()
            };
            return View("Views/Admin/AcademicProgram.cshtml", academicProgramList);
        }
    }
}
