using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;

namespace SimpleRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        // In-memory user store
        private static readonly ConcurrentDictionary<Guid, User> users = new();

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserInput input)
        {
            if (string.IsNullOrWhiteSpace(input.Name) || string.IsNullOrWhiteSpace(input.Email))
                return BadRequest(new { error = "Missing name or email" });

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = input.Name,
                Email = input.Email
            };
            users[user.Id] = user;
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(Guid id)
        {
            if (users.TryGetValue(id, out var user))
                return Ok(user);
            return NotFound(new { error = "User not found" });
        }
    }

    public class UserInput
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class User : UserInput
    {
        public Guid Id { get; set; }
    }
}




