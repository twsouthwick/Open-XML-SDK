﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation.Schema;
using DocumentFormat.OpenXml.Validation.Semantic;
using System;
using System.Collections.Generic;

namespace DocumentFormat.OpenXml.Validation
{
    /// <summary>
    /// Defines the OpenXmlValidator.
    /// </summary>
    public class OpenXmlValidator
    {
        private readonly ValidationSettings _settings;
        private readonly ValidationCache _cache;

        /// <summary>
        /// Initializes a new instance of the OpenXmlValidator.
        /// </summary>
        /// <remarks>
        /// Default to FileFormat.Office2007.
        /// </remarks>
        public OpenXmlValidator()
            : this(FileFormatVersions.Office2007)
        {
        }

        /// <summary>
        /// Initializes a new instance of the OpenXmlValidator.
        /// </summary>
        /// <param name="fileFormat">The target file format to be validated against.</param>
        /// <remarks>
        /// Default to FileFormat.Office2007.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="fileFormat"/> parameter is not a known format.</exception>
        public OpenXmlValidator(FileFormatVersions fileFormat)
        {
            fileFormat.ThrowExceptionIfFileFormatNotSupported(nameof(fileFormat));

            _settings = new ValidationSettings(fileFormat);
            _cache = new ValidationCache(fileFormat);
        }

        /// <summary>
        /// Gets the target file format to be validated against.
        /// </summary>
        public FileFormatVersions FileFormat => _settings.FileFormat;

        /// <summary>
        /// Gets or sets the maximum number of errors the OpenXmlValidator will return.
        /// Default is 1000. A zero (0) value means no limitation.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Throw when the value set is less than zero.</exception>
        public int MaxNumberOfErrors
        {
            get => _settings.MaxNumberOfErrors;

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _settings.MaxNumberOfErrors = value;
            }
        }

        /// <summary>
        /// Validates the specified document.
        /// </summary>
        /// <param name="openXmlPackage">The target WordprocessingDocument, SpreadsheetDocument or PresentationDocument.</param>
        /// <returns>A set of validation errors.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the "openXmlPackage" parameter is null.</exception>
        public IEnumerable<ValidationErrorInfo> Validate(OpenXmlPackage openXmlPackage)
        {
            if (openXmlPackage == null)
            {
                throw new ArgumentNullException(nameof(openXmlPackage));
            }

            if (openXmlPackage.OpenSettings.MarkupCompatibilityProcessSettings.ProcessMode != MarkupCompatibilityProcessMode.NoProcess &&
                openXmlPackage.OpenSettings.MarkupCompatibilityProcessSettings.TargetFileFormatVersions != FileFormat)
            {
                var exceptionMessage = SR.Format(
                    ExceptionMessages.DocumentFileFormatVersionMismatch,
                    openXmlPackage.OpenSettings.MarkupCompatibilityProcessSettings.TargetFileFormatVersions,
                    FileFormat);

                throw new InvalidOperationException(exceptionMessage);
            }

            return ValidateCore(openXmlPackage);
        }

        /// <summary>
        /// Validates the specified content in the OpenXmlPart.
        /// </summary>
        /// <param name="openXmlPart">The target OpenXmlPart.</param>
        /// <returns>A set of validation errors.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the "openXmlPart" parameter is null.</exception>
        /// <exception cref="InvalidOperationException">Throw when the specified part is not a defined part in the specified FileFormat version.</exception>
        public IEnumerable<ValidationErrorInfo> Validate(OpenXmlPart openXmlPart)
        {
            if (openXmlPart == null)
            {
                throw new ArgumentNullException(nameof(openXmlPart));
            }

            var openXmlPackage = openXmlPart.OpenXmlPackage;
            if (openXmlPackage.OpenSettings.MarkupCompatibilityProcessSettings.ProcessMode != MarkupCompatibilityProcessMode.NoProcess &&
                openXmlPackage.OpenSettings.MarkupCompatibilityProcessSettings.TargetFileFormatVersions != FileFormat)
            {
                var exceptionMessage = SR.Format(
                    ExceptionMessages.DocumentFileFormatVersionMismatch,
                    openXmlPackage.OpenSettings.MarkupCompatibilityProcessSettings.TargetFileFormatVersions,
                    FileFormat);

                throw new InvalidOperationException(exceptionMessage);
            }

            FileFormat.ThrowIfNotInVersion(openXmlPart);

            return ValidateCore(openXmlPart);
        }

        private IEnumerable<ValidationErrorInfo> ValidateCore(OpenXmlPart part)
        {
            var validator = _cache.GetOrCreateDocumentValidator(part.OpenXmlPackage.ApplicationType);

            return validator.Validate(part, _settings);
        }

        private IEnumerable<ValidationErrorInfo> ValidateCore(OpenXmlPackage package)
        {
            var validator = _cache.GetOrCreateDocumentValidator(package.ApplicationType);

            return validator.Validate(package, _settings);
        }

        /// <summary>
        /// Validates the specified element.
        /// </summary>
        /// <param name="openXmlElement">The target OpenXmlElement.</param>
        /// <returns>A set of validation errors.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the "openXmlElement" parameter is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the "openXmlElement" is type of OpenXmlUnknownElement, OpenXmlMiscNode, AlternateContent, AlternateContentChoice or AlternateContentFallback.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the "openXmlElement" is not defined in the specified FileFormat.</exception>
        public IEnumerable<ValidationErrorInfo> Validate(OpenXmlElement openXmlElement)
        {
            if (openXmlElement == null)
            {
                throw new ArgumentNullException(nameof(openXmlElement));
            }

            if (openXmlElement is OpenXmlUnknownElement)
            {
                throw new ArgumentOutOfRangeException(nameof(openXmlElement), ExceptionMessages.CannotValidateUnknownElement);
            }

            if (openXmlElement is OpenXmlMiscNode)
            {
                throw new ArgumentOutOfRangeException(nameof(openXmlElement), ExceptionMessages.CannotValidateMiscNode);
            }

            if (openXmlElement is AlternateContent ||
                openXmlElement is AlternateContentChoice ||
                openXmlElement is AlternateContentFallback)
            {
                throw new ArgumentOutOfRangeException(nameof(openXmlElement), ExceptionMessages.CannotValidateAcbElement);
            }

            FileFormat.ThrowIfNotInVersion(openXmlElement);

            var validationContext = new ValidationContext(_settings, _cache)
            {
                Element = openXmlElement,
            };

            _cache.SchemaValidator.Validate(validationContext);

            validationContext.Element = openXmlElement;

            var semanticValidator = _cache.GetOrCreateSemanticValidator(ApplicationType.All);
            semanticValidator.Validate(validationContext);

            return validationContext.Errors;
        }
    }
}
