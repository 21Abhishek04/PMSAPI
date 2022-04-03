using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PMSAPI.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PMSAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class PatientController : ControllerBase
    {
        
        
            private IConfiguration _config;
            public PMSWEBContext db;

            public PatientController(IConfiguration config, PMSWEBContext _db)
            {
                _config = config;
                db = _db;
            }



       //[HttpGet]
       // public IActionResult Get()
       // {
       //     return Ok(db.Patient);
       // }

        [HttpPost("AddPatient")]

        public IActionResult AddPatient(Patient patient)
        {
            if (patient.PatientId == null)
            {
                return NotFound();
            }
            db.Patient.Add(patient);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PatientExists(patient.PatientId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }
        private bool PatientExists(string id)
        {
            return db.Patient.Any(e => e.PatientId == id);
        }


        [HttpGet("GetById/{Id}")]
        public IActionResult GetById(string Id)
        {
            var dep = db.Patient.Where(x => x.PatientId == Id);
            return Ok(dep);
        }


        [HttpPut("BookAppointment")]

        public IActionResult PutAppointment(Appointment appointment, string Id)
        {
            var dep = db.Appointment.FirstOrDefault(x => x.DoctorId == Id);
            db.Appointment.Update(dep);
            db.SaveChanges();
            return Ok();
        }

        [HttpPost("BookAppointment")]
     
        public IActionResult BookAppointment(Appointment appointment)
        {
            db.Appointment.Add(appointment);
            db.SaveChanges();
            return Ok();
        }

        [HttpPost("BookAppointment/{Id}")]

        public IActionResult BookAppointment(string Id)
        {

            var dep = db.Doctor.Where(x => x.DoctorId == Id);
           
            
            db.Update(dep);
            db.SaveChanges();
            return Ok();
        }


     
        
        [HttpGet("GetDoctors")]
        public IActionResult GetDoctors()
        {
            var dep = db.Doctor;
            return Ok(dep);
        }



        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] PatientLogin patientLogin)
        {
            var user = Authenticate(patientLogin);

            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("User not found");
        }

        private string Generate(Patient patient)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, patient.PatientName),
                new Claim(ClaimTypes.Name, patient.Password)

            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Patient Authenticate(PatientLogin patientLogin)
        {
            var currentUser = db.Patient.FirstOrDefault(o => o.PatientName.ToLower() == patientLogin.PatientName.ToLower() && o.Password == patientLogin.Password);

            if (currentUser != null)
            {
                return currentUser;
            }

            return null;
        }
    }

 
}
