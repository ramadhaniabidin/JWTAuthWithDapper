﻿using JWTAuthWithDapper.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthWithDapper.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class EmployeeController: Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var result = _employeeRepository.GetAll();
            return Ok(result);
        }
    }
}
