using MediatR;

namespace CarRentalSystem.Application.Features.Dashboard.Queries.GetDashboardStats
{
    public record GetDashboardStatsQuery : IRequest<GetDashboardStatsResponse>;

    public record GetDashboardStatsResponse
    {
        public int TotalCustomers { get; init; }
        //VEHICLES
        public int TotalVehicles { get; init; }
        public int AvailableVehicles { get; init; }
        public int RentedVehicles { get; init; }
        public int VehiclesInMaintenance { get; init; }
        //RESERVATIONS
        public int TotalReservations { get; init; }
        public int ActiveReservations { get; init; }
        public int PendingReservations { get; init; }
        public int CompletedReservationsThisMonth { get; init; }
        public int ConfirmedReservationsThisMonth { get; init; }
        public int CancelledReservationsThisMonth { get; init; }
        public decimal TotalRevenueThisMonth { get; init; }
        //PAYMENTS
        public decimal PendingPayments { get; init; }
    }
}
