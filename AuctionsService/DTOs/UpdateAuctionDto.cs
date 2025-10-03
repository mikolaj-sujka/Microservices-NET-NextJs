using System.ComponentModel.DataAnnotations;

namespace AuctionService.DTOs;

public class UpdateAuctionDto
{
    [Required]
    public int ReservePrice { get; set; }
    [Required]
    public DateTime AuctionEnd { get; set; }
    [Required]
    public string Make { get; set; }
    public string Model { get; set; }
    [Required]
    public int Year { get; set; }
    public string Color { get; set; }
    [Required]
    public int Mileage { get; set; }
    public string ImageUrl { get; set; }
}