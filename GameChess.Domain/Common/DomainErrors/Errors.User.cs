using ErrorOr;

namespace GameChess.Domain.Common.DomainErrors;

public static partial class Errors
{
    public static class User
    {
        public static Error NotFound =>
            Error.NotFound(code: "User.NotFound", description: "User not Found");
    }
}