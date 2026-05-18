using RestaurantApi.Dtos;

namespace RestaurantApi.Services;

public interface IRestaurantService
{
    Task<RestaurantResponse> CreateAsync(CreateRestaurantRequest request, CancellationToken cancellationToken);

    Task<IReadOnlyList<RestaurantResponse>> SearchByNameAsync(string name, CancellationToken cancellationToken);

    Task<IReadOnlyList<RestaurantWithMemberCountResponse>> GetMembersAgedAsync(string name, int age, CancellationToken cancellationToken);
}
