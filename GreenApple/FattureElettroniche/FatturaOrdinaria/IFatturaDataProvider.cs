using System;
using System.Collections.Generic;
using FatturaElettronicaOrdinaria;

namespace WithNetFramework.FatturaOrdinaria
{
    public interface IFatturaDataProvider
    {
        decimal Aliquota { get; }
        string Causale { get; }
        string CedenteCAP { get; }
        string CedenteCitta { get; }
        int CedenteCivico { get; }
        string CedenteCodicePaese { get; }
        string CedenteIndirizzo { get; }
        string CedenteIndirizzoCompleto { get; }
        string CedenteNazione { get; }
        string CedentePIva { get; }
        string CedenteProvincia { get; }
        string CedenteRagSoc { get; }
        string CessionarioCAP { get; }
        string CessionarioCitta { get; }
        string CessionarioCodiceFiscale { get; }
        string CessionarioCodicePaese { get; }
        string CessionarioNazione { get; }
        string CessionarioPartitaIva { get; }
        string CessionarioPostaCAP { get; }
        string CessionarioPostaCitta { get; }
        string CessionarioPostaNazione { get; }
        string CessionarioPostaProvincia { get; }
        string CessionarioPostaSedeCivico { get; }
        string CessionarioPostaSedeIndirizzo { get; }
        string CessionarioPostaSedeIndirizzoCompleto { get; }
        string CessionarioProvincia { get; }
        string CessionarioRagSociale { get; }
        string CessionarioSedeCivico { get; }
        string CessionarioSedeIndirizzo { get; }
        string CessionarioSedeIndirizzoCompleto { get; }
        string CodiceDestinatario { get; }
        string Iban { get; }
        DateTime? DataEmissioneFattura { get; }
        FormatoTrasmissioneType FormatoTrasmissione { get; }
        string NumeroProgressivo { get; }
        string PECDestinatario { get; }
        List<RigaFattura> RigheFattura { get; set; }
        TipoDocumentoType TipoDocumento { get; }
        decimal Totale { get; set; }
        decimal TotaleDeiTotali { get; set; }
        decimal TotaleIva { get; set; }
        string Valuta { get; }
        NaturaType? NaturaRiepilogo { get; }
        bool NaturaSpecified { get;}
        string RiferimentoNormativo { get; }
        bool SkipValidation();
        
    }
}