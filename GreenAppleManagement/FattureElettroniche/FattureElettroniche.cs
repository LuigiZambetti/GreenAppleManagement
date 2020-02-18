using FatturaElettronicaOrdinaria;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using WithNetFramework.FatturaOrdinaria;

namespace FattureElettroniche
{
    public static class FattureElettroniche
    {
        public static FatturaResult CreateFatturaOrdinaria(int numFattura)
        {
            IFatturaDataProvider dataProvider = new FatturaOrdinariaDataProviderWithSqlCommands(numFattura);

            List<string> errors = Validate(dataProvider);
            if(errors.Count() > 0)
            {
                return new FatturaResult() { xml = null, filename = "", errors = errors };
            }

            FatturaElettronicaType fatturaGenerated = FatturaOrdinariaBuilder.GetFatturaOrdinaria(dataProvider);

            XmlSerializer xsSubmit = new XmlSerializer(typeof(FatturaElettronicaOrdinaria.FatturaElettronicaType));

            var xml = "";
            using (var sww = new Utf8StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, fatturaGenerated);
                    xml = sww.ToString(); 
                }
            }

            
            return new FatturaResult() { xml = xml, filename = dataProvider.CedenteCodicePaese + dataProvider.CedentePIva + "_" + dataProvider.NumeroProgressivo + ".xml", errors = new List<string>() };
        }

        public static List<string> Validate(IFatturaDataProvider dataProvider)
        {
            List<string> errors = new List<string>();
            
            if(!dataProvider.SkipValidation())
            {
                if (dataProvider.CessionarioCAP == null || dataProvider.CessionarioCAP == string.Empty || dataProvider.CessionarioCAP.Length != 5)
                    errors.Add("Il CAP del cliente non è valorizzato");

                if ((dataProvider.CessionarioCodiceFiscale == null || dataProvider.CessionarioCodiceFiscale == string.Empty) && (dataProvider.CessionarioPartitaIva == null || dataProvider.CessionarioPartitaIva == string.Empty))
                    errors.Add("La partita iva o il codice fiscale del cliente non sono valorizzati, valorizzarne almeno uno");
            }
            
            return errors;
        }
    }

    public class FatturaResult
    {
        public string xml { get; set; }
        public string filename { get; set; }
        public List<string> errors { get; set; }
    }
}
