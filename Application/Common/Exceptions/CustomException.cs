using System;
using System.Collections.Generic;
using Domain.BaseModels;

namespace Application.Common.Exceptions;

public class CustomException : Exception
{
    public IEnumerable<Error> Errors { get; }

    public CustomException(Error error)
    {
        Errors = new[] {error};
    }

    public CustomException(IEnumerable<Error> errors)
    {
        Errors = errors;
    }
}