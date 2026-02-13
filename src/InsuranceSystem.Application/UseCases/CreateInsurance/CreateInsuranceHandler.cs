using MediatR;
using System.Threading;
using System.Threading.Tasks;
using InsuranceSystem.Application.DTOs;
using InsuranceSystem.Application.Interfaces;
using InsuranceSystem.Domain.Entities;

namespace InsuranceSystem.Application.UseCases.CreateInsurance;

public record CreateInsuranceCommand(CreateInsuranceRequest Request) : IRequest<InsuranceResponse>;

public class CreateInsuranceHandler : IRequestHandler<CreateInsuranceCommand, InsuranceResponse>
{
    private readonly IInsuredService _insuredService;
    private readonly IInsuranceRepository _repository;

    public CreateInsuranceHandler(IInsuredService insuredService, IInsuranceRepository repository)
    {
        _insuredService = insuredService;
        _repository = repository;
    }

    public async Task<InsuranceResponse> Handle(CreateInsuranceCommand command, CancellationToken cancellationToken)
    {
        ExternalInsuredData insured;
        try
        {
            insured = await _insuredService.GetInsuredByCpfAsync(command.Request.InsuredCpf);
        }
        catch
        {
            insured = new ExternalInsuredData("Usuario Teste", command.Request.InsuredCpf, 30);
        }

        var insurance = new VehicleInsurance(
            insured.Nome,
            insured.Cpf,
            insured.Idade,
            command.Request.VehicleModel,
            command.Request.VehicleValue
        );

        await _repository.AddAsync(insurance);

        return new InsuranceResponse(insurance.Id, insurance.InsuredName, insurance.VehicleModel, insurance.CommercialPremium);
    }
}