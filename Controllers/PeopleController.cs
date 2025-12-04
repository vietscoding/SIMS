using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SIMS.Data;
using SIMS.Data;
using SIMS.Models;
using SIMS.ViewModels;
using System;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public IActionResult PeopleList(int page = 1)
        {
            int pageSize = 10;
            var query = _db.GetPeople();
            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var data = query
                .OrderBy(p => p.PersonId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            return View(data);
        }

        [HttpGet]
        public IActionResult GetPeople(
            int page = 1,
            string? name = null,
            string? citizenIdNumber = null,
            bool? gender = null,
            DateTime? dateOfBirthStart = null,
            DateTime? dateOfBirthEnd = null,
            string? email = null,
            string? phoneNumber = null,
            string? address = null,
            string? nationality = null

            )
        {
            int pageSize = 10;
            var query = _db.GetPeople().AsQueryable();

            //query = query.Where(p => !p.IsDeleted);

            if (!string.IsNullOrWhiteSpace(name))
            {
                var term = name.Trim();
                query = query.Where(p => (p.FullName ?? string.Empty).Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(citizenIdNumber))
            {
                var term = citizenIdNumber.Trim();
                query = query.Where(p => (p.CitizenIdNumber ?? string.Empty).Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                var term = email.Trim();
                query = query.Where(p => (p.Email ?? string.Empty).Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                var term = phoneNumber.Trim();
                query = query.Where(p => (p.PhoneNumber ?? string.Empty).Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(nationality))
            {
                var term = nationality.Trim();
                query = query.Where(p => (p.Nationality ?? string.Empty).Contains(term));
            }

            if (gender.HasValue)
            {
                query = query.Where(p => p.Gender == gender.Value);
            }

            if (dateOfBirthStart.HasValue)
            {
                query = query.Where(p => p.DateOfBirth >= dateOfBirthStart.Value);
            }

            if (dateOfBirthEnd.HasValue)
            {

                query = query.Where(p => p.DateOfBirth <= dateOfBirthEnd.Value);
            }

            if (!address.IsNullOrEmpty())
            {
                query = query.Where(p => (p.Address ?? "").Contains(address.Trim()));
            }


            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var data = query
                .OrderBy(p => p.PersonId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Json(new
            {
                people = data.Select(p => new
                {
                    personId = p.PersonId,
                    fullName = p.FullName,
                    citizenIdNumber = p.CitizenIdNumber,
                    gender = p.Gender,
                    dateOfBirth = p.DateOfBirth,
                    email = p.Email,
                    phoneNumber = p.PhoneNumber,
                    address = p.Address,
                    nationality = p.Nationality
                }),
                currentPage = page,
                totalPages = totalPages,
                totalItems = totalItems,
            });
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
            model.CreatedAt = DateTime.Now;
            model.UpdatedAt = DateTime.Now;

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

        [HttpGet]
        public IActionResult GetPersonDetails(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid Person ID." });
            }

            var person = _db.GetPeople()
                .AsNoTracking()
                .FirstOrDefault(p => p.PersonId == id);

            if (person == null || person.PersonId == 0)
            {
                return NotFound(new { message = "Person not found." });
            }
            return Json(new
            {
                personId = person.PersonId,
                fullName = person.FullName,
                citizenIdNumber = person.CitizenIdNumber,
                gender = person.Gender,
                dateOfBirth = person.DateOfBirth,
                email = person.Email,
                phoneNumber = person.PhoneNumber,
                address = person.Address,
                nationality = person.Nationality

            });
        }

        [HttpPost]
        public IActionResult UpdatePerson([FromBody] Person person)
        {
            if (person == null || person.PersonId <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid person data." });
            }
            var existingPerson = _db.GetPersonById(person.PersonId);
            if (existingPerson == null)
            {
                return NotFound(new { success = false, message = "Person not found." });
            }

            existingPerson.FullName = person.FullName?.Trim();
            existingPerson.CitizenIdNumber = person.CitizenIdNumber?.Trim();
            existingPerson.Gender = person.Gender;
            existingPerson.DateOfBirth = person.DateOfBirth;
            existingPerson.Email = person.Email;
            existingPerson.PhoneNumber = person.PhoneNumber;
            existingPerson.Address = person.Address;
            existingPerson.Nationality = person.Nationality;

            var updated = _db.UpdatePerson(existingPerson);
            if (!updated)
            {
                return StatusCode(500, new { message = "Failed to update person. Please try again." });
            }
            else
            {
                return Json(new { success = true, message = "Person updated successfully." });
            }
        }

    }
}
