using RestaurantApi.Dtos;

namespace RestaurantApi.Services;

public interface IPlayerService
{
    Task<PlayerResponse> CreateAsync(CreatePlayerRequest request, CancellationToken cancellationToken);
}
