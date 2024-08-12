using Game.Characters.Customers;
using Game.Subsystems.CookingSubsystem.Entities;

namespace Game.Subsystems.RestaurantSubsystem.Entities;

public sealed class RestaurantOrder
{
    public RestaurantOrder(CustomerCharacter customer, RestaurantTable table, Recipe request)
    {
        Customer = customer;
        Table = table;
        Request = request;
    }

    public CustomerCharacter Customer { get; private set; }

    public Recipe Request { get; private set; }

    public RestaurantTable Table { get; private set; }
}
