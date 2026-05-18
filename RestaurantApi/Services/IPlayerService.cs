using RestaurantApi.Dtos;

namespace RestaurantApi.Services;

public interface IPlayerService
{
    Task<PlayerResponse> CreateAsync(CreatePlayerRequest request, CancellationToken cancellationToken);

    Task<IReadOnlyList<PlayerResponse>> SearchByNameAsync(string? firstName, string? lastName, CancellationToken cancellationToken);
}
