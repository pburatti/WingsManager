using System;
using System.Collections.Generic;

#nullable disable

namespace WebInvoiceDatabase.Models
{
    public partial class Fornitori
    {
        public int IDFornitore { get; set; }
        public decimal FornitoreCodice { get; set; }
        public string RagSoc { get; set; }
        public string Indirizzo { get; set; }
        public string NumCivico { get; set; }
        public string Citta { get; set; }
        public string CodPostale { get; set; }
        public int? IDProvincia { get; set; }
        public string ProvinciaSigla { get; set; }
        public string ProvinciaDescr { get; set; }
        public int? IDNazione { get; set; }
        public string NazioneSigla { get; set; }
        public string NazioneDescr { get; set; }
        public string NazInt { get; set; }
        public string TelPreInt { get; set; }
        public string TelPreNaz { get; set; }
        public string Telefono { get; set; }
        public string FaxPreInt { get; set; }
        public string FaxPreNaz { get; set; }
        public string Fax { get; set; }
        public string Telex { get; set; }
        public string FaxNumCwt { get; set; }
        public bool? NumeroFaxVerde { get; set; }
        public short? Posizione { get; set; }
        public short? NrPlanimetria { get; set; }
        public int? IDFornitoreClasse { get; set; }
        public string FornitoreClasseSigla { get; set; }
        public int? IDHotelCatena { get; set; }
        public string HotelCatenaCodice { get; set; }
        public int? IDHotelCategoria { get; set; }
        public string HotelCategoriaCodice { get; set; }
        public short? NrStanze { get; set; }
        public short? NrPiani { get; set; }
        public short? NrDoppie { get; set; }
        public short? NrSingole { get; set; }
        public string PagamentoTipo { get; set; }
        public decimal? PercCommissione { get; set; }
        public int? IDAereopCittaIata { get; set; }
        public string AereopCittaIataSigla { get; set; }
        public short? KmAereoporto { get; set; }
        public int? IDCittaIata { get; set; }
        public string IataCittaSigla { get; set; }
        public short? KmCitta { get; set; }
        public short? KmStazFS { get; set; }
        public string Contatto { get; set; }
        public string NrVerdePrenotaz { get; set; }
        public string GalileoCodice { get; set; }
        public string AmadeusCodice { get; set; }
        public string SabreCodice { get; set; }
        public decimal? HarpCodice { get; set; }
        public string TipoFornitore { get; set; }
        public string CodiceFiscale { get; set; }
        public string PartitaIva { get; set; }
        public string ForIntTipo { get; set; }
        public string ForIntCodice { get; set; }
        public decimal? PeriodoApe1 { get; set; }
        public decimal? PeriodoChi1 { get; set; }
        public decimal? PeriodoApe2 { get; set; }
        public decimal? PeriodoChi2 { get; set; }
        public string Annullato { get; set; }
        public string Email { get; set; }
        public decimal? Abi { get; set; }
        public decimal? Cab { get; set; }
        public string ContoCorrente { get; set; }
        public bool? FlgFornitoreTO { get; set; }
        public int? IDFornitorePadre { get; set; }
        public short? TipoInvioVoucher { get; set; }
        public string EmailInvioVoucher { get; set; }
        public string UserVoucherWebConf { get; set; }
        public DateTime? CreazioneData { get; set; }
        public string CreazioneUtente { get; set; }
        public DateTime? LockDataOra { get; set; }
        public int? LockUserid { get; set; }
        public DateTime? UltAggDataOra { get; set; }
        public int? UltAggUserId { get; set; }
        public string CodiceIPA { get; set; }
        public string PEC_Fatturazione { get; set; }
        public string CausaleSDI { get; set; }
        public bool? FlgSendUnaFatturaElettronicaPerLotto { get; set; }
        public string RagSocFiscale { get; set; }
        public string IndirizzoFiscale { get; set; }
        public string NrCivicoFiscale { get; set; }
        public string CodPostaleFiscale { get; set; }
        public string CittaFiscale { get; set; }
        public string NazioneSiglaFiscale { get; set; }
    }
}
