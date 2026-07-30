using Orders.Worker.Audit;

namespace Orders.Worker.Handlers;

public sealed class RefundCompletedHandler
{
    public Task HandleAsync(
        string orderId,
        string refundId,
        decimal amount,
        string providerReference,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        LegacyAudit.Write("refund.completed", orderId, new { refundId, amount, providerReference });
        LegacyAudit.Write("payment.refunded", orderId, new { refundId, amount });
        return Task.CompletedTask;
    }
}