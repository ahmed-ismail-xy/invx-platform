namespace Invx.Invoicing.Domain.Enums;
public enum InvoiceStatus
{
    Draft,

    PendingApproval,

    Approved,

    Sent,

    Viewed,

    PartiallyPaid,

    Paid,

    Overdue,

    Cancelled,

    Voided
}