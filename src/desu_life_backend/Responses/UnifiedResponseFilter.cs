using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace desu.life.Responses
{
    public class UnifiedResponseFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {

            //如果包含NoWrapperAttribute则说明不需要对返回结果进行包装，直接返回原始值
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var actionWrapper = controllerActionDescriptor?.MethodInfo.GetCustomAttributes(typeof(NoWrapperAttribute), false).FirstOrDefault();
            var controllerWrapper = controllerActionDescriptor?.ControllerTypeInfo.GetCustomAttributes(typeof(NoWrapperAttribute), false).FirstOrDefault();
            if (actionWrapper != null || controllerWrapper != null)
            {
                return;
            }

            // 返回void时 是EmptyResult
            if (context.Result is EmptyResult emptyResult)
            {
                context.Result = new ObjectResult(UnifiedResponse<object>.Ok());
                return;
            }

            /*
            如果返回不是往输出流里扔对象，则不做转换
            例如文件下载是FileResult;
            又或者使用.net core自带IResult相关实现修改状态码;
            或者直接返回ControllerBase.Ok()
            */

            if (context.Result is not ObjectResult objectResult) return;


            // 如果返回的状态码不是200，则不做转换（兼容Controller使用IActionResult直接修改HTTP状态码的行为）
            if (objectResult.StatusCode != null && objectResult.StatusCode != 200) return;


            //如果返回结果已经是UnifyResponse<T>类型的，则不需要进行再次包装了
            if (objectResult.DeclaredType is { IsGenericType: true } && objectResult.DeclaredType?.GetGenericTypeDefinition() == typeof(UnifiedResponse<>))
            {
                return;
            }

            context.Result = new ObjectResult(UnifiedResponse<object>.Ok(objectResult.Value));
        }
    }
}
