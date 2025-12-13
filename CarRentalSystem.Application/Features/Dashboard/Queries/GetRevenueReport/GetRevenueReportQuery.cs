using MediatR;

namespace CarRentalSystem.Application.Features.Dashboard.Queries.GetRevenueReport
{
    public record GetRevenueReportQuery : IRequest<GetRevenueReportResponse>
    {
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }

    public record DailyRevenueDto
    {
        public DateTime Date { get; init; }
        public decimal Amount { get; init; }
        public int PaymentCount { get; init; }
    }

    public record GetRevenueReportResponse
    {
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public decimal TotalRevenue { get; init; }
        public int TotalPayments { get; init; }
        public decimal AveragePaymentAmount { get; init; }
        public IEnumerable<DailyRevenueDto> DailyRevenue { get; init; } = Enumerable.Empty<DailyRevenueDto>();
    }
}
