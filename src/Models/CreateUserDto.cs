﻿using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Models;

public class CreateUserDto
{
    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? PasswordConfirmation { get; set; }

    public string? Nickname { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public int RoleId { get; set; } = 0;
}
