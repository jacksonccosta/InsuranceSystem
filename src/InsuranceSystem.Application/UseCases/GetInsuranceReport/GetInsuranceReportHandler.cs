using MediatR;
using System.Threading;
using System.Threading.Tasks;
using InsuranceSystem.Application.DTOs;
using InsuranceSystem.Application.Interfaces;

namespace InsuranceSystem.Application.UseCases.GetInsuranceReport;

public record GetInsuranceReportQuery : IRequest<InsuranceReportResponse>;

public class GetInsuranceReportHandler : IRequestHandler<GetInsuranceReportQuery, InsuranceReportResponse>
{
    private readonly IInsuranceRepository _repository;

    public GetInsuranceReportHandler(IInsuranceRepository repository)
    {
        _repository = repository;
    }

    public async Task<InsuranceReportResponse> Handle(GetInsuranceReportQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetReportAsync();
    }
}