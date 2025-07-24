using MovieDataSetStatic.Dtos.MoviesDataDto;

namespace MovieDataSetStatic.Repositories.MoviesDataRepository
{
    public interface IMoviesDataRepository
    {
        Task<List<ResultMoviesDataDto>> GetAllMoviesDataAsync();
        Task<GetByIdMoviesDataDto> GetByIdMoviesDataAsync(int id);
    }
}
