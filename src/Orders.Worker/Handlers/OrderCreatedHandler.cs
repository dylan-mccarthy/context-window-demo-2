using Orders.Worker.Audit;

namespace Orders.Worker.Handlers;

public sealed class OrderCreatedHandler
{
    public Task HandleAsync(
        string orderId,
        string customerId,
        string salesChannel,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        LegacyAudit.Write("order.created", orderId, new { customerId, salesChannel });
        LegacyAudit.Write("order.received", orderId, new { customerId });
        return Task.CompletedTask;
    }
}