//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WithNetFramework.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Prestazioni
    {
        public short PRnumero { get; set; }
        public Nullable<System.DateTime> PRdatainizio { get; set; }
        public Nullable<System.DateTime> PRdatafine { get; set; }
        public Nullable<int> PRcliente { get; set; }
        public string PRcategoria { get; set; }
        public Nullable<float> PRtipodiritti { get; set; }
        public Nullable<double> PRtotale { get; set; }
        public Nullable<double> PRdiritti { get; set; }
        public Nullable<double> PRanticipi { get; set; }
        public Nullable<double> PRtrattenute { get; set; }
        public Nullable<short> PRfattura { get; set; }
        public bool PRfatturato { get; set; }
        public bool PRfatturare { get; set; }
        public Nullable<System.DateTime> PRdatafattura { get; set; }
        public Nullable<int> PRnumerogiorni { get; set; }
        public string PRutilizzo { get; set; }
        public string PRcitinfat { get; set; }
        public string PRspedirea { get; set; }
        public string PRbuonoordine { get; set; }
        public Nullable<double> PRspese { get; set; }
        public Nullable<double> PRivaspese { get; set; }
        public Nullable<double> PRrivalsa { get; set; }
        public string PRnote { get; set; }
        public string PRcondpag { get; set; }
        public bool PRrivalsaSN { get; set; }
        public bool PRIVASN { get; set; }
        public Nullable<double> PRgiorni { get; set; }
        public Nullable<double> PRtotalefattura { get; set; }
        public bool PRpagata { get; set; }
        public string PRchiave { get; set; }
        public string PRdescSpese { get; set; }
        public Nullable<double> PRspese1 { get; set; }
        public string PRdescSpese1 { get; set; }
        public Nullable<double> PRspese2 { get; set; }
        public string PRdescSpese2 { get; set; }
        public string PRmemo { get; set; }
        public Nullable<double> PRImponibile { get; set; }
        public Nullable<double> PRpctIVA { get; set; }
        public Nullable<double> PRimportoIVA { get; set; }
        public Nullable<short> PRannoDoc { get; set; }
        public string PRlingua { get; set; }
        public string PRvaluta { get; set; }
        public Nullable<double> PRovertime { get; set; }
    }
}