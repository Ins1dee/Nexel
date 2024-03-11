using Nexel.Domain.Shared;

namespace Nexel.Domain.Errors;

public static class DomainErrors
{
    public static class Cell
    {
        public static Error NotFound(string id)
        {
            return new Error(
                $"Cell with id '{id}' not found");
        }
    }

    public static class Sheet
    {
        public static Error NotFound(string id)
        {
            return new Error(
                $"Sheet with id '{id}' not found");
        }
    }
}