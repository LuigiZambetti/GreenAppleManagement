using FatturaElettronicaOrdinaria;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using WithNetFramework.DB;
using System.Configuration;

namespace WithNetFramework.FatturaOrdinaria
{
    public class FatturaOrdinariaDataProviderWithSqlCommands : IFatturaDataProvider
    {
        private Clienti _cliente;
        private Documclienti _fattura;
        private List<Prestazioni> _prestazioni;
        private List<Servizi> _servizi;

        public FatturaOrdinariaDataProviderWithSqlCommands(int FatturaId)
        {
            _cliente = new DB.Clienti();
            _fattura = new DB.Documclienti();
            _prestazioni = new List<DB.Prestazioni>();
            _servizi = new List<DB.Servizi>();

            string query = GetQuery(FatturaId);
            DataTable dt = new DataTable();
            Load_DataTable(query,ref dt);
            if (dt.Rows.Count > 0)
            {
                MapFromDataRow<DB.Clienti>(_cliente, dt.Rows[0]);
                MapFromDataRow<DB.Documclienti>(_fattura, dt.Rows[0]);
                _prestazioni = MapFromDataTable<DB.Prestazioni>(new DB.Prestazioni(), dt);
                //_servizi = MapFromDataTable<DB.Servizi>(new DB.Servizi(), dt);
                _servizi = MapServiziFromDataTable(dt);
            }
            else
                throw new Exception("Non è stato possibile creare la fattura, dati sul DB mancanti");

            SetRigheFattura();

        }

        #region Trasmissione

        public FormatoTrasmissioneType FormatoTrasmissione { get { return FormatoTrasmissioneType.FPR12; } }
        public DateTime? DataEmissioneFattura { get { return _fattura.DCdata; } }
        
        public string NumeroProgressivo { get 
            {
                string[] splitted = _fattura.DCnumeroCompleto.Split('/');
                if(splitted[0].Length <= 5)
                    return splitted[0];
                else
                    return _fattura.DCnumero.ToString(); 

                //int parsed;
                //if(int.TryParse(splitted[0],out parsed))
                //{
                //    return parsed;
                //}
                //else
                //    return (int)_fattura.DCnumero; 
            } 
        }
        
        public string CodiceDestinatario { get { return GetCodiceDestinatario(); } }
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
        public string CessionarioPartitaIva { get { return GetCessionarioPartitaIvaCleaned(); } }
        public string CessionarioCodicePaese { get { return DecodeCountry(_cliente.CliNazione); } }

        public string CessionarioRagSociale { get { return _cliente.Cliragsoc; } }

        public string CessionarioSedeIndirizzoCompleto { get { return _cliente.Cliindirizzo; } }

        //indirizzo con civico va bene
        //public string CessionarioSedeIndirizzo { get { return TryGetIndirizzo(_cliente.Cliindirizzo); } }
        public string CessionarioSedeIndirizzo { get { return _cliente.Cliindirizzo; } }

        //civico non obbligatorio se specificato in indirizzo
        //public string CessionarioSedeCivico { get { return TryGetCivico(_cliente.Cliindirizzo); } }
        public string CessionarioSedeCivico { get { return null; } }

        public string CessionarioCitta { get { return _cliente.Clilocalita; } }
        public string CessionarioProvincia { get { return _cliente.Cliprovincia; } }
        public string CessionarioCAP { get { return IsEstero() ? "00000" : _cliente.CliCAP; } }
        public string CessionarioNazione { get { return DecodeCountry(_cliente.CliNazione); } }

        public string CessionarioPostaSedeIndirizzoCompleto { get { return _cliente.ClipostaIndirizzo; } }

        //indirizzo con civico va bene
        //public string CessionarioPostaSedeIndirizzo { get { return TryGetIndirizzo(_cliente.ClipostaIndirizzo); } }
        public string CessionarioPostaSedeIndirizzo { get { return _cliente.ClipostaIndirizzo; } }

        //civico non obbligatorio se specificato in indirizzo
        //public string CessionarioPostaSedeCivico { get { return TryGetCivico(_cliente.ClipostaIndirizzo); } }
        public string CessionarioPostaSedeCivico { get { return ""; } }

        public string CessionarioPostaCitta { get { return _cliente.Clipostalocalita; } }
        public string CessionarioPostaProvincia { get { return _cliente.Clipostaprov; } }
        public string CessionarioPostaCAP { get { return IsEstero() ? "00000" : _cliente.ClipostaCAP; } }
        public string CessionarioPostaNazione { get { return  DecodeCountry(_cliente.ClipostaNazione); } }

        #endregion

        #region DatiGeneraliDocumento

        public TipoDocumentoType TipoDocumento { get { return DecodeTipoDocumento(_fattura.DCtipo); } }
        public string Valuta { get { return (_fattura.DCvaluta == null || _fattura.DCvaluta == "0") ? "EUR" : _fattura.DCvaluta; } }

        public string Iban { get 
        {
            if (_fattura.DCPiazza != "")
            {
                string iban = _fattura.DCPiazza.ToUpper();

                if (iban.Contains("IBAN"))
                    iban = iban.Replace("IBAN", "");

                if (iban.Contains(":"))
                    iban = iban.Replace(":", "");

                iban = iban.Replace(" ", "");

                return iban;
            }
            else
                return null;
            
        } }
        public decimal Aliquota { get { return ((_cliente.GACalcoloIVA.Value) ? 22.00M : 0.00M); } }
        public decimal Totale { get; set; }
        public NaturaType? NaturaRiepilogo { get; set; }
        public bool NaturaSpecified { get; set; }
        public string RiferimentoNormativo { 
            get
            {
                if (NaturaSpecified && NaturaRiepilogo.HasValue)
                {
                    if (NaturaRiepilogo == NaturaType.N3) // aggiunggiamo articolo 8 se la natura è N3
                        return "Non imponibile IVA art.8 I comma lettera C e II COMMA del D.P.R. 633/72 e successive modifiche";
                    else
                        return "Non soggetta - fuori campo";
                }
                else
                    return "";
                
            } 
        }

        public decimal TotaleIva { get; set; }

        public decimal TotaleDeiTotali { get; set; }

        public string Causale { get {
            string dicIntenti = (cleanUpSpecialCharacters(_cliente.GANote).Length > 0) ? cleanUpSpecialCharacters(_cliente.GANote) + " - " : "";
            string causale = dicIntenti  // questa è la dichiarazione intenti
                + "Dal " + _prestazioni[0].PRdatainizio.Value.ToString("dd/MM/yyyy") + 
                    " al " + _prestazioni[0].PRdatafine.Value.ToString("dd/MM/yyyy") + " "
                    + cleanUpSpecialCharacters(_prestazioni[0].PRnote) + " "; //Descrizione del lavoro svolto
                    

            //if (NaturaRiepilogo == NaturaType.N3) // aggiunggiamo articolo 8 se la natura è N3
            //    causale += "Non imponibile IVA art.8 I comma lettera C e II COMMA del D.P.R. 633/72 e successive modifiche"; 
            
            return (causale.Length <= 200) ? causale : causale.Substring(0, 200); 
            
            }
        }
        #endregion

        #region righeFattura

        public List<RigaFattura> RigheFattura { get; set; }

        #endregion

        #region utilities
        public bool SkipValidation()
        {
            return IsEstero();
        }

        
        private string GetCodiceDestinatario()
        {
            if (IsEstero())
                return "XXXXXXX";

            if (_cliente.CliCodDestinatario == null || _cliente.CliCodDestinatario == "")
                return "0000000";

            return _cliente.CliCodDestinatario.Trim();
        }

        private string cleanUpSpecialCharacters(string inString)
        {
            string outString = inString;
            
            //outString = outString.Replace("à", "&aacute;");
            //outString = outString.Replace("è", "&eacute;");
            //outString = outString.Replace("ì", "&iacute;");
            //outString = outString.Replace("ò", "&oacute;");
            //outString = outString.Replace("ù", "&uacute;");

            outString = outString.Replace("à", "a");
            outString = outString.Replace("è", "e");
            outString = outString.Replace("ì", "i");
            outString = outString.Replace("ò", "o");
            outString = outString.Replace("ù", "u");
            
            return outString;
        }

        private void SetRigheFattura_OLD()
        {
            List<RigaFattura> tutte = new List<RigaFattura>();

            int counter = 0;
            foreach (DB.Servizi s in _servizi)
            {
                counter++;
                string desc = cleanUpSpecialCharacters(string.Format("{0} - {1}", s.CategoriaDesc, s.TipoSvcDesc));
                var ptot = (s.SRnumgiorni * s.SRImportoGA);

                //Se non c'è l'IVA bisogna aggiungere anche la natura -> N1 per art.15, N3 per estero
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
                        UnitaMisura = "Giorni",
                        Natura = null
                    });
                else
                {
                    var currentNatura = IsEstero() ? NaturaType.N3 : NaturaType.N1;
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
                        Natura = currentNatura,
                    });
                }
                

            }

            //OVERTIME
            if (_servizi.Any(x => x.SRimportoOvertimeGA.Value > 0))
            {
                if ((_cliente.GACalcoloIVA.Value))
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = ++counter,
                        Descrizione = "Overtime",
                        PrezzoTotale = RoundToTwo((decimal)_servizi.Select(x => x.SRimportoOvertimeGA.Value).Sum()),//(decimal)Math.Round(_servizi.Select(x => x.SRimportoOvertimeGA.Value).Sum(), 2),
                        PrezzoUnitario = RoundToTwo((decimal)_servizi.Select(x => x.SRimportoOvertimeGA.Value).Sum()),//(decimal)Math.Round(_servizi.Select(x => x.SRimportoOvertimeGA.Value).Sum(), 2),
                        AliquotaIva = RoundToTwo(22.00M),
                        IsQuantifiable = false,
                        Natura = null
                    });
                else
                {
                    var currentNatura = IsEstero() ? NaturaType.N3 : NaturaType.N1;
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = ++counter,
                        Descrizione = "Overtime",
                        PrezzoTotale = RoundToTwo((decimal)_servizi.Select(x => x.SRimportoOvertimeGA.Value).Sum()),//(decimal)Math.Round(_servizi.Select(x => x.SRimportoOvertimeGA.Value).Sum(), 2),
                        PrezzoUnitario = RoundToTwo((decimal)_servizi.Select(x => x.SRimportoOvertimeGA.Value).Sum()),//(decimal)Math.Round(_servizi.Select(x => x.SRimportoOvertimeGA.Value).Sum(), 2),
                        AliquotaIva = RoundToTwo(0.00M),
                        IsQuantifiable = false,
                        Natura = currentNatura
                    });
                }
                    

            }

            if(_fattura.DCdiritti.Value > 0)
            {
                // DIRITTI PRODUZIONE
                if ((_cliente.GACalcoloIVA.Value))
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = ++counter,
                        Descrizione = "Diritti produzione",
                        PrezzoTotale = RoundToTwo((decimal)_fattura.DCdiritti.Value),
                        PrezzoUnitario = RoundToTwo((decimal)_fattura.DCdiritti.Value),
                        AliquotaIva = RoundToTwo(22.00M),
                        IsQuantifiable = false,
                        Natura = null
                    });
                else
                {
                    var currentNatura = IsEstero() ? NaturaType.N3 : NaturaType.N1;
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = ++counter,
                        Descrizione = "Diritti produzione",
                        PrezzoTotale = RoundToTwo((decimal)_fattura.DCdiritti.Value),
                        PrezzoUnitario = RoundToTwo((decimal)_fattura.DCdiritti.Value),
                        AliquotaIva = RoundToTwo(0.00M),
                        IsQuantifiable = false,
                        Natura = currentNatura
                    });
                }
                    
            }

            if(_prestazioni.First().PRspese != null && _prestazioni.First().PRspese > 0)
            {
                if ((_cliente.GACalcoloIVA.Value))
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = ++counter,
                        Descrizione = "Extensions",
                        PrezzoTotale = RoundToTwo((decimal)_prestazioni.First().PRspese),
                        PrezzoUnitario = RoundToTwo((decimal)_prestazioni.First().PRspese),
                        AliquotaIva = RoundToTwo(22.00M),
                        IsQuantifiable = false,
                        Natura = null
                    });
                else
                {
                    var currentNatura = IsEstero() ? NaturaType.N3 : NaturaType.N1;
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = ++counter,
                        Descrizione = "Extensions",
                        PrezzoTotale = RoundToTwo((decimal)_prestazioni.First().PRspese),
                        PrezzoUnitario = RoundToTwo((decimal)_prestazioni.First().PRspese),
                        AliquotaIva = RoundToTwo(0.00M),
                        IsQuantifiable = false,
                        Natura = currentNatura
                    });
                }
            }

            if (_prestazioni.First().PRspese1 != null && _prestazioni.First().PRspese1 > 0)
            {
                if ((_cliente.GACalcoloIVA.Value))
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = ++counter,
                        Descrizione = "Spese Viaggio",
                        PrezzoTotale = RoundToTwo((decimal)_prestazioni.First().PRspese1),
                        PrezzoUnitario = RoundToTwo((decimal)_prestazioni.First().PRspese1),
                        AliquotaIva = RoundToTwo(22.00M),
                        IsQuantifiable = false,
                        Natura = null
                    });
                else
                {
                    var currentNatura = IsEstero() ? NaturaType.N3 : NaturaType.N1;
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = ++counter,
                        Descrizione = "Spese Viaggio",
                        PrezzoTotale = RoundToTwo((decimal)_prestazioni.First().PRspese1),
                        PrezzoUnitario = RoundToTwo((decimal)_prestazioni.First().PRspese1),
                        AliquotaIva = RoundToTwo(0.00M),
                        IsQuantifiable = false,
                        Natura = currentNatura
                    });
                }
            }

            if (_prestazioni.First().PRspese2 != null && _prestazioni.First().PRspese2 > 0)
            {
                if ((_cliente.GACalcoloIVA.Value))
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = ++counter,
                        Descrizione = "Spese Varie",
                        PrezzoTotale = RoundToTwo((decimal)_prestazioni.First().PRspese2),
                        PrezzoUnitario = RoundToTwo((decimal)_prestazioni.First().PRspese2),
                        AliquotaIva = RoundToTwo(22.00M),
                        IsQuantifiable = false,
                        Natura = null
                    });
                else
                {
                    var currentNatura = IsEstero() ? NaturaType.N3 : NaturaType.N1;
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = ++counter,
                        Descrizione = "Spese Varie",
                        PrezzoTotale = RoundToTwo((decimal)_prestazioni.First().PRspese2),
                        PrezzoUnitario = RoundToTwo((decimal)_prestazioni.First().PRspese2),
                        AliquotaIva = RoundToTwo(0.00M),
                        IsQuantifiable = false,
                        Natura = currentNatura
                    });
                }
            }
            
            if(_fattura.DCSconto != null && _fattura.DCSconto > 0)
            {
                tutte.Add(new RigaFattura()
                {
                    NumeroRiga = ++counter,
                    Descrizione = "Sconto",
                    PrezzoTotale = RoundToTwo((decimal)_fattura.DCScontato * -1),
                    PrezzoUnitario = RoundToTwo((decimal)_fattura.DCScontato * -1),
                    AliquotaIva = RoundToTwo(0.00M),
                    IsQuantifiable = false,
                    Natura = null
                });
            }

            this.RigheFattura = tutte;

            this.Totale = RoundToTwo(tutte.Select(x => x.PrezzoTotale).Sum()); //Math.Round(tutte.Select(x => x.PrezzoTotale).Sum(), 2);

            this.TotaleIva = RoundToTwo((_cliente.GACalcoloIVA.Value) ? this.Totale * 0.22M : 0); ;//(_cliente.GACalcoloIVA.Value) ? this.Totale * 0.22M : 0; //Math.Round((_cliente.GACalcoloIVA.Value) ? this.Totale * 0.22M : 0);

            var isEsente = !_cliente.GACalcoloIVA.Value;

            if (isEsente)
            {
                this.NaturaSpecified = true;
                if (IsEstero())
                    this.NaturaRiepilogo = NaturaType.N3;
                else
                    this.NaturaRiepilogo = NaturaType.N1;
            }   
            else
            {
                this.NaturaRiepilogo = null;
                this.NaturaSpecified = false;
            }

            this.TotaleDeiTotali = RoundToTwo(Totale + TotaleIva);//Math.Round(Totale + TotaleIva,2);
        }

        private void SetRigheFattura()
        {
            List<RigaFattura> tutte = new List<RigaFattura>();

            int counter = 0;
            foreach (DB.Servizi s in _servizi)
            {
                counter++;
                //string desc = cleanUpSpecialCharacters(string.Format("{0} - {1} - {2}", s.CategoriaDesc, s.TipoSvcDesc, _prestazioni[0].PRnote));
                string desc = cleanUpSpecialCharacters(string.Format("{0} - {1}", s.CategoriaDesc, s.TipoSvcDesc));
                var ptot = (s.SRnumgiorni * s.SRImportoGA);

                //Se non c'è l'IVA bisogna aggiungere anche la natura -> N1 per art.15, N3 per estero
                if (_cliente.GACalcoloIVA.Value)
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = counter,
                        Descrizione = desc,
                        Quantita = RoundToTwo(s.SRnumgiorni.Value),
                        PrezzoUnitario = RoundToTwo((decimal)s.SRImportoGA.Value),
                        PrezzoTotale = RoundToTwo((decimal)ptot.Value),
                        AliquotaIva = RoundToTwo(_fattura.DCpercIVA.Value),
                        IsQuantifiable = true,
                        UnitaMisura = "Giorni",
                        Natura = null
                    });
                else
                {
                    //var currentNatura = IsEstero() ? NaturaType.N3 : NaturaType.N1;
                    var currentNatura = IsEstero() ? NaturaType.N2 : NaturaType.N3;
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
                        Natura = currentNatura,
                    });
                }


            }

            //OVERTIME
            if (_servizi.Any(x => x.SRimportoOvertimeGA.Value != 0))
            {
                if ((_cliente.GACalcoloIVA.Value))
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = ++counter,
                        Descrizione = "Overtime",
                        PrezzoTotale = RoundToTwo((decimal)_servizi.Select(x => x.SRimportoOvertimeGA.Value).Sum()),//(decimal)Math.Round(_servizi.Select(x => x.SRimportoOvertimeGA.Value).Sum(), 2),
                        PrezzoUnitario = RoundToTwo((decimal)_servizi.Select(x => x.SRimportoOvertimeGA.Value).Sum()),//(decimal)Math.Round(_servizi.Select(x => x.SRimportoOvertimeGA.Value).Sum(), 2),
                        AliquotaIva = RoundToTwo(_fattura.DCpercIVA.Value),
                        IsQuantifiable = false,
                        Natura = null
                    });
                else
                {
                    //var currentNatura = IsEstero() ? NaturaType.N3 : NaturaType.N1;
                    var currentNatura = IsEstero() ? NaturaType.N2 : NaturaType.N3;
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = ++counter,
                        Descrizione = "Overtime",
                        PrezzoTotale = RoundToTwo((decimal)_servizi.Select(x => x.SRimportoOvertimeGA.Value).Sum()),//(decimal)Math.Round(_servizi.Select(x => x.SRimportoOvertimeGA.Value).Sum(), 2),
                        PrezzoUnitario = RoundToTwo((decimal)_servizi.Select(x => x.SRimportoOvertimeGA.Value).Sum()),//(decimal)Math.Round(_servizi.Select(x => x.SRimportoOvertimeGA.Value).Sum(), 2),
                        AliquotaIva = RoundToTwo(0.00M),
                        IsQuantifiable = false,
                        Natura = currentNatura
                    });
                }


            }

            if (_fattura.DCdiritti.Value != 0)
            {
                // DIRITTI PRODUZIONE
                if ((_cliente.GACalcoloIVA.Value))
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = ++counter,
                        Descrizione = "Diritti produzione",
                        PrezzoTotale = RoundToTwo((decimal)_fattura.DCdiritti.Value),
                        PrezzoUnitario = RoundToTwo((decimal)_fattura.DCdiritti.Value),
                        AliquotaIva = RoundToTwo(_fattura.DCpercIVA.Value),
                        IsQuantifiable = false,
                        Natura = null
                    });
                else
                {
                    //var currentNatura = IsEstero() ? NaturaType.N3 : NaturaType.N1;
                    var currentNatura = IsEstero() ? NaturaType.N2 : NaturaType.N3;
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = ++counter,
                        Descrizione = "Diritti produzione",
                        PrezzoTotale = RoundToTwo((decimal)_fattura.DCdiritti.Value),
                        PrezzoUnitario = RoundToTwo((decimal)_fattura.DCdiritti.Value),
                        AliquotaIva = RoundToTwo(0.00M),
                        IsQuantifiable = false,
                        Natura = currentNatura
                    });
                }

            }

            if (_prestazioni.First().PRspese != null && _prestazioni.First().PRspese != 0)
            {
                if ((_cliente.GACalcoloIVA.Value))
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = ++counter,
                        Descrizione = "Extensions",
                        PrezzoTotale = RoundToTwo((decimal)_prestazioni.First().PRspese),
                        PrezzoUnitario = RoundToTwo((decimal)_prestazioni.First().PRspese),
                        AliquotaIva = RoundToTwo(_fattura.DCpercIVA.Value),
                        IsQuantifiable = false,
                        Natura = null
                    });
                else
                {
                    //var currentNatura = IsEstero() ? NaturaType.N3 : NaturaType.N1;
                    var currentNatura = IsEstero() ? NaturaType.N2 : NaturaType.N3;
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = ++counter,
                        Descrizione = "Extensions",
                        PrezzoTotale = RoundToTwo((decimal)_prestazioni.First().PRspese),
                        PrezzoUnitario = RoundToTwo((decimal)_prestazioni.First().PRspese),
                        AliquotaIva = RoundToTwo(0.00M),
                        IsQuantifiable = false,
                        Natura = currentNatura
                    });
                }
            }

            if (_prestazioni.First().PRspese1 != null && _prestazioni.First().PRspese1 != 0)
            {
                if ((_cliente.GACalcoloIVA.Value))
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = ++counter,
                        Descrizione = "Spese Viaggio",
                        PrezzoTotale = RoundToTwo((decimal)_prestazioni.First().PRspese1),
                        PrezzoUnitario = RoundToTwo((decimal)_prestazioni.First().PRspese1),
                        AliquotaIva = RoundToTwo(_fattura.DCpercIVA.Value),
                        IsQuantifiable = false,
                        Natura = null
                    });
                else
                {
                    //var currentNatura = IsEstero() ? NaturaType.N3 : NaturaType.N1;
                    var currentNatura = IsEstero() ? NaturaType.N2 : NaturaType.N3;
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = ++counter,
                        Descrizione = "Spese Viaggio",
                        PrezzoTotale = RoundToTwo((decimal)_prestazioni.First().PRspese1),
                        PrezzoUnitario = RoundToTwo((decimal)_prestazioni.First().PRspese1),
                        AliquotaIva = RoundToTwo(0.00M),
                        IsQuantifiable = false,
                        Natura = currentNatura
                    });
                }
            }

            if (_prestazioni.First().PRspese2 != null && _prestazioni.First().PRspese2 != 0)
            {
                if ((_cliente.GACalcoloIVA.Value))
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = ++counter,
                        Descrizione = "Spese Varie",
                        PrezzoTotale = RoundToTwo((decimal)_prestazioni.First().PRspese2),
                        PrezzoUnitario = RoundToTwo((decimal)_prestazioni.First().PRspese2),
                        AliquotaIva = RoundToTwo(_fattura.DCpercIVA.Value),
                        IsQuantifiable = false,
                        Natura = null
                    });
                else
                {
                    //var currentNatura = IsEstero() ? NaturaType.N3 : NaturaType.N1;
                    var currentNatura = IsEstero() ? NaturaType.N2 : NaturaType.N3;
                    tutte.Add(new RigaFattura()
                    {
                        NumeroRiga = ++counter,
                        Descrizione = "Spese Varie",
                        PrezzoTotale = RoundToTwo((decimal)_prestazioni.First().PRspese2),
                        PrezzoUnitario = RoundToTwo((decimal)_prestazioni.First().PRspese2),
                        AliquotaIva = RoundToTwo(0.00M),
                        IsQuantifiable = false,
                        Natura = currentNatura
                    });
                }
            }

            if (_fattura.DCSconto != null && _fattura.DCSconto != 0)
            {
                tutte.Add(new RigaFattura()
                {
                    NumeroRiga = ++counter,
                    Descrizione = "Sconto",
                    PrezzoTotale = RoundToTwo((decimal)_fattura.DCScontato * -1),
                    PrezzoUnitario = RoundToTwo((decimal)_fattura.DCScontato * -1),
                    AliquotaIva = RoundToTwo(_fattura.DCpercIVA.Value),
                    IsQuantifiable = false,
                    Natura = null
                });
            }

            this.RigheFattura = tutte;

            this.Totale = RoundToTwo(tutte.Select(x => x.PrezzoTotale).Sum()); //Math.Round(tutte.Select(x => x.PrezzoTotale).Sum(), 2);

            this.TotaleIva = RoundToTwo((_cliente.GACalcoloIVA.Value) ? this.Totale * ((decimal)_fattura.DCpercIVA.Value / 100 ) : 0.00M); ;//(_cliente.GACalcoloIVA.Value) ? this.Totale * 0.22M : 0; //Math.Round((_cliente.GACalcoloIVA.Value) ? this.Totale * 0.22M : 0);

            var isEsente = !_cliente.GACalcoloIVA.Value;

            if (isEsente)
            {
                this.NaturaSpecified = true;
                if (IsEstero())
                    this.NaturaRiepilogo = NaturaType.N2;
                else
                    this.NaturaRiepilogo = NaturaType.N3;
            }
            else
            {
                this.NaturaRiepilogo = null;
                this.NaturaSpecified = false;
            }

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
                    return "GB";
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
                    result.Add((T)MapFromDataRow<T>((T)o, row));
                }

            return result;
        }

        private List<DB.Servizi> MapServiziFromDataTable(DataTable dt)
        {
            List<DB.Servizi> result = new List<DB.Servizi>();
            if (dt != null && dt.Rows.Count > 0)
                foreach (DataRow row in dt.Rows)
                {
                    result.Add((DB.Servizi)MapFromDataRow<DB.Servizi>(new DB.Servizi(), row));
                }

            return result;
        }

        private T MapFromDataRow<T>(T destination, DataRow row)
        {
            PropertyInfo[] pinfos = destination.GetType().GetProperties();
            foreach (PropertyInfo pi in pinfos)
            {
                if (row.Table.Columns.Contains(pi.Name))
                    pi.SetValue(destination, (DBNull.Value == row[pi.Name]) ? null : row[pi.Name], null);
            }
            return (T)destination;
        }

        private string GetQuery(int FatturaId)
        {
            return "SELECT " +
            "F.DCdata, F.DCnumero, F.DCnumeroCompleto, F.DCtipo, F.DCvaluta, F.DCSconto, F.DCScontato, F.DCDiritti, F.DCPercIva, F.DCIVA, F.DCPiazza," +
            "C.CliCF, C.CliIVA, C.CliNazione, C.Cliragsoc, C.Cliindirizzo, C.Clilocalita, C.Cliprovincia, C.CliCAP, C.CliNazione, C.ClipostaIndirizzo, C.Clipostalocalita, C.Clipostaprov, C.ClipostaCAP, C.ClipostaNazione, C.GACalcoloIVA, C.CliCodDestinatario, C.GANote, " +
            "S.SRnumgiorni, S.SRImportoGA, S.SRimportoOvertimeGA, " +
            "CS.CSdescrizione AS CategoriaDesc, " +
            "TS.CSdescrizione AS TipoSvcDesc, " +
            "P.PRdiritti, P.PRspese, P.PRspese1, P.PRspese2, P.PRnote, P.PRdatainizio, P.PRdatafine " +
            "FROM Documclienti F " +
            "JOIN Clienti C ON F.DCcliente = C.Clicodice " +
            "JOIN Prestazioni P ON F.DCid = P.PRfattura " +
            "JOIN Servizi S ON P.PRnumero = S.SRnumeroprestazione " +
            "JOIN Lista_CategoriaServizi CS ON S.SRcategoria = CS.CScodice " +
            "JOIN Lista_TipoServizio TS ON S.SRtiposervizio = TS.CScodice " +
            "WHERE F.DCid = " + FatturaId;
        }

        static public string Load_DataTable(string sqlQuery, ref DataTable DTReturn, string pConnectionstringName = "CnnStr")
        {
            string Ret = "";
            SqlDataAdapter DtAdp;
            string realConnectionString = System.Configuration.ConfigurationManager.AppSettings[pConnectionstringName];
            SqlConnection Cn = new SqlConnection(realConnectionString);
            try
            {
                Cn.Open();
                DtAdp = new SqlDataAdapter(sqlQuery, Cn);
                DtAdp.Fill(DTReturn);
                DtAdp.Dispose();
            }
            catch (System.Exception myError)
            {
            }
            finally
            {
                Cn.Close();
                Cn.Dispose();
            }
            return Ret;
        }

        public bool IsEstero()
        {
            return DecodeCountry(_cliente.CliNazione) != "IT";
        }

        public string GetCessionarioPartitaIvaCleaned()
        {
            string current = _cliente.CliIVA;

            current = current.Trim();
            current = current.Replace(" ", string.Empty);

            Regex regexStartsWith2Chars = new Regex("^[a-zA-Z]{2}");
            Match m = regexStartsWith2Chars.Match(current);
            if (m.Success)
                return current.Substring(2, current.Length - 2);
            else
                return current;
        }
    }
}
