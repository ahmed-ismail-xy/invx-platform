using Invx.Invoicing.Domain.Enums;
using Invx.SharedKernel.Domain.Primitives.Entities;

namespace Invx.Invoicing.Domain.Entities;
public class InvoiceDeliveryAttempt : Entity
{
    public DeliveryMethod DeliveryMethod { get; private set; }
    public string DeliveryAddress { get; private set; }

    public DeliveryStatus Status { get; private set; }
    public DateTime AttemptedAt { get; private set; }
    public DateTime? DeliveredAt { get; private set; }

    public string FailureReason { get; private set; }
    public int RetryCount { get; private set; }
    public DateTime? NextRetryAt { get; private set; }

    public string MessageSubject { get; private set; }
    public Guid? TemplateUsed { get; private set; }

    public bool WasOpened { get; private set; }
    public DateTime? FirstOpenedAt { get; private set; }
    public DateTime? LastOpenedAt { get; private set; }
    public int OpenCount { get; private set; }

    private InvoiceDeliveryAttempt() { }

    public InvoiceDeliveryAttempt(
        DeliveryMethod deliveryMethod,
        string deliveryAddress,
        string messageSubject = null,
        Guid? templateUsed = null) : base(Guid.NewGuid())
    {
        DeliveryMethod = deliveryMethod;
        DeliveryAddress = deliveryAddress ?? throw new ArgumentNullException(nameof(deliveryAddress));
        MessageSubject = messageSubject;
        TemplateUsed = templateUsed;
        Status = DeliveryStatus.Pending;
        AttemptedAt = DateTime.UtcNow;
        RetryCount = 0;
        OpenCount = 0;
    }

    public void MarkAsSent()
    {
        Status = DeliveryStatus.Sent;
    }

    public void MarkAsDelivered()
    {
        Status = DeliveryStatus.Delivered;
        DeliveredAt = DateTime.UtcNow;
    }

    public void MarkAsFailed(string reason, DateTime? nextRetryAt = null)
    {
        Status = DeliveryStatus.Failed;
        FailureReason = reason;
        RetryCount++;
        NextRetryAt = nextRetryAt;
    }

    public void MarkAsBounced(string reason)
    {
        Status = DeliveryStatus.Bounced;
        FailureReason = reason;
    }

    public void RecordOpen()
    {
        WasOpened = true;
        OpenCount++;
        LastOpenedAt = DateTime.UtcNow;

        if (FirstOpenedAt == null)
            FirstOpenedAt = DateTime.UtcNow;
    }
}