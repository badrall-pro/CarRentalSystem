using CarRentalSystem.Domain.Enums;
using CarRentalSystem.Domain.Interfaces;
using MediatR;

namespace CarRentalSystem.Application.Features.Dashboard.Queries.GetRevenueReport
{
    public class GetRevenueReportQueryHandler : IRequestHandler<GetRevenueReportQuery, GetRevenueReportResponse>
    {
        private readonly IPaymentRepository _paymentRepository;

        public GetRevenueReportQueryHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<GetRevenueReportResponse> Handle(
            GetRevenueReportQuery request,
            CancellationToken cancellationToken)
        {
            var payments = await _paymentRepository.GetByDateRangeAsync(
                request.StartDate,
                request.EndDate,
                cancellationToken);

            var completedPayments = payments
                .Where(p => p.Status == PaymentStatus.Completed)
                .ToList();

            var totalRevenue = completedPayments.Sum(p => p.Amount);
            var totalPayments = completedPayments.Count;
            var averagePayment = totalPayments > 0 ? totalRevenue / totalPayments : 0;

            var dailyRevenue = completedPayments
                .GroupBy(p => p.PaymentDate.Date)
                .Select(g => new DailyRevenueDto
                {
                    Date = g.Key,
                    Amount = g.Sum(p => p.Amount),
                    PaymentCount = g.Count()
                })
                .OrderBy(d => d.Date);

            return new GetRevenueReportResponse
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                TotalRevenue = totalRevenue,
                TotalPayments = totalPayments,
                AveragePaymentAmount = averagePayment,
                DailyRevenue = dailyRevenue
            };
        }
    }
}
