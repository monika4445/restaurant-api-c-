using RestaurantApi.Dtos;

namespace RestaurantApi.Services;

public interface IMembershipService
{
    Task<MembershipResponse> CreateAsync(CreateMembershipRequest request, CancellationToken cancellationToken);

    Task<IReadOnlyList<PlayerMembershipsResponse>> GetByPlayerNameAsync(string firstName, string lastName, CancellationToken cancellationToken);
}
