using MediatR;

namespace CarRentalSystem.Application.Features.Reservations.Queries.GetReservationById
{
    public record GetReservationByIdQuery(Guid Id) : IRequest<GetReservationByIdResponse>;

    public record GetReservationByIdResponse
    {
        public Guid Id { get; init; }
        //CUSTOMER INFO
        public Guid CustomerId { get; init; }
        public string CustomerName { get; init; } = string.Empty;
        public string CustomerEmail { get; init; } = string.Empty;
        public string CustomerPhone { get; init; } = string.Empty;
        //VEHICLE INFO
        public Guid VehicleId { get; init; }
        public string VehicleBrand { get; init; } = string.Empty;
        public string VehicleModel { get; init; } = string.Empty;
        public string VehicleLicensePlate { get; init; } = string.Empty;
        public string VehicleTypeName { get; init; } = string.Empty;
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public int RentalDays { get; init; }
        public decimal DailyRate { get; init; }
        public decimal TotalAmount { get; init; }
        public decimal AmountPaid { get; init; }
        public decimal RemainingBalance { get; init; }
        public bool IsFullyPaid { get; init; }
        public string Status { get; init; } = string.Empty;
        public string? Notes { get; init; }
        public string? QRCode { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public IEnumerable<PaymentInfoDto> Payments { get; init; } = Enumerable.Empty<PaymentInfoDto>();
    }

    public record PaymentInfoDto
    {
        public Guid Id { get; init; }
        public decimal Amount { get; init; }
        public string Method { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public DateTime PaymentDate { get; init; }
    }
}
