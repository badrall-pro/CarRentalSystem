using CarRentalSystem.Domain.Enums;
using CarRentalSystem.Domain.Interfaces;
using MediatR;

namespace CarRentalSystem.Application.Features.Reservations.Queries.GetReservationById
{
    public class GetReservationByIdQueryHandler : IRequestHandler<GetReservationByIdQuery, GetReservationByIdResponse>
    {
        private readonly IReservationRepository _reservationRepository;

        public GetReservationByIdQueryHandler(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<GetReservationByIdResponse> Handle(
            GetReservationByIdQuery request,
            CancellationToken cancellationToken)
        {
            var reservation = await _reservationRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
            if (reservation == null)
            {
                throw new KeyNotFoundException($"Reservation with ID '{request.Id}' not found.");
            }

            var amountPaid = reservation.Payments
                .Where(p => p.Status == PaymentStatus.Completed)
                .Sum(p => p.Amount);

            return new GetReservationByIdResponse
            {
                Id = reservation.Id,
                CustomerId = reservation.CustomerId,
                CustomerName = reservation.Customer?.FullName ?? string.Empty,
                CustomerEmail = reservation.Customer?.Email ?? string.Empty,
                CustomerPhone = reservation.Customer?.PhoneNumber ?? string.Empty,
                VehicleId = reservation.VehicleId,
                VehicleBrand = reservation.Vehicle?.Brand ?? string.Empty,
                VehicleModel = reservation.Vehicle?.Model ?? string.Empty,
                VehicleLicensePlate = reservation.Vehicle?.LicensePlate ?? string.Empty,
                VehicleTypeName = reservation.Vehicle?.VehicleType?.Name ?? string.Empty,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                RentalDays = reservation.GetRentalDays(),
                DailyRate = reservation.GetRentalDays() > 0 ? reservation.TotalAmount / reservation.GetRentalDays() : 0,
                TotalAmount = reservation.TotalAmount,
                AmountPaid = amountPaid,
                RemainingBalance = reservation.GetRemainingBalance(amountPaid),
                IsFullyPaid = reservation.IsFullyPaid(),
                Status = reservation.Status.ToString(),
                Notes = reservation.Notes,
                QRCode = reservation.QRCode,
                CreatedAt = reservation.CreatedAt,
                UpdatedAt = reservation.UpdatedAt,
                Payments = reservation.Payments.Select(p => new PaymentInfoDto
                {
                    Id = p.Id,
                    Amount = p.Amount,
                    Method = p.Method.ToString(),
                    Status = p.Status.ToString(),
                    PaymentDate = p.PaymentDate
                })
            };
        }
    }
}
