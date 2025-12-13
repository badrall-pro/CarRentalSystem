using CarRentalSystem.Domain.Enums;
using CarRentalSystem.Domain.Interfaces;
using MediatR;

namespace CarRentalSystem.Application.Features.Dashboard.Queries.GetDashboardStats
{
    public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, GetDashboardStatsResponse>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IPaymentRepository _paymentRepository;

        public GetDashboardStatsQueryHandler(
            ICustomerRepository customerRepository,
            IVehicleRepository vehicleRepository,
            IReservationRepository reservationRepository,
            IPaymentRepository paymentRepository)
        {
            _customerRepository = customerRepository;
            _vehicleRepository = vehicleRepository;
            _reservationRepository = reservationRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<GetDashboardStatsResponse> Handle(
            GetDashboardStatsQuery request,
            CancellationToken cancellationToken)
        {
            // Customer stats
            var customers = await _customerRepository.GetAllAsync(cancellationToken);
            var totalCustomers = customers.Count();

            // Vehicle stats
            var vehicles = await _vehicleRepository.GetAllAsync(cancellationToken);
            var vehicleList = vehicles.ToList();
            var totalVehicles = vehicleList.Count;
            var availableVehicles = vehicleList.Count(v => v.Status == VehicleStatus.Available);
            var rentedVehicles = vehicleList.Count(v => v.Status == VehicleStatus.Rented);
            var vehiclesInMaintenance = vehicleList.Count(v => v.Status == VehicleStatus.Maintenance);

            // Reservation stats
            var reservations = await _reservationRepository.GetAllAsync(cancellationToken);
            var reservationList = reservations.ToList();
            var totalReservations = reservationList.Count;
            var activeReservations = reservationList.Count(r => r.Status == ReservationStatus.Active);
            var pendingReservations = reservationList.Count(r => r.Status == ReservationStatus.Pending);

            // This month stats
            var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
            
            var completedThisMonth = reservationList.Count(r =>
                r.Status == ReservationStatus.Completed &&
                r.UpdatedAt.HasValue &&
                r.UpdatedAt.Value >= startOfMonth &&
                r.UpdatedAt.Value <= endOfMonth);
            
            var confirmedThisMonth = reservationList.Count(r =>
                r.Status == ReservationStatus.Confirmed &&
                r.UpdatedAt.HasValue &&
                r.UpdatedAt.Value >= startOfMonth &&
                r.UpdatedAt.Value <= endOfMonth);
            
            var cancelledThisMonth = reservationList.Count(r =>
                r.Status == ReservationStatus.Cancelled &&
                r.UpdatedAt.HasValue &&
                r.UpdatedAt.Value >= startOfMonth &&
                r.UpdatedAt.Value <= endOfMonth);

            
            // Payment stats
            var paymentsThisMonth = await _paymentRepository.GetByDateRangeAsync(startOfMonth, endOfMonth, cancellationToken);
            var totalRevenueThisMonth = paymentsThisMonth
                .Where(p => p.Status == PaymentStatus.Completed)
                .Sum(p => p.Amount);

            // Calculate pending payments (sum of remaining balances for active/confirmed reservations)
            var activeAndConfirmedReservations = reservationList
                .Where(r => r.Status == ReservationStatus.Active || r.Status == ReservationStatus.Confirmed)
                .ToList();

            decimal pendingPayments = 0;
            foreach (var reservation in activeAndConfirmedReservations)
            {
                var paidAmount = await _paymentRepository.GetTotalAmountByReservationIdAsync(reservation.Id, cancellationToken);
                pendingPayments += reservation.TotalAmount - paidAmount;
            }

            return new GetDashboardStatsResponse
            {
                TotalCustomers = totalCustomers,
                TotalVehicles = totalVehicles,
                AvailableVehicles = availableVehicles,
                RentedVehicles = rentedVehicles,
                VehiclesInMaintenance = vehiclesInMaintenance,
                ActiveReservations = activeReservations,
                PendingReservations = pendingReservations,
                TotalReservations = totalReservations,
                CompletedReservationsThisMonth = completedThisMonth,
                ConfirmedReservationsThisMonth = confirmedThisMonth,
                CancelledReservationsThisMonth = cancelledThisMonth,
                TotalRevenueThisMonth = totalRevenueThisMonth,
                PendingPayments = pendingPayments
            };
        }
    }
}
