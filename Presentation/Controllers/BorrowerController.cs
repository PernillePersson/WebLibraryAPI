using Business.Service;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BorrowerController : Controller
{
    private readonly IBorrowerService _borrowerService;
    private readonly ILogger<BorrowerController> _logger;
    
    public BorrowerController(IBorrowerService borrowerService, ILogger<BorrowerController> logger)
    {
        _borrowerService = borrowerService;
        _logger = logger;
    }
    
    [HttpGet]
    [Route("GetBorrowers")]
    public IActionResult GetBorrowers()
    {
        var borrowers = _borrowerService.GetBorrowers();
        _logger.Log(LogLevel.Information, "GetBorrowers called, returning {0} borrowers", borrowers.Count);
        return Ok(borrowers);
    }

    [HttpGet("{id}")]
    public IActionResult GetBorrower(Guid id)
    {
        var borrower = _borrowerService.GetBorrower(id);
        _logger.Log(LogLevel.Information, "GetBorrower called with id {0}", id);

        if (borrower == null)
        {
            _logger.Log(LogLevel.Information, "GetBorrower called with id {0}, but no borrower with that id exists", id);
            return NotFound();
        }
        return Ok(borrower);
    }

    [HttpPost]
    public IActionResult CreateBorrower([FromBody] Borrower borrower)
    {
        try
        {
            var newBorrower = _borrowerService.CreateBorrower(borrower);
            _logger.Log(LogLevel.Information, "CreateBorrower called");
            return CreatedAtAction(nameof(GetBorrower), new { id = newBorrower.Id }, newBorrower);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.Log(LogLevel.Information, "CreateBorrower called, but no borrower was created");
            throw;
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateBorrower(Guid id, [FromBody] Borrower borrower)
    {
        if (!_borrowerService.BorrowerExists(id))
        {
            _logger.Log(LogLevel.Information, "UpdateBorrower called with id {0}, but no borrower with that id exists", id);
            return NotFound();
        }
        
        _logger.Log(LogLevel.Information, "UpdateBorrower called with id {0}", id);
        var updatedBook = _borrowerService.UpdateBorrower(id, borrower);
        return Ok(updatedBook);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBorrower(Guid id)
    {
        if (!_borrowerService.BorrowerExists(id))
        {
            _logger.Log(LogLevel.Information, "DeleteBorrower called with id {0}, but no borrower with that id exists", id);

            return NotFound();
        }
        _logger.Log(LogLevel.Information, "DeleteBorrower called with id {0}", id);

        _borrowerService.DeleteBorrower(id);
        return NoContent();
    }
}