using System;
using System.Collections.Generic;
using System.Text;

namespace WingsManager.Model.Managers.Entity
{
    public class Voucher
    {
        public long? Id { get; set; }
        public int? CwtAgencyCode { get; set; }
        public int? HarpCode { get; set; }
        public int? ClientCode { get; set; }
        public string ClientName { get; set; }
        public int? DocumentId { get; set; }
        public DateTime? DocumentIssueDate { get; set; }
        public string PassengerName { get; set; }
        public string VoucherNumber { get; set; }
        public string VoucherType { get; set; }
        public int? VoucherSendType { get; set; }
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
        public bool? isVoucherCliente { get; set; }
        public bool? FlgPaidCC { get { return !string.IsNullOrEmpty(this.WingsProductCCCode); } }
        public string BillingDocument { get; set; }
        public string ArchivePath { get; set; }
        public string ArchiveFileName { get; set; }
    }
}
