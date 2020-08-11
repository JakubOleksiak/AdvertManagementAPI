using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdvertApi.DTOS;
using AdvertApi.Models;
using AdvertApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdvertApi.Controllers
{
    [Route("api/campaigns")]
    [ApiController]
    public class CampaignsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public CampaignsController([FromServices] IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        [Authorize(Roles = "client")]
        public IActionResult GetCampaigns()
        {
            List<Campaign> campaigns =  _dbService.getCampaigns();

            campaigns = campaigns.OrderByDescending(e => e.StartDate).ToList();

            return Ok(campaigns);
        }

        [HttpPost]
        [Authorize(Roles = "client")]
        [Route("new")]
        public IActionResult AddCampaign([FromBody] AddCampaignRequestDTO dto)
        {
            bool flag = _dbService.checkBuildings(dto.FromIdBuilding, dto.ToIdBuilding);

            if (!flag)
            {
                return StatusCode(400);
            }

            List<Banner> banners =
                _dbService.CalculateNewBanners(dto.FromIdBuilding, dto.ToIdBuilding, dto.PricePerSquareMeter);

            double areaSum = 0;

            foreach (Banner banner in banners)
            {
                areaSum += banner.Area;
            }

            double totalPrice = areaSum * dto.PricePerSquareMeter;

            Campaign campaign = new Campaign
            {
                IdClient = dto.IdClient,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                PricePerSquareMeter = dto.PricePerSquareMeter,
                FromIdBuilding = dto.FromIdBuilding,
                ToIdBuilding = dto.ToIdBuilding
            };

            Campaign campaignRes = _dbService.AddCampaignAndBanners(campaign, banners);

            AddCampaignResponseDTO responseDto = new AddCampaignResponseDTO
            {
                Campaign = campaignRes,
                FullPrice = totalPrice
            };

            return Created("", responseDto);
        }
    }
}