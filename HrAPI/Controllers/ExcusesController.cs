﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HrAPI.Models;
using System.Security.Claims;

using Microsoft.AspNetCore.Identity;

namespace HrAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcusesController : ControllerBase
    {
        private readonly UserManager<ApplicationUser>userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ExcusesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _context = context;
            this.httpContextAccessor = httpContextAccessor;
            
        }

        // GET: api/Excuses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Excuse>>> GetExcuses()
        {
            return await _context.Excuses.ToListAsync();
        }

        // GET: api/Excuses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Excuse>> GetExcuse(int id)
        {
            var excuse = await _context.Excuses.FindAsync(id);

            if (excuse == null)
            {
                return NotFound();
            }

            return excuse;
        }

        // PUT: api/Excuses/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExcuse(int id, Excuse excuse)
        {
            if (id != excuse.ID)
            {
                return BadRequest();
            }

            _context.Entry(excuse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExcuseExists(id))
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

        // POST: api/Excuses
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Excuse>> PostExcuse(Excuse excuse)
        {
            //System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            //var id = userManager.GetUserId(User);
            //var user = await userManager.GetUserAsync(HttpContext.User);
            ///var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            //var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            //var id = excuse.UserID;
            //id = id + "";
            //var user = userManager.GetUserId()

            var emp = _context.Employees.Where(e => e.Email == excuse.email).FirstOrDefault();
            excuse.Employee = emp;
            excuse.EmployeeID = emp.ID;
            
            _context.Excuses.Add(excuse);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExcuse", new { id = excuse.ID }, excuse);
        }

        // DELETE: api/Excuses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Excuse>> DeleteExcuse(int id)
        {
            var excuse = await _context.Excuses.FindAsync(id);
            if (excuse == null)
            {
                return NotFound();
            }

            _context.Excuses.Remove(excuse);
            await _context.SaveChangesAsync();

            return excuse;
        }

        private bool ExcuseExists(int id)
        {
            return _context.Excuses.Any(e => e.ID == id);
        }
    }
}