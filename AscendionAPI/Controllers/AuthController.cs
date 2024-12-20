﻿using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AscendionAPI.Models.DTO;
using NZWalks.API.Repositories;
using NZWalks.API.Models.DTO;

namespace AscendionAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly ITokenRepository tokenRepository;

    public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
    {
        this.userManager = userManager;
        this.tokenRepository = tokenRepository;
    }

    // POST: /api/Auth/Register
    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        var identityUser = new IdentityUser
        {
            UserName = registerRequestDto.Username,
            Email = registerRequestDto.Username
        };

        var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

        if (identityResult.Succeeded)
        {
            // Add roles to this User
            if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
            {
                identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                if (identityResult.Succeeded)
                {
                    return Ok("User was registered! Please login.");
                }
            }
        }

        return BadRequest("Something went wrong");
    }

    // POST: /api/Auth/Login
    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var user = await userManager.FindByEmailAsync(loginRequestDto.Username);

        if (user != null)
        {
            var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (checkPasswordResult)
            {
                // Get Roles for this user
                var roles = await userManager.GetRolesAsync(user);

                if (roles != null)
                {
                    // Create Token

                    var authToken = tokenRepository.CreateJWTToken(user, roles.ToList());

                    // Only primary role is returned. Change this behavior to suit your frontend app.
                    var response = new LoginResponseDto
                    {
                        AuthToken = authToken,
                        Email = user.Email,
                        Role = roles[0]
                    };

                    return Ok(response);
                }
            }
        }

        return BadRequest("Username or password incorrect");
    }
}
