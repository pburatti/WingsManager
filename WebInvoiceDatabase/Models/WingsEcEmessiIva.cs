using System;
using System.Collections.Generic;

#nullable disable

namespace WebInvoiceDatabase.Models
{
    public partial class WingsEcEmessiIva
    {
        public int IdWingsEcEmessiIva { get; set; }
        public string EcNumero { get; set; }
        public string CodiceIVA { get; set; }
        public string AliquotaIVA { get; set; }
        public decimal? ImportoImponibile { get; set; }
        public decimal? ImportoIVA { get; set; }
        public DateTime? ECData { get; set; }
    }
}
