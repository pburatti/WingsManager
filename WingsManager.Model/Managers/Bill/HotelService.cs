using System;
using System.Collections.Generic;
using System.Text;

namespace WingsManager.Model.Managers.Bill
{
    [Obsolete]
    public class HotelService
    {
        public decimal Tax { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public decimal RoomRate { get; set; }
        public string VoucherSeries { get; set; }
        public string VoucherCode { get; set; }
        public string HotelName { get; set; }
        public string RoomType { get; set; }
        public DateTime? CheckInDate { get; set; }
        public string PassengerName { get; set; }
    }
}
