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
    public class BooksController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly UserManager<User> _userManager;
        public BooksController(IRepository repository, UserManager<User> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("GetAllBooks")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                var books = await _repository.GetAllBooks();
                return Ok(books);
               
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }

        }

        [HttpGet]
        [Route("GetBookById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetBooksById(Guid id)
        {

            var existingBook = await _repository.GetBookById(id);

            if (existingBook == null) return NotFound(existingBook);

            else
            {
                return Ok(existingBook);
            }
        }

        [HttpGet]
        [Route("GetBooksByAuthorId")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetBooksByAuthorId(Guid id)
        {

            var existingBook = await _repository.GetBooksByAuthorId(id);

            if (existingBook == null) return NotFound(existingBook);

            else
            {
                return Ok(existingBook);
            }
        }

        [HttpPost]
        [Route("AddBook")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddAuthor(BookVM bookVM, Guid authorId)
        {
            var existingAuthor = await _repository.GetAuthorByGuid(authorId);
            if (existingAuthor == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByNameAsync(userId);

            var book = new Book
            {
                BookId = Guid.NewGuid(),
                BookName = bookVM.BookName,
                Publisher = bookVM.Publisher,
                DatePublished = bookVM.DatePublished,
                CopiesSold = bookVM.CopiesSold,
                Author = existingAuthor,
                CreatedBy =  Guid.Parse(user.Id)
            };

            try
            {
                _repository.Add(book);
                await _repository.SaveAllChangesAsync();
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e);
            }

            return Ok();
        }

        [HttpPut]
        [Route("UpdateBook")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateBook(Guid id, BookVM bookVM)
        {
            var existingBook = await _repository.GetBookById(id);

            if (existingBook == null) return NotFound();

            try
            {
                existingBook.BookName = bookVM.BookName;
                existingBook.Publisher = bookVM.Publisher;
                existingBook.DatePublished = bookVM.DatePublished;
                existingBook.CopiesSold = bookVM.CopiesSold;

                await _repository.SaveAllChangesAsync();
            }
            catch (Exception e)
            {

                return BadRequest(e);
            }

            return Ok();
        }

        [HttpDelete]
        [Route("DeleteBook")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var existingBook = await _repository.GetBookById(id);

            if (existingBook == null) return NotFound(existingBook);

            try
            {
                _repository.Delete(existingBook);
                await _repository.SaveAllChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }

           
        }

    }
}
