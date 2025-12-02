using Microsoft.AspNetCore.Mvc;
using SIMS.Data;
using Microsoft.AspNetCore.Mvc;
using SIMS.Data;
using SIMS.ViewModels;
using SIMS.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace SIMS.Controllers
{
    public class PeopleController : Controller
    {

        private readonly DatabaseHelper _db;
        public PeopleController(DatabaseHelper db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddPerson([FromBody] SIMS.Models.Person model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Invalid request payload." });
            }

            // Normalize strings
            model.FullName = model.FullName?.Trim();
            model.CitizenIdNumber = model.CitizenIdNumber?.Trim();
            model.Email = model.Email?.Trim();
            model.PhoneNumber = model.PhoneNumber?.Trim();
            model.Address = model.Address?.Trim();
            model.Nationality = model.Nationality?.Trim();

            var validator = new SIMS.Models.Person();

            // Basic server-side validation
            if (string.IsNullOrWhiteSpace(model.FullName) || !validator.IsValidFullName(model.FullName))
            {
                return BadRequest(new { message = "Full name is required and must contain valid characters." });
            }

            if (!validator.IsValidCitizenIdNumber(model.CitizenIdNumber))
            {
                return BadRequest(new { message = "Citizen ID must be exactly 12 digits." });
            }

            if (!validator.IsValidGender(model.Gender))
            {
                return BadRequest(new { message = "Gender is required." });
            }

            if (!validator.IsValidDateOfBirth(model.DateOfBirth))
            {
                return BadRequest(new { message = "Invalid date of birth." });
            }

            if (!string.IsNullOrWhiteSpace(model.PhoneNumber) && !validator.IsValidPhoneNumber(model.PhoneNumber))
            {
                return BadRequest(new { message = "Invalid phone number format. Expected 0 followed by 9 digits." });
            }

            // Duplicate check by CitizenIdNumber
            var exists = _db.GetAllPeople().Any(p => (p.CitizenIdNumber ?? string.Empty) == (model.CitizenIdNumber ?? string.Empty));
            if (exists)
            {
                return Conflict(new { message = $"A person with Citizen ID '{model.CitizenIdNumber}' already exists." });
            }

            // Timestamps and defaults
            model.Created = DateTime.Now;
            model.Updated = DateTime.Now;

            var added = _db.AddPerson(model);
            if (!added)
            {
                return StatusCode(500, new { message = "Failed to add person. Please try again." });
            }

            return Ok(new
            {
                message = "Person added successfully.",
                person = new
                {
                    model.PersonId,
                    model.FullName,
                    model.CitizenIdNumber,
                    model.Gender,
                    DateOfBirth = model.DateOfBirth,
                    model.Email,
                    model.PhoneNumber,
                    model.Address,
                    model.Nationality
                }
            });
        }
    }
}
