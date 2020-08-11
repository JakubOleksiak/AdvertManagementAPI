using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AdvertApi.DTOS;
using AdvertApi.Models;
using AdvertApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AdvertApi.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IDbService _dbService;
        private readonly IConfiguration _configuration;

        public ClientsController([FromServices] IDbService dbService, IConfiguration configuration)
        {
            _dbService = dbService;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult RegisterClient([FromBody] RegisterClientDTO dto)
        {
            var client = _dbService.RegisterClient(dto);
            if (client == null)
            {
                return BadRequest();
            }

            return Created("", client);
        }

        [HttpPost]
        [Route("ref-token/{refToken}")]
        public IActionResult RefreshToken(string refToken)
        {
            var client = _dbService.GetClientFromRefreshToken(refToken);
            if (client == null) return NotFound();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, client.Login),
                new Claim(ClaimTypes.Name, client.FirstName + " " + client.LastName),
                new Claim(ClaimTypes.Role, "client")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "JakubSpZoo",
                audience: "clients",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: cred
            );


            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = _dbService.SetRefreshToken(client.Login, Guid.NewGuid().ToString())
            });
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginRequestDTO loginDto)
        {
            var ifClientExists = _dbService.checkIfClientExists(loginDto.Login);
            if (!ifClientExists) return Unauthorized();

            Client client = _dbService.getClient(loginDto.Login);

            var salt = _dbService.getSalt(client.IdClient);

            var passHash = _dbService.getHashedPassword(client.IdClient);

            if (!Salter.Validate(loginDto.Password, salt, passHash)) return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, loginDto.Login),
                new Claim(ClaimTypes.Name, client.FirstName + " " + client.LastName),
                new Claim(ClaimTypes.Role, "client")
            };

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("aewfesfgesfesge42345yu6jresrgfeafegesg"));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "JakubSpZoo",
                audience: "clients",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: cred
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = _dbService.SetRefreshToken(client.Login, Guid.NewGuid().ToString())
            });
        }
    }
}