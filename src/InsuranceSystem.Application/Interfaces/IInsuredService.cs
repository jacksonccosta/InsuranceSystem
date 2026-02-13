using Refit;
using System.Threading.Tasks;
using InsuranceSystem.Application.DTOs;

namespace InsuranceSystem.Application.Interfaces;

public interface IInsuredService
{
    // Vamos assumir que existe um endpoint GET /insured/{cpf}
    [Get("/insured/{cpf}")]
    Task<ExternalInsuredData> GetInsuredByCpfAsync(string cpf);
}