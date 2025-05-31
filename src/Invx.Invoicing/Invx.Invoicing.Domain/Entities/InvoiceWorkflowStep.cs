using Invx.Invoicing.Domain.Enums;
using Invx.SharedKernel.Domain.Primitives.Entities;

namespace Invx.Invoicing.Domain.Entities;
public class InvoiceWorkflowStep : Entity
{
    public string StepName { get; private set; }
    public int StepOrder { get; private set; }
    public bool IsRequired { get; private set; }

    public WorkflowStepStatus Status { get; private set; }

    public Guid? AssignedTo { get; private set; }
    public Guid? AssignedBy { get; private set; }
    public DateTime? AssignedAt { get; private set; }

    public DateTime? CompletedAt { get; private set; }
    public Guid? CompletedBy { get; private set; }
    public WorkflowResolution? Resolution { get; private set; }
    public string ResolutionNotes { get; private set; }

    public DateTime? DueDate { get; private set; }
    public bool IsOverdue => DueDate.HasValue && DateTime.UtcNow > DueDate.Value && Status == WorkflowStepStatus.Pending;
    public int EscalationLevel { get; private set; }

    public Guid? DelegatedTo { get; private set; }
    public string DelegationReason { get; private set; }
    public DateTime? DelegatedAt { get; private set; }

    private InvoiceWorkflowStep() { }

    public InvoiceWorkflowStep(
        string stepName,
        int stepOrder,
        bool isRequired,
        Guid? assignedTo = null,
        DateTime? dueDate = null) : base(Guid.NewGuid())
    {
        StepName = stepName ?? throw new ArgumentNullException(nameof(stepName));
        StepOrder = stepOrder;
        IsRequired = isRequired;
        Status = WorkflowStepStatus.Pending;
        AssignedTo = assignedTo;
        DueDate = dueDate;
        EscalationLevel = 0;
    }

    public void Assign(Guid userId, Guid assignedBy)
    {
        if (Status != WorkflowStepStatus.Pending)
            throw new InvalidOperationException($"Cannot assign step in status: {Status}");

        AssignedTo = userId;
        AssignedBy = assignedBy;
        AssignedAt = DateTime.UtcNow;
        Status = WorkflowStepStatus.InProgress;
    }

    public void Complete(Guid completedBy, WorkflowResolution resolution, string notes = null)
    {
        if (Status != WorkflowStepStatus.InProgress)
            throw new InvalidOperationException($"Cannot complete step in status: {Status}");

        CompletedAt = DateTime.UtcNow;
        CompletedBy = completedBy;
        Resolution = resolution;
        ResolutionNotes = notes;
        Status = resolution == WorkflowResolution.Approved
            ? WorkflowStepStatus.Approved
            : WorkflowStepStatus.Rejected;
    }

    public void Delegate(Guid delegatedTo, string reason, Guid delegatedBy)
    {
        if (Status != WorkflowStepStatus.InProgress)
            throw new InvalidOperationException($"Cannot delegate step in status: {Status}");

        DelegatedTo = delegatedTo;
        DelegationReason = reason;
        DelegatedAt = DateTime.UtcNow;
        AssignedTo = delegatedTo;
        AssignedBy = delegatedBy;
    }

    public void Escalate()
    {
        EscalationLevel++;
    }

    public void Skip(Guid skippedBy, string reason)
    {
        if (IsRequired)
            throw new InvalidOperationException("Cannot skip required workflow step");

        Status = WorkflowStepStatus.Skipped;
        CompletedBy = skippedBy;
        CompletedAt = DateTime.UtcNow;
        ResolutionNotes = reason;
    }
}