namespace AdditionalInformationService
{
    public class Currency
    {
        public string code { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
    }

    public class Flags
    {
        public string png { get; set; }
        public string svg { get; set; }
    }

    public class Language
    {
        public string name { get; set; }
        public string iso639_1 { get; set; }
        public string iso639_2 { get; set; }
        public string nativeName { get; set; }
    }

    public class Root
    {
        public int area { get; set; }
        public string cioc { get; set; }
        public string flag { get; set; }
        public double gini { get; set; }
        public string name { get; set; }
        public Flags flags { get; set; }
        public List<int> latlng { get; set; }
        public string region { get; set; }
        public List<string> borders { get; set; }
        public string capital { get; set; }
        public string demonym { get; set; }
        public List<Language> languages { get; set; }
        public string subregion { get; set; }
        public List<string> timezones { get; set; }
        public string alpha2Code { get; set; }
        public string alpha3Code { get; set; }
        public List<Currency> currencies { get; set; }
        public string nativeName { get; set; }
        public int population { get; set; }
        public bool independent { get; set; }
        public string numericCode { get; set; }
        public List<string> callingCodes { get; set; }
        public List<string> topLevelDomain { get; set; }
        public double populationDensity { get; set; }
    }


}
