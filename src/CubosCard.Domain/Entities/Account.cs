namespace CubosCard.Domain.Entities;

public class Account : BaseEntity
{
    public Guid Id { get; private set; }

    public Guid PersonId { get; private set; }

    public string Branch { get; private set; }

    public string AccountNumber { get; private set; }

    public decimal Amount { get; private set; }

    public virtual Person Person { get; set; }

    public virtual ICollection<Card> Cards { get; set; }
    public virtual ICollection<Transaction> Transactions { get; set; }

    public static Account Create(AccountModel model)
    {
        try
        {
            Account account = new() { Id = Guid.NewGuid() };

            account.SetBranch(model.Branch);
            account.SetAccountNumber(model.AccountNumber);
            account.SetAmount(model.Amount);
            account.SetPersonId(model.PersonId);
            account.SetCreated(DateTime.Now);
            account.SetUpdated(DateTime.Now);

            return account;
        }
        catch
        {
            throw;
        }
    }

    private void SetBranch(string branch)
    {
        if (string.IsNullOrWhiteSpace(branch))
            throw new ArgumentException("Branch cannot be null or empty.", nameof(branch));
        Branch = branch;
    }

    private void SetAccountNumber(string accountNumber)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            throw new ArgumentException("AccountNumber cannot be null or empty.", nameof(accountNumber));
        AccountNumber = accountNumber;
    }

    private void SetPersonId(Guid personId)
    {
        if (personId == Guid.Empty)
            throw new ArgumentException("PersonId cannot be empty.", nameof(personId));
    }

    private void SetAmount(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be less than zero.", nameof(amount));

        Amount = amount;
    }

    public record struct AccountModel
    (
        string Branch,
        string AccountNumber,
        Guid PersonId,
        decimal Amount
    );
}
