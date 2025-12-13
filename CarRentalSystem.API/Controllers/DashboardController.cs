using CarRentalSystem.Application.Features.Dashboard.Queries.GetDashboardStats;
using CarRentalSystem.Application.Features.Dashboard.Queries.GetRevenueReport;
using CarRentalSystem.Application.Features.Dashboard.Queries.GetVehicleUtilization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrator,Employee")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get dashboard statistics overview
    /// </summary>
    [HttpGet("stats")]
    [ProducesResponseType(typeof(GetDashboardStatsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetDashboardStatsResponse>> GetStats()
    {
        var query = new GetDashboardStatsQuery();
        var response = await _mediator.Send(query);
        return Ok(response);
    }

    /// <summary>
    /// Get revenue report for a date range
    /// </summary>
    [HttpGet("revenue")]
    [ProducesResponseType(typeof(GetRevenueReportResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GetRevenueReportResponse>> GetRevenueReport(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
        {
            return BadRequest(new { error = "Start date must be before or equal to end date" });
        }

        var query = new GetRevenueReportQuery
        {
            StartDate = startDate,
            EndDate = endDate
        };
        var response = await _mediator.Send(query);
        return Ok(response);
    }

    /// <summary>
    /// Get vehicle utilization report
    /// </summary>
    [HttpGet("vehicle-utilization")]
    [ProducesResponseType(typeof(GetVehicleUtilizationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GetVehicleUtilizationResponse>> GetVehicleUtilization(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
        {
            return BadRequest(new { error = "Start date must be before or equal to end date" });
        }

        var query = new GetVehicleUtilizationQuery
        {
            StartDate = startDate,
            EndDate = endDate
        };
        var response = await _mediator.Send(query);
        return Ok(response);
    }
}
