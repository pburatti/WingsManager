using System;
using System.Collections.Generic;
using System.Text;

namespace WingsManager.Model.Managers.Entity
{
    public class Supplier
    {
        public int? Id { get; set; }
        public int? Code { get; set; }
        public string Name { get; set; }
        public string FaxNumber { get; set; }
        public string CountryCode { get; set; }
        public string EmailAddress { get; set; }
        public int? SendVoucherType { get; set; }
    }
}
