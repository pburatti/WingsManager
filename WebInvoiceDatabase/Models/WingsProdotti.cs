using System;
using System.Collections.Generic;

#nullable disable

namespace WebInvoiceDatabase.Models
{
    public partial class WingsProdotti
    {
        public int IDProdotto { get; set; }
        public string ProdottoSigla { get; set; }
        public string ProdottoDesc { get; set; }
        public int? IDTipoProdotto { get; set; }
        public string TipoProdottoSigla { get; set; }
        public string FlgNazInt { get; set; }
        public bool? FlgTktElettronico { get; set; }
        public bool? FlgGDS { get; set; }
        public bool? FlgReferral { get; set; }
        public bool? FlgPrepaid { get; set; }
        public bool? FlgCommissionabile { get; set; }
        public bool? FlgSIA { get; set; }
        public bool? FlgTransactionFee { get; set; }
        public bool? FlgMerchantfee { get; set; }
        public bool? FlgRitornoComm { get; set; }
        public bool? FlgLowCost { get; set; }
        public bool? FlgSpeseDiverse { get; set; }
        public bool? FlgStatistiche { get; set; }
        public bool? FlgPenalita { get; set; }
        public string ProdottoFee { get; set; }
        public DateTime? DtCreazione { get; set; }
        public string SipaxModelloTkt { get; set; }
        public bool? FlgUatp { get; set; }
        public string ContoContabileCliente { get; set; }
        public bool? FlgcccWT { get; set; }
        public bool? FlgRiconciliaCC { get; set; }
        public bool? FlgAncillary { get; set; }
        public bool? FlgBMF { get; set; }
        public bool? FlgCAS { get; set; }
        public bool? FlgMiniMeeting { get; set; }
        public int? IDEvento { get; set; }
        public bool? FlgHipCip { get; set; }
        public string CodiceIvaSigla { get; set; }
        public string CCSigla { get; set; }
        public string NumeroCarta { get; set; }
        public decimal? AgenziaCwtCodice { get; set; }
        public bool? FlgGestioneAIDA { get; set; }
        public string EmailInvioRiconciliazioneAida { get; set; }
        public string DescrizioneCarta { get; set; }
        public string AccountNbr { get; set; }
    }
}
