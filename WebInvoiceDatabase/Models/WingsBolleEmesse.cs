using System;
using System.Collections.Generic;

#nullable disable

namespace WebInvoiceDatabase.Models
{
    public partial class WingsBolleEmesse
    {
        public int IdBollaEmessa { get; set; }
        public decimal? AgenziaCWTCodice { get; set; }
        public decimal? Slip { get; set; }
        public int? PraticaNumeroWings { get; set; }
        public short? OpeTipo { get; set; }
        public short? DocumentoTipo { get; set; }
        public decimal? AnnoFattura { get; set; }
        public DateTime? OpeData { get; set; }
        public DateTime? PartenzaData { get; set; }
        public string PNR { get; set; }
        public string PaxNome { get; set; }
        public decimal? ClienteCodice { get; set; }
        public string ClRagSoc { get; set; }
        public decimal? AziendaCodice { get; set; }
        public string Matricola { get; set; }
        public string CentroDiCosto { get; set; }
        public string Autorizzazione { get; set; }
        public string CodiceRiferimento { get; set; }
        public string Buyer { get; set; }
        public string Bolla { get; set; }
        public byte? Inviato { get; set; }
        public byte? LastSend { get; set; }
        public string NomeAllegati { get; set; }
        public int? ImportazioneNumero { get; set; }
        public DateTime? ImportazioneDataOra { get; set; }
        public string NomeFile { get; set; }
        public bool? FlgInviatoPA { get; set; }
        public byte? PreInvioPA { get; set; }
        public bool? FlgOmessoInPA { get; set; }
    }
}
