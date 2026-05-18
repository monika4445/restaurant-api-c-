using RestaurantApi.Dtos;

namespace RestaurantApi.Services;

public interface IFavoriteService
{
    Task<FavoriteResponse> CreateAsync(CreateFavoriteRequest request, CancellationToken cancellationToken);
}
