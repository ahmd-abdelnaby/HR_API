using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HrAPI.Models;
using HrAPI.DTO;
using HrAPI.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace HrAPI.Controllers
{
    [Authorize(AuthenticationSchemes =
    JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        //private readonly EmployeeCore employeeCore;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        //private readonly string email;
        //private readonly Employee empCurrentUser;

        public EmployeesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
            //System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            //email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            //empCurrentUser = _context.Employees.Where(e => e.Email == email).FirstOrDefault();
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);


        // GET: api/Employees
        [HttpGet]
        public IEnumerable<EmployeeDTO> GetEmployees()
        {

            var emps = _context.Employees.Select(e => new EmployeeDTO
            {
                ID=e.ID,
                Name = e.Name,
                Profession = e.Profession.Name,
                GraduatioYear = e.GraduatioYear,
                Address = e.Address,
                Code = e.Code,
                DateOfBirth = e.DateOfBirth,
                Email = e.Email,
                gender = e.gender,
                HiringDateHiringDate = e.HiringDateHiringDate,
                MaritalStatus = e.MaritalStatus,
                Phone = e.Phone,
                RelevantPhone = e.RelevantPhone,
                Photo=e.photo
            }).ToList();
            return emps;
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public EmployeeDTO GetEmployee(int id)
        {
            //var ff =  _context.Employees.Include(e=>e.Profession).FirstOrDefault(e=>e.ID==id).Profession;
            var e =  _context.Employees.Include(e=>e.Profession).FirstOrDefault(e=>e.ID == id);
            var emp = new EmployeeDTO
            {
                ID = e.ID,
                Name = e.Name,
                Profession = e.Profession.Name,
                GraduatioYear = e.GraduatioYear,
                Address = e.Address,
                Code = e.Code,
                DateOfBirth = e.DateOfBirth,
                Email = e.Email,
                gender = e.gender,
                HiringDateHiringDate = e.HiringDateHiringDate,
                MaritalStatus = e.MaritalStatus,
                Phone = e.Phone,
                RelevantPhone = e.RelevantPhone,
                Photo = e.photo
            };
            if (emp == null)
            {
                return null;
            }

            return emp;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.ID)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.ID }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return employee;
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.ID == id);
        }
        [HttpPost]
        [Route("api/dashboard/UploadImage")]
        public ActionResult UploadFile(IFormFile file)
        {
            var ImagesTypes = new List<string>() { "image/jpg", "image/jpeg", "image/png" };
            var FileTypes = new List<string>() { "application/pdf", "application/doc", "application/docs" };
            //var user = GetCurrentUserAsync();
            //var emp = _context.Employees.Where(e => e.Email == user.Result.Email).FirstOrDefault();
            string path;
            if (ImagesTypes.Contains(file.ContentType))
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", file.FileName);
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

            }
            else if(FileTypes.Contains(file.ContentType))
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/leaves/", file.FileName);
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            
            return StatusCode(StatusCodes.Status201Created);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getImage/{ImageName}")]
        public IActionResult ImageGet(string ImageName)
        {
            //ImageName = "#6M@CX79)G77LT&9F&G8^P0XYA2^YNE9J2GO^WCA.jpg";
            if (ImageName == null)
                return Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot/images", ImageName);

            var memory = new MemoryStream();
            var ext = System.IO.Path.GetExtension(path);
            using (var stream = new FileStream(path, FileMode.Open))
            {
                 stream.CopyTo(memory);
            }
            memory.Position = 0;
            var contentType = "APPLICATION/octet-stream";
            return File(memory, contentType, Path.GetFileName(path));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getFile/{FName}")]
        public IActionResult getFile(string FName)
        {
            //FName = "H4QV1OHX0A7H5ZQ1EEE4I004TMKRBF79XTZONS1J.jpg";
            if (FName == null)
                return Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot/leaves/", FName);

            var memory = new MemoryStream();
            var ext = System.IO.Path.GetExtension(path);
            using (var stream = new FileStream(path, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;
            var contentType = "APPLICATION/pdf";
            //return File(Path.GetFileName(path), contentType, FName);
            return File(memory, contentType, Path.GetFileName(path));
        }

        //for current user
        [HttpGet]
        [Route("EmployeeByProfession")]
        public IEnumerable<EmployeeDTO> EmployeeByProfession()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var empCurrentUser = _context.Employees.Where(e => e.Email == email).FirstOrDefault();
            var emps = _context.Employees.Where(e=>e.ProfessionID== empCurrentUser.ProfessionID).Select(e => new EmployeeDTO
            {
                ID = e.ID,
                Name = e.Name,
                Profession = e.Profession.Name,
                GraduatioYear = e.GraduatioYear,
                Address = e.Address,
                Code = e.Code,
                DateOfBirth = e.DateOfBirth,
                Email = e.Email,
                gender = e.gender,
                HiringDateHiringDate = e.HiringDateHiringDate,
                MaritalStatus = e.MaritalStatus,
                Phone = e.Phone,
                RelevantPhone = e.RelevantPhone,
                Photo = e.photo
            }).ToList();
            return emps;
        }
        [HttpGet]
        [Route("GetAllEmployeesByProfession/{id}")]
        public IEnumerable<EmployeeDTO> GetAllEmployeesByProfession(int id)
        {
            var emps = _context.Employees.Where(e => e.ProfessionID == id).Select(e => new EmployeeDTO
            {
                ID = e.ID,
                Name = e.Name,
                Profession = e.Profession.Name,
                GraduatioYear = e.GraduatioYear,
                Address = e.Address,
                Code = e.Code,
                DateOfBirth = e.DateOfBirth,
                Email = e.Email,
                gender = e.gender,
                HiringDateHiringDate = e.HiringDateHiringDate,
                MaritalStatus = e.MaritalStatus,
                Phone = e.Phone,
                RelevantPhone = e.RelevantPhone,
                Photo = e.photo
            }).ToList();
            return emps;
        }

    }
}
