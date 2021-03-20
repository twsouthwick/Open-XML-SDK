﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DocumentFormat.OpenXml.Validation;

namespace DocumentFormat.OpenXml.Framework
{
    internal class ListValidator : IValidator
    {
        public static IValidator Instance { get; } = new ListValidator();

        private ListValidator()
        {
        }

        public void Validate(ValidationContext context)
        {
            var current = context.Stack.Current;

            if (current is null || current.Value is null || current.Property is null)
            {
                return;
            }

            if (!current.Value.IsValid)
            {
                var id = current.IsAttribute ? "Sch_AttributeValueDataTypeDetailed" : "Sch_ElementValueDataTypeDetailed";
                var description = current.IsAttribute ? ValidationResources.Sch_AttributeValueDataTypeDetailed : ValidationResources.Sch_ElementValueDataTypeDetailed;

                if (string.IsNullOrEmpty(current.Value.InnerText))
                {
                    context.CreateError(
                        id: id,
                        description: SR.Format(description, current.Property.QName, current.Value.InnerText, current.IsAttribute ? ValidationResources.Sch_EmptyAttributeValue : ValidationResources.Sch_EmptyElementValue),
                        errorType: ValidationErrorType.Schema);
                }
                else
                {
                    context.CreateError(
                        id: id,
                        description: SR.Format(description, current.Property.QName, current.Value.InnerText, string.Empty),
                        errorType: ValidationErrorType.Schema);
                }
            }
        }
    }
}
