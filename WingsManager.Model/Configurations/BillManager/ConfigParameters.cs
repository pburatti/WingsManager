using System;
using System.Collections.Generic;
using System.Text;

namespace WingsManager.Model.Configurations.BillManager
{
    public class ConfigParameters
    {
        public PatternsRegEx Patterns { get; set; }
        public FoldersPath FoldersPath { get; set; }
    }

    public class PatternsRegEx
    {
        /// <summary>
        /// Working in BillManager "BOLLA"
        /// </summary>
        public string Bill_IT { get; set; }
        /// <summary>
        /// Working in BillManager "CREDIT NOTE"
        /// </summary>
        public string CreditNote_EN { get; set; }
        /// <summary>
        /// Working in BillManager "NOTA DI CREDITO"
        /// </summary>
        public string CreditNote_IT { get; set; }
        /// <summary>
        /// Working in BillManager "FATTURA"
        /// </summary>
        public string Invoice_IT { get; set; }
        /// <summary>
        /// Working in BillManager "INVOICE"
        /// </summary>
        public string Invoice_EN { get; set; }
        /// <summary>
        /// Working in BillManager "BOLLA A CREDITO"
        /// </summary>
        public string CreditBill_IT { get; set; }
        /// <summary>
        /// Working in Account Statement "NOTA DI DEBITO RIEPILOGATIVA"
        /// </summary>
        public string DebitNoteReport_IT { get; set; }
        /// <summary>
        /// Working in Account Statement "DEBIT NOTE"
        /// </summary>
        public string DebitNoteReport_EN { get; set; }
        /// <summary>
        /// Working in Account Statement "FATTURA RIEPILOGATIVA"
        /// </summary>
        public string InvoiceReport_IT { get; set; }
        /// <summary>
        /// Working in Account Statement "INVOICE REPORT"
        /// </summary>
        public string InvoiceReport_EN { get; set; }
        /// <summary>
        /// Working in Account Statement "NOTA DI CREDITO RIEPILOGATIVA"
        /// </summary>
        public string CreditNoteReport_IT { get; set; }
        /// <summary>
        /// Working in Account Statement "INVOICE REPORT CREDIT"
        /// </summary>
        public string CreditNoteReport_EN { get; set; }
        public string Proforma { get; set; }
         
        public string CDC { get; set; }
        public string DocumentNumber { get; set; }
        public string CustomerNumber { get; set; }
        public string HeaderPerformanceRow { get; set; }
        public string ServiceBoxPassenger { get; set; }
        public string DepartureDate { get; set; }
        public string DocumentWingsNumber { get; set; }

    }
    public class FoldersPath
    {
        public string Original { get; set; }
        public string Processed { get; set; }
        public string InvoiceReport { get; set; }
        public string CreditNote { get; set; }
        public string ProformaAndOtherDocs { get; set; }
        public string OtherDocs { get; set; }
        public string Discarded { get; set; }
        public string Error { get; set; }
    }
}
