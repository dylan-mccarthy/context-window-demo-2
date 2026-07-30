using Orders.Worker.Audit;

namespace Orders.Worker.Handlers;

public sealed class PaymentRejectedHandler
{
    public Task HandleAsync(
        string orderId,
        string paymentId,
        string reason,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        LegacyAudit.Write("payment.rejected", orderId, new { paymentId, reason });
        return Task.CompletedTask;
    }
}