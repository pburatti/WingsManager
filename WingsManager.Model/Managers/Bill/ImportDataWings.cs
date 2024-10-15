using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace WingsManager.Model.Managers.Bill
{
    public enum DocumentType
    {
        Unknown = -1,
        Discarded = 0,
        /// <summary>
        /// bolla 'BOLLA'=Bill_IT,'BOLLA A CREDITO'=CreditBill_IT
        /// </summary>
        Bill = 1,
        /// <summary>
        /// bolla "CREDIT NOTE"=CreditNote_EN,"NOTA DI CREDITO"=CreditNote_IT
        /// </summary>
        CreditNote = 2,
        /// <summary>
        /// ec "DEBIT NOTE"=DebitNoteReport_EN,"NOTA DI DEBITO RIEPILOGATIVA"=DebitNoteReport_IT
        /// </summary>
        DebitNote = 3,
        /// <summary>
        /// ec "FATTURA RIEPILOGATIVA"=InvoiceReport_IT,"INVOICE REPORT"=InvoiceReport_EN
        /// </summary>
        InvoiceReport = 4,
        /// <summary>
        /// ec "NOTA DI CREDITO RIEPILOGATIVA"=CreditNoteReport_IT,"INVOICE REPORT CREDIT"=CreditNoteReport_EN
        /// </summary>
        CreditNoteReport = 5,
        /// <summary>
        /// 
        /// </summary>
        Proforma = 6,
        /// <summary>
        /// ?
        /// </summary>
        BillEN = 7,
        /// <summary>
        /// bolla "FATTURA"=Invoice_IT,"INVOICE"=Invoice_EN
        /// </summary>
        Invoice = 8,
        /// <summary>
        /// ?
        /// </summary>
        CreditNoteReportInsert = 9,
        /// <summary>
        /// ?
        /// </summary>
        CreditNoteReportMove = 10,
    }
    public enum DocumentLanguage
    { 
        None = 0,
        IT = 1,
        EN = 2
    }
    public class ImportDataWings
    {
        public DocumentType DocumentType { get; set; }
        public DocumentLanguage DocumentLanguage { get; set; }
        public DateTime? DocumentIssueDate { get; set; }
        public string DocumentName { get; set; }
        public int CwtAgencyCode { get; set; }
        public string CostCenter { get; set; }
        public int DocumentNumber { get; set; }
        public int DocumentWingsNumber { get; set; }
        public int ClientCode { get; set; }
        public string ClientName { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string BillingDocument { get; set; }
        public string PassengerName { get; set; }
        public string GDSPnr { get; set; }

        public string ArchivePath { get; set; }
        public string ArchiveFileName { get; set; }
    }
}
