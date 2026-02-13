using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using InsuranceSystem.Application.DTOs;
using InsuranceSystem.Application.UseCases.CreateInsurance;
using InsuranceSystem.Application.UseCases.GetInsuranceById;
using InsuranceSystem.Application.UseCases.GetInsuranceReport;

namespace InsuranceSystem.API.Controllers;

/// <summary>
/// Gerencia as operações relacionadas ao cálculo e consulta de seguros de veículos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class InsuranceController : ControllerBase
{
    private readonly IMediator _mediator;

    public InsuranceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Calcula e registra um novo seguro de veículo.
    /// </summary>
    /// <remarks>
    /// Este endpoint recebe os dados do veículo e do segurado (CPF), consulta informações adicionais 
    /// em um serviço externo (mockado), realiza o cálculo dos prêmios (Risco, Puro, Comercial) 
    /// e persiste o registro no banco de dados.
    /// 
    /// **Fórmulas:**
    /// * Taxa de Risco = (Valor Veículo * 5) / (2 * Valor Veículo)
    /// * Prêmio de Risco = Taxa de Risco * Valor Veículo
    /// * Prêmio Puro = Prêmio de Risco * (1 + 3%)
    /// * Prêmio Comercial = Prêmio Puro * (1 + 5%)
    /// </remarks>
    /// <param name="request">Dados necessários para o cálculo: CPF do segurado, Modelo e Valor do Veículo.</param>
    /// <returns>Os dados do seguro calculado, incluindo os valores dos prêmios.</returns>
    /// <response code="200">Seguro calculado e registrado com sucesso.</response>
    /// <response code="400">Dados inválidos fornecidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(InsuranceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateInsuranceRequest request)
    {
        var result = await _mediator.Send(new CreateInsuranceCommand(request));
        return Ok(result);
    }

    /// <summary>
    /// Busca os detalhes de um seguro pelo seu ID.
    /// </summary>
    /// <param name="id">Identificador único (GUID) do seguro.</param>
    /// <returns>Detalhes completos do seguro, incluindo valores calculados e dados do segurado.</returns>
    /// <response code="200">Seguro encontrado.</response>
    /// <response code="404">Seguro não encontrado para o ID informado.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(InsuranceDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetInsuranceByIdQuery(id));
        if (result == null) return NotFound();
        return Ok(result);
    }

    /// <summary>
    /// Obtém um relatório estatístico com as médias dos seguros registrados.
    /// </summary>
    /// <remarks>
    /// Retorna a média aritmética do Valor dos Veículos e dos Prêmios Comerciais de todos os seguros na base de dados.
    /// </remarks>
    /// <returns>Objeto contendo as médias calculadas e o total de registros.</returns>
    /// <response code="200">Relatório gerado com sucesso.</response>
    [HttpGet("report")]
    [ProducesResponseType(typeof(InsuranceReportResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReport()
    {
        var result = await _mediator.Send(new GetInsuranceReportQuery());
        return Ok(result);
    }
}