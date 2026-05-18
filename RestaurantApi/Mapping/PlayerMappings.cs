using RestaurantApi.Domain;
using RestaurantApi.Dtos;

namespace RestaurantApi.Mapping;

public static class PlayerMappings
{
    public static Address ToEntity(this AddressDto dto) => new()
    {
        StreetNumber = dto.StreetNumber.Trim(),
        Line1 = dto.Line1.Trim(),
        Line2 = dto.Line2?.Trim(),
        City = dto.City.Trim(),
        State = dto.State.Trim(),
        Postal = dto.Postal.Trim(),
        Country = dto.Country.Trim()
    };

    public static AddressDto ToDto(this Address address) => new()
    {
        StreetNumber = address.StreetNumber,
        Line1 = address.Line1,
        Line2 = address.Line2,
        City = address.City,
        State = address.State,
        Postal = address.Postal,
        Country = address.Country
    };

    public static Player ToEntity(this CreatePlayerRequest request) => new()
    {
        FirstName = request.FirstName.Trim(),
        LastName = request.LastName.Trim(),
        Dob = request.Dob,
        PrimaryAddress = request.PrimaryAddress.ToEntity(),
        AlternateAddress = request.AlternateAddress.ToEntity(),
        OfficeAddress = request.OfficeAddress.ToEntity(),
        MobileNumber = request.MobileNumber.Trim(),
        Email = request.Email.Trim(),
        DriversLicense = request.DriversLicense.Trim(),
        Passport = request.Passport.Trim()
    };

    public static PlayerResponse ToResponse(this Player player) => new()
    {
        Id = player.Id,
        FirstName = player.FirstName,
        LastName = player.LastName,
        Dob = player.Dob,
        PrimaryAddress = player.PrimaryAddress.ToDto(),
        AlternateAddress = player.AlternateAddress.ToDto(),
        OfficeAddress = player.OfficeAddress.ToDto(),
        MobileNumber = player.MobileNumber,
        Email = player.Email,
        DriversLicense = player.DriversLicense,
        Passport = player.Passport
    };

    public static PlayerMembershipsResponse ToMembershipsResponse(this Player player, IReadOnlyCollection<Guid> favoriteRestaurantIds) => new()
    {
        Id = player.Id,
        FirstName = player.FirstName,
        LastName = player.LastName,
        Dob = player.Dob,
        PrimaryAddress = player.PrimaryAddress.ToDto(),
        AlternateAddress = player.AlternateAddress.ToDto(),
        OfficeAddress = player.OfficeAddress.ToDto(),
        MobileNumber = player.MobileNumber,
        Email = player.Email,
        DriversLicense = player.DriversLicense,
        Passport = player.Passport,
        Restaurants = player.Memberships.Select(m => new RestaurantInMembershipResponse
        {
            Id = m.Restaurant.Id,
            Name = m.Restaurant.Name,
            Address = m.Restaurant.Address,
            ContactNumber = m.Restaurant.ContactNumber,
            HoursOfOperation = m.Restaurant.HoursOfOperation,
            IsFavoriteRestaurant = favoriteRestaurantIds.Contains(m.RestaurantId)
        }).ToList()
    };
}
