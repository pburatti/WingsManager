using System;
using System.Collections.Generic;
using System.Text;

namespace WingsManager.Model.Managers.Entity
{
    public class Client
    {
        public int? Id { get; set; }
        public int? Code { get; set; }
        public string Name { get; set; }
        public int? CwtAgencyCode { get; set; }
        public string WingsDeliveryCode { get; set; }
    }
}
