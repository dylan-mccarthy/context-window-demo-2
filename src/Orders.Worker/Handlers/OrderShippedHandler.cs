using Orders.Worker.Audit;

namespace Orders.Worker.Handlers;

public sealed class OrderShippedHandler(IAuditSink auditSink)
{
    public async Task HandleAsync(
        string orderId,
        string shipmentId,
        string carrier,
        string trackingNumber,
        CancellationToken cancellationToken)
    {
        await auditSink.WriteAsync(
            new AuditEvent("order.shipped", orderId, new { shipmentId, carrier }),
            cancellationToken);
        await auditSink.WriteAsync(
            new AuditEvent("shipment.tracking-assigned", orderId, new { shipmentId, trackingNumber }),
            cancellationToken);
    }
}