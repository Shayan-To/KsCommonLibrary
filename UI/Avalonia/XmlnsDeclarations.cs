using Avalonia.Metadata;

[assembly: XmlnsPrefix(NS.KsCommonLibrary, "ks")]
[assembly: XmlnsDefinition(NS.KsCommonLibrary, NS.Ks_Avalonia)]
[assembly: XmlnsDefinition(NS.KsCommonLibrary, NS.Ks_Avalonia + ".Controls")]
[assembly: XmlnsDefinition(NS.KsCommonLibrary, NS.Ks_Avalonia + ".Converters")]
[assembly: XmlnsDefinition(NS.KsCommonLibrary, NS.Ks_Avalonia + ".Utils")]

static class NS
{
    public const string KsCommonLibrary = "KsCommonLibrary";
    public const string Ks_Avalonia = "Ks.Avalonia";
}
