using Invx.Invoicing.Domain.Enums;
using Invx.Invoicing.Domain.ValueObjects;
using Invx.SharedKernel.Domain.Primitives.Entities;

namespace Invx.Invoicing.Domain.Entities;
public sealed class InvoiceLineItem : Entity
{
    public string LineNumber { get; private set; }
    public int SortOrder { get; private set; }

    public Guid? ProductId { get; private set; }
    public string ProductSku { get; private set; }
    public string Description { get; private set; }

    public decimal Quantity { get; private set; }
    public UnitOfMeasure UnitOfMeasure { get; private set; }

    public Money UnitPrice { get; private set; }
    public Money ExtendedPrice { get; private set; }
    public PricingModel PricingModel { get; private set; }

    public bool IsTaxable { get; private set; }
    public TaxConfiguration TaxConfiguration { get; private set; }
    public TaxCalculation CalculatedTax { get; private set; }

    public LineDiscount LineDiscount { get; private set; }

    public DateTime? ServiceDateStart { get; private set; }
    public DateTime? ServiceDateEnd { get; private set; }
    public decimal? BillableHours { get; private set; }
    public decimal? HourlyRate { get; private set; }

    public Guid? ProjectId { get; private set; }
    public Guid? JobId { get; private set; }
    public string CostCenter { get; private set; }

    public RevenueSchedule RevenueSchedule { get; private set; }

    public bool AffectsInventory { get; private set; }
    public Guid? WarehouseLocationId { get; private set; }

    private InvoiceLineItem() { }

    public InvoiceLineItem(
        string lineNumber,
        int sortOrder,
        string description,
        decimal quantity,
        UnitOfMeasure unitOfMeasure,
        Money unitPrice,
        PricingModel pricingModel = PricingModel.Fixed,
        Guid? productId = null,
        string productSku = null) : base(Guid.NewGuid())
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));
        if (unitPrice.Amount < 0)
            throw new ArgumentException("Unit price cannot be negative", nameof(unitPrice));

        LineNumber = lineNumber;
        SortOrder = sortOrder;
        ProductId = productId;
        ProductSku = productSku;
        Description = description;
        Quantity = quantity;
        UnitOfMeasure = unitOfMeasure;
        UnitPrice = unitPrice;
        PricingModel = pricingModel;
        ExtendedPrice = unitPrice.Multiply(quantity);
        IsTaxable = true;
    }

    public void UpdateQuantity(decimal newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(newQuantity));

        Quantity = newQuantity;
        ExtendedPrice = UnitPrice.Multiply(newQuantity);
    }

    public void UpdateUnitPrice(Money newUnitPrice)
    {
        if (newUnitPrice.Amount < 0)
            throw new ArgumentException("Unit price cannot be negative", nameof(newUnitPrice));

        UnitPrice = newUnitPrice;
        ExtendedPrice = newUnitPrice.Multiply(Quantity);
    }

    public void ApplyTaxConfiguration(TaxConfiguration taxConfig)
    {
        TaxConfiguration = taxConfig ?? throw new ArgumentNullException(nameof(taxConfig));
        IsTaxable = !taxConfig.IsExempt;

        if (IsTaxable)
        {
            var taxableAmount = LineDiscount != null ? ExtendedPrice.Subtract(LineDiscount.DiscountAmount) : ExtendedPrice;
            var taxAmount = taxableAmount.Multiply(taxConfig.TaxRate / 100m);
            CalculatedTax = new TaxCalculation(taxConfig, taxableAmount, taxAmount, DateTime.UtcNow);
        }
    }

    public void ApplyLineDiscount(LineDiscount discount)
    {
        LineDiscount = discount ?? throw new ArgumentNullException(nameof(discount));

        // Recalculate tax if applicable
        if (IsTaxable && TaxConfiguration != null)
        {
            ApplyTaxConfiguration(TaxConfiguration);
        }
    }

    public Money GetLineTotal()
    {
        var total = ExtendedPrice;

        if (LineDiscount != null)
            total = total.Subtract(LineDiscount.DiscountAmount);

        if (CalculatedTax != null)
            total = total.Add(CalculatedTax.CalculatedTax);

        return total;
    }

    public void SetRevenueSchedule(RevenueSchedule schedule)
    {
        RevenueSchedule = schedule ?? throw new ArgumentNullException(nameof(schedule));
    }

    public void SetServicePeriod(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
            throw new ArgumentException("Start date cannot be after end date");

        ServiceDateStart = startDate;
        ServiceDateEnd = endDate;
    }

    public void SetTimeTracking(decimal billableHours, decimal hourlyRate)
    {
        if (billableHours < 0 || hourlyRate < 0)
            throw new ArgumentException("Hours and rate must be non-negative");

        BillableHours = billableHours;
        HourlyRate = hourlyRate;
    }
}