using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApiHome4.DTOs;
using WebApiHome4.Models;
using WebApiHome4.Repositories;
using WebApiHome4.Results;

namespace WebApiHome4.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BooksController(IBookRepository repository, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ReturnResult<IEnumerable<BookDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ReturnResult<IEnumerable<BookDto>>>> GetAll(CancellationToken cancellationToken)
    {
        var books = await repository.GetAllAsync(cancellationToken);
        return Ok(ReturnResult<IEnumerable<BookDto>>.Ok(mapper.Map<IEnumerable<BookDto>>(books), "Список книг получен."));
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(ReturnResult<IEnumerable<BookDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ReturnResult<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReturnResult<IEnumerable<BookDto>>>> SearchByAuthor([FromQuery] string? author, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(author))
        {
            return BadRequest(ReturnResult<object>.Failure("Укажите автора в параметре author."));
        }

        var books = await repository.SearchByAuthorAsync(author.Trim(), cancellationToken);
        return Ok(ReturnResult<IEnumerable<BookDto>>.Ok(mapper.Map<IEnumerable<BookDto>>(books), $"Результаты поиска по автору: {author}."));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ReturnResult<BookDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ReturnResult<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReturnResult<BookDto>>> GetById(int id, CancellationToken cancellationToken)
    {
        var book = await repository.GetByIdAsync(id, cancellationToken);
        if (book is null) return NotFound(ReturnResult<object>.Failure($"Книга с ID {id} не найдена."));
        return Ok(ReturnResult<BookDto>.Ok(mapper.Map<BookDto>(book), "Книга получена."));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReturnResult<BookDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ReturnResult<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReturnResult<BookDto>>> Create(CreateBookDto dto, CancellationToken cancellationToken)
    {
        var book = mapper.Map<Book>(dto);
        await repository.AddAsync(book, cancellationToken);
        var result = ReturnResult<BookDto>.Ok(mapper.Map<BookDto>(book), "Книга добавлена.");
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ReturnResult<BookDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ReturnResult<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ReturnResult<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReturnResult<BookDto>>> Update(int id, UpdateBookDto dto, CancellationToken cancellationToken)
    {
        var existing = await repository.GetByIdAsync(id, cancellationToken);
        if (existing is null) return NotFound(ReturnResult<object>.Failure($"Книга с ID {id} не найдена."));
        mapper.Map(dto, existing);
        await repository.UpdateAsync(existing, cancellationToken);
        return Ok(ReturnResult<BookDto>.Ok(mapper.Map<BookDto>(existing), "Данные книги обновлены."));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ReturnResult<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ReturnResult<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReturnResult<object>>> Delete(int id, CancellationToken cancellationToken)
    {
        if (!await repository.DeleteAsync(id, cancellationToken))
            return NotFound(ReturnResult<object>.Failure($"Книга с ID {id} не найдена."));
        return Ok(ReturnResult<object>.Ok(new { id }, "Книга удалена."));
    }
}
