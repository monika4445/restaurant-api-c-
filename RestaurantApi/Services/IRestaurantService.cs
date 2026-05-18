using RestaurantApi.Dtos;

namespace RestaurantApi.Services;

public interface IRestaurantService
{
    Task<RestaurantResponse> CreateAsync(CreateRestaurantRequest request, CancellationToken cancellationToken);
}
