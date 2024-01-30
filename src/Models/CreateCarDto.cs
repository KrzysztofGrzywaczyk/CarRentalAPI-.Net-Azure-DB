using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static CarRentalAPI.Entities.Car;

namespace CarRentalAPI.Models;

public class CreateCarDto
{
    [Required]
    [MaxLength(9)]
    [RegularExpression(@"^[A-Za-z]\w{1,2}-[A-Za-z0-9]{4,5}$", ErrorMessage = "Please enter proper plate format od XX-XXXX do XXX-XXXX")]
    public string? PlateNumber { get; set; }

    [Required]
    [MaxLength(20)]
    public string? Brand { get; set; }

    [Required]
    [MaxLength(20)]
    public string? Model { get; set; }

    [Required]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "Please enter only proper year in format YYYY")]
    public int Year { get; set; }

    [Required]
    public string? Fuel { get; set; }

    [Required]
    [RegularExpression(@"^[A-F]$", ErrorMessage = "You may only enter one capital letter A-F to mark a segment")]
    public char Segment { get; set; }
}
