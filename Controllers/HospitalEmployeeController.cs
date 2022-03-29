using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalEmployeeController : ControllerBase
    {

        public PMSWEBContext db;

        public HospitalEmployeeController(PMSWEBContext db1)
        {
            db = db1;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(db.Doctor);
        }

      

        [HttpGet("{Id}")]
        public IActionResult GetById(string Id)
        {
            var dep = db.Doctor.FirstOrDefault(x => x.DoctorId == Id);


            return Ok(dep);
        }

        [HttpPost]
        public IActionResult AddDoctor(Doctor doctor)
        {
            try
            {

                db.Doctor.Add(doctor);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }

        }



        

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var dept = db.Doctor.Where(x => x.DoctorId == id).Single<Doctor>();
            db.Doctor.Remove(dept);
            db.SaveChanges();
            return Ok();
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


    }

}

