using RestaurantApi.Dtos;

namespace RestaurantApi.Services;

public interface IFavoriteService
{
    Task<FavoriteResponse> CreateAsync(CreateFavoriteRequest request, CancellationToken cancellationToken);

    Task<IReadOnlyList<PlayerFavoritesResponse>> GetByPlayerNameAsync(string firstName, string lastName, CancellationToken cancellationToken);
}
