using CarRentalSystem.Domain.Interfaces;
using MediatR;

namespace CarRentalSystem.Application.Features.Payments.Commands.RefundPayment
{
    public class RefundPaymentCommandHandler : IRequestHandler<RefundPaymentCommand, RefundPaymentResponse>
    {
        private readonly IPaymentRepository _paymentRepository;

        public RefundPaymentCommandHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<RefundPaymentResponse> Handle(
            RefundPaymentCommand request,
            CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (payment == null)
            {
                throw new KeyNotFoundException($"Payment with ID '{request.Id}' not found.");
            }

            payment.Refund();
            await _paymentRepository.UpdateAsync(payment, cancellationToken);

            return new RefundPaymentResponse
            {
                Id = payment.Id,
                Amount = payment.Amount,
                Status = payment.Status.ToString(),
                Message = $"Payment of {payment.Amount:C} has been refunded.",
                UpdatedAt = payment.UpdatedAt
            };
        }
    }
}
