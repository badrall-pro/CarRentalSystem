using CarRentalSystem.Application.Features.Auth.Commands.Login;
using CarRentalSystem.Application.Features.Auth.Commands.RegisterAdmin;
using CarRentalSystem.Application.Features.Auth.Queries.GetCurrentUser;
using CarRentalSystem.Application.Features.Customers.Commands.RegisterCustomer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarRentalSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginCommand command)
    {
        try
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
       
    }

    // GETS THE CURRENTLY AUTHENTICATED USER'S INFORMATION
    [HttpGet("me")]
    [Authorize] // ← Requires valid JWT token
    [ProducesResponseType(typeof(CurrentUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CurrentUserResponse>> GetCurrentUser()
    {
        // Extract user ID from JWT token claims
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { error = "Invalid token" });
        }

        var query = new GetCurrentUserQuery(userId);
        var response = await _mediator.Send(query);

        return Ok(response);
    }


    /// <summary>
    // REGISTER AN ADMINISTRATOR (TODO: REMOVE OR PROTECT IN PRODUCTION!)
    /// </summary>
    [HttpPost("register-admin")]
    [ProducesResponseType(typeof(RegisterAdminResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RegisterAdminResponse>> RegisterAdmin(
        [FromBody] RegisterAdminCommand command)
    {
        try
        {
            var response = await _mediator.Send(command);
            return CreatedAtAction(nameof(RegisterAdmin), new { id = response.Id }, response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

