using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using UtilityTools.DataStructureManager;

namespace WingsManager.Model.Imports
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class WingsXmlDocument
    {
        public const string _pageBreakReplacer = "@@@";

        public WingsXmlMetaData MetaData { get; set; }

        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text { get; set; }

        public static async Task<WingsXmlDocument> GetInstance(Stream xmlStream, CancellationToken cancellationToken)
        {
            WingsXmlDocument instance = null;
            if (!xmlStream.CanRead || xmlStream.Length == 0)
                return null;
            
            instance = XmlManager.Deserialize<WingsXmlDocument>(xmlStream, new XmlRootAttribute("Document"));

            if (instance == null)
                return null;

            if (instance.MetaData != null)
            {
                xmlStream.Position = 0;

                //VAT Box
                XDocument xdoc = await XDocument.LoadAsync(xmlStream, LoadOptions.None, cancellationToken);
                if (xdoc == null)
                    return null;
                if (instance.MetaData != null)
                    instance.MetaData.InizializeVATBoxByXml(xdoc.Element("Document")?.Element("MetaData")?.Element("VAT_BOX"));
            }

            return instance;
        }
        public static async Task<string> GetBillingDocument(Stream xmlStream, CancellationToken cancellationToken)
        {
            if (!xmlStream.CanRead || xmlStream.Length == 0)
                return null;

            XDocument xdoc = await XDocument.LoadAsync(xmlStream, LoadOptions.None, cancellationToken);
            if (xdoc == null)
                return null;

            string billingDocument = ((System.Xml.Linq.XText)(((System.Xml.Linq.XContainer)xdoc.LastNode).LastNode)).Value?.Trim();

            return WingsXmlDocument.GetBillingDocument(billingDocument);
        }
        public static string GetBillingDocument(string billingDocument)
        {
            string pageBreakReplacer = "@@@";
            if (string.IsNullOrEmpty(billingDocument))
                return null;            

            billingDocument = billingDocument.Replace("<PageBreak           >", pageBreakReplacer)
                                             .Replace("&", "&#35;");

            XElement xmlBillingDocumentNode = XElement.Parse(billingDocument);

            billingDocument = xmlBillingDocumentNode?.Value?.Trim();

            return billingDocument;
        }

        public static string NormalizeWingsDatePattern(string wingsString)
        {
            if (string.IsNullOrEmpty(wingsString))
                return null;

            return wingsString.ToLower().Replace("mm", "MM");
        }
        public static DateTime? NormalizeAndParseDate(WingsXmlDate xmlDate)
        {
            if (xmlDate == null)
                return null;

            return NormalizeAndParseDate(xmlDate.Value.Trim(), WingsXmlDocument.NormalizeWingsDatePattern(xmlDate.Format));
        }
        public static DateTime? NormalizeAndParseDate(string wingsString, string patternDate)
        {
            if (string.IsNullOrEmpty(wingsString))
                return null;

            string wingsCleanString = null;
            if (patternDate.Count(x => x == 'y') <= 2)
                wingsCleanString = wingsString.PadLeft(8, '0');
            else
                wingsCleanString = wingsString.PadLeft(10, '0');

            DateTime dtDocumentConverted = default;
            if (DateTime.TryParseExact(wingsCleanString, patternDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtDocumentConverted))
                return dtDocumentConverted;

            return null;
        }
        public static decimal? NormalizeWingsAmount(string wingsString, string languageCulture)
        {
            if (string.IsNullOrEmpty(wingsString))
                return default;

            string cleanString = (wingsString.Last() == '-') ? string.Concat("-", wingsString.Substring(0, wingsString.Length - 1)) : wingsString;
            decimal valueConverted = 0;
            if (Decimal.TryParse(cleanString, NumberStyles.Currency, CultureInfo.GetCultureInfo(languageCulture), out valueConverted))
                return valueConverted;
            else
                return null;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class WingsXmlMetaData
    {
        /// <remarks/>
        public string ReplaceDocument { get; set; }

        /// <remarks/>
        public WingsXmlCWTOrganisation CWTOrganisation { get; set; }

        /// <remarks/>
        public WingsXmlCustomer Customer { get; set; }

        /// <remarks/>
        public WingsXmlDocumentType DocumentType { get; set; }

        /// <remarks/>
        public string Number { get; set; }

        /// <remarks/>
        public WingsXmlDate Date { get; set; }

        [XmlElement("SalesLine")] 
        public List<WingsXmlSalesLine> SalesLine { get; set; }

        public List<WingsXmlVATBox> VATBoxWrapper { get; set; }

        /// <remarks/>
        public decimal TotalInPreferredCurrency { get; set; }

        /// <remarks/>
        public string PreferredCurrency { get; set; }

        public void InizializeVATBoxByXml(XElement vatBoxNode)
        {
            if (vatBoxNode == null)
                return;

            if(this.VATBoxWrapper == null)
                this.VATBoxWrapper = new List<WingsXmlVATBox>();
            if (this.VATBoxWrapper.Count > 0)
                this.VATBoxWrapper.Clear();

            string lastNodeName = vatBoxNode.Elements().LastOrDefault()?.Name.LocalName;
            if (!string.IsNullOrEmpty(lastNodeName))
            {
                int lastNodeIdx = 0;
                int.TryParse(lastNodeName.Substring(lastNodeName.LastIndexOf('_') + 1), out lastNodeIdx);

                for (int idxRow = 0; idxRow < lastNodeIdx; idxRow++)
                {
                    WingsXmlVATBox vatDetail = new WingsXmlVATBox();
                    vatDetail.VAT_CD = vatBoxNode.Element($"VAT_CD_{idxRow + 1}")?.Value;
                    string vatPercentString = vatBoxNode.Element($"VAT_PC_{idxRow + 1}")?.Value;
                    if(!string.IsNullOrEmpty(vatPercentString))
                        vatDetail.VAT_PC = string.Format(WingsXmlDocument.NormalizeWingsAmount(vatPercentString?.Substring(0, vatPercentString.Length - 1).ToString(), "en-Gb").ToString(), "#0.00");//Bisogna normalizzare
                    vatDetail.VAT_BASE = WingsXmlDocument.NormalizeWingsAmount(vatBoxNode.Element($"VAT_BASE_{idxRow + 1}")?.Value, "en-Gb");
                    vatDetail.VAT_VAL = WingsXmlDocument.NormalizeWingsAmount(vatBoxNode.Element($"VAT_VAL_{idxRow + 1}")?.Value, "en-Gb");

                    this.VATBoxWrapper.Add(vatDetail);
                }
            }
        }
        [Obsolete]
        public void InizializeVATBoxByBillingDocument(XElement billingDocumentNode, string headerPattern, string footerPattern)
        {
            int startIdxVATBox = billingDocumentNode.Value?.LastIndexOf(headerPattern) ?? 0;
            string tempVATBox = billingDocumentNode.Value?.Substring(startIdxVATBox);
            int endIdxVATBox = tempVATBox?.LastIndexOf(footerPattern) ?? 0;
            string[] arrayVATBox = tempVATBox?.Substring(0, endIdxVATBox).Split(Environment.NewLine.ToArray());

            this.VATBoxWrapper = new List<WingsXmlVATBox>();
            for (int idxVATBoxLine = 0; idxVATBoxLine < arrayVATBox.Length; idxVATBoxLine++)
            {
                if (idxVATBoxLine == 0)
                    continue;

                string vatLine = arrayVATBox[idxVATBoxLine]?.Trim();

                if (string.IsNullOrEmpty(vatLine))
                    break;

                WingsXmlVATBox vatDetail = new WingsXmlVATBox();
                vatDetail.VAT_CD = vatLine.Substring(0, 1);
                vatDetail.VAT_PC = vatLine.Substring(4, 5);
                vatDetail.VAT_BASE = WingsXmlDocument.NormalizeWingsAmount(vatLine.Substring(16, 12), "it-IT");
                vatDetail.VAT_VAL = WingsXmlDocument.NormalizeWingsAmount(vatLine.Substring(31, 12), "it-IT");

                this.VATBoxWrapper.Add(vatDetail);
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class WingsXmlCWTOrganisation
    {
        /// <remarks/>
        public string BranchNumber { get; set; }

        /// <remarks/>
        public string BranchName { get; set; }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class WingsXmlCustomer
    {
        /// <remarks/>
        public string CustomerNumber { get; set; }

        /// <remarks/>
        public string CustomerName { get; set; }

        /// <remarks/>
        public string Country { get; set; }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class WingsXmlDocumentType
    {
        /// <remarks/>
        public byte TypeDocument { get; set; }

        /// <remarks/>
        public string Language { get; set; }

        /// <remarks/>
        public string Description { get; set; }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class WingsXmlDate: IComparable
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Format { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value { get; set; }

        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj.ToString());
        }
    }

    public class WingsXmlVATBox
    {
        public string VAT_CD { get; set; }
        public string VAT_PC { get; set; }
        public decimal? VAT_BASE { get; set; }
        public decimal? VAT_VAL { get; set; }
    }

    #region SalesLine
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    /// <remarks/>
    public partial class WingsXmlSalesLine
    {

        /// <remarks/>
        public uint DossierNumber { get; set; }

        /// <remarks/>
        public string TravelDocNumber { get; set; }

        /// <remarks/>
        public WingsXmlDate DepDate { get; set; }

        /// <remarks/>
        public decimal PriceIssuingCurrency { get; set; }

        /// <remarks/>
        public string IssuingCurrency { get; set; }

        public WingsXMLCreditCard CreditCard { get; set; }

        /// <remarks/>
        public WingsXMLSalesLineType SalesLineType { get; set; }

        /// <remarks/>
        public string CRSP19 { get; set; }

        /// <remarks/>
        public string CRSN19 { get; set; }

        /// <remarks/>
        public string PNDT19 { get; set; }

        /// <remarks/>
        public short DOSL19 { get; set; }

        /// <remarks/>
        public short DSLC19 { get; set; }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class WingsXMLSalesLineType
    {
        /// <remarks/>
        public byte TypeSalesLine { get; set; }

        /// <remarks/>
        public string Language { get; set; }

        /// <remarks/>
        public string Description { get; set; }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class WingsXMLCreditCard
    {
        /// <remarks/>
        public string CompanyName { get; set; }

        /// <remarks/>
        public string CreditCardNumber { get; set; }
    }
    #endregion

    #region VAT_BOX are dinamically and is not possible to provide before how many will be
    ///// <remarks/>
    //[System.SerializableAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    //public partial class DocumentMetaDataVAT_BOX
    //{
    //    /// <remarks/>
    //    public string VAT_CD_1
    //    {
    //        get
    //        {
    //            return this.vAT_CD_1Field;
    //        }
    //        set
    //        {
    //            this.vAT_CD_1Field = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public string VAT_PC_1
    //    {
    //        get
    //        {
    //            return this.vAT_PC_1Field;
    //        }
    //        set
    //        {
    //            this.vAT_PC_1Field = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public decimal VAT_BASE_1
    //    {
    //        get
    //        {
    //            return this.vAT_BASE_1Field;
    //        }
    //        set
    //        {
    //            this.vAT_BASE_1Field = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public decimal VAT_VAL_1
    //    {
    //        get
    //        {
    //            return this.vAT_VAL_1Field;
    //        }
    //        set
    //        {
    //            this.vAT_VAL_1Field = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public byte VAT_CD_2
    //    {
    //        get
    //        {
    //            return this.vAT_CD_2Field;
    //        }
    //        set
    //        {
    //            this.vAT_CD_2Field = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public string VAT_PC_2
    //    {
    //        get
    //        {
    //            return this.vAT_PC_2Field;
    //        }
    //        set
    //        {
    //            this.vAT_PC_2Field = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public decimal VAT_BASE_2
    //    {
    //        get
    //        {
    //            return this.vAT_BASE_2Field;
    //        }
    //        set
    //        {
    //            this.vAT_BASE_2Field = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public decimal VAT_VAL_2
    //    {
    //        get
    //        {
    //            return this.vAT_VAL_2Field;
    //        }
    //        set
    //        {
    //            this.vAT_VAL_2Field = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public byte VAT_CD_3
    //    {
    //        get
    //        {
    //            return this.vAT_CD_3Field;
    //        }
    //        set
    //        {
    //            this.vAT_CD_3Field = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public string VAT_PC_3
    //    {
    //        get
    //        {
    //            return this.vAT_PC_3Field;
    //        }
    //        set
    //        {
    //            this.vAT_PC_3Field = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public decimal VAT_BASE_3
    //    {
    //        get
    //        {
    //            return this.vAT_BASE_3Field;
    //        }
    //        set
    //        {
    //            this.vAT_BASE_3Field = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public decimal VAT_VAL_3
    //    {
    //        get
    //        {
    //            return this.vAT_VAL_3Field;
    //        }
    //        set
    //        {
    //            this.vAT_VAL_3Field = value;
    //        }
    //    }
    //}
    #endregion
}
