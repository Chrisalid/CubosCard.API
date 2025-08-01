namespace CubosCard.Domain.Entities;

public class Person : BaseEntity
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string Document { get; private set; }

    public string Password { get; private set; }

    public virtual ICollection<Account> Accounts { get; set; }
    public virtual ICollection<AuthToken> AuthTokens { get; set; }

    public static Person Create(PersonModel model)
    {
        try
        {
            Person person = new() { Id = Guid.NewGuid() };

            person.SetName(model.Name);
            person.SetDocument(model.Document);
            person.SetPassword(model.Password);
            person.SetCreated(DateTime.Now);
            person.SetUpdated(DateTime.Now);

            return person;
        }
        catch
        {
            throw;
        }
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));

        Name = name;
    }

    private void SetDocument(string document)
    {
        if (string.IsNullOrWhiteSpace(document))
            throw new ArgumentException("Document cannot be null or empty.", nameof(document));

        Document = document;
    }

    private void SetPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));

        Password = password;
    }

    public record PersonModel
    (
        string Name,
        string Document,
        string Password
    );
}
