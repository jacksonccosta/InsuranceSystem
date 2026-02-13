using System;

namespace InsuranceSystem.Domain.Entities;

public class VehicleInsurance
{
    private const decimal MARGEM_SEGURANCA = 0.03m;
    private const decimal LUCRO = 0.05m;

    public Guid Id { get; private set; }
    public string InsuredName { get; private set; }
    public string InsuredCpf { get; private set; }
    public int InsuredAge { get; private set; }
    public string VehicleModel { get; private set; }
    public decimal VehicleValue { get; private set; }

    public decimal RiskRate { get; private set; }
    public decimal RiskPremium { get; private set; }
    public decimal PurePremium { get; private set; }
    public decimal CommercialPremium { get; private set; }

    protected VehicleInsurance() { }

    public VehicleInsurance(string name, string cpf, int age, string model, decimal value)
    {
        Id = Guid.NewGuid();
        InsuredName = name;
        InsuredCpf = cpf;
        InsuredAge = age;
        VehicleModel = model;
        VehicleValue = value;

        CalculateInsurance();
    }

    private void CalculateInsurance()
    {
        RiskRate = ((VehicleValue * 5) / (2 * VehicleValue));

        RiskPremium = (RiskRate / 100m) * VehicleValue;

        PurePremium = RiskPremium * (1 + MARGEM_SEGURANCA);

        CommercialPremium = PurePremium * (1 + LUCRO);
    }
}