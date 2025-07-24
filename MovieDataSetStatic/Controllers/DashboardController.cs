using Microsoft.AspNetCore.Mvc;
using MovieDataSetStatic.Repositories.MoviesDataRepository;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDataSetStatic.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IMoviesDataRepository _moviesDataRepository;
        public DashboardController(IMoviesDataRepository moviesDataRepository)
        {
            _moviesDataRepository = moviesDataRepository;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var allMovies = await _moviesDataRepository.GetAllMoviesDataAsync();

            // En yüksek puanlı 5 film
            var top5Movies = allMovies
                .OrderByDescending(m => m.vote_average)
                .Take(5)
                .Select(m => new
                {
                    Title = m.original_title,
                    Genres = m.spoken_languages,
                    Overview = m.overview
                })
                .ToList();

            var genresCount = allMovies
                .SelectMany(m => (m.spoken_languages ?? "").Split(',', System.StringSplitOptions.RemoveEmptyEntries))
                .Select(g => g.Trim())
                .GroupBy(g => g)
                .ToDictionary(g => g.Key, g => g.Count());
            var productionCount = allMovies
                .SelectMany(m => (m.production_companies ?? "").Split(',', System.StringSplitOptions.RemoveEmptyEntries))
                .Select(p => p.Trim())
                .GroupBy(p => p)
                .ToDictionary(p => p.Key, p => p.Count());
            var top100 = allMovies.OrderByDescending(m => m.vote_average).Take(100).ToList();
            var top100Genre = top100
                .SelectMany(m => (m.spoken_languages ?? "").Split(',', System.StringSplitOptions.RemoveEmptyEntries))
                .Select(g => g.Trim())
                .GroupBy(g => g)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key;

            // En çok filme sahip 10 prodüksiyon şirketi
            var top10Productions = allMovies
                .SelectMany(m => (m.production_companies ?? "").Split(',', System.StringSplitOptions.RemoveEmptyEntries))
                .Select(p => p.Trim())
                .GroupBy(p => p)
                .OrderByDescending(g => g.Count())
                .Take(10)
                .ToDictionary(g => g.Key, g => g.Count());

            // Puan aralıklarına göre film sayısı (0-2, 2-4, 4-6, 6-8, 8-10)
            var ratingRanges = new[] { (0f, 2f), (2f, 4f), (4f, 6f), (6f, 8f), (8f, 10f) };
            var ratingDistribution = ratingRanges
                .Select(r => new {
                    Range = $"{r.Item1}-{r.Item2}",
                    Count = allMovies.Count(m => m.vote_average >= r.Item1 && m.vote_average < r.Item2)
                })
                .ToList();

            // Her türün ortalama puanı
            var genreAverages = allMovies
                .SelectMany(m => (m.spoken_languages ?? "").Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                    .Select(g => new { Genre = g.Trim(), m.vote_average }))
                .GroupBy(x => x.Genre)
                .ToDictionary(g => g.Key, g => (double)g.Average(x => x.vote_average));

            // Tablo için sayfalama
            int pageSize = 10;
            var pagedMovies = allMovies
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new {
                    Title = m.original_title,
                    Genres = (m.spoken_languages ?? "").Split(',', System.StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList(),
                    VoteAverage = Math.Round(m.vote_average, 1),
                    Productions = (m.production_companies ?? "").Split(',', System.StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Take(2).ToList()
                })
                .ToList();
            int totalPages = (int)Math.Ceiling(allMovies.Count / (double)pageSize);
            ViewBag.PagedMovies = pagedMovies;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            ViewBag.Top5Movies = top5Movies;
            ViewBag.GenresCount = genresCount;
            ViewBag.ProductionCount = productionCount;
            ViewBag.Top100Genre = top100Genre;
            ViewBag.Top10Productions = top10Productions;
            ViewBag.RatingDistribution = ratingDistribution;
            ViewBag.GenreAverages = genreAverages;
            return View();
        }
    }
} 