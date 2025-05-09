using MudBlazor;

namespace Layout;

public static class CustomThemes
{
    public static MudTheme GetThemes() => new()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "#FFB07C",        // Laranja Pastel (Botões e Destaques)
            Secondary = "#EFA77B",      // Damasco (Links e Ícones)
            Background = "#FFCBA4",     // Pêssego (Fundo Principal)
            TextPrimary = "#6B2E14",    // Marrom Avermelhado (Texto Principal)
            Surface = "#FFCBA4",        // Pêssego (Fundo Principal)
            DrawerBackground = "#FFCBA4", // Pêssego
            DrawerText = "#6B2E14",     // Marrom Avermelhado
            AppbarBackground = "#FFB07C", // Laranja Pastel
        },
        PaletteDark = new PaletteDark()
        {
            Primary = "#C14D36",        // Terracota (Botões e Destaques)
            Secondary = "#9A3B1B",      // Cobre Escuro (Links e Ícones)
            Background = "#E87722",      // Abóbora Profundo (Fundo Principal)
            TextPrimary = "#FFFFFF",    // Branco (Texto Principal)
            Surface = "#E87722",        // Abóbora Profundo (Fundo Principal)
            DrawerBackground = "#E87722", // Abóbora Profundo
            DrawerText = "#FFFFFF",      // Branco
            AppbarBackground = "#C14D36", // Terracota
        }
    };
}