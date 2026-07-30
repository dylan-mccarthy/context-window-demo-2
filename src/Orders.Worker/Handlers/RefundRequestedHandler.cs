using Orders.Worker.Audit;

namespace Orders.Worker.Handlers;

public sealed class RefundRequestedHandler
{
    public Task HandleAsync(
        string orderId,
        string refundId,
        decimal amount,
        string reason,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        LegacyAudit.Write("refund.requested", orderId, new { refundId, amount, reason });
        return Task.CompletedTask;
    }
}