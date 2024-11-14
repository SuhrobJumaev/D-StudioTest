namespace Todo.API.Helpers
{
    public  static class Utils
    {
        public static string PasswordByDefault => "123456";
        public static string EmailByDefault = "admin@gmail.com";
        public const string JsonContentType = "application/json";

        public const string API_VERSION_1 = "1.0";
        public const string API_VERSION_2 = "2.0";
        public const string ApiVersionTag = "api-version";

        public static class ErrorMessage
        {
            public static string UserNotFound = "User not found";
            public static string InvalidUserId = "Invalid User id";
            public static string RefreshTokenNotFound = "Requested refresh token not found";
            public static string RefreshTokenExpiried = "Refresh token had expiried";
            public static string Success = "Request is successfully";
            public static string Failed = "Request is failed";
            public static string InvallidUserEmailOrPassword = "Invalid user email or password.";
            public static string UserAlreadyExists = "User with this email already exists";

            public static string TaskNotFound = "Task not found";
            public static string InvalidTaskId = "Invalid Task id";
        }
    }
}
