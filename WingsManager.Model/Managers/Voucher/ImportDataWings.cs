using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace WingsManager.Model.Managers.Voucher
{
    //public enum DocumentType
    //{
    //    Unknown = -1,
    //    Discarded = 0,
    //    Bill = 1,
    //    CreditNote = 2,
    //    DebitNote = 3,
    //    InvoiceReport = 4,
    //    CreditNoteReport = 5,
    //    Proforma = 6,
    //    BillEN = 7,
    //    Invoice = 8,
    //    CreditNoteReportInsert = 9,
    //    CreditNoteReportMove = 10,        
    //}

    public class ImportDataWings
    {
        public int CwtAgencyCode { get; set; }
        public string HarpCode { get; set; }
        public int ClientCode { get; set; }
        public string ClientName { get; set; }
        public int DocumentId { get; set; }
        public DateTime? DocumentIssueDate { get; set; }
        public string PassengerName { get; set; }
        public string VoucherNumber { get; set; }
        public string VoucherType { get; set; }
        public int VoucherSendType { get; set; }
        public string VoucherSeries { get; set; }
        public string WingsProductCode { get; set; }
        public string WingsProductCCCode { get; set; }
        public string SupplierDescription { get; set; }
        public string SupplierFax { get; set; }
        public string SupplierDeliveryCode { get; set; }
        public string SupplierCountryCode { get; set; }
        public string SupplierEmailAddress { get; set; }
        public DateTime? DataIn { get; set; }
        public DateTime? DataOut { get; set; }
        public decimal? TotalAmount { get; set; }
        /// <summary>
        /// inserted property for parse voucher type
        /// </summary>
        public bool isVoucherCliente { get; set; }
        public bool FlgPaidCC { get { return !string.IsNullOrEmpty(this.WingsProductCCCode); } }
        public string BillingDocument { get; set; }
        public string ArchivePath { get; set; }
        public string ArchiveFileName { get; set; }
        public long VoucherIdInserted { get; set; }
    }
}
