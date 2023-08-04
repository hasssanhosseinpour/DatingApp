
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace API.Controllers
{
//     [ApiController]
//     [Route("api/[controller]")]  // localhost/api/users
     [Authorize]
    public class UsersController : BaseApiController
    {   
       //ctor + tab
        private readonly DataContext _context;
       public UsersController(DataContext context)
       {
            _context = context;        
       } 
       [AllowAnonymous]
       [HttpGet]
       public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
       {
            var users = await _context.Users.ToListAsync();
            return users;
       }
       [HttpGet ("{id}")]
       public async Task<ActionResult<AppUser>> GetUser(int id)
       {
            var user = await _context.Users.FindAsync(id);
            return user;
       }
    }
}