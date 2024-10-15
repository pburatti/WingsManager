using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebInvoiceDatabase.Models;

#nullable disable

namespace WebInvoiceDatabase.Context
{
    public partial class WebInvoiceContext : DbContext
    {
        public WebInvoiceContext()
        {
        }
        public WebInvoiceContext(DbContextOptions<WebInvoiceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AgenzieCwt> AgenzieCwts { get; set; }
        public virtual DbSet<Clienti> Clientis { get; set; }
        public virtual DbSet<Fornitori> Fornitoris { get; set; }
        public virtual DbSet<InvioFaxEmail> InvioFaxEmails { get; set; }
        public virtual DbSet<WingsBolleEmesse> WingsBolleEmesses { get; set; }
        public virtual DbSet<WingsECEmessi> WingsECEmessis { get; set; }
        public virtual DbSet<WingsEcEmessiIva> WingsEcEmessiIvas { get; set; }
        public virtual DbSet<WingsProdotti> WingsProdottis { get; set; }
        public virtual DbSet<WingsVoucherEmessi> WingsVoucherEmessis { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer(this._connectionString);
                //optionsBuilder.UseSqlServer(this._connectionString, builder =>
                //{//to avoid error of "An exception has been raised that is likely due to a transient failure" 18/07/2022
                //    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                //});

            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AgenzieCwt>(entity =>
            {
                entity.HasKey(e => e.IDAgenziaCwt);

                entity.ToTable("AgenzieCwt");

                entity.HasIndex(e => new { e.PaeseCwtCodice, e.AgenziaCwtCodice }, "IX_AgenzieCwt_PaeseCwt_AgenziaCwt_Key")
                    .IsUnique();

                entity.Property(e => e.AgenziaChiusa).HasDefaultValueSql("((0))");

                entity.Property(e => e.AgenziaCwtCodice).HasColumnType("numeric(3, 0)");

                entity.Property(e => e.AgenziaResponsabile)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.CessazioneData).HasColumnType("datetime");

                entity.Property(e => e.Citta)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CodPostale)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Descr)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.EMail)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Fax)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.HotelSupportFax)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.HotelSupportTel)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IataCodice)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Indirizzo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Indirizzo2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InvioVcrSr56).HasDefaultValueSql("((0))");

                entity.Property(e => e.InvioVoucherTipo).HasDefaultValueSql("((1))");

                entity.Property(e => e.LockDataOra).HasColumnType("datetime");

                entity.Property(e => e.NazioneSigla)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.OrarioApertura)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PaeseCwtCodice).HasColumnType("numeric(3, 0)");

                entity.Property(e => e.ProvinciaSigla)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.PseudoCityCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PseudoCityCodeSabre)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TipoCartaIntestata)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UltAggDataOra).HasColumnType("datetime");
            });

            modelBuilder.Entity<Clienti>(entity =>
            {
                entity.HasKey(e => e.IDCliente);

                entity.ToTable("Clienti");

                entity.HasIndex(e => e.ClienteCodice, "IX_ClienteCodice")
                    .IsUnique()
                    .HasFillFactor((byte)90);

                entity.Property(e => e.ABICodice).HasColumnType("numeric(5, 0)");

                entity.Property(e => e.AgenziaCwtCodice).HasColumnType("numeric(3, 0)");

                entity.Property(e => e.AziendaCodice).HasColumnType("numeric(7, 0)");

                entity.Property(e => e.AziendaFSPitSigla)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CABCodice).HasColumnType("numeric(5, 0)");

                entity.Property(e => e.Cap)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.CapSociale).HasColumnType("numeric(15, 2)");

                entity.Property(e => e.CategoriaClienteCodice).HasColumnType("numeric(3, 0)");

                entity.Property(e => e.Citta)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ClienteCg).HasColumnType("numeric(7, 0)");

                entity.Property(e => e.ClienteCodice).HasColumnType("numeric(7, 0)");

                entity.Property(e => e.CodRifAmmPA)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Codaziendacliente)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Codice azienda sul sistema informativo del cliente");

                entity.Property(e => e.CodiceCIG)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CodiceFiscale)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CodiceIPA)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("Indice delle Pubbliche Amministrazioni (IPA)");

                entity.Property(e => e.CodiceIvaPA)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CondizionePagamentoCodice)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ContattoPA)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ContoCorrente)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreazioneData).HasColumnType("datetime");

                entity.Property(e => e.Datastarttfee).HasColumnType("datetime");

                entity.Property(e => e.DivisaSigla)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.EmailNotificaFatturaElettronica)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.EstrazioneAData).HasColumnType("datetime");

                entity.Property(e => e.EstrazioneDaData).HasColumnType("datetime");

                entity.Property(e => e.Fax)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Fido).HasColumnType("numeric(15, 2)");

                entity.Property(e => e.FisicaGiuridica)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.FlgB2GO).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgBlackList).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgFatturaElettronicaRidotta).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgNDDpagataInCC)
                    .HasDefaultValueSql("((1))")
                    .HasComment("No Send NDD a saldo Zero");

                entity.Property(e => e.FlgNoSendAllegBollePA).HasDefaultValueSql("((1))");

                entity.Property(e => e.FlgNoSendAllegECPA).HasDefaultValueSql("((1))");

                entity.Property(e => e.FlgNoSendFatturaElettronica).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgNoSendNDDPA)
                    .HasDefaultValueSql("((1))")
                    .HasComment("Non inviare NDD alla PA");

                entity.Property(e => e.FlgNoSendReferenceFatturaElettronica).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgNoSendSaldoZero).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgSendUnaFatturaElettronicaPerLotto).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgSend_PI_CodFis).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgSplitIva)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Gestione Fatture PA");

                entity.Property(e => e.FormaDiPagamentoSigla)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.GruppoCodice).HasColumnType("numeric(7, 0)");

                entity.Property(e => e.GruppoFornitoriTerzi).HasColumnType("numeric(2, 0)");

                entity.Property(e => e.IDDocumentoPA)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Indirizzo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LockDataOra).HasColumnType("datetime");

                entity.Property(e => e.NazioneSigla)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.NrContrattoPA)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PEC_Fatturazione)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.PartitaIva)
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.PercMarkup).HasColumnType("numeric(5, 2)");

                entity.Property(e => e.PeriodicitaECCodice).HasColumnType("numeric(3, 0)");

                entity.Property(e => e.ProvinciaSigla)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.RagSoc)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Responsabile)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Telex)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TerminiEmissioneFattura)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TotNonRend).HasColumnType("numeric(13, 2)");

                entity.Property(e => e.TotRendNonPag).HasColumnType("numeric(13, 2)");

                entity.Property(e => e.TrattamentoScontoCodice).HasColumnType("numeric(3, 0)");

                entity.Property(e => e.UltAggDataOra).HasColumnType("datetime");

                entity.Property(e => e.UltimoECEmessoData).HasColumnType("datetime");

                entity.Property(e => e.ViewImportiVoucher).HasDefaultValueSql("((0))");

                entity.Property(e => e.WingsCodConsCont)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.WingsCodConsStat)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.WingsCodEssBase)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Fornitori>(entity =>
            {
                entity.HasKey(e => e.IDFornitore);

                entity.ToTable("Fornitori");

                entity.HasIndex(e => e.GalileoCodice, "IX_Fornitori_GalileoCodice");

                entity.HasIndex(e => e.HarpCodice, "IX_Fornitori_HarpCodice");

                entity.HasIndex(e => e.FornitoreCodice, "IX_Fornitori_Key")
                    .IsUnique();

                entity.Property(e => e.Abi).HasColumnType("numeric(5, 0)");

                entity.Property(e => e.AereopCittaIataSigla)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.AmadeusCodice)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Annullato)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Cab).HasColumnType("numeric(5, 0)");

                entity.Property(e => e.CausaleSDI)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Citta)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CittaFiscale)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CodPostale)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CodPostaleFiscale)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CodiceFiscale)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.CodiceIPA)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Contatto)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ContoCorrente)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreazioneData).HasColumnType("datetime");

                entity.Property(e => e.CreazioneUtente)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmailInvioVoucher)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Fax)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FaxNumCwt)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FaxPreInt)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.FaxPreNaz)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.FlgSendUnaFatturaElettronicaPerLotto).HasDefaultValueSql("((1))");

                entity.Property(e => e.ForIntCodice)
                    .HasMaxLength(17)
                    .IsUnicode(false);

                entity.Property(e => e.ForIntTipo)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.FornitoreClasseSigla)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.FornitoreCodice).HasColumnType("numeric(7, 0)");

                entity.Property(e => e.GalileoCodice)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.HarpCodice).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.HotelCategoriaCodice)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.HotelCatenaCodice)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.IataCittaSigla)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Indirizzo)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.IndirizzoFiscale)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.LockDataOra).HasColumnType("datetime");

                entity.Property(e => e.NazInt)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.NazioneDescr)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.NazioneSigla)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.NazioneSiglaFiscale)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.NrCivicoFiscale)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.NrVerdePrenotaz)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NumCivico)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroFaxVerde).HasDefaultValueSql("((0))");

                entity.Property(e => e.PEC_Fatturazione)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.PagamentoTipo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.PartitaIva)
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.PercCommissione).HasColumnType("numeric(5, 2)");

                entity.Property(e => e.PeriodoApe1).HasColumnType("numeric(4, 0)");

                entity.Property(e => e.PeriodoApe2).HasColumnType("numeric(4, 0)");

                entity.Property(e => e.PeriodoChi1).HasColumnType("numeric(4, 0)");

                entity.Property(e => e.PeriodoChi2).HasColumnType("numeric(4, 0)");

                entity.Property(e => e.ProvinciaDescr)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.ProvinciaSigla)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.RagSoc)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.RagSocFiscale)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.SabreCodice)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TelPreInt)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.TelPreNaz)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Telex)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TipoFornitore)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.TipoInvioVoucher)
                    .HasDefaultValueSql("((1))")
                    .HasComment("1=Fax 2=Email");

                entity.Property(e => e.UltAggDataOra).HasColumnType("datetime");

                entity.Property(e => e.UserVoucherWebConf)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InvioFaxEmail>(entity =>
            {
                entity.HasKey(e => e.IdInvioFaxEmailProg)
                    .IsClustered(false);

                entity.ToTable("InvioFaxEmail");

                entity.HasIndex(e => e.DataOraInvio, "IX_InvioFaxEmail_DataOraInvio");

                entity.HasIndex(e => new { e.IdDocumento, e.Stato, e.IdApplicazione }, "IX_IdDocumento_Stato_IdApplicazione");

                entity.Property(e => e.CallID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DataOraInvio).HasColumnType("datetime");

                entity.Property(e => e.DataOraRicevuto)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DescStato)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmailMittente)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FaxEmailDest)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.FaxReceived)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NomeAllegati)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.NomeMittente)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Visualizzare).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<WingsBolleEmesse>(entity =>
            {
                //entity.HasNoKey();
                entity.HasKey(e => e.IdBollaEmessa)
                    .IsClustered(false);

                entity.ToTable("WingsBolleEmesse");

                entity.HasIndex(e => e.OpeData, "IX_WingsBolleEmesse_OpeData");

                entity.HasIndex(e => new { e.ClienteCodice, e.AnnoFattura, e.Slip }, "IX_WingsBolleEmesse_Cliente_Bolla");

                entity.HasIndex(e => new { e.OpeData, e.ClienteCodice }, "IX_WingsBolleEmesse_DtBolla_Cliente");

                entity.HasIndex(e => e.ImportazioneDataOra, "IX_WingsBolleEmesse_DtImportazione");

                entity.HasIndex(e => new { e.AgenziaCWTCodice, e.AnnoFattura, e.Slip, e.PraticaNumeroWings, e.ClienteCodice }, "IX_WingsBolleEmesse_Key")
                    .IsUnique();

                entity.HasIndex(e => e.OpeData, "IX_WingsBolleEmesse_OpeData");

                entity.Property(e => e.AgenziaCWTCodice).HasColumnType("decimal(3, 0)");

                entity.Property(e => e.AnnoFattura).HasColumnType("numeric(4, 0)");

                entity.Property(e => e.Autorizzazione)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.AziendaCodice).HasColumnType("decimal(7, 0)");

                entity.Property(e => e.Bolla).HasColumnType("text");

                entity.Property(e => e.Buyer)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CentroDiCosto)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ClRagSoc)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClienteCodice).HasColumnType("decimal(7, 0)");

                entity.Property(e => e.CodiceRiferimento)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.FlgInviatoPA).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgOmessoInPA).HasDefaultValueSql("((0))");

                entity.Property(e => e.IdBollaEmessa).ValueGeneratedOnAdd();

                entity.Property(e => e.ImportazioneDataOra)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Matricola)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NomeAllegati)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NomeFile)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OpeData).HasColumnType("datetime");

                entity.Property(e => e.PNR)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.PartenzaData).HasColumnType("datetime");

                entity.Property(e => e.PaxNome)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Slip).HasColumnType("decimal(7, 0)");
            });

            modelBuilder.Entity<WingsECEmessi>(entity =>
            {
                entity.HasKey(e => e.IdECEmesso)
                    .IsClustered(false);

                entity.ToTable("WingsECEmessi");

                entity.HasIndex(e => e.ECData, "IX_WingsECEmessi_ECData");

                entity.HasIndex(e => new { e.ECData, e.ClienteCodice }, "IX_WingsECEmessi_EcData_Cliente");

                entity.HasIndex(e => e.ECDataStampa, "IX_WingsECEmessi_dtStampa");

                entity.Property(e => e.ClienteDescrizione)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentoDescrizione)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ECArchiviato).HasDefaultValueSql("((0))");

                entity.Property(e => e.ECArchiviatoData).HasColumnType("datetime");

                entity.Property(e => e.ECData).HasColumnType("datetime");

                entity.Property(e => e.ECDataStampa).HasColumnType("datetime");

                entity.Property(e => e.ECFileName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ECFilePath)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.ECInviatoCliente).HasDefaultValueSql("((0))");

                entity.Property(e => e.ECInviatoClienteData).HasColumnType("datetime");

                entity.Property(e => e.ECNumero)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ECRistampato).HasDefaultValueSql("((0))");

                entity.Property(e => e.ECTesto).HasColumnType("text");

                entity.Property(e => e.FlgDocManuale)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Classifica documenti manipolati in direzione ");

                entity.Property(e => e.FlgInviatoPA).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgOmessoInPA).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgOmessoInWI)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Documento escluso da WI");

                entity.Property(e => e.ImpBollo).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ImpIva).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ImpNettoIva).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ImpPagato).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ImpSaldo).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ImpTotale).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ImportazioneDataOra).HasColumnType("datetime");

                entity.Property(e => e.ImportazioneLastDataOra).HasColumnType("datetime");
            });

            modelBuilder.Entity<WingsEcEmessiIva>(entity =>
            {
                entity.HasKey(e => e.IdWingsEcEmessiIva)
                    .IsClustered(false);

                entity.ToTable("WingsEcEmessiIva");

                entity.HasIndex(e => e.ECData, "IX_WingsEcEmessiIva_ECData");

                entity.HasIndex(e => new { e.EcNumero, e.CodiceIVA }, "IX_WingsEcEmessiIva_Key");

                entity.Property(e => e.AliquotaIVA)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.CodiceIVA)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ECData).HasColumnType("datetime");

                entity.Property(e => e.EcNumero)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ImportoIVA).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ImportoImponibile).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<WingsProdotti>(entity =>
            {
                entity.HasKey(e => e.IDProdotto);

                entity.ToTable("WingsProdotti");

                entity.HasIndex(e => e.ProdottoSigla, "IX_WingsProdotti")
                    .IsUnique();

                entity.Property(e => e.AccountNbr)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AgenziaCwtCodice).HasColumnType("decimal(3, 0)");

                entity.Property(e => e.CCSigla)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CodiceIvaSigla)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ContoContabileCliente)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DescrizioneCarta).IsUnicode(false);

                entity.Property(e => e.DtCreazione).HasColumnType("datetime");

                entity.Property(e => e.EmailInvioRiconciliazioneAida).IsUnicode(false);

                entity.Property(e => e.FlgAncillary).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgBMF).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgCAS).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgCommissionabile).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgGDS).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgGestioneAIDA).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgHipCip).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgLowCost).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgMerchantfee).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgMiniMeeting).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgNazInt)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.FlgPenalita).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgPrepaid).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgReferral).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgRiconciliaCC).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgRitornoComm).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgSIA).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgSpeseDiverse).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgStatistiche).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgTktElettronico).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgTransactionFee).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgUatp).HasDefaultValueSql("((0))");

                entity.Property(e => e.FlgcccWT)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica prodotto per addebito servizio pagato con carta credito cwt");

                entity.Property(e => e.NumeroCarta)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ProdottoDesc)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProdottoFee)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ProdottoSigla)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.SipaxModelloTkt)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.TipoProdottoSigla)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<WingsVoucherEmessi>(entity =>
            {
                //entity.HasNoKey();
                entity.HasKey(e => e.IdVoucherEmesso)
                    .IsClustered(false);

                entity.ToTable("WingsVoucherEmessi");

                entity.HasIndex(e => e.EmissioneData, "IX_WingsVoucherEmessi_EmissioneData");

                entity.HasIndex(e => new { e.AcquisizioneDataOra, e.NazioneSigla, e.VoucherSerie, e.WingsCodConsCont }, "IX_WingsVoucherEmessi_AcquisizioneDataOra_NazioneSigla_Serie_WingsCodConsCont");

                entity.HasIndex(e => new { e.AgenziaCwtCodice, e.EmissioneData }, "IX_WingsVoucherEmessi_AgenziaDataEmi");

                entity.HasIndex(e => e.IdVoucherEmesso, "IX_WingsVoucherEmessi_IdVoucherEmesso");

                entity.HasIndex(e => new { e.AgenziaCwtCodice, e.ClienteCodice, e.PraticaNumeroWings, e.VoucherSerie, e.VoucherNumero }, "IX_WingsVoucherEmessi_WingsVoucherEmessi")
                    .IsUnique();

                entity.Property(e => e.AcquisizioneDataOra)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AgenziaCwtCodice).HasColumnType("decimal(9, 0)");

                entity.Property(e => e.ClienteDescrizione)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DataIn).HasColumnType("datetime");

                entity.Property(e => e.DataOut).HasColumnType("datetime");

                entity.Property(e => e.EmissioneData).HasColumnType("datetime");

                entity.Property(e => e.FlgPagatoCC)
                    .HasDefaultValueSql("((0))")
                    .HasComment("In acquisizione del vcr si cerca di determinare la forma di pagamento da Genesis");

                entity.Property(e => e.FornitoreDescrizione)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FornitoreFax)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.HarpCodice)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ImportoTotale).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ImportoTotaleCliente).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.NazioneSigla)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.NomeFileAcquisito)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PaxNome)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProdottoSigla)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.VoucherCliente)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.VoucherFornitore)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.VoucherNumero)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.VoucherSerie)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.VoucherType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WingsCodConsCont)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
