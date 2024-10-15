using System;
using System.Collections.Generic;

#nullable disable

namespace WebInvoiceDatabase.Models
{
    public partial class InvioFaxEmail
    {
        public long IdInvioFaxEmailProg { get; set; }
        public long IdInvioFaxEmail { get; set; }
        public long? IdDocumento { get; set; }
        public int? IdApplicazione { get; set; }
        public string CallID { get; set; }
        public string NomeMittente { get; set; }
        public string EmailMittente { get; set; }
        public int? IdTipoDestinatario { get; set; }
        public int? FaxEmail { get; set; }
        public string FaxEmailDest { get; set; }
        public int? Stato { get; set; }
        public string DescStato { get; set; }
        public DateTime? DataOraInvio { get; set; }
        public string DataOraRicevuto { get; set; }
        public string FaxReceived { get; set; }
        public string NomeAllegati { get; set; }
        public byte? Visualizzare { get; set; }
    }
}
