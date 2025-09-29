#nullable enable
using MoviesApp.Models;
using System.Threading;
using System.Threading.Tasks;

namespace MoviesApp.Services
{
    public interface IOmdbService
    {
        Task<Movie?> GetByTitleAsync(string title, CancellationToken ct);
    }
}