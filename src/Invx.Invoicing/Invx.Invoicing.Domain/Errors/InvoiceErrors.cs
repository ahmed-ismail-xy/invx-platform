using Invx.Invoicing.Domain.Errors.Resources;
using Invx.SharedKernel.Domain.Primitives.Errors;

namespace Invx.Invoicing.Domain.Errors;
public static class InvoiceErrors
{
    #region Invoice Creation & Validation

    public static readonly DomainError InvoiceNumberRequired = DomainError.BusinessRuleViolation(
        "InvoiceNumberRequired",
        InvoicingResource.INVX_Invoice_InvoiceNumberRequired,
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Invoice must have a unique business identifier",
            ["Field"] = "InvoiceNumber"
        });

    public static readonly DomainError InvoiceNumberAlreadyExists = DomainError.BusinessRuleViolation(
        "InvoiceNumberAlreadyExists",
        "An invoice with this number already exists for the tenant.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Invoice numbers must be unique within tenant",
            ["Field"] = "InvoiceNumber"
        });

    public static readonly DomainError BillingPartyRequired = DomainError.BusinessRuleViolation(
        "BillingPartyRequired",
        "Billing party information is required for invoice creation.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Invoice must have valid billing party",
            ["Field"] = "BillingParty"
        });

    public static readonly DomainError InvoiceRecipientRequired = DomainError.BusinessRuleViolation(
        "InvoiceRecipientRequired",
        "Invoice recipient information is required for invoice creation.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Invoice must have valid recipient",
            ["Field"] = "InvoiceRecipient"
        });

    #endregion

    #region Invoice Status & Lifecycle

    public static readonly DomainError InvalidStatusTransition = DomainError.BusinessRuleViolation(
        "InvalidStatusTransition",
        "The requested status transition is not allowed from the current status.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Invoice status transitions must follow valid state machine",
            ["Field"] = "Status"
        });

    public static readonly DomainError CannotModifyNonDraftInvoice = DomainError.BusinessRuleViolation(
        "CannotModifyNonDraftInvoice",
        "Invoice can only be modified when in DRAFT status.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Only draft invoices can be modified",
            ["AllowedStatus"] = "DRAFT"
        });

    public static readonly DomainError CannotCancelPaidInvoice = DomainError.BusinessRuleViolation(
        "CannotCancelPaidInvoice",
        "Cannot cancel an invoice that has been paid.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Paid invoices cannot be cancelled",
            ["RequiredAction"] = "Create credit note instead"
        });

    public static readonly DomainError CannotSendUnapprovedInvoice = DomainError.BusinessRuleViolation(
        "CannotSendUnapprovedInvoice",
        "Invoice must be approved before it can be sent to customer.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Invoice approval required before sending",
            ["RequiredStatus"] = "APPROVED"
        });

    #endregion

    #region Date Validation

    public static readonly DomainError IssueDateRequired = DomainError.BusinessRuleViolation(
        "IssueDateRequired",
        "Issue date is required for invoice creation.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Invoice must have valid issue date",
            ["Field"] = "IssueDate"
        });

    public static readonly DomainError DueDateMustBeAfterIssueDate = DomainError.BusinessRuleViolation(
        "DueDateMustBeAfterIssueDate",
        "Due date must be after or equal to the issue date.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Due date cannot be before issue date",
            ["Fields"] = new[] { "IssueDate", "DueDate" }
        });

    public static readonly DomainError ServicePeriodInvalid = DomainError.BusinessRuleViolation(
        "ServicePeriodInvalid",
        "Service period end date must be after or equal to start date.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Valid service period required",
            ["Fields"] = new[] { "ServicePeriodStart", "ServicePeriodEnd" }
        });

    public static readonly DomainError IssueDateCannotBeInFuture = DomainError.BusinessRuleViolation(
        "IssueDateCannotBeInFuture",
        "Issue date cannot be in the future.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Issue date must be current or past date",
            ["Field"] = "IssueDate"
        });

    #endregion

    #region Financial Validation

    public static readonly DomainError InvoiceTotalMustBePositive = DomainError.BusinessRuleViolation(
        "InvoiceTotalMustBePositive",
        "Invoice total must be greater than zero.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Invoice must have positive total amount",
            ["Field"] = "TotalMoney"
        });

    public static readonly DomainError CurrencyMismatch = DomainError.BusinessRuleViolation(
        "CurrencyMismatch",
        "All monetary amounts must use the same currency as the invoice currency.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Consistent currency across all amounts",
            ["Field"] = "Currency"
        });

    public static readonly DomainError InvalidExchangeRate = DomainError.BusinessRuleViolation(
        "InvalidExchangeRate",
        "Exchange rate must be greater than zero for multi-currency invoices.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Valid exchange rate required for currency conversion",
            ["Field"] = "ExchangeRate"
        });

    public static readonly DomainError TaxCalculationMismatch = DomainError.BusinessRuleViolation(
        "TaxCalculationMismatch",
        "Calculated tax amounts do not match the sum of line item taxes.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Tax calculations must be consistent",
            ["Field"] = "TaxBreakdown"
        });

    #endregion

    #region Line Items

    public static readonly DomainError InvoiceMustHaveLineItems = DomainError.BusinessRuleViolation(
        "InvoiceMustHaveLineItems",
        "Invoice must contain at least one line item.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Minimum one line item required",
            ["Field"] = "LineItems"
        });

    public static readonly DomainError LineItemQuantityMustBePositive = DomainError.BusinessRuleViolation(
        "LineItemQuantityMustBePositive",
        "Line item quantity must be greater than zero.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Line item quantities must be positive",
            ["Field"] = "Quantity"
        });

    public static readonly DomainError LineItemDescriptionRequired = DomainError.BusinessRuleViolation(
        "LineItemDescriptionRequired",
        "Line item must have a description.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Line item description is mandatory",
            ["Field"] = "Description"
        });

    public static readonly DomainError LineItemUnitPriceMustBePositive = DomainError.BusinessRuleViolation(
        "LineItemUnitPriceMustBePositive",
        "Line item unit price must be greater than or equal to zero.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Unit price cannot be negative",
            ["Field"] = "UnitPrice"
        });

    public static readonly DomainError DuplicateLineItemNumbers = DomainError.BusinessRuleViolation(
        "DuplicateLineItemNumbers",
        "Line item numbers must be unique within the invoice.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Unique line numbers required",
            ["Field"] = "LineNumber"
        });

    #endregion

    #region Approval Workflow

    public static readonly DomainError ApprovalRequired = DomainError.BusinessRuleViolation(
        "ApprovalRequired",
        "Invoice requires approval before proceeding to the next status.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Approval workflow must be completed",
            ["Field"] = "ApprovalStatus"
        });

    public static readonly DomainError InsufficientApprovalAuthority = DomainError.BusinessRuleViolation(
        "InsufficientApprovalAuthority",
        "User does not have sufficient authority to approve this invoice.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Approval authority validation required",
            ["Field"] = "ApprovedBy"
        });

    public static readonly DomainError CannotApproveOwnInvoice = DomainError.BusinessRuleViolation(
        "CannotApproveOwnInvoice",
        "User cannot approve their own created invoice.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Self-approval not allowed",
            ["Field"] = "ApprovedBy"
        });

    public static readonly DomainError WorkflowStepOutOfOrder = DomainError.BusinessRuleViolation(
        "WorkflowStepOutOfOrder",
        "Workflow steps must be completed in the specified order.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Sequential workflow step execution required",
            ["Field"] = "WorkflowSteps"
        });

    #endregion

    #region Credit Notes

    public static readonly DomainError CreditNoteExceedsOriginalAmount = DomainError.BusinessRuleViolation(
        "CreditNoteExceedsOriginalAmount",
        "Credit note amount cannot exceed the original invoice amount.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Credit cannot exceed original invoice",
            ["Field"] = "CreditedAmount"
        });

    public static readonly DomainError CannotCreditDraftInvoice = DomainError.BusinessRuleViolation(
        "CannotCreditDraftInvoice",
        "Cannot create credit note for draft invoice.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Credit notes only for issued invoices",
            ["RequiredStatus"] = "SENT|PAID|OVERDUE"
        });

    public static readonly DomainError CreditNoteReasonRequired = DomainError.BusinessRuleViolation(
        "CreditNoteReasonRequired",
        "Credit note must have a specified reason.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Credit note reason is mandatory",
            ["Field"] = "CreditReason"
        });

    #endregion

    #region Payment Allocation

    public static readonly DomainError PaymentAllocationExceedsInvoiceTotal = DomainError.BusinessRuleViolation(
        "PaymentAllocationExceedsInvoiceTotal",
        "Payment allocation amount cannot exceed the invoice total.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Payment allocation cannot exceed invoice amount",
            ["Field"] = "AllocatedAmount"
        });

    public static readonly DomainError CannotAllocateToVoidedInvoice = DomainError.BusinessRuleViolation(
        "CannotAllocateToVoidedInvoice",
        "Cannot allocate payment to a voided invoice.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "No payments on voided invoices",
            ["Field"] = "InvoiceStatus"
        });

    public static readonly DomainError NegativePaymentAllocation = DomainError.BusinessRuleViolation(
        "NegativePaymentAllocation",
        "Payment allocation amount must be positive.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Positive payment allocations only",
            ["Field"] = "AllocatedAmount"
        });

    #endregion

    #region Digital Signature & Legal

    public static readonly DomainError DigitalSignatureRequired = DomainError.BusinessRuleViolation(
        "DigitalSignatureRequired",
        "Invoice requires digital signature before it can be finalized.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Digital signature mandatory for legal compliance",
            ["Field"] = "DigitalSignatureHash"
        });

    public static readonly DomainError InvalidDigitalSignature = DomainError.BusinessRuleViolation(
        "InvalidDigitalSignature",
        "Digital signature is invalid or corrupted.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Valid digital signature required",
            ["Field"] = "DigitalSignatureHash"
        });

    public static readonly DomainError LegalSequenceNumberRequired = DomainError.BusinessRuleViolation(
        "LegalSequenceNumberRequired",
        "Legal sequence number is required for regulatory compliance.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Legal sequence number mandatory",
            ["Field"] = "LegalSequenceNumber"
        });

    #endregion

    #region Tax Compliance

    public static readonly DomainError TaxJurisdictionRequired = DomainError.BusinessRuleViolation(
        "TaxJurisdictionRequired",
        "Tax jurisdiction must be specified for taxable invoices.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Tax jurisdiction required for compliance",
            ["Field"] = "TaxJurisdiction"
        });

    public static readonly DomainError InvalidTaxRate = DomainError.BusinessRuleViolation(
        "InvalidTaxRate",
        "Tax rate must be between 0 and 100 percent.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Valid tax rate range required",
            ["Field"] = "TaxRate",
            ["ValidRange"] = "0-100"
        });

    public static readonly DomainError TaxExemptionCertificateRequired = DomainError.BusinessRuleViolation(
        "TaxExemptionCertificateRequired",
        "Tax exemption certificate number is required for exempt invoices.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Tax exemption requires valid certificate",
            ["Field"] = "ExemptionCertificateNumber"
        });

    #endregion

    #region Multi-tenancy

    public static readonly DomainError TenantIdRequired = DomainError.BusinessRuleViolation(
        "TenantIdRequired",
        "Tenant ID is required for multi-tenant isolation.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Multi-tenant isolation required",
            ["Field"] = "TenantId"
        });

    public static readonly DomainError CrossTenantAccessDenied = DomainError.BusinessRuleViolation(
        "CrossTenantAccessDenied",
        "Cannot access invoice from different tenant.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Tenant data isolation enforced",
            ["Field"] = "TenantId"
        });

    #endregion

    #region Delivery & Communication

    public static readonly DomainError DeliveryAddressRequired = DomainError.BusinessRuleViolation(
        "DeliveryAddressRequired",
        "Delivery address is required for the selected delivery method.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Valid delivery address required",
            ["Field"] = "DeliveryAddress"
        });

    public static readonly DomainError InvalidDeliveryMethod = DomainError.BusinessRuleViolation(
        "InvalidDeliveryMethod",
        "Selected delivery method is not supported or configured.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Supported delivery method required",
            ["Field"] = "DeliveryMethod"
        });

    public static readonly DomainError MaxRetryAttemptsExceeded = DomainError.BusinessRuleViolation(
        "MaxRetryAttemptsExceeded",
        "Maximum delivery retry attempts have been exceeded.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Delivery retry limit enforced",
            ["Field"] = "RetryCount"
        });

    #endregion

    #region Business Operations

    public static readonly DomainError RecurringInvoicePatternRequired = DomainError.BusinessRuleViolation(
        "RecurringInvoicePatternRequired",
        "Recurring invoices must have a valid recurrence pattern.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Valid recurrence pattern required",
            ["Field"] = "RecurrencePattern"
        });

    public static readonly DomainError ProjectIdRequiredForProjectInvoice = DomainError.BusinessRuleViolation(
        "ProjectIdRequiredForProjectInvoice",
        "Project ID is required for project-based invoices.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Project reference required for project invoices",
            ["Field"] = "ProjectId"
        });

    public static readonly DomainError BillableHoursExceedAvailable = DomainError.BusinessRuleViolation(
        "BillableHoursExceedAvailable",
        "Billable hours cannot exceed available hours for the service period.",
        new Dictionary<string, object>
        {
            ["BusinessRule"] = "Billable hours must not exceed available",
            ["Field"] = "BillableHours"
        });

    #endregion

    #region Invalid Operations

    public static readonly DomainError CannotDeleteInvoiceWithPayments = DomainError.InvalidOperation(
        "DeleteInvoiceWithPayments",
        "Cannot delete invoice that has associated payments.",
        new Dictionary<string, object>
        {
            ["Reason"] = "Financial integrity protection",
            ["RequiredAction"] = "Void invoice instead of deleting"
        });

    public static readonly DomainError CannotModifyApprovedInvoice = DomainError.InvalidOperation(
        "ModifyApprovedInvoice",
        "Cannot modify invoice after it has been approved.",
        new Dictionary<string, object>
        {
            ["Reason"] = "Approval workflow integrity",
            ["RequiredAction"] = "Create new invoice or credit note"
        });

    public static readonly DomainError CannotChangeInvoiceType = DomainError.InvalidOperation(
        "ChangeInvoiceType",
        "Cannot change invoice type after creation.",
        new Dictionary<string, object>
        {
            ["Reason"] = "Business process integrity",
            ["RequiredAction"] = "Create new invoice with correct type"
        });

    #endregion
}
