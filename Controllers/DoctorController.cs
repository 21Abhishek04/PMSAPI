using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class DoctorController : ControllerBase
    {


        private readonly IConfiguration _config;
        private readonly PMSWEBContext db;

        public DoctorController(IConfiguration config, PMSWEBContext _db)
        {
            _config = config;
            db = _db;
        }

        [HttpGet("{id}")]
        public IActionResult GetPatientById(string id)
        {
            var dep = db.Patient.Where(x => x.PatientId == id);
            return Ok(dep);
        }


        [HttpGet("DepartmentId/{id}")]
        public IActionResult GetDepartmentId(byte id)
        {
            var dep = db.Doctor.FirstOrDefault(x => x.DepartmentId == id);
            return Ok(dep);
        }

        [HttpGet("GetPatients")]
        public IActionResult GetPatients()
        {
            
            return Ok(db.Patient);
        }

        [HttpGet("GetAppointment")]
        public IActionResult GetAppointment()
        {
            
            return Ok(db.Appointment);
        }

     
        public IActionResult GetPatient(string Search)
        {
            var dep = db.Patient.Where(x => x.PatientId.StartsWith(Search) || Search == null).ToList();
            return Ok(dep);
        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] DoctorLogin doctorLogin)
        {
            var user = Authenticate(doctorLogin);

            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("User not found");
        }

        private string Generate(Doctor doctor)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, doctor.DoctorName),
                new Claim(ClaimTypes.Name, doctor.Password)

            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Doctor Authenticate(DoctorLogin doctorLogin)
        {
            var currentUser = db.Doctor.FirstOrDefault(o => o.DoctorName.ToLower() == doctorLogin.DoctorName.ToLower() && o.Password == doctorLogin.Password);

            if (currentUser != null)
            {
                return currentUser;
            }

            return null;
        }
    }

}
