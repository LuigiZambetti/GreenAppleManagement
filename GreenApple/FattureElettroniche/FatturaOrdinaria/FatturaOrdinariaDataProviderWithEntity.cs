using FatturaElettronicaOrdinaria;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WithNetFramework.FatturaOrdinaria
{
    //TODO: RICCARDO prima di fare "archivia" bisogna compilare l'applicazione e assicurarsi che funzioni. Non hai implementato l'interfaccia e quindi si rompeva: bisognava o commentare l' "implements" come ho fatto io o soddisfare l'interfaccia
    public class FatturaOrdinariaDataProviderWithEntity : IFatturaDataProvider
    {
        private DB.Clienti _cliente;
        private DB.Documclienti _fattura;
        private List<DB.Prestazioni> _prestazioni;
        private List<DB.Servizi> _servizi;
        //private List<DB.Lista_PrestazioniTipi> _categoriePrestazioni;

        public FatturaOrdinariaDataProviderWithEntity(DB.Clienti cliente, DB.Documclienti fattura, List<DB.Prestazioni> prestazioni, List<DB.Servizi> servizi,/* List<DB.Lista_PrestazioniTipi> categoriePrestazioni*/ List<DB.Lista_CategoriaServizi> categorieServizi, List<DB.Lista_Tiposervizio> tipoServizi)
        {
            _cliente = cliente;
            _fattura = fattura;
            _prestazioni = prestazioni;
            _servizi = servizi;
            //_categoriePrestazioni = categoriePrestazioni;

            SetRigheFattura(categorieServizi, tipoServizi);
        }

        #region Trasmissione

        public FormatoTrasmissioneType FormatoTrasmissione { get { return FormatoTrasmissioneType.FPR12; } }
        public DateTime? DataEmissioneFattura { get { return _fattura.DCdata; } }
        public string NumeroProgressivo { get { return _fattura.DCnumero.ToString(); } }
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
        public string CessionarioCodicePaese { get { return DecodeCountry(_cliente.CliNazione); } }

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
        public decimal Aliquota { get { return ((_cliente.GACalcoloIVA.Value) ? 22 : 0); } }
        public decimal Totale { get; set; }
        public NaturaType? NaturaRiepilogo { get { return NaturaType.N1; } }
        public bool NaturaSpecified { get { return false; } }
        public string RiferimentoNormativo { get { return ""; } }
        public decimal TotaleIva { get; set; }

        public decimal TotaleDeiTotali { get; set; }

        public string Causale { get; set; }
        public string Iban
        {
            get
            {

                string iban = _fattura.DCPiazza.ToUpper();

                if (iban.Contains("IBAN"))
                    iban = iban.Replace("IBAN", "");

                if (iban.Contains(":"))
                    iban = iban.Replace(":", "");

                iban = iban.Replace(" ", "");

                return iban;
            }
        }
        #endregion
        
        #region righeFattura

        public List<RigaFattura> RigheFattura { get; set; }

        #endregion

        #region utilities
        public bool SkipValidation()
        {
            return false;
        }

        private void SetRigheFattura(List<DB.Lista_CategoriaServizi> categorieServizi, List<DB.Lista_Tiposervizio> tipoServizi)
        {
            List<RigaFattura> tutte = new List<RigaFattura>();

            int counter = 0;
            foreach (DB.Servizi s in _servizi)
            {
                counter++;
                string desc = string.Format("{0} - {1}", categorieServizi.First(x => x.CScodice == int.Parse(s.SRcategoria)).CSdescrizione, tipoServizi.First(x => x.CScodice == int.Parse(s.SRtiposervizio)).CSdescrizione);
                var ptot = (s.SRnumgiorni * s.SRImportoGA);
                
                //Se non c'è l'IVA bisogna aggiungere anche la natura -> N1 per art.15
                if (_cliente.GACalcoloIVA.Value)
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = counter,
                        Descrizione = desc,
                        Quantita = RoundToTwo(s.SRnumgiorni.Value),
                        PrezzoUnitario = RoundToTwo((decimal)s.SRImportoGA.Value),
                        PrezzoTotale = RoundToTwo((decimal)ptot.Value),
                        AliquotaIva = RoundToTwo(22.00M),
                        IsQuantifiable = true,
                        UnitaMisura = "Giorni"
                    });
                else
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = counter,
                        Descrizione = desc,
                        Quantita = RoundToTwo(s.SRnumgiorni.Value),
                        PrezzoUnitario = RoundToTwo((decimal)s.SRImportoGA.Value),
                        PrezzoTotale = RoundToTwo((decimal)ptot.Value),
                        AliquotaIva = RoundToTwo(0.00M),
                        IsQuantifiable = true,
                        UnitaMisura = "Giorni",
                        Natura = NaturaType.N1
                    });

            }

            //OVERTIME
            if (_servizi.Any(x => x.SRimportoOvertimeGA.Value > 0))
            {
                if((_cliente.GACalcoloIVA.Value))
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = ++counter,
                        Descrizione = "Overtime",
                        PrezzoTotale = RoundToTwo((decimal)_servizi.Select(x => x.SRimportoOvertimeGA.Value).Sum()),//(decimal)Math.Round(_servizi.Select(x => x.SRimportoOvertimeGA.Value).Sum(), 2),
                        AliquotaIva = RoundToTwo(22.00M),
                        IsQuantifiable = false
                    });
                else
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = ++counter,
                        Descrizione = "Overtime",
                        PrezzoTotale = RoundToTwo((decimal)_servizi.Select(x => x.SRimportoOvertimeGA.Value).Sum()),//(decimal)Math.Round(_servizi.Select(x => x.SRimportoOvertimeGA.Value).Sum(), 2),
                        AliquotaIva = RoundToTwo(0.00M),
                        IsQuantifiable = false,
                        Natura = NaturaType.N1
                    });

            }


            //DIRITTI PRODUZIONE
            if ((_cliente.GACalcoloIVA.Value))
                tutte.Add(new RigaFattura()
                {
                    NumeroRiga = ++counter,
                    Descrizione = "Diritti produzione",
                    PrezzoTotale = RoundToTwo((decimal)_prestazioni.First().PRdiritti.Value),
                    AliquotaIva = RoundToTwo(22.00M),
                    IsQuantifiable = false
                });
            else
                tutte.Add(new RigaFattura()
                {
                    NumeroRiga = ++counter,
                    Descrizione = "Diritti produzione",
                    PrezzoTotale = RoundToTwo((decimal)_prestazioni.First().PRdiritti.Value),
                    AliquotaIva = RoundToTwo(0.00M),
                    IsQuantifiable = false,
                    Natura = NaturaType.N1
                });

            this.RigheFattura = tutte;

            this.Totale = RoundToTwo(tutte.Select(x => x.PrezzoTotale).Sum()); //Math.Round(tutte.Select(x => x.PrezzoTotale).Sum(), 2);

            this.TotaleIva = RoundToTwo((_cliente.GACalcoloIVA.Value) ? this.Totale * 0.22M : 0); ;//(_cliente.GACalcoloIVA.Value) ? this.Totale * 0.22M : 0; //Math.Round((_cliente.GACalcoloIVA.Value) ? this.Totale * 0.22M : 0);


            this.TotaleDeiTotali = RoundToTwo(Totale + TotaleIva);//Math.Round(Totale + TotaleIva,2);
        }

        private decimal RoundToTwo(decimal inDec)
        {
            string decstring = inDec.ToString("N2");
            return decimal.Parse(decstring);
        }

        private string DecodeCountry(string country)
        {
            switch (country.ToUpper())
            {
                //case "AFRICA":
                //    ???;
                case "ARGENTINA":
                    return "AR";
                case "BRASIL":
                case "BRASILE":
                    return "BR";
                case "CANADA":
                    return "CA";
                case "EMIRATI ARABI":
                case "UNITED ARAB EMIRATES":
                    return "AE";
                case "ITALIA":
                case "ITALY":
                case "IATLIA":
                case "MILANO":
                    return "IT";
                case "IRELAND":
                case "IRLANDA":
                    return "IE";
                case "GERMANIA":
                case "GERMANY":
                case "MONACO":
                case "MUNCHEN":
                    return "DE";
                case "FINLAND":
                    return "FI";
                case "FRANCIA":
                case "FRANCE":
                case "FR":
                    return "FR";
                case "C.P. 23454 MEXICO":
                case "MESSICO":
                    return "MX";
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
                case "SINGAPORE":
                    return "SG";
                case "ENGLAND":
                case "UNITED KINGDON":
                case "UNITED KINGDOM":
                case "GB":
                case "INGHILTERRA":
                case "UK":
                case "GREAT BRITAIN":
                case "LONDON":
                case "GRAN BRETAGNA":
                case "REGNO UNITO":
                    return "UK";
                case "NETHERLANDS":
                case "OLANDA":
                case "HOLLAND":
                case "THE NETHERLANDS":
                case "NEDERLAND":
                    return "NL";
                case "RUSSIA":
                    return "RU";
                case "GIAPPONE":
                case "TOKYIO":
                case "JAPAN":
                case "TOKIO":
                    return "JP";
                case "USA":
                case "UNITED STATES":
                case "FLORIDA":
                case "NEW YORK":
                case "NY":
                case "UNITED STATES OF AMERICA":
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
                case "PORTOGALLO":
                    return "PT";
                case "TURCHIA":
                    return "TR";
                case "KUWAIT":
                    return "KW";
                case "MALESIA":
                    return "MY";
                case "REPUBLIC OF KOREA":
                case "SOUTH KOREA":
                    return "KR";
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
                return TipoDocumentoType.TD01;
            else if (tipo == "NOTA DI CREDITO")
                return TipoDocumentoType.TD04;

            return TipoDocumentoType.TD01;
        }
        #endregion

        private List<T> MapFromDataTable<T>(object o, DataTable dt)
        {
            List<T> result = new List<T>();
            if (dt != null && dt.Rows.Count > 0)
                foreach (DataRow row in dt.Rows)
                {
                    result.Add(MapFromDataRow<T>(o, row));
                }

            return result;
        }

        private T MapFromDataRow<T>(object destination, DataRow row)
        {
            PropertyInfo[] pinfos = destination.GetType().GetProperties();
            foreach (PropertyInfo pi in pinfos)
            {
                pi.SetValue(destination, (DBNull.Value == row[pi.Name]) ? null : row[pi.Name], null);
            }
            return (T) destination;
        }
    }

}
