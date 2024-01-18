using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaxtureAssignAuthAPI.DBContext;
using BaxtureAssignAuthAPI.Models;
using BaxtureAssignAuthAPI.Repository.IRepository;
using BaxtureAssignAuthAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BaxtureAssignAuthAPI.HelperClass;

namespace BaxtureAssignAuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly IJWTServiceRepository _jWTServiceRepository;
        private readonly IUserRepository _userRepository;

        public UsersController(UserDbContext context, IUserRepository userRepository, IJWTServiceRepository jWTServiceRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _jWTServiceRepository = jWTServiceRepository;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Login(string username, string password)
        {
            var existingUser = _context.User
           .SingleOrDefault(u => u.Username == username && u.Password == password);

            if (existingUser != null)
            {
                // Authentication successful
                var token = _jWTServiceRepository.GenerateToken(existingUser.Id, existingUser.Username, existingUser.IsAdmin);
                return Ok(new { Token = token });
            }

            // Authentication failed
            return Unauthorized();
        }

        // GET: api/Users
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<object> GetUser()
        {

            IEnumerable<User> users = await _userRepository.GetUsers();
            return Ok(users);


        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {

            if (!Guid.TryParse(id, out _))
            {
                return BadRequest("Invalid userId format");
            }
            User user = await _userRepository.GetUserById(id);
            if (user == null)
                return NotFound($"User with ID {id} not found");

            return Ok(user);

        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> PutUser(string userId, User updateUser)
        {

            if (!Guid.TryParse(userId, out _))
            {
                return BadRequest("Invalid userId format");
            }

            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Check if the logged-in user is an admin or the owner of the record
            if (!User.IsInRole("Admin") && loggedInUserId != userId)
            {
                return Forbid(); // Non-admin users cannot change other users' data
            }

            var UserExists = await _userRepository.GetUserById(userId);

            if (UserExists == null)
            {
                // Record with id === userId doesn't exist
                return NotFound($"User with ID {userId} not found");
            }

            User updatedUser = await _userRepository.UpdateUser(updateUser);

            // Return the updated user with a status code 200
            return Ok(updatedUser);

        }

        // POST: api/Users
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<User>> PostUser([FromBody] User user)
        {

            if (user == null)
            {
                return BadRequest("Invalid user data");
            }

            // Additional validation as needed
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("Username and Password are required fields");
            }

            // Your existing code to create a user
            await _userRepository.CreateUser(user);

            // Return the created user with a status code 201
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);




        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (!Guid.TryParse(userId, out _))
            {
                return BadRequest("Invalid userId format");
            }

            var userToDelete = await _userRepository.GetUserById(userId);

            if (userToDelete == null)
            {
                // Record with id === userId doesn't exist
                return NotFound($"User with ID {userId} not found");
            }

            // Delete the user
            await _userRepository.DeleteUser(userId);

            // Return with status code 204 (No Content) as the record is deleted
            return NoContent();

        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("search")]
        public async Task<IActionResult> SearchUsers([FromBody] UserSearchRequest searchRequest)
        {
            // Validate searchRequest and apply filters, pagination, and sorting
            var filteredUsers = await _userRepository.SearchUsersAsync(searchRequest);

            // Return the filtered users with a status code 200
            return Ok(filteredUsers);

        }


        [Authorize(Policy = "AdminOnly")]
        [HttpPost("export")]
        public async Task<IActionResult> ExportUsers([FromBody] UserSearchRequest searchRequest)
        {
            var filteredUsers = await _userRepository.SearchUsersAsync(searchRequest);
            var userList = filteredUsers.ToList();

            byte[] fileContents = ExportHelper.ExportUsers(userList);

            // Add headers and return the file
            var fileName = $"UserExport_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

    }
}
