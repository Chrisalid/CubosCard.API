using System;
using CubosCard.Domain.Enums;

namespace CubosCard.Domain.Entities;

public class Card : BaseEntity
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public string Number { get; set; }

    public string CVV { get; set; }

    public CardType CardType { get; set; }

    public virtual Person Person { get; set; }

    public virtual Account Account { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; }

    public static Card Create(CardModel model)
    {
        try
        {
            Card card = new() { Id = Guid.NewGuid() };

            card.SetAccountId(model.AccountId);
            card.SetNumber(model.Number);
            card.SetCVV(model.CVV);
            card.SetCardType(model.CardType);
            card.SetCreated(DateTime.Now);
            card.SetUpdated(DateTime.Now);

            return card;
        }
        catch
        {
            throw;
        }
    }

    private void SetNumber(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Number cannot be null or empty.", nameof(number));

        Number = number;
    }

    private void SetCVV(string cvv)
    {
        if (string.IsNullOrWhiteSpace(cvv))
            throw new ArgumentException("CVV cannot be null or empty.", nameof(cvv));

        CVV = cvv;
    }

    private void SetCardType(CardType cardType)
    {
        if (!Enum.IsDefined<CardType>(cardType))
            throw new ArgumentException("Invalid card type.", nameof(cardType));

        CardType = cardType;
    }

    public void SetAccountId(Guid accountId)
    {
        if (accountId == Guid.Empty)
            throw new ArgumentException("AccountId cannot be empty.", nameof(accountId));

        AccountId = accountId;
    }

    public record CardModel
    (
        Guid AccountId,
        string Number,
        string CVV,
        CardType CardType
    );
}
