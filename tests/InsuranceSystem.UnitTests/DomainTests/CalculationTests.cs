using FluentAssertions;
using InsuranceSystem.Domain.Entities;
using Xunit;

namespace InsuranceSystem.UnitTests.DomainTests;

public class CalculationTests
{
    [Fact]
    public void Should_Calculate_Insurance_Correctly_According_To_PDF_Example()
    {
        var nome = "Teste";
        var cpf = "123";
        var idade = 30;
        var modelo = "Fiat";
        var valorVeiculo = 10000m;

        var seguro = new VehicleInsurance(nome, cpf, idade, modelo, valorVeiculo);

        seguro.RiskRate.Should().Be(2.5m);

        seguro.RiskPremium.Should().Be(250.00m);

        seguro.PurePremium.Should().Be(257.50m);

        seguro.CommercialPremium.Should().BeApproximately(270.37m, 0.01m);
    }
}