using Bookstore.Api.Common.Dtos;
using Bookstore.Api.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
