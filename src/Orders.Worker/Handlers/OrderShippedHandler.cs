using Orders.Worker.Audit;

namespace Orders.Worker.Handlers;

public sealed class OrderShippedHandler
{
    public Task HandleAsync(
        string orderId,
        string shipmentId,
        string carrier,
        string trackingNumber,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        LegacyAudit.Write("order.shipped", orderId, new { shipmentId, carrier });
        LegacyAudit.Write("shipment.tracking-assigned", orderId, new { shipmentId, trackingNumber });
        return Task.CompletedTask;
    }
}