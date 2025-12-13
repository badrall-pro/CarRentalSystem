using CarRentalSystem.Domain.Interfaces;
using MediatR;

namespace CarRentalSystem.Application.Features.Payments.Commands.CompletePayment
{
    public class CompletePaymentCommandHandler : IRequestHandler<CompletePaymentCommand, CompletePaymentResponse>
    {
        private readonly IPaymentRepository _paymentRepository;

        public CompletePaymentCommandHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<CompletePaymentResponse> Handle(
            CompletePaymentCommand request,
            CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (payment == null)
            {
                throw new KeyNotFoundException($"Payment with ID '{request.Id}' not found.");
            }

            payment.MarkAsCompleted();
            await _paymentRepository.UpdateAsync(payment, cancellationToken);

            return new CompletePaymentResponse
            {
                Id = payment.Id,
                Status = payment.Status.ToString(),
                Message = "Payment marked as completed successfully.",
                UpdatedAt = payment.UpdatedAt
            };
        }
    }
}
