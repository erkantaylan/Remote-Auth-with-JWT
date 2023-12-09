using System;
using System.ComponentModel.DataAnnotations;

namespace Identity.Api.User;

public class UserEntity
{
    public UserEntity()
    {
        Id = Guid.NewGuid();
    }

    [Key]
    public Guid Id { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
}