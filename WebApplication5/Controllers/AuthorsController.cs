using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication5.Models;
using WebApplication5.ViewModels;

namespace WebApplication5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly UserManager<User> _userManager;


        public AuthorsController(IRepository repository, UserManager<User> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("GetAllAuthors")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllAuthors()
        {
            try
            {
                var authors = await _repository.GetAllAuthors();
                return Ok(authors);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
            
        }

        [HttpGet]
        [Route("GetAuthorById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAuthorById(Guid id)
        {

            var existingAuthor = await _repository.GetAuthorByGuid(id);

            if (existingAuthor == null) return NotFound(existingAuthor);

            else
            {
                return Ok(existingAuthor);
            }
        }

        [HttpPost]
        [Route("AddAuthor")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddAuthor(AuthorVM authorVM)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByNameAsync(userId);

            var author = new Author
            {
                AuthorId = Guid.NewGuid(),
                AuthorName = authorVM.AuthorName,
                ActiveFrom = authorVM.ActiveFrom,
                ActiveTo = authorVM.ActiveTo,
                CreatedBy = Guid.Parse(user.Id)
            };

            try
            {
                _repository.Add(author);
                await _repository.SaveAllChangesAsync();
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e);
            }

            return Ok();
        }

        [HttpPut]
        [Route("UpdateAuthor")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateAuthor(Guid id, AuthorVM authorVM)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByNameAsync(userId);

            var existingAuthor = await _repository.GetAuthorByGuid(id);

            if (existingAuthor == null) return NotFound(existingAuthor);

            if (Guid.Parse(user.Id) != existingAuthor.CreatedBy) return StatusCode(StatusCodes.Status403Forbidden);
            try
            {
                existingAuthor.AuthorName = authorVM.AuthorName;
                existingAuthor.ActiveFrom = authorVM.ActiveFrom;
                existingAuthor.ActiveTo = authorVM.ActiveTo;

                await _repository.SaveAllChangesAsync();
            }
            catch (Exception e)
            {

                return BadRequest(e);
            }

            return Ok();
        }

        [HttpDelete]
        [Route("DeleteAuthor")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteAuthor(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByNameAsync(userId);

            var existingAuthor = await _repository.GetAuthorByGuid(id);

            if (existingAuthor == null) return NotFound(existingAuthor);
            if (Guid.Parse(user.Id) != existingAuthor.CreatedBy) return StatusCode(StatusCodes.Status403Forbidden);

            try
            {
                _repository.Delete(existingAuthor);
                await _repository.SaveAllChangesAsync();
                return NoContent();
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }

            //return Ok();
        }

    }
}
