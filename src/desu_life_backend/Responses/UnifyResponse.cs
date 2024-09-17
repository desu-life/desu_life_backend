namespace desu.life.Responses
{
    public class UnifyResponse<T>

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
        /// <param name="data">返回的数据</param>
        /// <returns></returns>
        public static UnifyResponse<T> Ok(T data)
        {
            return new UnifyResponse<T> { Status = StatusCodes.Status200OK, Data = data };
        }

        /// <summary>
        /// 业务失败状态返回结果
        /// </summary>
        /// <param name="message">错误编码</param>
        /// <returns></returns>
        public static UnifyResponse<T> Fail(string message)
        {
            return new UnifyResponse<T> { Status = StatusCodes.Status400BadRequest, Message = message };
        }

        /// <summary>
        /// 异常状态返回结果
        /// </summary>
        /// <param name="message">错误编码</param>
        /// <returns></returns>
        public static UnifyResponse<T> Err(string message)
        {
            return new UnifyResponse<T> { Status = StatusCodes.Status500InternalServerError, Message = message };
        }

        /// <summary>
        /// 自由组装返回结果
        /// </summary>
        /// <returns></returns>
        public static UnifyResponse<T> Of(int status, T data, string? msg = null)
        {
            return new UnifyResponse<T> { Status = status, Data = data, Message = msg };
        }


        /// <summary>
        /// 隐式将T转化为R<T>
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator UnifyResponse<T>(T value)
        {
            return Ok(value);
        }
    }
}
