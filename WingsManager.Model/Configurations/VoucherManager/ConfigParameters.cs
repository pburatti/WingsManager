using System;
using System.Collections.Generic;
using System.Text;

namespace WingsManager.Model.Configurations.VoucherManager
{
    public class ConfigParameters
    {
        public PatternsRegEx Patterns { get; set; }
        public FoldersPath FoldersPath { get; set; }
    }

    public class PatternsRegEx
    {
        public string AgencyCode { get; set; }
        public string HarpCode { get; set; }
        public string ClientCode { get; set; }
        public string DocumentId { get; set; }
        public string DocumentIssueDate { get; set; }
        public string PassengerName { get; set; }
        public string VoucherNumber { get; set; }
        public string VoucherType { get; set; }
        public string WingsProduct { get; set; }
        public string SupplierDescription { get; set; }
        public string SupplierFax_1 { get; set; }
        public string SupplierFax_2 { get; set; }
        public string SupplierDeliveryCode { get; set; }
        public string DataIn { get; set; }
        public string DataOut { get; set; }
        public string TotalAmount { get; set; }
        public string Commission { get; set; }
        public string Discount { get; set; }
        public string VoucherSeriesType { get; set; }
    }
    public class FoldersPath
    {
        public string Original { get; set; }
        public string Processed { get; set; }
        public string Error { get; set; }
    }
}
