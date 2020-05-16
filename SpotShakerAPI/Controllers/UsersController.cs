using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotShakerAPI.Data;
using SpotShakerAPI.Models;

namespace SpotShakerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SpotShakerAPIContext _context;

        public UsersController(SpotShakerAPIContext context)
        {
            _context = context;
        }
        // https://localhost:44382/api/users/admin/auth/
        [HttpPost("admin/auth/")]
        public IActionResult GetAuthentification([FromBody]User user)
        {
            try
            {
                User user1 = (from u in _context.User
                              where u.Login.Equals(user.Login) &&
                                    u.Pwd.Equals(user.Pwd)
                              select u).FirstOrDefault();

                if (user1 == null)
                {
                    return new JsonResult(user1);
                }
                else
                {
                    return new JsonResult(user1);
                }
            }
            catch (Exception e)
            {
                return new JsonResult(e.Message);
            }
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();         
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Login))
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

        // POST: api/Users
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("admin/adduser")]
        public async Task<ActionResult<User>> PostUser([FromBody]User user)
        {
            User u = new User();
            try
            {
                if((user.Login == "") || (user.Pwd == ""))
                {
                    return new JsonResult(u);
                }
                else if(UserExists(user.Login))
                {
                    u.Login = "exist";
                     return new JsonResult(u);
                }
                else
                {
                    _context.User.Add(user);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetUser", new { id = user.Id }, user);
                }   
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
            
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(string login)
        {
            return _context.User.Any(e => e.Login == login);
        }
    }
}
