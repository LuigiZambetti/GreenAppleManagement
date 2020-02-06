using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FatturaElettronicaOrdinaria;

namespace WithNetFramework.FatturaOrdinaria
{
    public static class FatturaOrdinariaBuilder
    {
        public static FatturaElettronicaType GetFatturaOrdinaria(IFatturaDataProvider data)
        {
            FatturaElettronicaType result = new FatturaElettronicaType();

            result.FatturaElettronicaHeader = GetFatturaElettronicaHeader(data);
            result.FatturaElettronicaBody = GetFatturaElettronicaBodyList(data);
            result.versione = FormatoTrasmissioneType.FPR12;

            return result;
        }

        #region Header
        public static FatturaElettronicaHeaderType GetFatturaElettronicaHeader(IFatturaDataProvider data)
        {
            FatturaElettronicaHeaderType result = new FatturaElettronicaHeaderType();

            result.DatiTrasmissione = GetDatiTrasmissione(data);
            result.CedentePrestatore = GetCedentePrestatore(data);
            result.CessionarioCommittente = GetCessionarioCommittente(data);

            //result.RappresentanteFiscale = GetRappresentanteFiscale();
            //result.SoggettoEmittente = SoggettoEmittenteType.CC;
            //result.SoggettoEmittenteSpecified = false;
            //result.TerzoIntermediarioOSoggettoEmittente = GetTerzoInterMediarioOSoggettoEmittente();

            return result;
        }

        public static TerzoIntermediarioSoggettoEmittenteType GetTerzoInterMediarioOSoggettoEmittente()
        {
            TerzoIntermediarioSoggettoEmittenteType result = new TerzoIntermediarioSoggettoEmittenteType();

            result.DatiAnagrafici = GetDatiAnagraficiTerzoIntermediarioType();

            return result;
        }

        public static DatiAnagraficiTerzoIntermediarioType GetDatiAnagraficiTerzoIntermediarioType()
        {
            DatiAnagraficiTerzoIntermediarioType result = new DatiAnagraficiTerzoIntermediarioType();

            //result.Anagrafica = GetAnagraficaType();
            result.CodiceFiscale = "";
            result.IdFiscaleIVA = GetIdFiscale("","");

            return result;
        }

        public static RappresentanteFiscaleType GetRappresentanteFiscale()
        {
            RappresentanteFiscaleType result = new RappresentanteFiscaleType();

            result.DatiAnagrafici = GetDatiAnagraficiRappresentanteType();

            return result;
        }

        public static DatiAnagraficiRappresentanteType GetDatiAnagraficiRappresentanteType()
        {
            DatiAnagraficiRappresentanteType result = new DatiAnagraficiRappresentanteType();

            //result.Anagrafica = GetAnagraficaType();
            result.CodiceFiscale = "";
            result.IdFiscaleIVA = GetIdFiscale("", "");

            return result;

        }

        public static DatiTrasmissioneType GetDatiTrasmissione(IFatturaDataProvider data)
        {
            DatiTrasmissioneType result = new DatiTrasmissioneType();

            result.CodiceDestinatario = data.CodiceDestinatario;
            result.FormatoTrasmissione = data.FormatoTrasmissione;
            result.IdTrasmittente = GetIdFiscale(data.CedentePIva, data.CedenteCodicePaese);
            result.ProgressivoInvio = data.NumeroProgressivo.ToString();

            //result.ContattiTrasmittente = GetContattiTrasmittenteType();
            //result.PECDestinatario = data.PECDestinatario;

            return result;
        }

        public static ContattiTrasmittenteType GetContattiTrasmittenteType(string email = "", string telefono = "")
        {
            ContattiTrasmittenteType result = new ContattiTrasmittenteType();

            result.Email = email;
            result.Telefono = telefono;

            return result;
        }

        public static CessionarioCommittenteType GetCessionarioCommittente(IFatturaDataProvider data)
        {
            CessionarioCommittenteType result = new CessionarioCommittenteType();

            result.DatiAnagrafici = GetDatiAnagraficiCessionario(data);
            result.Sede = GetIndirizzoType(data.CessionarioCAP, data.CessionarioCitta, data.CessionarioSedeIndirizzo, data.CessionarioNazione, data.CessionarioSedeCivico, data.CessionarioProvincia);

            //result.RappresentanteFiscale = GetRappresentanteFiscaleCessionarioType();
            //result.StabileOrganizzazione = GetIndirizzoType("", "", "", "", "", "");

            return result;
        }

        public static RappresentanteFiscaleCessionarioType GetRappresentanteFiscaleCessionarioType()
        {
            RappresentanteFiscaleCessionarioType result = new RappresentanteFiscaleCessionarioType();

            result.IdFiscaleIVA = GetIdFiscale("", "");
            result.Items = new List<string>() { "uno", "due" }.ToArray();
            result.ItemsElementName = new List<ItemsChoiceType1>() { ItemsChoiceType1.Nome, ItemsChoiceType1.Cognome }.ToArray();

            return result;
        }

        public static DatiAnagraficiCessionarioType GetDatiAnagraficiCessionario(IFatturaDataProvider data)
        {
            DatiAnagraficiCessionarioType result = new DatiAnagraficiCessionarioType();

            result.Anagrafica = GetAnagraficaOnlyDenominazioneType(data.CessionarioRagSociale);
            //if(!data.SkipValidation()) //skipvalidation è true quando si tratta di un cliente estero
            //{
            if (!string.IsNullOrEmpty(data.CessionarioPartitaIva))
                result.IdFiscaleIVA = GetIdFiscale(data.CessionarioPartitaIva, data.CessionarioCodicePaese);
            else
                result.CodiceFiscale = data.CessionarioCodiceFiscale;
            //}

            return result;
        }

        public static CedentePrestatoreType GetCedentePrestatore(IFatturaDataProvider data)
        {
            CedentePrestatoreType result = new CedentePrestatoreType();

            result.DatiAnagrafici = GetDatiAnagraficiCedente(data);
            result.Sede = GetIndirizzoType(data.CedenteCAP, data.CedenteCitta, data.CedenteIndirizzo, data.CedenteCodicePaese, data.CedenteCivico.ToString(), data.CedenteProvincia);

            //result.Contatti = GetContattiType();
            //result.IscrizioneREA = GetIscrizioneREA();
            //result.RiferimentoAmministrazione = "";
            //result.StabileOrganizzazione = GetIndirizzoType("", "", "", "", "", "");

            return result;
        }

        public static IscrizioneREAType GetIscrizioneREA()
        {
            return new IscrizioneREAType() {
                CapitaleSociale = 22,
                NumeroREA = "",
                CapitaleSocialeSpecified = true,
                SocioUnico = SocioUnicoType.SM,
                SocioUnicoSpecified = true,
                StatoLiquidazione = StatoLiquidazioneType.LN,
                Ufficio = "ufficio"
            };
        }


        public static ContattiType GetContattiType(string email = "", string fax = "", string telefono = "")
        {
            return new ContattiType() { Email = "", Fax = "", Telefono = "" };
        }

        public static DatiAnagraficiCedenteType GetDatiAnagraficiCedente(IFatturaDataProvider data)
        {
            DatiAnagraficiCedenteType result = new DatiAnagraficiCedenteType();

            
            result.Anagrafica = GetAnagraficaOnlyDenominazioneType(data.CedenteRagSoc);
            result.IdFiscaleIVA = GetIdFiscale(data.CedentePIva,data.CedenteCodicePaese);
            result.RegimeFiscale = RegimeFiscaleType.RF01;
            
            //result.CodiceFiscale = "";
            //result.DataIscrizioneAlbo = new DateTime();
            //result.DataIscrizioneAlboSpecified = false;
            //result.NumeroIscrizioneAlbo = "";
            //result.ProvinciaAlbo = 
            //result.AlboProfessionale = "";

            return result;
        }

        public static AnagraficaType GetAnagraficaOnlyDenominazioneType(string denominazione)
        {
            AnagraficaType result = new AnagraficaType();

            //result.CodEORI = "";
            result.Items = new List<string>() { denominazione }.ToArray();
            result.ItemsElementName = new List<ItemsChoiceType>() { ItemsChoiceType.Denominazione}.ToArray();
            //result.Titolo = "";

            return result;
        }

        #endregion

        #region Body

        public static FatturaElettronicaBodyType[] GetFatturaElettronicaBodyList(IFatturaDataProvider data)
        {
            List<FatturaElettronicaBodyType> result = new List<FatturaElettronicaBodyType>();

            result.Add(GetFatturaElettronicaBody(data));

            return result.ToArray();
         }

        public static FatturaElettronicaBodyType GetFatturaElettronicaBody(IFatturaDataProvider data)
        {
            FatturaElettronicaBodyType result = new FatturaElettronicaBodyType();

            result.DatiGenerali = GetDatiGeneraliType(data);

            result.DatiBeniServizi = GetDatiBeniServizi(data);

            result.DatiPagamento = GetDatiPagamentoTypeList(data);
            //result.Allegati = new AllegatiType[0];
            //result.DatiVeicoli = GetDatiVeicolo();

            return result;
        }

        public static DatiPagamentoType[] GetDatiPagamentoTypeList(IFatturaDataProvider data)
        {
            List<DatiPagamentoType> result = new List<DatiPagamentoType>();

            result.Add(GetDatiPagamentoType(data));

            return result.ToArray();
        }

        public static DatiPagamentoType GetDatiPagamentoType(IFatturaDataProvider data)
        {
            DatiPagamentoType result = new DatiPagamentoType();

            result.CondizioniPagamento = CondizioniPagamentoType.TP01;
            result.DettaglioPagamento = GetDettaglioPagamentoList(data);

            return result;
        }

        public static DettaglioPagamentoType[] GetDettaglioPagamentoList(IFatturaDataProvider data)
        {
            List<DettaglioPagamentoType> result = new List<DettaglioPagamentoType>();

            result.Add(GetDettaglioPagamentoType(data));

            return result.ToArray();
        }

        public static DettaglioPagamentoType GetDettaglioPagamentoType(IFatturaDataProvider data)
        {
            DettaglioPagamentoType result = new DettaglioPagamentoType();

            //result.ABI = "";
            //result.Beneficiario = "";
            //result.BIC = "";
            //result.CAB = "";
            //result.CFQuietanzante = "";
            //result.CodicePagamento = "";
            //result.CodUfficioPostale = "";
            //result.CognomeQuietanzante = "";
            //result.DataDecorrenzaPenale = new DateTime();
            //result.DataDecorrenzaPenaleSpecified = false;
            //result.DataLimitePagamentoAnticipato = new DateTime();
            //result.DataLimitePagamentoAnticipatoSpecified = false;
            //result.DataRiferimentoTerminiPagamento = new DateTime();
            //result.DataRiferimentoTerminiPagamentoSpecified = false;
            //result.DataScadenzaPagamento = new DateTime();
            //result.DataScadenzaPagamentoSpecified = false;
            //result.GiorniTerminiPagamento = "3";
            result.IBAN = data.Iban;
            //result.ImportoPagamento = 123;
            //result.IstitutoFinanziario = "";
            //result.ModalitaPagamento = ModalitaPagamentoType.MP04;
            //result.NomeQuietanzante = "";
            //result.PenalitaPagamentiRitardati = 0;
            //result.PenalitaPagamentiRitardatiSpecified = false;
            //result.ScontoPagamentoAnticipato = 0;
            //result.ScontoPagamentoAnticipatoSpecified = false;
            //result.TitoloQuietanzante = "";                

            return result;
        }

        public static DatiGeneraliType GetDatiGeneraliType(IFatturaDataProvider data)
        {
            DatiGeneraliType result = new DatiGeneraliType();

            result.DatiGeneraliDocumento = GetDatiGeneraliDocumento(data);

            //result.FatturaPrincipale = GetFatturaPrincipale();
            //result.DatiContratto = GetDatiContratto();
            //result.DatiConvenzione = GetDatiConvenzione();
            //result.DatiDDT = GetDatiDDT();
            //result.DatiFattureCollegate = GetDatiFattureCollegate();
            //result.DatiOrdineAcquisto = GetDatiOrdineDiAqcquisto();
            //result.DatiRicezione = GetDatiRicezione();
            //result.DatiSAL = GetDatiSAL();
            //result.DatiTrasporto = GetDatiTrasporto();

            return result;
        }

        public static DatiGeneraliDocumentoType GetDatiGeneraliDocumento(IFatturaDataProvider data)
        {
            DatiGeneraliDocumentoType result = new DatiGeneraliDocumentoType();

            result.TipoDocumento = data.TipoDocumento;
            result.Divisa = data.Valuta;
            result.Data = data.DataEmissioneFattura.Value;
            result.Numero = data.NumeroProgressivo.ToString();
            result.ImportoTotaleDocumentoSpecified = true;
            result.ImportoTotaleDocumento = data.TotaleDeiTotali;
            result.Causale = new List<string>() { data.Causale }.ToArray();

            //result.DatiBollo = new DatiBolloType() { BolloVirtuale = BolloVirtualeType.SI, ImportoBollo = 0 };
            //result.DatiCassaPrevidenziale = new DatiCassaPrevidenzialeType[0];
            //result.DatiRitenuta = new DatiRitenutaType() { AliquotaRitenuta = 22, CausalePagamento = CausalePagamentoType.D, ImportoRitenuta = 0, TipoRitenuta = TipoRitenutaType.RT01 };
            //result.Arrotondamento = 0;
            //result.ArrotondamentoSpecified = false;
            //result.Art73 = Art73Type.SI;
            //result.Art73Specified = false;

            return result;
        }

        public static FatturaPrincipaleType GetFatturaPrincipale()
        {
            FatturaPrincipaleType result = new FatturaPrincipaleType();

            result.DataFatturaPrincipale = new DateTime();
            result.NumeroFatturaPrincipale = "1";

            return result;
        }

        public static DatiBeniServiziType GetDatiBeniServizi(IFatturaDataProvider data)
        {
            DatiBeniServiziType result = new DatiBeniServiziType();

            result.DatiRiepilogo = GetDatiRiepilogoList(data);
            result.DettaglioLinee = GetDettaglioLineeList(data.RigheFattura);

            return result;

        }

        private static DettaglioLineeType[] GetDettaglioLineeList(List<RigaFattura> righe)
        {
            List<DettaglioLineeType> result = new List<DettaglioLineeType>();

            foreach(RigaFattura r in righe)
            {
                if(r.IsQuantifiable)
                {
                    result.Add(new DettaglioLineeType()
                    {
                        NumeroLinea = r.NumeroRiga.ToString(),
                        AliquotaIVA = r.AliquotaIva,
                        PrezzoUnitario = r.PrezzoUnitario,
                        Quantita = r.Quantita,
                        PrezzoTotale = r.PrezzoTotale,
                        QuantitaSpecified = r.IsQuantifiable,
                        Descrizione = r.Descrizione,
                        UnitaMisura = r.UnitaMisura,
                        Natura = r.Natura.HasValue ? r.Natura.Value : NaturaType.N1,
                        NaturaSpecified = r.Natura.HasValue
                    });
                }
                else
                {
                    result.Add(new DettaglioLineeType()
                    {
                        NumeroLinea = r.NumeroRiga.ToString(),
                        AliquotaIVA = r.AliquotaIva,
                        PrezzoUnitario = r.PrezzoUnitario,
                        PrezzoTotale = r.PrezzoTotale,
                        QuantitaSpecified = r.IsQuantifiable,
                        Descrizione = r.Descrizione,
                        Natura = r.Natura.HasValue ? r.Natura.Value : NaturaType.N1,
                        NaturaSpecified = r.Natura.HasValue
                    });
                }
            }

            //result.AddRange(righe.Select(x => new DettaglioLineeType()
            //{
            //    NumeroLinea = x.NumeroRiga.ToString(),
            //    AliquotaIVA = x.AliquotaIva,
            //    PrezzoUnitario = x.PrezzoUnitario,
            //    Quantita = x.Quantita, 
            //    PrezzoTotale = x.PrezzoTotale,
            //    QuantitaSpecified = x.IsQuantifiable,
            //    Descrizione = x.Descrizione

            //}));

            return result.ToArray();
        }

       
        public static DatiRiepilogoType[] GetDatiRiepilogoList(IFatturaDataProvider data)
        {
            List<DatiRiepilogoType> result = new List<DatiRiepilogoType>();

            result.Add(GetDatiRiepilogoType(data));

            return result.ToArray();
        }

        public static DatiRiepilogoType GetDatiRiepilogoType(IFatturaDataProvider data)
        {
            DatiRiepilogoType result = new DatiRiepilogoType();

            result.ImponibileImporto = data.Totale;
            result.AliquotaIVA = data.Aliquota;
            result.Imposta = data.TotaleIva;
            result.Natura = data.NaturaRiepilogo.HasValue ? data.NaturaRiepilogo.Value : NaturaType.N1;
            result.NaturaSpecified = data.NaturaSpecified;
            result.EsigibilitaIVA = EsigibilitaIVAType.I;
            result.EsigibilitaIVASpecified = true;
            result.RiferimentoNormativo = (data.RiferimentoNormativo.Length > 0) ? data.RiferimentoNormativo : null;
            
            //result.Arrotondamento = 0;
            //result.ArrotondamentoSpecified = false;
            //result.SpeseAccessorie = 0;
            //result.SpeseAccessorieSpecified = false;

            return result;
        }

        #endregion

        #region generics
        public static IndirizzoType GetIndirizzoType(string CAP, string Comune, string Indirizzo, string Nazione, string NumeroCivico, string Provincia)
        {
            return new IndirizzoType() { CAP = CAP, Comune = Comune, Indirizzo = Indirizzo, Nazione = Nazione, NumeroCivico = NumeroCivico, Provincia = Provincia };
        }

        public static IdFiscaleType GetIdFiscale(string idCodice, string idPaese)
        {
            return new IdFiscaleType() { IdCodice = idCodice, IdPaese = idPaese };
        }
        #endregion
    }
}
