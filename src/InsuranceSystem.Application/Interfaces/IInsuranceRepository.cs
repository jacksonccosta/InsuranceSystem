using System.Threading.Tasks;
using InsuranceSystem.Application.DTOs;
using InsuranceSystem.Domain.Entities;

namespace InsuranceSystem.Application.Interfaces;

public interface IInsuranceRepository
{
    Task AddAsync(VehicleInsurance insurance);
    Task<VehicleInsurance?> GetByIdAsync(Guid id);
    Task<InsuranceReportResponse> GetReportAsync();
}