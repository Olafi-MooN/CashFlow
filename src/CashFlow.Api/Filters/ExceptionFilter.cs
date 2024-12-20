﻿using CashFlow.Communication.Responses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CashFlow.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is CashFlowException)
            HandleProjectException(context);
        else
            ThrowUnknownError(context);
    }

    private static void HandleProjectException(ExceptionContext context)
    {
        var cashFlowException = (CashFlowException) context.Exception;
        var errorResponse = new ResponseErrorJson(cashFlowException.GetErrors());

        context.HttpContext.Response.StatusCode = cashFlowException.StatusCode;
        context.Result = new ObjectResult(errorResponse);
    }

    private static void ThrowUnknownError(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(new ResponseErrorJson([ResourceErrorMessages.UNKNOWN_ERROR, context.Exception.Message]));
    }
}
