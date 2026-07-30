using Orders.Worker.Audit;

namespace Orders.Worker.Handlers;

public sealed class OrderPackedHandler
{
    public Task HandleAsync(
        string orderId,
        string packageId,
        int itemCount,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        LegacyAudit.Write("order.packed", orderId, new { packageId, itemCount });
        LegacyAudit.Write("package.labelled", orderId, new { packageId });
        return Task.CompletedTask;
    }
}