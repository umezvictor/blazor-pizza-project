namespace BlazingPizza.Shared
{
    public class UpdateOrderStatusDto
    {
        public required string Status { get; set; }
        public int OrderId { get; set; }
    }
}
