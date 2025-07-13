using System.Net;
using Cobranca.Lib.Dominio.Exceptions;
using Cobranca.Lib.Dominio.Models;
using DnsClient.Internal;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace Cobranca.Gestao.Middlewares;

public class ErroMiddleware(ILogger<ErroMiddleware> logger) : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (RegraNegocioException ex)
        {
            var nomeFunction = context.FunctionDefinition.Name;
            logger.LogError(ex, "RegraNegocioException: Function: {Function}. Codigo: {Codigo}. Mensagem: {Mensagem}. StackTrace: {StackTrace}",
                nomeFunction, ex.ResponseModel?.Codigo, ex.ResponseModel?.Messagem, ex.StackTrace?.Replace("\n", "").Replace("\r\n", ""));

            if(ex.InnerException != null)
            {
                logger.LogError(ex.InnerException, "InnerException: Function: {Function}. Mensagem: {Mensagem}. StackTrace: {StackTrace}",
                    nomeFunction, ex.InnerException.Message, ex.InnerException.StackTrace?.Replace("\n", "").Replace("\r\n", ""));
            }
            await RetornaErroResponse(context, ex.ResponseModel, ex.StatusCode);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ErroInesperadoExcaption: Function: {Function}. Mensagem: {Mensagem}. StackTrace: {StackTrace}",
                context.FunctionDefinition.Name, ex.Message, ex.StackTrace?.Replace("\n", "").Replace("\r\n", ""));

            var responseErro = new ResponseModel
            {
                Codigo = "ErroInesperado",
                Messagem = "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."
            };

            var houveRetornoResponseData = await RetornaErroResponse(context, responseErro, HttpStatusCode.InternalServerError);
            
            if (!houveRetornoResponseData)
                logger.LogWarning("ErroMiddleware chamado por uma Function que não é HTTP. Não foi possível retornar resposta de erro.");
        }
    }


    private static async Task<bool> RetornaErroResponse(FunctionContext context, ResponseModel? responseModel, HttpStatusCode statusCode)
    {
        var httpResponseData = context.GetHttpResponseData();
        if (httpResponseData != null)
        {
            httpResponseData.StatusCode = statusCode;
            await httpResponseData.WriteAsJsonAsync(responseModel);
            return true;
        }

        var httpRequestData = await context.GetHttpRequestDataAsync();
        if (httpRequestData != null)
        {
            var response = httpRequestData.CreateResponse(statusCode);
            await response.WriteAsJsonAsync(responseModel);
            return true;
        }

        return false;
    }
}
