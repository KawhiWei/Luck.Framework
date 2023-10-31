namespace Luck.UnitTest.Pipeline;

public class TrainPaySuccessContext : ContextBase<OrderInfo, Response>
{

    public Domain Domain { get; set; } = default!;
}