﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using DocumentFormat.OpenXml.Framework;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation.Schema;
using DocumentFormat.OpenXml;

namespace DocumentFormat.OpenXml.Office.CoverPageProps
{
/// <summary>
/// <para>Defines the CoverPageProperties Class.</para>
/// <para>This class is available in Office 2007 or above.</para>
/// <para> When the object is serialized out as xml, its qualified name is cppr:CoverPageProperties.</para>
/// </summary>
/// <remarks>
/// The following table lists the possible child types:
/// <list type="bullet">
///<item><description>PublishDate &lt;cppr:PublishDate></description></item>
///<item><description>DocumentAbstract &lt;cppr:Abstract></description></item>
///<item><description>CompanyAddress &lt;cppr:CompanyAddress></description></item>
///<item><description>CompanyPhoneNumber &lt;cppr:CompanyPhone></description></item>
///<item><description>CompanyFaxNumber &lt;cppr:CompanyFax></description></item>
///<item><description>CompanyEmailAddress &lt;cppr:CompanyEmail></description></item>
/// </list>
/// </remarks>


[OfficeAvailability(FileFormatVersions.Office2007)]
[SchemaAttr(36, "CoverPageProperties")]
public partial class CoverPageProperties : OpenXmlCompositeElement
{
    
    
    /// <summary>
    /// Initializes a new instance of the CoverPageProperties class.
    /// </summary>
    public CoverPageProperties():base(){}
        /// <summary>
    ///Initializes a new instance of the CoverPageProperties class with the specified child elements.
    /// </summary>
    /// <param name="childElements">Specifies the child elements.</param>
    public CoverPageProperties(System.Collections.Generic.IEnumerable<OpenXmlElement> childElements)
        : base(childElements)
    {
    }
    /// <summary>
    /// Initializes a new instance of the CoverPageProperties class with the specified child elements.
    /// </summary>
    /// <param name="childElements">Specifies the child elements.</param>
    public CoverPageProperties(params OpenXmlElement[] childElements) : base(childElements)
    {
    }
    /// <summary>
    /// Initializes a new instance of the CoverPageProperties class from outer XML.
    /// </summary>
    /// <param name="outerXml">Specifies the outer XML of the element.</param>
    public CoverPageProperties(string outerXml)
        : base(outerXml)
    {
    }

    
private static readonly CompiledParticle _constraint = new CompositeParticle(ParticleType.Sequence, 1, 1)
{
    new ElementParticle(typeof(DocumentFormat.OpenXml.Office.CoverPageProps.PublishDate), 1, 1),
    new ElementParticle(typeof(DocumentFormat.OpenXml.Office.CoverPageProps.DocumentAbstract), 1, 1),
    new ElementParticle(typeof(DocumentFormat.OpenXml.Office.CoverPageProps.CompanyAddress), 1, 1),
    new ElementParticle(typeof(DocumentFormat.OpenXml.Office.CoverPageProps.CompanyPhoneNumber), 1, 1),
    new ElementParticle(typeof(DocumentFormat.OpenXml.Office.CoverPageProps.CompanyFaxNumber), 1, 1),
    new ElementParticle(typeof(DocumentFormat.OpenXml.Office.CoverPageProps.CompanyEmailAddress), 1, 1)
}.Compile();
internal override CompiledParticle CompiledParticle => _constraint;
    
        internal override OpenXmlCompositeType OpenXmlCompositeType => OpenXmlCompositeType.OneSequence;
        /// <summary>
    /// <para> PublishDate.</para>
    /// <para> Represents the following element tag in the schema: cppr:PublishDate </para>
    /// </summary>
    /// <remark>
    /// xmlns:cppr = http://schemas.microsoft.com/office/2006/coverPageProps
    /// </remark>
    public PublishDate PublishDate
	{
        get => GetElement<PublishDate>();
        set => SetElement(value);
	}
    /// <summary>
    /// <para> DocumentAbstract.</para>
    /// <para> Represents the following element tag in the schema: cppr:Abstract </para>
    /// </summary>
    /// <remark>
    /// xmlns:cppr = http://schemas.microsoft.com/office/2006/coverPageProps
    /// </remark>
    public DocumentAbstract DocumentAbstract
	{
        get => GetElement<DocumentAbstract>();
        set => SetElement(value);
	}
    /// <summary>
    /// <para> CompanyAddress.</para>
    /// <para> Represents the following element tag in the schema: cppr:CompanyAddress </para>
    /// </summary>
    /// <remark>
    /// xmlns:cppr = http://schemas.microsoft.com/office/2006/coverPageProps
    /// </remark>
    public CompanyAddress CompanyAddress
	{
        get => GetElement<CompanyAddress>();
        set => SetElement(value);
	}
    /// <summary>
    /// <para> CompanyPhoneNumber.</para>
    /// <para> Represents the following element tag in the schema: cppr:CompanyPhone </para>
    /// </summary>
    /// <remark>
    /// xmlns:cppr = http://schemas.microsoft.com/office/2006/coverPageProps
    /// </remark>
    public CompanyPhoneNumber CompanyPhoneNumber
	{
        get => GetElement<CompanyPhoneNumber>();
        set => SetElement(value);
	}
    /// <summary>
    /// <para> CompanyFaxNumber.</para>
    /// <para> Represents the following element tag in the schema: cppr:CompanyFax </para>
    /// </summary>
    /// <remark>
    /// xmlns:cppr = http://schemas.microsoft.com/office/2006/coverPageProps
    /// </remark>
    public CompanyFaxNumber CompanyFaxNumber
	{
        get => GetElement<CompanyFaxNumber>();
        set => SetElement(value);
	}
    /// <summary>
    /// <para> CompanyEmailAddress.</para>
    /// <para> Represents the following element tag in the schema: cppr:CompanyEmail </para>
    /// </summary>
    /// <remark>
    /// xmlns:cppr = http://schemas.microsoft.com/office/2006/coverPageProps
    /// </remark>
    public CompanyEmailAddress CompanyEmailAddress
	{
        get => GetElement<CompanyEmailAddress>();
        set => SetElement(value);
	}


    /// <inheritdoc/>
    public override OpenXmlElement CloneNode(bool deep) => CloneImp<CoverPageProperties>(deep);

}
/// <summary>
/// <para>Defines the PublishDate Class.</para>
/// <para>This class is available in Office 2007 or above.</para>
/// <para> When the object is serialized out as xml, its qualified name is cppr:PublishDate.</para>
/// </summary>


[OfficeAvailability(FileFormatVersions.Office2007)]
[SchemaAttr(36, "PublishDate")]
public partial class PublishDate : OpenXmlLeafTextElement
{
    
    
    /// <summary>
    /// Initializes a new instance of the PublishDate class.
    /// </summary>
    public PublishDate():base(){}
    
        /// <summary>
    /// Initializes a new instance of the PublishDate class with the specified text content.
    /// </summary>
    /// <param name="text">Specifies the text content of the element.</param>
    public PublishDate(string text):base(text)
    {
    }
    internal override OpenXmlSimpleType InnerTextToValue(string text)
    {
		return new StringValue(){ InnerText = text };
    }


    
    
    /// <inheritdoc/>
    public override OpenXmlElement CloneNode(bool deep) => CloneImp<PublishDate>(deep);

}
/// <summary>
/// <para>Defines the DocumentAbstract Class.</para>
/// <para>This class is available in Office 2007 or above.</para>
/// <para> When the object is serialized out as xml, its qualified name is cppr:Abstract.</para>
/// </summary>

[SchemaAttr(36, "Abstract")]
[OfficeAvailability(FileFormatVersions.Office2007)]
public partial class DocumentAbstract : OpenXmlLeafTextElement
{
    /// <summary>
    /// Initializes a new instance of the DocumentAbstract class.
    /// </summary>
    public DocumentAbstract():base(){}
    
        /// <summary>
    /// Initializes a new instance of the DocumentAbstract class with the specified text content.
    /// </summary>
    /// <param name="text">Specifies the text content of the element.</param>
    public DocumentAbstract(string text):base(text)
    {
    }
    internal override OpenXmlSimpleType InnerTextToValue(string text)
    {
		return new StringValue(){ InnerText = text };
    }

    /// <inheritdoc/>
    public override OpenXmlElement CloneNode(bool deep) => CloneImp<DocumentAbstract>(deep);


}
/// <summary>
/// <para>Defines the CompanyAddress Class.</para>
/// <para>This class is available in Office 2007 or above.</para>
/// <para> When the object is serialized out as xml, its qualified name is cppr:CompanyAddress.</para>
/// </summary>

[SchemaAttr(36, "CompanyAddress")]
[OfficeAvailability(FileFormatVersions.Office2007)]
public partial class CompanyAddress : OpenXmlLeafTextElement
{
    /// <summary>
    /// Initializes a new instance of the CompanyAddress class.
    /// </summary>
    public CompanyAddress():base(){}
    
        /// <summary>
    /// Initializes a new instance of the CompanyAddress class with the specified text content.
    /// </summary>
    /// <param name="text">Specifies the text content of the element.</param>
    public CompanyAddress(string text):base(text)
    {
    }
    internal override OpenXmlSimpleType InnerTextToValue(string text)
    {
		return new StringValue(){ InnerText = text };
    }

    /// <inheritdoc/>
    public override OpenXmlElement CloneNode(bool deep) => CloneImp<CompanyAddress>(deep);


}
/// <summary>
/// <para>Defines the CompanyPhoneNumber Class.</para>
/// <para>This class is available in Office 2007 or above.</para>
/// <para> When the object is serialized out as xml, its qualified name is cppr:CompanyPhone.</para>
/// </summary>

[SchemaAttr(36, "CompanyPhone")]
[OfficeAvailability(FileFormatVersions.Office2007)]
public partial class CompanyPhoneNumber : OpenXmlLeafTextElement
{
    /// <summary>
    /// Initializes a new instance of the CompanyPhoneNumber class.
    /// </summary>
    public CompanyPhoneNumber():base(){}
    
        /// <summary>
    /// Initializes a new instance of the CompanyPhoneNumber class with the specified text content.
    /// </summary>
    /// <param name="text">Specifies the text content of the element.</param>
    public CompanyPhoneNumber(string text):base(text)
    {
    }
    internal override OpenXmlSimpleType InnerTextToValue(string text)
    {
		return new StringValue(){ InnerText = text };
    }

    /// <inheritdoc/>
    public override OpenXmlElement CloneNode(bool deep) => CloneImp<CompanyPhoneNumber>(deep);


}
/// <summary>
/// <para>Defines the CompanyFaxNumber Class.</para>
/// <para>This class is available in Office 2007 or above.</para>
/// <para> When the object is serialized out as xml, its qualified name is cppr:CompanyFax.</para>
/// </summary>

[SchemaAttr(36, "CompanyFax")]
[OfficeAvailability(FileFormatVersions.Office2007)]
public partial class CompanyFaxNumber : OpenXmlLeafTextElement
{
    /// <summary>
    /// Initializes a new instance of the CompanyFaxNumber class.
    /// </summary>
    public CompanyFaxNumber():base(){}
    
        /// <summary>
    /// Initializes a new instance of the CompanyFaxNumber class with the specified text content.
    /// </summary>
    /// <param name="text">Specifies the text content of the element.</param>
    public CompanyFaxNumber(string text):base(text)
    {
    }
    internal override OpenXmlSimpleType InnerTextToValue(string text)
    {
		return new StringValue(){ InnerText = text };
    }

    /// <inheritdoc/>
    public override OpenXmlElement CloneNode(bool deep) => CloneImp<CompanyFaxNumber>(deep);


}
/// <summary>
/// <para>Defines the CompanyEmailAddress Class.</para>
/// <para>This class is available in Office 2007 or above.</para>
/// <para> When the object is serialized out as xml, its qualified name is cppr:CompanyEmail.</para>
/// </summary>

[SchemaAttr(36, "CompanyEmail")]
[OfficeAvailability(FileFormatVersions.Office2007)]
public partial class CompanyEmailAddress : OpenXmlLeafTextElement
{
    /// <summary>
    /// Initializes a new instance of the CompanyEmailAddress class.
    /// </summary>
    public CompanyEmailAddress():base(){}
    
        /// <summary>
    /// Initializes a new instance of the CompanyEmailAddress class with the specified text content.
    /// </summary>
    /// <param name="text">Specifies the text content of the element.</param>
    public CompanyEmailAddress(string text):base(text)
    {
    }
    internal override OpenXmlSimpleType InnerTextToValue(string text)
    {
		return new StringValue(){ InnerText = text };
    }

    /// <inheritdoc/>
    public override OpenXmlElement CloneNode(bool deep) => CloneImp<CompanyEmailAddress>(deep);


}
}
