using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using InsuranceSystem.Application.DTOs;
using InsuranceSystem.Application.Interfaces;

namespace InsuranceSystem.Application.UseCases.GetInsuranceById;

public record GetInsuranceByIdQuery(Guid Id) : IRequest<InsuranceDetailResponse?>;

public class GetInsuranceByIdHandler : IRequestHandler<GetInsuranceByIdQuery, InsuranceDetailResponse?>
{
    private readonly IInsuranceRepository _repository;

    public GetInsuranceByIdHandler(IInsuranceRepository repository)
    {
        _repository = repository;
    }

    public async Task<InsuranceDetailResponse?> Handle(GetInsuranceByIdQuery request, CancellationToken cancellationToken)
    {
        var insurance = await _repository.GetByIdAsync(request.Id);
        if (insurance == null) return null;

        return new InsuranceDetailResponse(
            insurance.Id,
            insurance.InsuredName,
            insurance.InsuredCpf,
            insurance.VehicleModel,
            insurance.VehicleValue,
            insurance.RiskRate,
            insurance.RiskPremium,
            insurance.PurePremium,
            insurance.CommercialPremium
        );
    }
}