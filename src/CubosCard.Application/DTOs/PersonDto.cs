using System;

namespace CubosCard.Application.DTOs;

public class PersonRequest
{
    public string Name { get; set; }

    public string Document { get; set; }

    public string Password { get; set; }
}

public class PersonResponse : PersonRequest
{
    public Guid Id { get; set; }
}
