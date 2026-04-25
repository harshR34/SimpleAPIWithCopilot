using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        // In-memory data store
        private static List<User> _users = new()
        {
            new User { Id = 1, Name = "Alice", Email = "alice@example.com", Age = 28 },
            new User { Id = 2, Name = "Bob",   Email = "bob@example.com",   Age = 34 }
        };

        // GET api/users
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll() => Ok(_users);

        // GET api/users/{id}
        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return user is null ? NotFound(new { message = $"User {id} not found" }) : Ok(user);
        }

        // POST api/users
        [HttpPost]
        public ActionResult<User> Create([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_users.Any(u => u.Email == user.Email))
                return Conflict(new { message = "Email already exists" });

            user.Id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
            _users.Add(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        // PUT api/users/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] User updated)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user is null) return NotFound(new { message = $"User {id} not found" });

            user.Name  = updated.Name;
            user.Email = updated.Email;
            user.Age   = updated.Age;
            return NoContent();
        }

        // DELETE api/users/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user is null) return NotFound(new { message = $"User {id} not found" });

            _users.Remove(user);
            return NoContent();
        }
    }
}