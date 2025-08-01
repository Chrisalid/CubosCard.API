using System;
using System.Text.RegularExpressions;

namespace CubosCard.Infrastructure.Utility;

public static class Utils
{
    public static string NormalizeStringDocument(string document)
    {
        if (string.IsNullOrWhiteSpace(document))
            throw new ArgumentException("Document cannot be null or empty.", nameof(document));

        return new string([.. document.Where(char.IsDigit)]);
    }

    public static string CreateCardMask(string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            throw new ArgumentException("Card number cannot be null or empty.", nameof(cardNumber));

        return $"XXXX XXXX XXXX {cardNumber[12..]}";
    }

    public static bool IsValidAccountNumber(string accountNumber)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            return false;

        var regexPattern = new Regex(@"^\d{7}-\d{1}$");

        return regexPattern.IsMatch(accountNumber);
    }
}
