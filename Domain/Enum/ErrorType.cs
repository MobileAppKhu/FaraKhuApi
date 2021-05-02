﻿namespace Domain.Enum
{
    public enum ErrorType
    {
        DuplicateUser,
        Unexpected,
        InvalidInput,
        UserNotFound,
        Unauthorized,
        CourseNotFound,
        EmailNotFound,
        CourseEventNotFound,
        UnauthorizedResetPassword,
        UnauthorizedValidation,
        EmailServiceException,
        DuplicatePassword,
        InvalidPassword,
        NewsNotFound
    }
}