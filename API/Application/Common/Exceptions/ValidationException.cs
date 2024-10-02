﻿using FluentValidation.Results;
using System.Runtime.Serialization;

namespace Application.Common.Exceptions
{
    [Serializable]
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Failures { get; }

        public ValidationException()
            : base("One or more validation errors occurred.")
        {
            Failures = new Dictionary<string, string[]>();
        }

        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(IEnumerable<ValidationFailure> validationFailures)
            : this()
        {
            var validationFailureGroups = validationFailures
                .GroupBy(f => f.PropertyName, f => f.ErrorMessage);

            foreach (var validationFailureGroup in validationFailureGroups)
            {
                var key = validationFailureGroup.Key;
                var value = validationFailureGroup.ToArray();
                Failures.Add(key, value);
            }
        }

        protected ValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
