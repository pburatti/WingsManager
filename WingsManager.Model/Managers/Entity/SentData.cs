using System;
using System.Collections.Generic;
using System.Text;

namespace WingsManager.Model.Managers.Entity
{
    public class EmailFaxSendData
    {
        public long? Id { get; set; }
        public int? ApplicationId { get; set; }
        public long? DocumentId { get; set; }
        public string SenderName { get; set; }
        public string SenderEmailAddress { get; set; }
        public int? ReceiverType { get; set; }
        public int? EmailFax { get; set; }
        public string EmailFaxDestination { get; set; }
        public int? State { get; set; }
        public string StateDescription { get; set; }
        public DateTime? SentDate { get; set; }
    }
}
