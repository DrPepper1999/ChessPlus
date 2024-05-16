using ErrorOr;

namespace GameChess.Domain.Common.DomainErrors;

public static partial class Errors
{
    public static class Account
    {
        public static Error AccountNotFound => Error.NotFound(
            code: "Account.NotFound",
            description: "Account with given ID does not exist");
    }
}