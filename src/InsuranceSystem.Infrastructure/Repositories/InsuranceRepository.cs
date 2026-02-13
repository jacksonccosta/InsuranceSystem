using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InsuranceSystem.Application.DTOs;
using InsuranceSystem.Application.Interfaces;
using InsuranceSystem.Domain.Entities;
using InsuranceSystem.Infrastructure.Context;

namespace InsuranceSystem.Infrastructure.Repositories;

public class InsuranceRepository : IInsuranceRepository
{
    private readonly ApplicationDbContext _context;

    public InsuranceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(VehicleInsurance insurance)
    {
        await _context.Insurances.AddAsync(insurance);
        await _context.SaveChangesAsync();
    }

    public async Task<VehicleInsurance?> GetByIdAsync(Guid id)
    {
        return await _context.Insurances.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<InsuranceReportResponse> GetReportAsync()
    {
        var hasData = await _context.Insurances.AnyAsync();
        if (!hasData) return new InsuranceReportResponse(0, 0, 0);

        var report = await _context.Insurances
            .GroupBy(x => 1)
            .Select(g => new InsuranceReportResponse(
                g.Average(x => x.VehicleValue),
                g.Average(x => x.CommercialPremium),
                g.Count()
            ))
            .FirstOrDefaultAsync();

        return report ?? new InsuranceReportResponse(0, 0, 0);
    }
}