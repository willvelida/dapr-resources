using Bookstore.Api.Common.Dtos;
using Bookstore.Api.Common.Models;

namespace Bookstore.Api.Services.Interfaces
{
    public interface IBookService
    {
        Task<List<Book>> GetBooksByCategory(string category);
        Task<Book?> GetBookById(string id);
        Task<string> CreateBook(BookDto bookDto);
        Task<bool> DeleteBook(string id);
        Task<bool> UpdateBook(string id, BookDto bookDto);
    }
}
