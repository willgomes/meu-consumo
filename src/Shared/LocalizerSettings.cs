namespace Shared;

public static class LocalizerSettings
{
    public const string NeutralCulture = "pt-BR";

    public static readonly string[] SupportedCultures = [NeutralCulture, "en-US", "es-MX"];

    public static readonly (string, string)[] SupportedCulturesWithName = [("Português Brasileiro", NeutralCulture), ("English", "en-US"), ("Spanish", "es-MX")];

    public const string CULTURE_KEY = "preferred-culture";

}