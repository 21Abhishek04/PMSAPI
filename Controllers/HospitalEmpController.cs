using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PMSAPI.Models;

namespace PMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class HospitalEmpController : ControllerBase
    {

        private IConfiguration _config;
        private readonly PMSWEBContext db;

        public HospitalEmpController(IConfiguration config, PMSWEBContext _db)
        {
            _config = config;
            db = _db;
        }




    // GET: api/HospitalEmp
    [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctor()
        {
            return await db.Doctor.ToListAsync();
        }

        // GET: api/HospitalEmp/5
        [HttpGet("GetDoctorById/{id}")]
        public async Task<ActionResult<Doctor>> GetDoctor(string id)
        {
            var doctor = await db.Doctor.FindAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return doctor;
        }

        // PUT: api/HospitalEmp/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("PutDoctor/{id}")]
        public async Task<IActionResult> PutDoctors(string id, Doctor doctor)
        {
            try
            {
                if (id != doctor.DoctorId)
                {
                    return BadRequest();
                }
                else
                {
                    db.Entry(doctor).State = EntityState.Modified;
                   var res= await db.SaveChangesAsync();
                    return Ok(res);
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
            
        }

 

        // DELETE: api/HospitalEmp/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Doctor>> DeleteDoctor(string id)
        {
            var doctor = await db.Doctor.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            db.Doctor.Remove(doctor);
            await db.SaveChangesAsync();

            return doctor;
        }

        // POST: api/HospitalEmp
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Doctor>> PostDoctor(Doctor doctor)
        {
            db.Doctor.Add(doctor);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DoctorExists(doctor.DoctorId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDoctor", new { id = doctor.DoctorId }, doctor);
        }

        private bool DoctorExists(string id)
        {
            return db.Doctor.Any(e => e.DoctorId == id);
        }


        [HttpGet("GetAppointments")]
        public IActionResult GetAppointments()
        {
            return Ok(db.Appointment);
        }


        [HttpGet("GetPatient")]
        public IActionResult GetPatients()
        {
            return Ok(db.Patient);
        }

        [HttpPost("AddPatient")]
        public IActionResult AddPatient(Patient patient)
        {
            try
            {

                db.Patient.Add(patient);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }

        }


        [HttpGet("PatientById/{Id}")]
        public IActionResult GetPatientById(string Id)
        {
            var dep = db.Patient.FirstOrDefault(x => x.PatientId == Id);


            return Ok(dep);
        }

        [HttpPut("PutPatient/{id}")]
        public async Task<IActionResult> PutPatient(string id, Patient patient)
        {
            try
            {
                if (id != patient.PatientId)
                {
                    return BadRequest();
                }
                else
                {
                    db.Entry(patient).State = EntityState.Modified;
                    var res = await db.SaveChangesAsync();
                    return Ok(res);
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }

        }

        private bool PatientExists(string id)
        {
            return db.Patient.Any(e => e.PatientId == id); 
        }

        [HttpGet("SearchPatient")]
        public IActionResult GetPatient(string Search)
        {

            var dep = db.Patient.Where(x => x.PatientId.StartsWith(Search) || Search == null).ToList();
            return Ok(dep);
        }



        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] AdminLogin adminLogin)
        {
            var user = Authenticate(adminLogin);

            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("User not found");
        }

        private string Generate(HospitalEmployee hospitalEmployee)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, hospitalEmployee.HemployeeName),
                new Claim(ClaimTypes.Name, hospitalEmployee.Password)

            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private HospitalEmployee Authenticate(AdminLogin adminLogin)
        {
            var currentUser = db.HospitalEmployee.FirstOrDefault(o => o.HemployeeName.ToLower() == adminLogin.HemployeeName.ToLower() && o.Password == adminLogin.Password);

            if (currentUser != null)
            {
                return currentUser;
            }

            return null;
        }

    }

}
