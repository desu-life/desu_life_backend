using desu.life.Error;

namespace desu.life.Responses
{
    public class UnifiedResponse<T>

    {
        /// <summary>
        /// 状态结果
        /// </summary>
        public int Status { get; set; } = StatusCodes.Status200OK;

        /// <summary>
        /// 消息描述
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// 成功状态返回结果
        /// </summary>
        /// <returns></returns>
        public static UnifiedResponse<T> Ok()
        {
            return new UnifiedResponse<T> { Status = StatusCodes.Status200OK };
        }


        /// <summary>
        /// 成功状态返回结果
        /// </summary>
        /// <param name="data">返回的数据</param>
        /// <returns></returns>
        public static UnifiedResponse<T> Ok(T data)
        {
            return new UnifiedResponse<T> { Status = StatusCodes.Status200OK, Data = data };
        }

        /// <summary>
        /// 业务失败状态返回结果
        /// </summary>
        /// <param name="message">错误编码</param>
        /// <returns></returns>
        public static UnifiedResponse<T> Fail(string message)
        {
            return new UnifiedResponse<T> { Status = StatusCodes.Status400BadRequest, Message = message };
        }

        /// <summary>
        /// 权限不足返回结果
        /// </summary>
        /// <param name="message">错误编码</param>
        /// <returns></returns>
        public static UnifiedResponse<T> UnAuthorized()
        {
            return new UnifiedResponse<T> { Status = StatusCodes.Status401Unauthorized, Message = ErrorCodes.Common.UnAuthorized };
        }

        /// <summary>
        /// 未认证返回结果
        /// </summary>
        /// <param name="message">错误编码</param>
        /// <returns></returns>
        public static UnifiedResponse<T> UnAuthenticated()
        {
            return new UnifiedResponse<T> { Status = StatusCodes.Status403Forbidden, Message = ErrorCodes.Common.UnAuthenticated };
        }

        /// <summary>
        /// 异常状态返回结果
        /// </summary>
        /// <param name="message">错误编码</param>
        /// <returns></returns>
        public static UnifiedResponse<T> Err(string message)
        {
            return new UnifiedResponse<T> { Status = StatusCodes.Status500InternalServerError, Message = message };
        }

        /// <summary>
        /// 自由组装返回结果
        /// </summary>
        /// <returns></returns>
        public static UnifiedResponse<T> Of(int status, T data, string? msg = null)
        {
            return new UnifiedResponse<T> { Status = status, Data = data, Message = msg };
        }


        /// <summary>
        /// 隐式将T转化为R<T>
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator UnifiedResponse<T>(T value)
        {
            return Ok(value);
        }
    }
}
