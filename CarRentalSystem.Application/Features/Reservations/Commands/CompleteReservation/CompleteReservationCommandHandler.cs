using CarRentalSystem.Domain.Interfaces;
using MediatR;

namespace CarRentalSystem.Application.Features.Reservations.Commands.CompleteReservation
{
    public class CompleteReservationCommandHandler : IRequestHandler<CompleteReservationCommand, CompleteReservationResponse>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IPaymentRepository _paymentRepository;

        public CompleteReservationCommandHandler(
            IReservationRepository reservationRepository,
            IVehicleRepository vehicleRepository,
            IPaymentRepository paymentRepository)
        {
            _reservationRepository = reservationRepository;
            _vehicleRepository = vehicleRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<CompleteReservationResponse> Handle(
            CompleteReservationCommand request,
            CancellationToken cancellationToken)
        {
            var reservation = await _reservationRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
            if (reservation == null)
            {
                throw new KeyNotFoundException($"Reservation with ID '{request.Id}' not found.");
            }

            reservation.Complete();
            await _reservationRepository.UpdateAsync(reservation, cancellationToken);

            // Mark vehicle as available
            var vehicle = reservation.Vehicle;
            if (request.FinalMileage.HasValue)
            {
                vehicle.UpdateMileage(request.FinalMileage.Value);
            }
            vehicle.MarkAsAvailable();
            await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);

            // Calculate payment info
            var amountPaid = await _paymentRepository.GetTotalAmountByReservationIdAsync(request.Id, cancellationToken);
            var remainingBalance = reservation.GetRemainingBalance(amountPaid);

            return new CompleteReservationResponse
            {
                Id = reservation.Id,
                Status = reservation.Status.ToString(),
                TotalAmount = reservation.TotalAmount,
                AmountPaid = amountPaid,
                RemainingBalance = remainingBalance,
                Message = remainingBalance > 0
                    ? $"Rental completed. Outstanding balance: {remainingBalance:C}"
                    : "Rental completed successfully. Fully paid.",
                UpdatedAt = reservation.UpdatedAt
            };
        }
    }
}
