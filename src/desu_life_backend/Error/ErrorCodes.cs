namespace desu.life.Error
{
    /// <summary>
    /// 错误码
    /// </summary>
    public static class ErrorCodes
    {
        /// <summary>
        /// 用户操作相关错误码
        /// </summary>
        public static class User
        {
            public const string EmailExists = "EMAIL_EXISTS";
            public const string EmailSendFailed = "EMAIL_SEND_FAILED";
        }
    }


}
