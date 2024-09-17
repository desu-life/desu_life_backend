using desu.life.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace desu.life.Error
{
    public class ExceptionHandler : IExceptionFilter
    {
        private readonly ILogger<ExceptionHandler> _logger;
        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            //异常返回结果包装
            var rspResult = R<object>.Err(context.Exception.Message);
            //日志记录
            _logger.LogError(context.Exception, context.Exception.Message);
            context.ExceptionHandled = true;
            context.Result = new InternalServerErrorObjectResult(rspResult);
        }

        public class InternalServerErrorObjectResult : ObjectResult
        {
            public InternalServerErrorObjectResult(object value) : base(value)
            {
                StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}
