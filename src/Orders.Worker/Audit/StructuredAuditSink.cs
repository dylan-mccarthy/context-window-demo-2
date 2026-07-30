using System.Text.Json;

namespace Orders.Worker.Audit;

public sealed class StructuredAuditSink(ILogger<StructuredAuditSink> logger) : IAuditSink
{
    public Task WriteAsync(AuditEvent auditEvent, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        logger.LogInformation(
            "Audit {EventName} for {CorrelationId}: {Payload}",
            auditEvent.EventName,
            auditEvent.CorrelationId,
            JsonSerializer.Serialize(auditEvent.Payload));

        return Task.CompletedTask;
    }
}