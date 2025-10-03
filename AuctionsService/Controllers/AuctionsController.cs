using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionsController(AuctionDbContext context, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
    {
        var auctions = await context.Auctions
            .Include(x => x.Item)
            .OrderBy(x => x.Item.Make)
            .ToListAsync();

        if (auctions == null || !auctions.Any()) return NotFound("No auctions found");

        return Ok(mapper.Map<IEnumerable<AuctionDto>>(auctions));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
    {
        var auction = await context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));
        
        if (auction == null) return NotFound();

        return Ok(mapper.Map<AuctionDto>(auction));
    }

    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto createAuctionDto)
    {
        var auction = mapper.Map<Auction>(createAuctionDto);

        auction.Seller = "test";
        context.Auctions.Add(auction);

        var result = await context.SaveChangesAsync() > 0;

        if (result)
        {
            return CreatedAtAction(nameof(GetAuctionById), new {id = auction.Id}, mapper.Map<AuctionDto>(auction));
        }
        return BadRequest("Failed to create auction");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
    {
        var auction = await context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));
        
        if (auction == null) return NotFound("Auction not found");

        auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
        auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
        auction.Item.Year = updateAuctionDto.Year != 0 ? updateAuctionDto.Year : auction.Item.Year;
        auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
        auction.Item.Mileage = updateAuctionDto.Mileage != 0 ? updateAuctionDto.Mileage : auction.Item.Mileage;
        auction.Item.ImageUrl = updateAuctionDto.ImageUrl ?? auction.Item.ImageUrl;

        var result = await context.SaveChangesAsync() > 0;
        if (result) return Ok();
        return BadRequest("Failed to update auction");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuction(Guid id)
    {
        var auction = await context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));
        
        if (auction == null) return NotFound("Auction not found");
        context.Auctions.Remove(auction);

        var result = await context.SaveChangesAsync() > 0;
        
        if (result) return Ok();
        return BadRequest("Failed to delete auction");
    }
}