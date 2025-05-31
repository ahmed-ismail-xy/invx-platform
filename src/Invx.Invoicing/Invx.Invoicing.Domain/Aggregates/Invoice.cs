using Invx.Invoicing.Domain.Entities;
using Invx.Invoicing.Domain.Enums;
using Invx.Invoicing.Domain.Events;
using Invx.Invoicing.Domain.ValueObjects;
using Invx.SharedKernel.Domain.Primitives.Entities;
using Invx.SharedKernel.Domain.Primitives.Errors;
using Invx.SharedKernel.Domain.Primitives.Results;

namespace Invx.Invoicing.Domain.Aggregates;
public class Invoice : AggregateRoot
{
    public Guid TenantId { get; private set; }
    public InvoiceNumber InvoiceNumber { get; private set; }

    public BillingParty BillingParty { get; private set; }
    public InvoiceRecipient InvoiceRecipient { get; private set; }

    public InvoiceStatus Status { get; private set; }
    public ApprovalStatus ApprovalStatus { get; private set; }

    public DateTime IssueDate { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime? ServicePeriodStart { get; private set; }
    public DateTime? ServicePeriodEnd { get; private set; }

    public PaymentTerms PaymentTerms { get; private set; }

    public Money SubtotalMoney { get; private set; }
    public Money TotalTaxAmount { get; private set; }
    public Money TotalDiscountAmount { get; private set; }
    public Money TotalMoney { get; private set; }

    public decimal ExchangeRate { get; private set; }
    public string BaseCurrency { get; private set; }
    public string InvoiceCurrency { get; private set; }

    public int ViewCount { get; private set; }
    public DateTime? FirstViewedAt { get; private set; }
    public DateTime? LastViewedAt { get; private set; }

    public string LegalSequenceNumber { get; private set; }

    public InvoiceType InvoiceType { get; private set; }
    public Guid? ParentInvoiceId { get; private set; }
    public Guid? OriginalInvoiceId { get; private set; }

    public string PublicNotes { get; private set; }
    public string PrivateNotes { get; private set; }
    public string TermsAndConditions { get; private set; }

    public Guid? ApprovedBy { get; private set; }
    public DateTime? ApprovedAt { get; private set; }
    public string ApprovalNotes { get; private set; }

    public GenerationSource GenerationSource { get; private set; }
    public Guid? SourceEntityId { get; private set; }

    private readonly List<InvoiceLineItem> _lineItems = [];
    private readonly List<InvoiceWorkflowStep> _workflowSteps = [];
    private readonly List<InvoiceDeliveryAttempt> _deliveryAttempts = [];

    public IReadOnlyList<InvoiceLineItem> LineItems => _lineItems.AsReadOnly();
    public IReadOnlyList<InvoiceWorkflowStep> WorkflowSteps => _workflowSteps.AsReadOnly();
    public IReadOnlyList<InvoiceDeliveryAttempt> DeliveryAttempts => _deliveryAttempts.AsReadOnly();

    public bool IsOverdue => Status != InvoiceStatus.Paid && DateTime.UtcNow.Date > DueDate;
    public bool IsPaid => Status == InvoiceStatus.Paid;
    public bool IsVoided => Status == InvoiceStatus.Voided;
    public bool IsCancelled => Status == InvoiceStatus.Cancelled;
    public bool CanBeModified => Status == InvoiceStatus.Draft;
    public bool RequiresApproval => ApprovalStatus != ApprovalStatus.NotRequired;
    public bool IsApproved => ApprovalStatus == ApprovalStatus.Approved;

    private Invoice() { }

    private Invoice(
        Guid tenantId,
        InvoiceNumber invoiceNumber,
        BillingParty billingParty,
        InvoiceRecipient invoiceRecipient,
        DateTime issueDate,
        PaymentTerms paymentTerms,
        string currency = "USD",
        InvoiceType invoiceType = InvoiceType.Standard,
        GenerationSource generationSource = GenerationSource.Manual) : base(Guid.NewGuid())
    {
        TenantId = tenantId;
        InvoiceNumber = invoiceNumber;
        BillingParty = billingParty;
        InvoiceRecipient = invoiceRecipient;
        IssueDate = issueDate;
        PaymentTerms = paymentTerms ;

        Status = InvoiceStatus.Draft;
        ApprovalStatus = ApprovalStatus.NotRequired;
        InvoiceType = invoiceType;
        GenerationSource = generationSource;

        DueDate = paymentTerms.CalculateDueDate(issueDate);

        BaseCurrency = currency;
        InvoiceCurrency = currency;
        ExchangeRate = 1.0m;

        SubtotalMoney = Money.Zero(currency);
        TotalTaxAmount = Money.Zero(currency);
        TotalDiscountAmount = Money.Zero(currency);
        TotalMoney = Money.Zero(currency);
        ViewCount = 0;

        RaiseDomainEvent(new InvoiceCreatedDomainEvent(TenantId, Id, InvoiceNumber));
    }

    public static Result<Invoice> CreateDraft(
    Guid tenantId,
    InvoiceNumber invoiceNumber,
    BillingParty billingParty,
    InvoiceRecipient invoiceRecipient,
    DateTime issueDate,
    PaymentTerms paymentTerms,
    string currency = "USD",
    InvoiceType invoiceType = InvoiceType.Standard,
    GenerationSource generationSource = GenerationSource.Manual)
    {
        return new Invoice(
            tenantId,
            invoiceNumber,
            billingParty,
            invoiceRecipient,
            issueDate,
            paymentTerms,
            currency,
            invoiceType,
            generationSource);
    }

    public Result AddLineItem(InvoiceLineItem lineItem)
    {
        if (!CanBeModified)
            return DomainError.Create("INVX.Invoice.CannotModify", $"Cannot modify invoice in status: {Status}");

        _lineItems.Add(lineItem ?? throw new ArgumentNullException(nameof(lineItem)));
        RecalculateTotals();
        IncrementVersion();

        RaiseDomainEvent(new InvoiceLineItemAddedDomainEvent(TenantId, Id, lineItem.Id));

        return Result.Success();
    }

    public void RemoveLineItem(Guid lineItemId)
    {
        if (!CanBeModified)
            throw new InvalidOperationException($"Cannot modify invoice in status: {Status}");

        var lineItem = _lineItems.FirstOrDefault(li => li.Id == lineItemId);
        if (lineItem == null)
            throw new ArgumentException($"Line item not found: {lineItemId}");

        _lineItems.Remove(lineItem);
        RecalculateTotals();
    }

    public void UpdateLineItem(Guid lineItemId, Action<InvoiceLineItem> updateAction)
    {
        if (!CanBeModified)
            throw new InvalidOperationException($"Cannot modify invoice in status: {Status}");

        var lineItem = _lineItems.FirstOrDefault(li => li.Id == lineItemId);
        if (lineItem == null)
            throw new ArgumentException($"Line item not found: {lineItemId}");

        updateAction(lineItem);
        RecalculateTotals();
    }

    private void RecalculateTotals()
    {
        SubtotalMoney = _lineItems.Aggregate(Money.Zero(InvoiceCurrency), (sum, item) => sum.Add(item.ExtendedPrice));

        TotalDiscountAmount = _lineItems
            .Where(item => item.LineDiscount != null)
            .Aggregate(Money.Zero(InvoiceCurrency), (sum, item) => sum.Add(item.LineDiscount.DiscountAmount));

        TotalTaxAmount = _lineItems
            .Where(item => item.Tax != null)
            .Aggregate(Money.Zero(InvoiceCurrency), (sum, item) => sum.Add(item.Tax.TaxAmount));

        TotalMoney = SubtotalMoney
            .Subtract(TotalDiscountAmount)
            .Add(TotalTaxAmount);
    }

    public void SetCurrency(string currency, decimal exchangeRate = 1.0m)
    {
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty", nameof(currency));
        if (exchangeRate <= 0)
            throw new ArgumentException("Exchange rate must be positive", nameof(exchangeRate));
        
        BaseCurrency = currency.ToUpperInvariant();
        InvoiceCurrency = currency.ToUpperInvariant();
        ExchangeRate = exchangeRate;
        RecalculateTotals();
    }

    public void SetDueDate(DateTime dueDate)
    {
        if (dueDate < IssueDate)
            throw new ArgumentException("Due date cannot be before issue date", nameof(dueDate));
        DueDate = dueDate;
    }

    public void SetServicePeriod(DateTime? start, DateTime? end)
    {
        if (start.HasValue && end.HasValue && start > end)
            throw new ArgumentException("Service period start cannot be after end", nameof(start));

        ServicePeriodStart = start;
        ServicePeriodEnd = end;
    }

    public void SetPaymentTerms(PaymentTerms terms)
    {
        PaymentTerms = terms ?? throw new ArgumentNullException(nameof(terms));
        DueDate = PaymentTerms.CalculateDueDate(IssueDate);
    }

    public void SetBillingParty(BillingParty billingParty)
    {
        BillingParty = billingParty ?? throw new ArgumentNullException(nameof(billingParty));
    }

    public void SetInvoiceRecipient(InvoiceRecipient recipient)
    {
        InvoiceRecipient = recipient ?? throw new ArgumentNullException(nameof(recipient));
    }

    public void SetLegalSequenceNumber(string sequenceNumber)
    {
        LegalSequenceNumber = sequenceNumber ?? throw new ArgumentNullException(nameof(sequenceNumber));
    }

    public void SetParentInvoice(Guid parentInvoiceId)
    {
        ParentInvoiceId = parentInvoiceId;

        RaiseDomainEvent(new InvoiceParentUpdatedDomainEvent(TenantId, Id, parentInvoiceId));
    }

    public void SetOriginalInvoice(Guid originalInvoiceId)
    {
        OriginalInvoiceId = originalInvoiceId;
        RaiseDomainEvent(new InvoiceOriginalUpdatedDomainEvent(TenantId, Id, originalInvoiceId));
    }

    public void SetPublicNotes(string notes)
    {
        PublicNotes = notes;
    }

    public void SetPrivateNotes(string notes)
    {
        PrivateNotes = notes;
    }

    public void SetTermsAndConditions(string terms)
    {
        TermsAndConditions = terms;
    }

    public void SetApprovalStatus(
        ApprovalStatus status,
        Guid? approvedBy = null,
        string notes = null)
    {
        if (status == ApprovalStatus.Approved && approvedBy == null)
            throw new ArgumentException("Approved by must be provided for approved status", nameof(approvedBy));
        ApprovalStatus = status;
        ApprovedBy = approvedBy;
        ApprovedAt = status == ApprovalStatus.Approved ? DateTime.UtcNow : null;
        ApprovalNotes = notes;
    }

    public void AddWorkflowStep(InvoiceWorkflowStep step)
    {
        ArgumentNullException.ThrowIfNull(step);
        if (!CanBeModified) throw new InvalidOperationException($"Cannot modify invoice in status: {Status}");
        
        _workflowSteps.Add(step);
    }

    public void RemoveWorkflowStep(Guid stepId)
    {
        if (!CanBeModified) throw new InvalidOperationException($"Cannot modify invoice in status: {Status}");

        var step = _workflowSteps.FirstOrDefault(s => s.Id == stepId);
        if (step == null) throw new ArgumentException($"Workflow step not found: {stepId}");
        _workflowSteps.Remove(step);
    }

    public void UpdateWorkflowStep(Guid stepId, Action<InvoiceWorkflowStep> updateAction)
    {
        if (!CanBeModified) throw new InvalidOperationException($"Cannot modify invoice in status: {Status}");
        var step = _workflowSteps.FirstOrDefault(s => s.Id == stepId);
        if (step == null) throw new ArgumentException($"Workflow step not found: {stepId}");
        updateAction(step);
    }

    public void AddDeliveryAttempt(InvoiceDeliveryAttempt attempt)
    {
        if (!CanBeModified) throw new InvalidOperationException($"Cannot modify invoice in status: {Status}");
        _deliveryAttempts.Add(attempt);
    }

    public void RemoveDeliveryAttempt(Guid attemptId)
    {
        if (!CanBeModified) throw new InvalidOperationException($"Cannot modify invoice in status: {Status}");
        var attempt = _deliveryAttempts.FirstOrDefault(a => a.Id == attemptId);
        if (attempt == null) throw new ArgumentException($"Delivery attempt not found: {attemptId}");
        _deliveryAttempts.Remove(attempt);
    }

    public void UpdateDeliveryAttempt(Guid attemptId, Action<InvoiceDeliveryAttempt> updateAction)
    {
        if (!CanBeModified) throw new InvalidOperationException($"Cannot modify invoice in status: {Status}");
        var attempt = _deliveryAttempts.FirstOrDefault(a => a.Id == attemptId);
        if (attempt == null) throw new ArgumentException($"Delivery attempt not found: {attemptId}");
        updateAction(attempt);
    }

    public void MarkAsViewed()
    {
        ViewCount++;
    }

    public void Approve(Guid approvedBy, string notes = null)
    {
        if (ApprovalStatus == ApprovalStatus.Approved)
            throw new InvalidOperationException("Invoice is already approved");

        SetApprovalStatus(ApprovalStatus.Approved, approvedBy, notes);
        ApprovedAt = DateTime.UtcNow;
    }

    public void Reject(Guid rejectedBy, string notes = null)
    {
        if (ApprovalStatus == ApprovalStatus.Rejected)
            throw new InvalidOperationException("Invoice is already rejected");

        SetApprovalStatus(ApprovalStatus.Rejected, rejectedBy, notes);
        ApprovedAt = DateTime.UtcNow;
    }

    public void Void(Guid voidedBy, string notes = null)
    {
        if (Status == InvoiceStatus.Voided)
            throw new InvalidOperationException("Invoice is already voided");

        Status = InvoiceStatus.Voided;
    }

    public void Cancel(Guid cancelledBy, string notes = null)
    {
        if (Status == InvoiceStatus.Cancelled)
            throw new InvalidOperationException("Invoice is already cancelled");

        Status = InvoiceStatus.Cancelled;
    }

    public void MarkAsPaid(DateTime paidAt, string paymentReference = null)
    {
        if (Status == InvoiceStatus.Paid)
            throw new InvalidOperationException("Invoice is already paid");

        Status = InvoiceStatus.Paid;
    }

    public void MarkAsDraft()
    {
        if (Status == InvoiceStatus.Draft)
            throw new InvalidOperationException("Invoice is already in draft status");
        Status = InvoiceStatus.Draft;
    }

    public void MarkAsSent(
        Guid sentBy,
        DateTime sentAt,
        string deliveryMethod,
        string deliveryAddress)
    {
        if (Status == InvoiceStatus.Sent)
            throw new InvalidOperationException("Invoice is already sent");

        Status = InvoiceStatus.Sent;
    }

    public void SetIssueDate(DateTime issueDate)
    {
        if (issueDate > DateTime.UtcNow)
            throw new ArgumentException("Issue date cannot be in the future", nameof(issueDate));
        IssueDate = issueDate;
        DueDate = PaymentTerms.CalculateDueDate(issueDate);
    }

    public void SetExchangeRate(decimal exchangeRate)
    {
        if (exchangeRate <= 0)
            throw new ArgumentException("Exchange rate must be positive", nameof(exchangeRate));
        ExchangeRate = exchangeRate;
    }

    public override string ToString()
    {
        return $"Invoice {InvoiceNumber} - Status: {Status}, Total: {TotalMoney}";
    }

    public override bool Equals(object obj)
    {
        if (obj is Invoice other)
            return Id == other.Id && TenantId == other.TenantId;
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, TenantId);
    }

    public static bool operator ==(Invoice left, Invoice right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Invoice left, Invoice right)
    {
        return !(left == right);
    }
}