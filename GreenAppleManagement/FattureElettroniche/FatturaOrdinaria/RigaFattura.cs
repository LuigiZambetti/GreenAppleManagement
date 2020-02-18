using FatturaElettronicaOrdinaria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WithNetFramework.FatturaOrdinaria
{
    public class RigaFattura
    {
        public int NumeroRiga { get; set; }
        public string Descrizione { get; set; }
        public decimal Quantita { get; set; }
        public decimal PrezzoUnitario { get; set; }
        public decimal PrezzoTotale { get; set; }
        public decimal AliquotaIva { get; set; }
        public bool IsQuantifiable { get; set; }
        public string UnitaMisura { get; set; }
        public NaturaType? Natura { get; set; }
    }
}
