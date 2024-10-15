using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace WingsManager.Model.Managers.AccountStatement
{
    public enum DocumentType
    {
        None = 0,
        DebitNote = 2,
        InvoiceReport = 1,
        CreditNoteReport = 3
    }
    public enum LanguageType
    {
        None = 0,
        IT = 1,
        EN = 2
    }

    public class ImportDataWings
    {
        public DocumentType DocumentType { get; set; }
        public LanguageType DocumentLanguage { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string DocumentNumber { get; set; }
        public int DocumentPageCount { get; set; }
        public DateTime? CreationFileDate { get; set; }
        public int ClientCode { get; set; }
        public string ClientName { get; set; }
        public int CwtAgencyCode { get; set; }
        public WingsTotalAmounts TotalAmounts { get; set; }
        public List<WingsVATDetails> VATDetails { get; set; }
        public string BillingDocument { get; set; }

        public string ArchivePath { get; set; }
        public string ArchiveFileName { get; set; }

        public bool OutFlgManualDoc { get; set; }
        public string OutRestampPath { get; set; }

        //** Methods **
        public string GetDocumentDescription()
        {
            if (this.DocumentType == DocumentType.None || this.DocumentLanguage == LanguageType.None)
                return null;

            switch (this.DocumentType)
            {
                case DocumentType.DebitNote:
                    return "NOTA DI DEBITO";
                case DocumentType.InvoiceReport:
                    return "FATTURA RIEPILOGATIVA";
                case DocumentType.CreditNoteReport:
                    return "NOTA DI CREDITO RIEPILOGATIVA";
                default:
                    return null;
            }
        }
        public string GetDefaultPatternDate()
        {
            //here is possible check with document type the property default pattern date
            return "dd/MM/yyyy";
        }
    }

    public class WingsTotalAmounts
    {
        /// <summary>
        /// Importo Netto Iva
        /// </summary>
        public decimal? VATNetAmount { get; set; }
        /// <summary>
        /// Importo IVA
        /// </summary>
        public decimal? VATAmount { get; set; }
        /// <summary>
        /// Totale importo
        /// </summary>
        public decimal? TotalAmount { get; set; }
        /// <summary>
        /// Totale pagato
        /// </summary>
        public decimal? PaidAmount { get; set; }
        /// <summary>
        /// Importo Bollo
        /// </summary>
        public decimal? StampAmount { get; set; }
        /// <summary>
        /// Saldo
        /// </summary>
        public decimal? BalanceAmount { get; set; }
    }
    public class WingsVATDetails
    {
        /// <summary>
        /// Codice IVA
        /// </summary>
        public string VATCode { get; set; }
        /// <summary>
        /// Aliquota IVA
        /// </summary>
        public string VATPercent { get; set; }
        /// <summary>
        /// Imponibile
        /// </summary>
        public decimal? Rateable { get; set; }
        /// <summary>
        /// Importo IVA
        /// </summary>
        public decimal? VATAmount { get; set; }
    }
}
