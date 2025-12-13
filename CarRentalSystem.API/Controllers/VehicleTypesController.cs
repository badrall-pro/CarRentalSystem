using CarRentalSystem.Application.Features.VehicleTypes.Commands.CreateVehicleType;
using CarRentalSystem.Application.Features.VehicleTypes.Commands.DeleteVehicleType;
using CarRentalSystem.Application.Features.VehicleTypes.Commands.UpdateVehicleType;
using CarRentalSystem.Application.Features.VehicleTypes.Queries.GetAllVehicleTypes;
using CarRentalSystem.Application.Features.VehicleTypes.Queries.GetVehicleTypeById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VehicleTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehicleTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all vehicle types with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(GetAllVehicleTypesResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetAllVehicleTypesResponse>> GetAll(
        [FromQuery] bool includeInactive = false,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetAllVehicleTypesQuery
        {
            IncludeInactive = includeInactive,
            Page = page,
            PageSize = pageSize
        };
        var response = await _mediator.Send(query);
        return Ok(response);
    }

    /// <summary>
    /// Get a vehicle type by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetVehicleTypeByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetVehicleTypeByIdResponse>> GetById(Guid id)
    {
        try
        {
            var query = new GetVehicleTypeByIdQuery(id);
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Create a new vehicle type
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(typeof(CreateVehicleTypeResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CreateVehicleTypeResponse>> Create([FromBody] CreateVehicleTypeCommand command)
    {
        try
        {
            var response = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing vehicle type
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(typeof(UpdateVehicleTypeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UpdateVehicleTypeResponse>> Update(Guid id, [FromBody] UpdateVehicleTypeCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest(new { error = "ID in URL does not match ID in request body" });
        }

        try
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Delete a vehicle type
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(typeof(DeleteVehicleTypeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeleteVehicleTypeResponse>> Delete(Guid id)
    {
        try
        {
            var command = new DeleteVehicleTypeCommand(id);
            var response = await _mediator.Send(command);
            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}
