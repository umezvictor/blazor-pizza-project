using System.Net.Http.Json;

public class HttpRepository : IRepository
{

    private readonly HttpClient _httpClient;

    public HttpRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<OrderWithStatus>> GetOrdersAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<OrderWithStatus>>("orders") ?? new();
    }

    public Task<List<OrderWithStatus>> GetOrdersAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<OrderWithStatus> GetOrderWithStatus(int orderId)
    {
        throw new NotImplementedException();
    }

    public Task<OrderWithStatus> GetOrderWithStatus(int orderId, string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<PizzaSpecial>> GetSpecials()
    {
        return await _httpClient.GetFromJsonAsync<List<PizzaSpecial>>("specials") ?? new();
    }

    public async Task<List<Topping>> GetToppings()
    {
        return await _httpClient.GetFromJsonAsync<List<Topping>>("toppings") ?? new();
    }

    public async Task<int> PlaceOrder(Order order)
    {
        var response = await _httpClient.PostAsJsonAsync("orders", order);
        var newOrderId = await response.Content.ReadFromJsonAsync<int>();
        return newOrderId;

    }

    public async Task SubscribeToNotifications(NotificationSubscription subscription)
    {
        var response = await _httpClient.PutAsJsonAsync("notifications/subscribe", subscription);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateOrderStatusAsync(UpdateOrderStatusDto request)
    {
        throw new NotImplementedException();
    }

}
