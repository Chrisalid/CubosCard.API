namespace CubosCard.Domain.Entities;

public class BaseEntity
{
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public void SetCreated(DateTime created) => CreatedAt = created;
    public void SetUpdated(DateTime updated) => UpdatedAt = updated;
    public void SetDeleted(DateTime? deleted) => DeletedAt = deleted;
}
