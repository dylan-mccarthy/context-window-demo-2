using System.Text.Json;
using Orders.Worker.Audit;
using Orders.Worker.Handlers;

namespace Orders.Worker.Tests.Handlers;

public sealed class HandlerAuditTests
{
    [Fact]
    public async Task Handlers_WriteExpectedLegacyAuditEvents()
    {
        var events = new List<AuditEvent>();
        using var capture = LegacyAudit.Capture(events);
        var auditSink = new RecordingAuditSink(events);
        var cancellationToken = CancellationToken.None;

        await new OrderCreatedHandler(auditSink).HandleAsync("order-1", "customer-1", "web", cancellationToken);
        await new PaymentAcceptedHandler(auditSink).HandleAsync("order-1", "payment-1", 125.50m, cancellationToken);
        await new PaymentRejectedHandler().HandleAsync("order-2", "payment-2", "declined", cancellationToken);
        await new OrderPackedHandler().HandleAsync("order-1", "package-1", 3, cancellationToken);
        await new OrderShippedHandler().HandleAsync("order-1", "shipment-1", "ParcelCo", "TRACK-1", cancellationToken);
        await new OrderCancelledHandler().HandleAsync("order-2", "customer request", "customer-2", cancellationToken);
        await new RefundRequestedHandler().HandleAsync("order-2", "refund-1", 25m, "damaged", cancellationToken);
        await new RefundCompletedHandler().HandleAsync("order-2", "refund-1", 25m, "provider-1", cancellationToken);

        Assert.Collection(
            events,
            auditEvent => AssertEvent(auditEvent, "order.created", "customerId", "customer-1"),
            auditEvent => AssertEvent(auditEvent, "order.received", "customerId", "customer-1"),
            auditEvent => AssertEvent(auditEvent, "payment.accepted", "paymentId", "payment-1"),
            auditEvent => AssertEvent(auditEvent, "order.paid", "paymentId", "payment-1"),
            auditEvent => AssertEvent(auditEvent, "payment.rejected", "reason", "declined"),
            auditEvent => AssertEvent(auditEvent, "order.packed", "itemCount", "3"),
            auditEvent => AssertEvent(auditEvent, "package.labelled", "packageId", "package-1"),
            auditEvent => AssertEvent(auditEvent, "order.shipped", "carrier", "ParcelCo"),
            auditEvent => AssertEvent(auditEvent, "shipment.tracking-assigned", "trackingNumber", "TRACK-1"),
            auditEvent => AssertEvent(auditEvent, "order.cancelled", "cancelledBy", "customer-2"),
            auditEvent => AssertEvent(auditEvent, "fulfilment.stopped", "reason", "customer request"),
            auditEvent => AssertEvent(auditEvent, "refund.requested", "reason", "damaged"),
            auditEvent => AssertEvent(auditEvent, "refund.completed", "providerReference", "provider-1"),
            auditEvent => AssertEvent(auditEvent, "payment.refunded", "amount", "25"));
    }

    private static void AssertEvent(AuditEvent auditEvent, string eventName, string field, string value)
    {
        Assert.Equal(eventName, auditEvent.EventName);
        var payload = JsonSerializer.SerializeToElement(auditEvent.Payload);
        Assert.Equal(value, payload.GetProperty(field).ToString());
    }

    private sealed class RecordingAuditSink(ICollection<AuditEvent> events) : IAuditSink
    {
        public Task WriteAsync(AuditEvent auditEvent, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            events.Add(auditEvent);
            return Task.CompletedTask;
        }
    }
}