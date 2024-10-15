using System;
using System.Collections.Generic;

#nullable disable

namespace WebInvoiceDatabase.Models
{
    public partial class WingsECEmessi
    {
        public int IdECEmesso { get; set; }
        public string ECNumero { get; set; }
        public DateTime? ECData { get; set; }
        public DateTime? ECDataStampa { get; set; }
        public int? ClienteCodice { get; set; }
        public string ClienteDescrizione { get; set; }
        public short? DocumentoTipo { get; set; }
        public string DocumentoDescrizione { get; set; }
        public string ECTesto { get; set; }
        public int? AgenziaCwtCodice { get; set; }
        public string ECFilePath { get; set; }
        public string ECFileName { get; set; }
        public byte? LastSend { get; set; }
        public decimal? ImpNettoIva { get; set; }
        public decimal? ImpIva { get; set; }
        public decimal? ImpTotale { get; set; }
        public decimal? ImpPagato { get; set; }
        public decimal? ImpBollo { get; set; }
        public decimal? ImpSaldo { get; set; }
        public short? ImportazioneNumero { get; set; }
        public DateTime? ImportazioneDataOra { get; set; }
        public DateTime? ImportazioneLastDataOra { get; set; }
        public int? NumeroPagine { get; set; }
        public short? ECInviatoCliente { get; set; }
        public DateTime? ECInviatoClienteData { get; set; }
        public short? ECArchiviato { get; set; }
        public DateTime? ECArchiviatoData { get; set; }
        public short? ECRistampato { get; set; }
        public bool? FlgOmessoInWI { get; set; }
        public bool? FlgDocManuale { get; set; }
        public bool? FlgOmessoInPA { get; set; }
        public bool? FlgInviatoPA { get; set; }
    }
}
