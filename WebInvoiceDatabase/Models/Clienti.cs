using System;
using System.Collections.Generic;

#nullable disable

namespace WebInvoiceDatabase.Models
{
    public partial class Clienti
    {
        public int IDCliente { get; set; }
        public int? IDAgenziaCwt { get; set; }
        public decimal AgenziaCwtCodice { get; set; }
        public decimal ClienteCodice { get; set; }
        public string RagSoc { get; set; }
        public string Indirizzo { get; set; }
        public string Cap { get; set; }
        public string Citta { get; set; }
        public int? IDProvincia { get; set; }
        public string ProvinciaSigla { get; set; }
        public int? IDNazione { get; set; }
        public string NazioneSigla { get; set; }
        public string Telefono { get; set; }
        public string Fax { get; set; }
        public string Telex { get; set; }
        public string CodiceFiscale { get; set; }
        public string PartitaIva { get; set; }
        public int? IDCondizionePagamento { get; set; }
        public string CondizionePagamentoCodice { get; set; }
        public int? IDCAB { get; set; }
        public decimal? ABICodice { get; set; }
        public decimal? CABCodice { get; set; }
        public string ContoCorrente { get; set; }
        public string FisicaGiuridica { get; set; }
        public bool? ClienteMIS { get; set; }
        public decimal? Fido { get; set; }
        public decimal? CapSociale { get; set; }
        public int? IDDivisa { get; set; }
        public string DivisaSigla { get; set; }
        public int? IDGruppo { get; set; }
        public decimal? GruppoCodice { get; set; }
        public int? IDAzienda { get; set; }
        public decimal? AziendaCodice { get; set; }
        public int? IDAziendaFSPit { get; set; }
        public string AziendaFSPitSigla { get; set; }
        public decimal? GruppoFornitoriTerzi { get; set; }
        public int? IDPeriodicitaEC { get; set; }
        public decimal? PeriodicitaECCodice { get; set; }
        public int? IDCategoriaCliente { get; set; }
        public decimal? CategoriaClienteCodice { get; set; }
        public int? IDTrattamentoSconto { get; set; }
        public decimal? TrattamentoScontoCodice { get; set; }
        public int? IDFormaDiPagamento { get; set; }
        public string FormaDiPagamentoSigla { get; set; }
        public decimal? TotRendNonPag { get; set; }
        public decimal? TotNonRend { get; set; }
        public string Responsabile { get; set; }
        public decimal? PercMarkup { get; set; }
        public DateTime? EstrazioneDaData { get; set; }
        public DateTime? EstrazioneAData { get; set; }
        public DateTime? UltimoECEmessoData { get; set; }
        public int? AssegnatoUtente { get; set; }
        public decimal? ClienteCg { get; set; }
        public DateTime? Datastarttfee { get; set; }
        public DateTime? CreazioneData { get; set; }
        public int? CreazioneUtente { get; set; }
        public DateTime? LockDataOra { get; set; }
        public int? LockUserid { get; set; }
        public DateTime? UltAggDataOra { get; set; }
        public int? UltAggUserId { get; set; }
        public string Codaziendacliente { get; set; }
        public string WingsCodConsCont { get; set; }
        public string WingsCodConsStat { get; set; }
        public string WingsCodEssBase { get; set; }
        public byte? ViewImportiVoucher { get; set; }
        public bool? FlgBlackList { get; set; }
        public string CodiceIPA { get; set; }
        public string CodiceCIG { get; set; }
        public bool? FlgSplitIva { get; set; }
        public bool? FlgNDDpagataInCC { get; set; }
        public string ContattoPA { get; set; }
        public bool? FlgNoSendNDDPA { get; set; }
        public bool? FlgNoSendAllegBollePA { get; set; }
        public bool? FlgNoSendAllegECPA { get; set; }
        public bool? FlgNoSendSaldoZero { get; set; }
        public string CodiceIvaPA { get; set; }
        public string CodRifAmmPA { get; set; }
        public string NrContrattoPA { get; set; }
        public string PEC_Fatturazione { get; set; }
        public string IDDocumentoPA { get; set; }
        public string TerminiEmissioneFattura { get; set; }
        public bool? FlgFatturaElettronicaRidotta { get; set; }
        public bool? FlgNoSendFatturaElettronica { get; set; }
        public string EmailNotificaFatturaElettronica { get; set; }
        public bool? FlgNoSendReferenceFatturaElettronica { get; set; }
        public bool? FlgSendUnaFatturaElettronicaPerLotto { get; set; }
        public bool? FlgSend_PI_CodFis { get; set; }
        public bool? FlgB2GO { get; set; }
    }
}
