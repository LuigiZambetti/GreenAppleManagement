using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FatturaElettronicaSemplificata;

namespace WithNetFramework
{
    public class FatturaSemplificataDataProviderWithEntity
    {
        private DB.Clienti _cliente;
        private DB.Documclienti _fattura;
        private List<DB.Prestazioni> _prestazioni;
        private List<DB.Servizi> _servizi;
        private List<DB.Lista_PrestazioniTipi> _categoriePrestazioni;
        public FatturaSemplificataDataProviderWithEntity(DB.Clienti cliente, DB.Documclienti fattura, List<DB.Prestazioni> prestazioni, List<DB.Servizi> servizi,List<DB.Lista_PrestazioniTipi> categoriePrestazioni)
        {
            _cliente = cliente;
            _fattura = fattura;
            _prestazioni = prestazioni;
            _servizi = servizi;
            _categoriePrestazioni = categoriePrestazioni;
        }

        /*
         RagSoc	                        Indirizzo	            Localita	            CAP	    Nazione	PIVA	    AliquotaIVA
        THE GREEN APPLE ITALIA S.r.l.	Via G.Agnesi, 3	        Milano 	                20135   Italia	11052530158	22
             */
             
        #region Trasmissione

        public FormatoTrasmissioneType FormatoTrasmissione { get { return FormatoTrasmissioneType.FSM10; } }
        public DateTime? DataEmissioneFattura { get { return _fattura.DCdata; } }
        public int NumeroProgressivo { get { return (int)_fattura.DCnumero; } }
        public string CodiceDestinatario { get { return "0000000"; } }
        public string PECDestinatario { get { return ""; } }
        
        #endregion

        #region Cedente
        public string CedenteRagSoc { get { return "THE GREEN APPLE ITALIA S.r.l."; } }

        public string CedenteIndirizzoCompleto { get { return "Via G.Agnesi, 3"; } }
        public int CedenteCivico { get { return 3; } }
        public string CedenteIndirizzo { get { return "Via G.Agnesi"; } }
        public string CedenteCitta { get { return "Milano"; } }
        public string CedenteCAP { get { return "20135"; } }
        public string CedentePIva { get { return "11052530158"; } }
        public string CedenteNazione { get { return "Italia"; } }
        public string CedenteProvincia { get { return "MI"; } }
        public string CedenteCodicePaese { get { return "IT"; } }
        #endregion

        #region CessionarioCommittente

        public string CessionarioCodiceFiscale { get { return _cliente.CliCF; } }
        public string CessionarioPartitaIva { get { return _cliente.CliIVA; } }
        public string CessionarioCodicePaese { get { return DecodeCountry(_cliente.CliNazione); } }//TODO: bisogna gestire tutti i codici, i paesi, e altri dati sballati nel DB

        public string CessionarioRagSociale { get { return _cliente.Cliragsoc; } }

        public string CessionarioSedeIndirizzoCompleto { get { return _cliente.Cliindirizzo; } }
        public string CessionarioSedeIndirizzo { get { return TryGetIndirizzo(_cliente.Cliindirizzo); } }
        public string CessionarioSedeCivico { get { return TryGetCivico(_cliente.Cliindirizzo); } }
        public string CessionarioCitta { get { return _cliente.Clilocalita; } }
        public string CessionarioProvincia { get { return _cliente.Cliprovincia; } }
        public string CessionarioCAP { get { return _cliente.CliCAP; } }
        public string CessionarioNazione { get { return _cliente.CliNazione; } }

        public string CessionarioPostaSedeIndirizzoCompleto { get { return _cliente.ClipostaIndirizzo; } }
        public string CessionarioPostaSedeIndirizzo { get { return TryGetIndirizzo(_cliente.ClipostaIndirizzo); } }
        public string CessionarioPostaSedeCivico { get { return TryGetCivico(_cliente.ClipostaIndirizzo); } }
        public string CessionarioPostaCitta { get { return _cliente.Clipostalocalita; } }
        public string CessionarioPostaProvincia { get { return _cliente.Clipostaprov; } }
        public string CessionarioPostaCAP { get { return _cliente.ClipostaCAP; } }
        public string CessionarioPostaNazione { get { return _cliente.ClipostaNazione; } }

        #endregion

        #region DatiGeneraliDocumento

        public TipoDocumentoType TipoDocumento { get { return DecodeTipoDocumento(_fattura.DCtipo); } }
        public string Valuta { get { return (_fattura.DCvaluta == null || _fattura.DCvaluta == "0") ? "EUR" : _fattura.DCvaluta; } }
        public decimal Aliquota { get { return 22.00M; } }

        #endregion


        #region utilities
        private string DecodeCountry(string country)
        {
            switch (country.ToUpper())
            {
                case "ITALIA":
                case "ITALY":
                case "IATLIA":
                case "MILANO":
                    return "IT";
                case "GERMANIA":
                case "GERMANY":
                    return "DE";
                case "FRANCIA":
                case "FRANCE":
                    return "FR";
                case "SVIZZERA":
                case "SUISSE":
                case "SWITZERLAND":
                    return "CH";
                case "REPUBBLICA CECA":
                case "CECOSLOVACCHIA":
                    return "CZ";
                case "SPAGNA":
                case "ESPANA":
                case "SPAIN":
                    return "ES";
                case "ENGLAND":
                case "UNITED KINGDON":
                case "UNITED KINGDOM":
                case "GB":
                case "INGHILTERRA":
                case "UK":
                case "GREAT BRITAIN":
                case "LONDON":
                    return "UK";
                case "NETHERLANDS":
                case "OLANDA":
                case "HOLLAND":
                case "THE NETHERLANDS":
                    return "NL";
                case "RUSSIA":
                    return "RU";
                case "GIAPPONE":
                case "TOKYIO":
                    return "JP";
                case "USA":
                case "UNITED STATES":
                case "FLORIDA":
                case "NEW YORK":
                    return "US";
                case "ROMANIA":
                    return "RO";
                case "AUSTRALIA":
                    return "AU";
                case "BULGARIA":
                    return "BG";
                case "SWEDEN":
                case "SVEZIA":
                    return "SE";
                case "DANIMARCA":
                    return "DK";
                case "MEXICO":
                    return "MX";
                case "CINA":
                case "CHINA":
                    return "CN";
                case "NORWAY":
                    return "NO";
                case "UKRAINE":
                    return "UA";
                case "POLONIA":
                    return "PL";
                case "GRECIA":
                case "THESSALONIKI GREECE":
                    return "GR";
                case "CYPRUS":
                case "LIMASSOL CYPRUS":
                    return "CY";
                case "LIBANO":
                    return "LB";
                case "TURKEY":
                    return "TR";
                default:
                    return "IT";
            }
        }

        private string TryGetCivico(string indCompleto)
        {
            string[] splittedSpace = indCompleto.Split(' ');
            string[] splitComma = indCompleto.Split(',');
            string[] splitPoint = indCompleto.Split('.');

            List<string> candidates = splittedSpace.Concat(splitComma).Concat(splitPoint).ToList();

            int dummyInt = 0;
            List<string> possibleResults = candidates.Where(x => int.TryParse(x, out dummyInt)).ToList();

            if (possibleResults.Any())
                return possibleResults.First();
            else
                return "";
        }

        private string TryGetIndirizzo(string indCompleto)
        {
            string[] splittedSpace = indCompleto.Split(' ');
            string[] splitComma = indCompleto.Split(',');
            string[] splitPoint = indCompleto.Split('.');

            List<string> candidates = splittedSpace.Concat(splitComma).Concat(splitPoint).ToList();

            int dummyInt = 0;
            List<string> possibleResults = candidates.Where(x => int.TryParse(x, out dummyInt)).ToList();


            if (possibleResults.Any())
            {
                string civico = possibleResults.First();
                int index = indCompleto.IndexOf(civico);
                string cleanAddress = (index < 0)
                    ? indCompleto
                    : indCompleto.Remove(index, civico.Length);

                return cleanAddress;
            }
                
            else
                return indCompleto;
        }
        
        private TipoDocumentoType DecodeTipoDocumento(string tipo)
        {
            if (tipo == "FATTURA")
                return TipoDocumentoType.TD07;
            else if (tipo == "NOTA DI CREDITO")
                return TipoDocumentoType.TD08;

            return TipoDocumentoType.TD07;
        }
        #endregion
    }

    
}
