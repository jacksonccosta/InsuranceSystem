using System;

namespace InsuranceSystem.Application.DTOs;

public record CreateInsuranceRequest(string InsuredCpf, string VehicleModel, decimal VehicleValue);

public record InsuranceResponse(Guid Id, string InsuredName, string VehicleModel, decimal CommercialPremium);

public record InsuranceDetailResponse(
    Guid Id,
    string InsuredName,
    string InsuredCpf,
    string VehicleModel,
    decimal VehicleValue,
    decimal RiskRate,
    decimal RiskPremium,
    decimal PurePremium,
    decimal CommercialPremium
);

public record InsuranceReportResponse(decimal AverageVehicleValue, decimal AverageCommercialPremium, int TotalInsurances);

public record ExternalInsuredData(string Nome, string Cpf, int Idade);