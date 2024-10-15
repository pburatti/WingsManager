using System;
using System.Collections.Generic;

#nullable disable

namespace WebInvoiceDatabase.Models
{
    public partial class WingsVoucherEmessi
    {
        public long IdVoucherEmesso { get; set; }
        public int? PraticaNumeroWings { get; set; }
        public decimal? AgenziaCwtCodice { get; set; }
        public string VoucherSerie { get; set; }
        public string VoucherNumero { get; set; }
        public DateTime? EmissioneData { get; set; }
        public int? ClienteCodice { get; set; }
        public string ClienteDescrizione { get; set; }
        public string PaxNome { get; set; }
        public string FornitoreFax { get; set; }
        public string FornitoreDescrizione { get; set; }
        public DateTime? DataIn { get; set; }
        public DateTime? DataOut { get; set; }
        public short? Inviato { get; set; }
        public int? LastSend { get; set; }
        public string VoucherCliente { get; set; }
        public string VoucherFornitore { get; set; }
        public string NomeFileAcquisito { get; set; }
        public decimal? ImportoTotale { get; set; }
        public int? Slip { get; set; }
        public int? IdBollaEmessa { get; set; }
        public DateTime? AcquisizioneDataOra { get; set; }
        public string VoucherType { get; set; }
        public string HarpCodice { get; set; }
        public string WingsCodConsCont { get; set; }
        public string NazioneSigla { get; set; }
        public decimal? ImportoTotaleCliente { get; set; }
        public bool? FlgPagatoCC { get; set; }
        public string ProdottoSigla { get; set; }
    }
}
