﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        public PMSWEBContext db;

        public DoctorController(PMSWEBContext db1)
        {
            db = db1;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(db.Patient);
        }


    }
}
