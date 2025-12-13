using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Interfaces;
using MediatR;

namespace CarRentalSystem.Application.Features.Reservations.Commands.CreateReservation
{
    public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, CreateReservationResponse>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public CreateReservationCommandHandler(
            IReservationRepository reservationRepository,
            ICustomerRepository customerRepository,
            IVehicleRepository vehicleRepository)
        {
            _reservationRepository = reservationRepository;
            _customerRepository = customerRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<CreateReservationResponse> Handle(
            CreateReservationCommand request,
            CancellationToken cancellationToken)
        {
            // Validate customer exists
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID '{request.CustomerId}' not found.");
            }

            // Check customer eligibility
            if (!customer.IsEligible())
            {
                throw new InvalidOperationException("Customer is not eligible to make reservations.");
            }

            // Validate vehicle exists and is available
            var vehicle = await _vehicleRepository.GetByIdWithTypeAsync(request.VehicleId, cancellationToken);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with ID '{request.VehicleId}' not found.");
            }

            // Check vehicle availability for the date range
            var isAvailable = await _vehicleRepository.IsAvailableAsync(
                request.VehicleId,
                request.StartDate,
                request.EndDate,
                cancellationToken);

            if (!isAvailable)
            {
                throw new InvalidOperationException("Vehicle is not available for the selected dates.");
            }

            // Get effective daily rate
            var dailyRate = vehicle.GetEffectiveDailyRate();

            // Create the reservation
            var reservation = Reservation.Create(
                customerId: request.CustomerId,
                vehicleId: request.VehicleId,
                startDate: request.StartDate,
                endDate: request.EndDate,
                dailyRate: dailyRate,
                notes: request.Notes
            );

            await _reservationRepository.AddAsync(reservation, cancellationToken);

            return new CreateReservationResponse
            {
                Id = reservation.Id,
                CustomerId = reservation.CustomerId,
                CustomerName = customer.FullName,
                VehicleId = reservation.VehicleId,
                VehicleInfo = $"{vehicle.Brand} {vehicle.Model} ({vehicle.LicensePlate})",
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                RentalDays = reservation.GetRentalDays(),
                TotalAmount = reservation.TotalAmount,
                Status = reservation.Status.ToString(),
                Message = "Reservation created successfully. Awaiting confirmation.",
                CreatedAt = reservation.CreatedAt
            };
        }
    }
}
