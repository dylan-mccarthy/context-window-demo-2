namespace Orders.Worker.Audit;

public sealed record AuditEvent(
    string EventName,
    string CorrelationId,
    object Payload);