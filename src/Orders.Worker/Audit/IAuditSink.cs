namespace Orders.Worker.Audit;

public interface IAuditSink
{
    Task WriteAsync(AuditEvent auditEvent, CancellationToken cancellationToken);
}