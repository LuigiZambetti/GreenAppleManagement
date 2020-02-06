using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FatturaElettronicaSemplificata;

namespace WithNetFramework
{
    public static class FatturaSemplificataBuilderTest
    {
        //root
        public static FatturaElettronicaType GetFattura(FatturaSemplificataDataProviderWithEntity data)
        {
            var result =  new FatturaElettronicaType();

            result.FatturaElettronicaBody = GetFatturaElettronicaBodyList(data);
            result.FatturaElettronicaHeader = GetFatturaElettronicaHeader(data);
            result.versione = FormatoTrasmissioneType.FSM10;

            return result;
        }

        //L1
        public static FatturaElettronicaHeaderType GetFatturaElettronicaHeader(FatturaSemplificataDataProviderWithEntity data)
        {

            var result = new FatturaElettronicaHeaderType();

            result.CedentePrestatore = GetCedentePrestatore(data);
            result.CessionarioCommittente = GetCessionarioCommittente(data);
            result.DatiTrasmissione = new DatiTrasmissioneType() {
                CodiceDestinatario = data.CodiceDestinatario,
                FormatoTrasmissione = data.FormatoTrasmissione,
                IdTrasmittente = GetIdFiscale(data.CedentePIva,data.CedenteCodicePaese),
                PECDestinatario = data.PECDestinatario,
                ProgressivoInvio = data.NumeroProgressivo.ToString()
            };
            
            //TODO: mancano dati soggetto emittente, forse neanche serve
            //result.SoggettoEmittente = SoggettoEmittenteType. .CC;
            result.SoggettoEmittenteSpecified = false;

            return result;
        }

        public static FatturaElettronicaBodyType[] GetFatturaElettronicaBodyList(FatturaSemplificataDataProviderWithEntity data)
        {
            var result = new List<FatturaElettronicaBodyType>();

            result.Add(GetFatturaElettronicaBody(data));

            return result.ToArray();
        }

        public static FatturaElettronicaBodyType GetFatturaElettronicaBody(FatturaSemplificataDataProviderWithEntity data)
        {

            var result = new FatturaElettronicaBodyType();

            result.DatiGenerali = GetDatiGeneraliForBody(data);
            //result.DatiBeniServizi = GetDatiBeniServiziListForBody(data);
            result.Allegati = GetAllegatiTypeListForBody();

            return result;
        }

        //L2+
        
        //body
        public static AllegatiType[] GetAllegatiTypeListForBody()
        {
            var result = new List<AllegatiType>();

            result.Add(GetAllegatiTypeForBody());

            return result.ToArray();
        }
        
        //body
        public static AllegatiType GetAllegatiTypeForBody()
        {
            var result = new AllegatiType();

            return result;
        }

        //body
        public static DatiGeneraliType GetDatiGeneraliForBody(FatturaSemplificataDataProviderWithEntity data)
        {
            var result = new DatiGeneraliType();

            result.DatiGeneraliDocumento = GetDatiGeneraliDocumento(data);
            result.DatiFatturaRettificata = null;//GetDatiFatturaRettificata(data);

            return result;
        }
        
        //body
        //public static DatiBeniServiziType[] GetDatiBeniServiziListForBody(FatturaSemplificataDataProviderWithEntity data)
        //{
        //    var result = new List<DatiBeniServiziType>();

        //    foreach(SingleBodyItemDataProvider sbid in data.prestazioni)
        //    {

        //        result.Add(GetDatiBeniServiziForBody(sbid,data.Aliquota));
        //    }

        //    return result.ToArray();
        //}
        
        ////body
        //public static DatiBeniServiziType GetDatiBeniServiziForBody(SingleBodyItemDataProvider data, decimal Aliquota)
        //{
        //    var result = new DatiBeniServiziType();

        //    result.RiferimentoNormativo = "";
        //    result.NaturaSpecified = false;
        //    //result.Natura = null;
        //    result.Importo = (decimal) data.Importo;
        //    result.Descrizione = data.Descrizione;
        //    result.DatiIVA = GetDatiIVAType(Aliquota, data.Iva);

        //    return result;
        //}

        //header
        public static CedentePrestatoreType GetCedentePrestatore(FatturaSemplificataDataProviderWithEntity data)
        {
            CedentePrestatoreType result = new CedentePrestatoreType();

            result.CodiceFiscale = data.CedentePIva;
            result.IdFiscaleIVA = GetIdFiscale(data.CedentePIva, data.CedenteCodicePaese);
            result.Items = new List<string>() { data.CedenteRagSoc }.ToArray();
            result.ItemsElementName = new List<ItemsChoiceType>() { ItemsChoiceType.Denominazione }.ToArray();
            result.StabileOrganizzazione = GetIndirizzoType(data.CedenteCAP,data.CedenteCitta,data.CedenteIndirizzo,data.CedenteNazione,data.CedenteCivico.ToString(),data.CedenteProvincia);


            //result.IscrizioneREA = new IscrizioneREAType() { CapitaleSociale = 22, NumeroREA = "", CapitaleSocialeSpecified = true, SocioUnico = SocioUnicoType.SM, SocioUnicoSpecified = true, StatoLiquidazione = StatoLiquidazioneType.LN, Ufficio = "ufficio" };
            //result.RappresentanteFiscale = GetRappresentanteFiscale(GetIdFiscale("asdf", "asdf"), new List<string>() { "uno", "due" }.ToArray(), new List<ItemsChoiceType1>() { ItemsChoiceType1.Cognome, ItemsChoiceType1.Cognome, ItemsChoiceType1.Denominazione }.ToArray());
            //result.RegimeFiscale = RegimeFiscaleType.RF01;

            return result;
        }
        
        //header
        public static CessionarioCommittenteType GetCessionarioCommittente(FatturaSemplificataDataProviderWithEntity data)
        {
            var result = new CessionarioCommittenteType();

            if (!string.IsNullOrEmpty(data.CessionarioPartitaIva))
                result.IdentificativiFiscali = new IdentificativiFiscaliType() { IdFiscaleIVA = GetIdFiscale(data.CessionarioCodicePaese, data.CessionarioPartitaIva) };
            else
                result.IdentificativiFiscali = new IdentificativiFiscaliType { CodiceFiscale = data.CessionarioCodiceFiscale };



            result.AltriDatiIdentificativi = new AltriDatiIdentificativiType()
            {
                Items = new List<string>() { data.CessionarioRagSociale }.ToArray(),
                ItemsElementName = new List<ItemsChoiceType2>() { ItemsChoiceType2.Denominazione }.ToArray(),
                Sede = GetIndirizzoType(data.CessionarioCAP,data.CessionarioCitta,data.CessionarioSedeIndirizzo,data.CessionarioNazione, data.CessionarioSedeCivico,data.CessionarioProvincia),
                StabileOrganizzazione = GetIndirizzoType(data.CessionarioPostaCAP,data.CessionarioPostaCitta,data.CessionarioPostaSedeIndirizzo,data.CessionarioPostaNazione,data.CessionarioPostaSedeCivico,data.CessionarioPostaProvincia),

                //TODO: chissà se c'è
                RappresentanteFiscale = null//GetRappresentanteFiscale(GetIdFiscale("asdf", "asdf"), new List<string>() { "uno", "due" }.ToArray(), new List<ItemsChoiceType1>() { ItemsChoiceType1.Nome, ItemsChoiceType1.Cognome, ItemsChoiceType1.Denominazione }.ToArray()),
        };

            return result;
        }
        
        //leaves
        public static IndirizzoType GetIndirizzoType(string CAP, string Comune, string Indirizzo, string Nazione, string NumeroCivico, string Provincia)
        {
            return new IndirizzoType() { CAP = CAP, Comune = Comune, Indirizzo = Indirizzo, Nazione = Nazione, NumeroCivico = NumeroCivico, Provincia = Provincia };
        }

        public static IdFiscaleType GetIdFiscale(string idCodice, string idPaese)
        {
            return new IdFiscaleType() { IdCodice = idCodice, IdPaese = idPaese };
        }

        public static RappresentanteFiscaleType GetRappresentanteFiscale(IdFiscaleType idFiscale, string[] items, ItemsChoiceType1[] itemElementsName)
        {
            return new RappresentanteFiscaleType()
            {
                IdFiscaleIVA = idFiscale,
                Items = items,
                ItemsElementName = itemElementsName
            };
        }

        public static DatiGeneraliDocumentoType GetDatiGeneraliDocumento(FatturaSemplificataDataProviderWithEntity data)
        {
            DatiGeneraliDocumentoType result = new DatiGeneraliDocumentoType();

            result.Data = data.DataEmissioneFattura.Value;
            result.Divisa = data.Valuta;
            result.Numero = data.NumeroProgressivo.ToString();
            result.TipoDocumento = data.TipoDocumento;
            
            return result;
        }

        public static DatiFatturaRettificataType GetDatiFatturaRettificata()
        {
            DatiFatturaRettificataType result = new DatiFatturaRettificataType();

            result.DataFR = new DateTime();
            result.ElementiRettificati = "";
            result.NumeroFR = "";

            return result;
        }

        public static DatiIVAType GetDatiIVAType(decimal aliquota, double imposta)
        {
            DatiIVAType result = new DatiIVAType();

            result.Aliquota = aliquota;
            result.AliquotaSpecified = true;
            result.Imposta = (decimal)imposta;
            result.ImpostaSpecified = true;

            return result;
        }
    }
}
