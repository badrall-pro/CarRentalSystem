using CarRentalSystem.Domain.Interfaces;
using MediatR;

namespace CarRentalSystem.Application.Features.Payments.Queries.GetPaymentById
{
    public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, GetPaymentByIdResponse>
    {
        private readonly IPaymentRepository _paymentRepository;

        public GetPaymentByIdQueryHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<GetPaymentByIdResponse> Handle(
            GetPaymentByIdQuery request,
            CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (payment == null)
            {
                throw new KeyNotFoundException($"Payment with ID '{request.Id}' not found.");
            }

            return new GetPaymentByIdResponse
            {
                Id = payment.Id,
                ReservationId = payment.ReservationId,
                Amount = payment.Amount,
                Method = payment.Method.ToString(),
                Status = payment.Status.ToString(),
                PaymentDate = payment.PaymentDate,
                TransactionReference = payment.TransactionReference,
                Notes = payment.Notes,
                CreatedAt = payment.CreatedAt,
                UpdatedAt = payment.UpdatedAt
            };
        }
    }
}
