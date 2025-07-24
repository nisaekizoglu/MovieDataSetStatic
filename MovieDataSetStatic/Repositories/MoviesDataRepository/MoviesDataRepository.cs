using Dapper;
using MovieDataSetStatic.Context;
using MovieDataSetStatic.Dtos.MoviesDataDto;

namespace MovieDataSetStatic.Repositories.MoviesDataRepository
{
    public class MoviesDataRepository : IMoviesDataRepository
    {
        private readonly DapperContext _dapperContext;

        public MoviesDataRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<List<ResultMoviesDataDto>> GetAllMoviesDataAsync()
        {
            string query = "Select Top 200 * From MoviesData";
            var connection = _dapperContext.CreateConnection();
            var values = await connection.QueryAsync<ResultMoviesDataDto>(query);
            return values.ToList();
        }

        public async Task<GetByIdMoviesDataDto> GetByIdMoviesDataAsync(int id)
        {
            string query = "Select * From MoviesData Where id=@id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            var connection = _dapperContext.CreateConnection();
            var values = await connection.QueryFirstOrDefaultAsync<GetByIdMoviesDataDto>(query);
            return values;
        }
    }
}
