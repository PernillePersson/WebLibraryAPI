using AutoMapper;
using Business.Service;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly IMapper _mapper;
    private readonly ILogger<BookController> _logger;
    
    public BookController(IBookService bookService, IMapper mapper, ILogger<BookController> logger)
    {
        _bookService = bookService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    [Route("GetBooks")]
    public IActionResult GetBooks()
    {
        var books = _bookService.GetBooks();
        _logger.Log(LogLevel.Information, "GetBooks called, returning {0} books", books.Count);
        return Ok(books);
    }

    [HttpGet("{id}")]
    public IActionResult GetBook(Guid id)
    {
        var book = _bookService.GetBook(id);
        _logger.Log(LogLevel.Information, "GetBook called with id {0}", id);

        if (book == null)
        {
            _logger.Log(LogLevel.Information, "GetBook called with id {0}, but no book with that id exists", id);
            return NotFound();
        }
        return Ok(book);
    }

    [HttpPost]
    public IActionResult CreateBook([FromBody] Book book)
    {
        try
        {
            var newBook = _bookService.CreateBook(book);
            _logger.Log(LogLevel.Information, "CreateBook called");
            return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, newBook);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.Log(LogLevel.Information, "CreateBook called, but no books were created");
            throw;
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBook(Guid id)
    {
        if (!_bookService.BookExists(id))
        {
            _logger.Log(LogLevel.Information, "DeleteBook called with id {0}, but no book with that id exists", id);
            return NotFound();
        }
        _logger.Log(LogLevel.Information, "DeleteBook called with id {0}", id);
        _bookService.DeleteBook(id);
        return NoContent();
    }

    [HttpGet]
    [Route("GetAvailableBooks")]
    public IActionResult GetAvailableBooks()
    {
        try
        {
            var books = _bookService.GetAvailableBooks();
            _logger.Log(LogLevel.Information, "GetAvailableBooks called, found {0} books", books.Count);
            return Ok(books);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.Log(LogLevel.Information, "GetAvailableBooks called, but no available books found");
            throw;
        }
        
    }
    
    [HttpPut("availability/{id}")]
    public IActionResult UpdateAvailability(Guid id, [FromBody] BookAvailabilityDto bookAvailabilityDto)
    {
        var book = _bookService.GetBook(id);

        if (book == null)
        {
            _logger.Log(LogLevel.Information, "UpdateBook called with id {0} called, but no book with this id exists", id);
            return NotFound();
        }
        _logger.Log(LogLevel.Information, "UpdateBook called with id {0} called", id);
        _mapper.Map(bookAvailabilityDto, book);
        var updatedBook = _bookService.UpdateBook(id, book);
        return Ok(updatedBook);
    }
}
