using CarRentalSystem.Domain.Enums;
using CarRentalSystem.Domain.Interfaces;
using MediatR;

namespace CarRentalSystem.Application.Features.Dashboard.Queries.GetVehicleUtilization
{
    public class GetVehicleUtilizationQueryHandler : IRequestHandler<GetVehicleUtilizationQuery, GetVehicleUtilizationResponse>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IReservationRepository _reservationRepository;

        public GetVehicleUtilizationQueryHandler(
            IVehicleRepository vehicleRepository,
            IReservationRepository reservationRepository)
        {
            _vehicleRepository = vehicleRepository;
            _reservationRepository = reservationRepository;
        }

        public async Task<GetVehicleUtilizationResponse> Handle(
            GetVehicleUtilizationQuery request,
            CancellationToken cancellationToken)
        {
            var vehicles = await _vehicleRepository.GetAllWithTypeAsync(cancellationToken);
            var vehicleList = vehicles.ToList();

            var totalDays = (int)(request.EndDate - request.StartDate).TotalDays + 1;
            var vehicleUtilizations = new List<VehicleUtilizationDto>();

            foreach (var vehicle in vehicleList)
            {
                var reservations = await _reservationRepository.GetByVehicleIdAsync(vehicle.Id, cancellationToken);
                
                // Get only completed or active reservations within the date range
                var relevantReservations = reservations
                    .Where(r => (r.Status == ReservationStatus.Completed || r.Status == ReservationStatus.Active) &&
                               r.StartDate <= request.EndDate &&
                               r.EndDate >= request.StartDate)
                    .ToList();

                var totalRentalDays = 0;
                decimal totalRevenue = 0;

                foreach (var reservation in relevantReservations)
                {
                    // Calculate overlapping days with the requested period
                    var effectiveStart = reservation.StartDate < request.StartDate ? request.StartDate : reservation.StartDate;
                    var effectiveEnd = reservation.EndDate > request.EndDate ? request.EndDate : reservation.EndDate;
                    var days = (int)(effectiveEnd - effectiveStart).TotalDays + 1;
                    totalRentalDays += days;
                    
                    // Prorate revenue based on days in period
                    var reservationDays = reservation.GetRentalDays();
                    var dailyRevenue = reservationDays > 0 ? reservation.TotalAmount / reservationDays : 0;
                    totalRevenue += dailyRevenue * days;
                }

                var utilizationPercentage = totalDays > 0 ? (double)totalRentalDays / totalDays * 100 : 0;

                vehicleUtilizations.Add(new VehicleUtilizationDto
                {
                    VehicleId = vehicle.Id,
                    VehicleInfo = $"{vehicle.Brand} {vehicle.Model} ({vehicle.LicensePlate})",
                    VehicleType = vehicle.VehicleType?.Name ?? string.Empty,
                    TotalRentalDays = totalRentalDays,
                    ReservationCount = relevantReservations.Count,
                    TotalRevenue = totalRevenue,
                    UtilizationPercentage = Math.Round(utilizationPercentage, 2)
                });
            }

            var averageUtilization = vehicleUtilizations.Any()
                ? vehicleUtilizations.Average(v => v.UtilizationPercentage)
                : 0;

            return new GetVehicleUtilizationResponse
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                TotalDays = totalDays,
                AverageUtilization = Math.Round(averageUtilization, 2),
                VehicleUtilizations = vehicleUtilizations.OrderByDescending(v => v.UtilizationPercentage)
            };
        }
    }
}
