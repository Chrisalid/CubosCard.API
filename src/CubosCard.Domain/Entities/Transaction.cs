using CubosCard.Domain.Enums;

namespace CubosCard.Domain.Entities;

public class Transaction : BaseEntity
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public TransactionType Type { get; set; }

    public decimal Value { get; set; }

    public string Description { get; set; }

    public virtual Account Account { get; set; }

    public static Transaction Create(TransactionModel model)
    {
        try
        {
            Transaction transaction = new() { Id = Guid.NewGuid() };

            transaction.SetAccountId(model.AccountId);
            transaction.SetValue(model.Value);
            transaction.SetDescription(model.Description);
            transaction.SetTransactionType(model.Type);
            transaction.SetCreated(DateTime.Now);
            transaction.SetUpdated(DateTime.Now);

            return transaction;
        }
        catch
        {
            throw;
        }
    }

    private void SetValue(decimal value)
    {
        if (value <= 0)
            throw new ArgumentException("Value must be grater than zero.", nameof(value));

        Value = value;
    }

    private void SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be null or empty.", nameof(description));

        Description = description;
    }

    private void SetAccountId(Guid accountId)
    {
        if (accountId == Guid.Empty)
            throw new ArgumentException("AccountId cannot be empty.", nameof(accountId));

        AccountId = accountId;
    }

    private void SetTransactionType(TransactionType type)
    {
        if (Enum.IsDefined(type))
            throw new ArgumentException("TransactionType cannot be empty.", nameof(type));

        Type = type;
    }

    public record TransactionModel
    (
        Guid AccountId,
        decimal Value,
        string Description,
        TransactionType Type
    );
}
