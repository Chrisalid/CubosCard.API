using System;
using System.Text.Json.Serialization;
using CubosCard.Domain.Enums;

namespace CubosCard.Application.DTOs;

public class AccountRequest
{
    public Guid PersonId { get; set; }
    public string Branch { get; set; }

    public string Account { get; set; }
}

public class AccountResponse
{
    public Guid Id { get; set; }

    public string Branch { get; set; }

    public string Account { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}

public class CardRequest
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CardType Type { get; set; }

    public string Number { get; set; }

    public string CVV { get; set; }
}

public class CardResponse
{
    public Guid Id { get; set; }

    public string Number { get; set; }

    public string CVV { get; set; }

    public string Type { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}

public class TransactionRequest
{
    public decimal Value { get; set; }

    public string Description { get; set; }
}

public class TransactionResponse : TransactionRequest
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}

public class InternalTransactionRequest
{
    public Guid ReceiverAccountId { get; set; }

    public decimal Value { get; set; }

    public string Description { get; set; }
}

public class BalanceResponse
{
    public decimal Balance { get; set; }
}
