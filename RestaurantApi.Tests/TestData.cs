namespace RestaurantApi.Tests;

public static class TestData
{
    public static object Restaurant(string name = "Test Diner", string address = "1 Main St") => new
    {
        name,
        address,
        contactNumber = "+1-555-0100",
        hoursOfOperation = "Mon-Sun 9-21"
    };

    public static object Address() => new
    {
        streetNumber = "12",
        line1 = "Sample St",
        line2 = (string?)null,
        city = "Yerevan",
        state = "YR",
        postal = "0010",
        country = "AM"
    };

    public static object Player(
        string firstName = "Ada",
        string lastName = "Lovelace",
        string email = "ada@example.com",
        string license = "DL-12345",
        string passport = "PA-98765",
        string dob = "1990-12-10") => new
    {
        firstName,
        lastName,
        dob,
        primaryAddress = Address(),
        alternateAddress = Address(),
        officeAddress = Address(),
        mobileNumber = "+374-99-111-111",
        email,
        driversLicense = license,
        passport
    };
}
