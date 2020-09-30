using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HrAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using HrAPI.DTO;
using HrAPI.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrAPI.Controllers
{
    
    [Authorize(AuthenticationSchemes =
    JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly string email;
        private readonly UserManager<ApplicationUser>userManager;
        private readonly Employee CurrentEmpUser;

        public LeaveRequestsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);

        // GET: api/LeaveRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveDTO>>> GetLeaveRequests()
        {
            return await _context.LeaveRequests.Select(ex => new LeaveDTO
                {
                    ID = ex.ID,
                    EmployeeName = ex.Employee.Name,
                    Profession = ex.Employee.Profession.Name,
                    Status = ex.Status,
                    Comment = ex.Comment,
                    Date = ex.Date,
                    EmployeeID = ex.EmployeeID,
                    AlternativeAddress = ex.AlternativeAddress,
                    AlternativeEmp = ex.AlternativeEmp.Name,
                    Days = ex.Days,
                    LeaveTypeID = ex.LeaveTypeID,
                    Start = ex.Start
                }).ToListAsync();

        }
        [HttpGet]
        [Route("GetLeaveTypes")]
        public async Task<ActionResult<IEnumerable<LeavesTypes>>> GetLeaveTypes()
        {
            return await _context.LeavesTypes.ToListAsync();
        }

        // GET: api/LeaveRequests/5
        [HttpGet("{id}")]
        public LeaveDTO GetLeaveRequest(int id)
        {
            var leaveRequest =  _context.LeaveRequests.Include(e=>e.Employee).ThenInclude(e=>e.Profession).Include(e => e.AlternativeEmp)
                .Include(e=>e.LeaveType).FirstOrDefault(e=>e.ID==id);
            var LeaveRequestDTO = new LeaveDTO
            {
                ID = leaveRequest.ID,
                EmployeeName = leaveRequest.Employee.Name,
                Profession = leaveRequest.Employee.Profession.Name,
                Status = leaveRequest.Status,
                End = leaveRequest.Start.AddDays(leaveRequest.Days),
                Comment = leaveRequest.Comment,
                Date = leaveRequest.Date,
                EmployeeID = leaveRequest.EmployeeID,
                AlternativeAddress = leaveRequest.AlternativeAddress,
                AlternativeEmp = leaveRequest.AlternativeEmp.Name,
                Days = leaveRequest.Days,
                LeaveTypeID = leaveRequest.LeaveTypeID,
                Start = leaveRequest.Start
            };
            return LeaveRequestDTO;
        }
        [Route("ApprovedLeaves")]
        public async Task<ActionResult<IEnumerable<LeaveDTO>>> GetApprovedLeaves()
        {
            return await _context.LeaveRequests.Where(ex => ex.Status == "approved").Select(ex => new LeaveDTO
            {
                ID = ex.ID,
                EmployeeName=ex.Employee.Name,
                Profession=ex.Employee.Profession.Name,
                Status = ex.Status,
                Comment = ex.Comment,
                Date = ex.Date,
                EmployeeID = ex.EmployeeID,
                AlternativeAddress = ex.AlternativeAddress,
                AlternativeEmp = ex.AlternativeEmp.Name,
                Days = ex.Days,
                LeaveTypeID = ex.LeaveTypeID,
                Start = ex.Start
            }).ToListAsync();
        }
        [Route("DisApprovedLeaves")]
        public async Task<ActionResult<IEnumerable<LeaveDTO>>> GetDisApprovedLeaves()
        {
            return await _context.LeaveRequests.Where(ex => ex.Status == "disapproved").Select(ex => new LeaveDTO
            {
                ID = ex.ID,
                EmployeeName = ex.Employee.Name,
                Profession = ex.Employee.Profession.Name,
                Status = ex.Status,
                Comment = ex.Comment,
                Date = ex.Date,
                EmployeeID = ex.EmployeeID,
                AlternativeAddress = ex.AlternativeAddress,
                AlternativeEmp = ex.AlternativeEmp.Name,
                Days = ex.Days,
                LeaveTypeID = ex.LeaveTypeID,
                Start = ex.Start

            }).ToListAsync();
        }
        [Route("PendingLeaves")]
        public async Task<ActionResult<IEnumerable<LeaveDTO>>> GetPendingLeaves()
        {
            var R= await _context.LeaveRequests.Where(ex => ex.Start >= DateTime.Now && ex.Status == "pending").Select(ex => new LeaveDTO
            {
                ID = ex.ID,
                EmployeeName = ex.Employee.Name,
                Profession = ex.Employee.Profession.Name,
                Status = ex.Status,
                Comment = ex.Comment,
                Date = ex.Date,
                EmployeeID = ex.EmployeeID,
                AlternativeAddress = ex.AlternativeAddress,
                AlternativeEmp = ex.AlternativeEmp.Name,
                Days = ex.Days,
                LeaveTypeID = ex.LeaveTypeID,
                Start = ex.Start
            }).ToListAsync();
            return R;
        }

        //[HttpGet("{id}")]
        [Route("AcceptLeave/{id}")]
        public ActionResult AcceptLeave(int id)
        {
            var leave = _context.LeaveRequests.Find(id);

            if (leave == null)
            {
                return NotFound();
            }
            leave.Status = "approved";
            _context.SaveChanges();
            return Ok();
        }
        [Route("RejectLeave/{id}")]
        public ActionResult RejectLeave(int id)
        {
            var leave = _context.LeaveRequests.Find(id);

            if (leave == null)
            {
                return NotFound();
            }
            leave.Status = "disapproved";
            _context.SaveChanges();
            return Ok();
        }
        [Route("PreviousLeaves")]
        public async Task<ActionResult<IEnumerable<LeaveDTO>>> PreviousLeaves()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var emp = _context.Employees.Where(e => e.Email == email).FirstOrDefault();

            return await _context.LeaveRequests.Where(ex => ex.Employee.Email == email).Select(ex => new LeaveDTO
            {
                ID = ex.ID,
                EmployeeName = ex.Employee.Name,
                Profession = ex.Employee.Profession.Name,
                Status = ex.Status,
                Comment = ex.Comment,
                Date = ex.Date,
                EmployeeID=ex.EmployeeID,
                AlternativeAddress=ex.AlternativeAddress,
                AlternativeEmp=ex.AlternativeEmp.Name,
                Days=ex.Days,
                LeaveTypeID=ex.LeaveTypeID,
                Start=ex.Start

            }).ToListAsync();
        }

        [HttpGet]
        [Route("getLeaveRequestfiles/{id}")]
        public IEnumerable<string> getLeaveRequestfiles(int id)
        {
            return _context.LeaveFiles.Where(l => l.LeaveRequestID == id).Select(l=>l.FileName).ToList();
        }
        // PUT: api/LeaveRequests/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeaveRequest(int id, LeaveRequest leaveRequest)
        {
            if (id != leaveRequest.ID)
            {
                return BadRequest();
            }

            _context.Entry(leaveRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaveRequestExists(id))
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

        // POST: api/LeaveRequests
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<LeaveRequest>> PostLeaveRequest(LeaveRequestVM leaveRequest)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var CurrentEmpUser = _context.Employees.Where(e => e.Email == email).FirstOrDefault();
            if (leaveRequest.EmployeeID==0)
            {
                leaveRequest.EmployeeID= CurrentEmpUser.ID;
            }
            leaveRequest.Date = DateTime.Now;
            LeaveRequest lv = new LeaveRequest
            {
                Date = leaveRequest.Date,
                Comment = leaveRequest.Comment,
                AlternativeEmpID = leaveRequest.AlternativeEmpID,
                EmployeeID = leaveRequest.EmployeeID,
                LeaveTypeID = leaveRequest.LeaveTypeID,
                AlternativeAddress = leaveRequest.AlternativeAddress,
                Start = leaveRequest.Start,
                Status = leaveRequest.Status,
                Days = leaveRequest.Days
            };
            _context.LeaveRequests.Add(lv);
            await _context.SaveChangesAsync();
            if (leaveRequest.LeavesFiles != null)
            {
                var files = leaveRequest.LeavesFiles.Split(',').ToList();

                files.Remove("");
                foreach (var item in files)
                {
                    LeaveFiles leaveFiles = new LeaveFiles
                    {
                        FileName = item,
                        LeaveRequestID = lv.ID
                    };
                    _context.LeaveFiles.Add(leaveFiles);
                }
            }
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetLeaveRequest", new { id = leaveRequest.ID }, leaveRequest);
        }

        // DELETE: api/LeaveRequests/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<LeaveRequest>> DeleteLeaveRequest(int id)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            if (leaveRequest == null)
            {
                return NotFound();
            }

            _context.LeaveRequests.Remove(leaveRequest);
            await _context.SaveChangesAsync();

            return leaveRequest;
        }

        private bool LeaveRequestExists(int id)
        {
            return _context.LeaveRequests.Any(e => e.ID == id);
        }
    }
}
