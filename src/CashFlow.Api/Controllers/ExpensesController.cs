using CashFlow.Application.Interfaces;
using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(RequestRegisterExpensiveJson), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Register([FromServices] IUseCase<RequestRegisterExpensiveJson, Task<ResponseRegisteredExpenseJson>> useCase, [FromBody] RequestRegisterExpensiveJson request)
    {
        var response = useCase.Execute(request);

        return Created(string.Empty, response);
    }
}
