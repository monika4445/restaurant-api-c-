using RestaurantApi.Domain;
using RestaurantApi.Dtos;

namespace RestaurantApi.Mapping;

public static class RestaurantMappings
{
    public static Restaurant ToEntity(this CreateRestaurantRequest request) => new()
    {
        Name = request.Name.Trim(),
        Address = request.Address.Trim(),
        ContactNumber = request.ContactNumber.Trim(),
        HoursOfOperation = request.HoursOfOperation.Trim()
    };

    public static RestaurantResponse ToResponse(this Restaurant restaurant) => new()
    {
        Id = restaurant.Id,
        Name = restaurant.Name,
        Address = restaurant.Address,
        ContactNumber = restaurant.ContactNumber,
        HoursOfOperation = restaurant.HoursOfOperation
    };
}
