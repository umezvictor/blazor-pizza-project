using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.JSInterop;

namespace BlazingPizza.Client;

//this class handles state manaqement for the order process
public class OrderState
{
    public bool ShowingConfigureDialog { get; private set; }
    public bool IsToppingPositionSelected { get; set; } = false;

    public Pizza? ConfiguringPizza { get; private set; }

    public Order Order { get; set; } = new Order();

    [JsonIgnore]
    public IJSRuntime JSRuntime { get; set; }

    public void ShowConfigurePizzaDialog(PizzaSpecial special)
    {

        ConfiguringPizza = new Pizza()
        {
            Special = special,
            SpecialId = special.Id,
            Size = Pizza.DefaultSize,
            Toppings = new List<PizzaTopping>(),
        };

        ShowingConfigureDialog = true;
    }

    public void CancelConfigurePizzaDialog()
    {
        ConfiguringPizza = null;

        ShowingConfigureDialog = false;
    }

    public async Task ConfirmConfigurePizzaDialog(bool isPositionSelected)
    {
        if (ConfiguringPizza is not null)
        {
            if (ConfiguringPizza.Toppings.Any() && isPositionSelected == false)
            {
                await ShowAlert(JSRuntime, "Please select a position for your topping");
                return;
            }
            Order.Pizzas.Add(ConfiguringPizza);
            ConfiguringPizza = null;
        }

        SaveStateToStorage(JSRuntime);
        ShowingConfigureDialog = false;
    }

    public void ResetOrder()
    {
        Order = new Order();
        SaveStateToStorage(JSRuntime);
    }

    public void RemoveConfiguredPizza(Pizza pizza)
    {
        Order.Pizzas.Remove(pizza);
    }

    public async Task GetStateFromLocalStorage(IJSRuntime jsRuntime)
    {

        var locallyStoredState = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "blazingPizza.orderState");
        var deserializedState =
                JsonSerializer.Deserialize<OrderState>(locallyStoredState, new JsonSerializerOptions { IncludeFields = true });

        Order = deserializedState.Order;
        ShowingConfigureDialog = deserializedState.ShowingConfigureDialog;
        ConfiguringPizza = deserializedState.ConfiguringPizza;

    }

    public async Task SaveStateToStorage(IJSRuntime jsRuntime)
    {

        var stateAsJson = JsonSerializer.Serialize(this, new JsonSerializerOptions { IncludeFields = true });
        await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "blazingPizza.orderState", stateAsJson);

    }

    public async Task ShowAlert(IJSRuntime jsRuntime, string message)
    {

        await jsRuntime.InvokeVoidAsync("alert", message);

    }

}