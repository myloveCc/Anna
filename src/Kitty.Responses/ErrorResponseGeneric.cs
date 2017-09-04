using System.Collections.Generic;
using Kitty.Errors;

namespace Kitty.Responses
{
    public class ErrorResponse<T> : Response<T>
    {
        public ErrorResponse(Error error) 
            : base(new List<Error> {error})
        {
            
        }
        public ErrorResponse(List<Error> errors) 
            : base(errors)
        {
        }
    }
}