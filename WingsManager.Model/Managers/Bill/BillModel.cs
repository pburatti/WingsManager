using System;
using System.Collections.Generic;
using System.Text;

namespace WingsManager.Model.Managers.Bill
{
    [Obsolete]
    public class BillModel
    {
        public string Agency { get; set; }
        public string Number { get; set; }
        public DateTime? IssueDate { get; set; }
        public string ClientId { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? WingsNumber { get; set; }
        public string PassengerName { get; set; }
        public string Text { get; set; }
    }
}
