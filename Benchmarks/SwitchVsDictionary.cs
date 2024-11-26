using System.Collections.Frozen;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

public class SwitchVsDictionary
{
    public class IndexDoc
    {
        public string TitleEn { get; set; }
        public string TitleDe { get; set; }
        public string TitleEs { get; set; }
        public string TitleFr { get; set; }
        public string TitleIt { get; set; }
        public string TitlePt { get; set; }
        public string TitlePl { get; set; }
        public string TitleCs { get; set; }
        public string TitleZh { get; set; }
        public string TitleJa { get; set; }
        public string TitleRo { get; set; }
        public string TitleSi { get; set; }
        public string TitleTh { get; set; }
        public string TitleNl { get; set; }
        public string TitleHr { get; set; }
    }
    
    private static readonly IReadOnlyDictionary<string, Func<IndexDoc, string>> TitleRead = new Dictionary<string, Func<IndexDoc, string>>
    {
        ["DE"] = doc => doc.TitleDe,
        ["EN"] = doc => doc.TitleEn,
        ["ES"] = doc => doc.TitleEs,
        ["FR"] = doc => doc.TitleFr,
        ["IT"] = doc => doc.TitleIt,
        ["PT"] = doc => doc.TitlePt,
        ["PL"] = doc => doc.TitlePl,
        ["CS"] = doc => doc.TitleCs,
        ["ZH"] = doc => doc.TitleZh,
        ["JA"] = doc => doc.TitleJa,
        ["RO"] = doc => doc.TitleRo,
        ["SI"] = doc => doc.TitleSi,
        ["TH"] = doc => doc.TitleTh,
        ["NL"] = doc => doc.TitleNl,
        ["HR"] = doc => doc.TitleHr
    }.ToFrozenDictionary();

    private static string GetText(IndexDoc doc, IEnumerable<string> languageCodes)
    {
        foreach (var languageCode in languageCodes)
        {
            if (string.Equals(languageCode, "DE", StringComparison.InvariantCultureIgnoreCase) && doc is { TitleDe.Length: >0 })
                return doc.TitleDe;
            if (string.Equals(languageCode, "EN", StringComparison.InvariantCultureIgnoreCase) && doc is { TitleEn.Length: >0 })
                return doc.TitleEn;
            if (string.Equals(languageCode, "ES", StringComparison.InvariantCultureIgnoreCase) && doc is { TitleEs.Length: >0 })
                return doc.TitleEs;
            if (string.Equals(languageCode, "FR", StringComparison.InvariantCultureIgnoreCase) && doc is { TitleFr.Length: >0 })
                return doc.TitleFr;
            if (string.Equals(languageCode, "IT", StringComparison.InvariantCultureIgnoreCase) && doc is { TitleIt.Length: >0 })
                return doc.TitleIt;
            if (string.Equals(languageCode, "PT", StringComparison.InvariantCultureIgnoreCase) && doc is { TitlePt.Length: >0 })
                return doc.TitlePt;
            if (string.Equals(languageCode, "PL", StringComparison.InvariantCultureIgnoreCase) && doc is { TitlePl.Length: >0 })
                return doc.TitlePl;
            if (string.Equals(languageCode, "CS", StringComparison.InvariantCultureIgnoreCase) && doc is { TitleCs.Length: >0 })
                return doc.TitleCs;
            if (string.Equals(languageCode, "ZH", StringComparison.InvariantCultureIgnoreCase) && doc is { TitleZh.Length: >0 })
                return doc.TitleZh;
            if (string.Equals(languageCode, "JA", StringComparison.InvariantCultureIgnoreCase) && doc is { TitleJa.Length: >0 })
                return doc.TitleJa;
            if (string.Equals(languageCode, "RO", StringComparison.InvariantCultureIgnoreCase) && doc is { TitleRo.Length: >0 })
                return doc.TitleRo;
            if (string.Equals(languageCode, "SI", StringComparison.InvariantCultureIgnoreCase) && doc is { TitleSi.Length: >0 })
                return doc.TitleSi;
            if (string.Equals(languageCode, "TH", StringComparison.InvariantCultureIgnoreCase) && doc is { TitleTh.Length: >0 })
                return doc.TitleTh;
            if (string.Equals(languageCode, "NL", StringComparison.InvariantCultureIgnoreCase) && doc is { TitleNl.Length: >0 })
                return doc.TitleNl;
            if (string.Equals(languageCode, "HR", StringComparison.InvariantCultureIgnoreCase) && doc is { TitleHr.Length: >0 })
                return doc.TitleHr;
        }

        return string.Empty;
    }

    [Benchmark]
    public void FirstElementDictionary()
    {
        var doc = new IndexDoc { TitleEn = "EN", TitleDe = "DE", TitleEs = "ES"};
        var result = new[] { "EN", "IT" }.Select(lc => TitleRead[lc](doc)).First(s => !string.IsNullOrEmpty(s));
    }
    
    [Benchmark]
    public void FirstElementSpecialMethod()
    {
        var doc = new IndexDoc { TitleEn = "EN", TitleDe = "DE", TitleEs = "ES"};
        var result = GetText(doc, ["EN", "IT"]);
    }
    
    [Benchmark]
    public void LastElementDictionary()
    {
        var doc = new IndexDoc { TitleHr = "HR", TitleDe = "DE", TitleEs = "ES"};
        var result = new[] { "EN", "IT", "DE", "HR" }.Select(lc => TitleRead[lc](doc)).First(s => !string.IsNullOrEmpty(s));
    }
    
    [Benchmark]
    public void LastElementSpecialMethod()
    {
        var doc = new IndexDoc { TitleHr = "HR", TitleDe = "DE", TitleEs = "ES"};
        var result = GetText(doc, ["EN", "IT", "DE", "HR"]);
    }
}