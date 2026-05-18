using RestaurantApi.Dtos;

namespace RestaurantApi.Services;

public interface IMembershipService
{
    Task<MembershipResponse> CreateAsync(CreateMembershipRequest request, CancellationToken cancellationToken);
}
