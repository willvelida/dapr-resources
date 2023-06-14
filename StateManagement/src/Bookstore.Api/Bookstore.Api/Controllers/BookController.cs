using Bookstore.Api.Common.Dtos;
using Bookstore.Api.Common.Models;
using Bookstore.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BookController> _logger;

        public BookController(ILogger<BookController> logger, IBookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Book))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string category)
        {
            try
            {
                var books = await _bookService.GetBooksByCategory(category);
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(Get)}: {ex.Message}. {ex.InnerException}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{bookId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Book))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBook(string bookId)
        {
            try
            {
                var book = await _bookService.GetBookById(bookId);

                if (book is not null)
                {
                    return Ok(book);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(GetBook)}: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] BookDto bookDto)
        {
            try
            {
                var bookId = await _bookService.CreateBook(bookDto);

                if (bookId is not null)
                {
                    return Created($"/api/books/{bookId}", null);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(Post)}: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{bookId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(string bookId, [FromBody] BookDto bookDto)
        {
            try
            {
                var updated = await _bookService.UpdateBook(bookId, bookDto);

                if (updated)
                {
                    return Ok();
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(Put)}: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{bookId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string bookId)
        {
            try
            {
                var deleted = await _bookService.DeleteBook(bookId);

                if (deleted)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(Delete)}: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
