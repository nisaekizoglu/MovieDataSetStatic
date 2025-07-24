using Microsoft.AspNetCore.Mvc;
using MovieDataSetStatic.Repositories.MoviesDataRepository;

namespace MovieDataSetStatic.Controllers
{
    public class MoviesDataController : Controller
    {
        private readonly IMoviesDataRepository _moviesDataRepository;

        public MoviesDataController(IMoviesDataRepository moviesDataRepository)
        {
            _moviesDataRepository = moviesDataRepository;
        }

        public async Task<IActionResult> MoviesList()
        {
            var values =await _moviesDataRepository.GetAllMoviesDataAsync();
            return View(values);
        }
    }
}
