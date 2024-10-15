using System;
using System.Collections.Generic;
using System.Text;

namespace WingsManager.Model.Configurations.AccountStatementManager
{
    public enum AmountClassType
    {
        None = 0,
        InvoiceReportIT = 110,
        DebitNoteIT = 120,
        InvoiceReportEN = 1110,
        DebitNoteEN = 1120,
    }

    public class ConfigParameters
    {
        public PatternsRegEx Patterns { get; set; }
        public FoldersPath FoldersPath { get; set; }
    }

    public class PatternsRegEx
    {
        public string ClientCodeIT { get; set; }
        public string ECData { get; set; }
        public string DebitNoteIT { get; set; }
        public string InvoiceReportIT { get; set; }
        public string CreditNoteReportIT { get; set; }
        public string DebitNoteEN { get; set; }
        public string InvoiceReportEN { get; set; }
        public string CreditNoteEN { get; set; }
        public string CreditNote2EN { get; set; }

        public string VATHeadingIT { get; set; }
        public string VATHeadingEN { get; set; }
        public string VATHeadingBoxFooter { get; set; }

        public string AgencyCwtCode { get; set; }

        public AmountDataPattern InvoiceReportAmountIT { get; set; }
        public AmountDataPattern DebitNoteAmountIT { get; set; }
        public AmountDataPattern InvoiceReportAmountEN { get; set; }
        public AmountDataPattern DebitNoteAmountEN { get; set; }

        public AmountDataPattern GetAmountsPattern(AmountClassType amountClassType)
        {
            if (amountClassType == AmountClassType.None)
                return null;

            switch (amountClassType)
            {
                case AmountClassType.InvoiceReportIT:
                    return this.InvoiceReportAmountIT;
                case AmountClassType.DebitNoteIT:
                    return this.DebitNoteAmountIT;
                case AmountClassType.InvoiceReportEN:
                    return this.InvoiceReportAmountEN;
                case AmountClassType.DebitNoteEN:
                    return this.DebitNoteAmountEN;
                default:
                    return null;
            }
        }
        public string GetVATHeadingPattern(string language)
        {
            if (string.IsNullOrEmpty(language))
                return null;

            switch (language)
            {
                case "IT":
                    return this.VATHeadingIT;
                case "EN":
                    return this.VATHeadingEN;
                default:
                    return null;
            }
        }
    }

    public class AmountDataPattern
    {
        public string VATNetAmount { get; set; }
        public string VATAmount { get; set; }
        public string TotalAmount { get; set; }
        public string PaidAmount { get; set; }
        public string StampAmount { get; set; }
        public string BalanceAmount { get; set; }
    }
    public class FoldersPath
    {
        public string Original { get; set; }
        public string Reprint { get; set; }
        public string Processed { get; set; }
        public string Error { get; set; }
    }
}
