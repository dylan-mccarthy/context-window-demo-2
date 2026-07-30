using Orders.Worker.Audit;

namespace Orders.Worker.Handlers;

public sealed class OrderCancelledHandler
{
    public Task HandleAsync(
        string orderId,
        string reason,
        string cancelledBy,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        LegacyAudit.Write("order.cancelled", orderId, new { reason, cancelledBy });
        LegacyAudit.Write("fulfilment.stopped", orderId, new { reason });
        return Task.CompletedTask;
    }
}