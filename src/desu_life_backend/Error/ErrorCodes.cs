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
            public const string UserNotExists = "USER_NOT_EXISTS";
            public const string EmailAlreadyConfirmed =  "EMAIL_ALREADY_CONFIRMED";
            public const string EmailConfirmFailed =  "EMAIL_CONFIRM_FAILED";

        }
    }


}
