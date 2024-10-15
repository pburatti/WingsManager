using System;
using System.Collections.Generic;

#nullable disable

namespace WebInvoiceDatabase.Models
{
    public partial class AgenzieCwt
    {
        public int IDAgenziaCwt { get; set; }
        public int IDPaeseCwt { get; set; }
        public decimal PaeseCwtCodice { get; set; }
        public decimal AgenziaCwtCodice { get; set; }
        public string Descr { get; set; }
        public string Indirizzo { get; set; }
        public string Indirizzo2 { get; set; }
        public string CodPostale { get; set; }
        public string Citta { get; set; }
        public int? IDProvincia { get; set; }
        public string ProvinciaSigla { get; set; }
        public int? IDNazione { get; set; }
        public string NazioneSigla { get; set; }
        public string Telefono { get; set; }
        public string Fax { get; set; }
        public string HotelSupportFax { get; set; }
        public string HotelSupportTel { get; set; }
        public string EMail { get; set; }
        public string IataCodice { get; set; }
        public string AgenziaResponsabile { get; set; }
        public DateTime? CessazioneData { get; set; }
        public string PseudoCityCode { get; set; }
        public string PseudoCityCodeSabre { get; set; }
        public string OrarioApertura { get; set; }
        public int? IPAgenziaCodice { get; set; }
        public int? AgenziaGruppoCodice { get; set; }
        public bool? Pubblica { get; set; }
        public short? TaxRefund { get; set; }
        public short? InvioVoucherTipo { get; set; }
        public bool? InvioVcrSr56 { get; set; }
        public bool? AgenziaChiusa { get; set; }
        public string TipoCartaIntestata { get; set; }
        public DateTime? LockDataOra { get; set; }
        public int? LockUserid { get; set; }
        public DateTime? UltAggDataOra { get; set; }
        public int? UltAggUserId { get; set; }
    }
}
