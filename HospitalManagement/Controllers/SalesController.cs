﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Controllers
{
    public class SalesController : Controller
    {
        public IActionResult Invoice()
        {
            return View();
        }
    }
}
