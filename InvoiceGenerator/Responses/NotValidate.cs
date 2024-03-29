﻿using FluentValidation.Results;

namespace InvoiceGenerator.Responses
{
    public class NotValidate
    {
        public Dictionary<string, string[]> Errors { get; set; }

        public NotValidate(List<ValidationFailure> validationErrors)
        {
            Errors = validationErrors
                .Where(x => x != null)
                .GroupBy(
                    x => x.PropertyName,
                    x => x.ErrorMessage,
                    (propertyName, errorMessages) => new
                    {
                        Key = propertyName,
                        Values = errorMessages.Distinct().ToArray()
                    })
                .ToDictionary(x => x.Key, x => x.Values);
        }
    }
}
