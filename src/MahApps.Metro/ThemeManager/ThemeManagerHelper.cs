using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace MahApps.Metro
{
    public static class ThemeManagerHelper
    {

        public static Dictionary<string, Color> AccentColors { get; private set; }

        static ThemeManagerHelper()
        {
            AccentColors = new Dictionary<string, Color>();
            AccentColors.Add("Amber", (Color)ColorConverter.ConvertFromString("#FFF0A30A"));
            AccentColors.Add("Blue", (Color)ColorConverter.ConvertFromString("#FF119EDA"));
            AccentColors.Add("Brown", (Color)ColorConverter.ConvertFromString("#FF825A2C"));
            AccentColors.Add("Cobalt", (Color)ColorConverter.ConvertFromString("#FF0050EF"));
            AccentColors.Add("Crimson", (Color)ColorConverter.ConvertFromString("#FFA20025"));
            AccentColors.Add("Cyan", (Color)ColorConverter.ConvertFromString("#FF1BA1E2"));
            AccentColors.Add("Emerald", (Color)ColorConverter.ConvertFromString("#FF008A00"));
        }

        public static ResourceDictionary CreateAppStyleBy(Color color, string Theme, bool NoTransparentAccent)
        {

            var Name = "Custom" + (ThemeManager.ColorSchemes.Count(x => x.Name.StartsWith("Custom")) + 1);

            var theme = GetThemeResourceDictionary(Theme, color, AccentName: Name, NoTransparentAccent: NoTransparentAccent);

            ThemeManager.AddTheme(theme);
            ThemeManager.ChangeTheme(Application.Current, theme["Theme.Name"].ToString());

            return theme;
        }


        private static XamlColorSchemeGenerator.GeneratorParameters GetGeneratorParameters()
        {
            return JsonConvert.DeserializeObject<XamlColorSchemeGenerator.GeneratorParameters>(GetGeneratorParametersJson());
        }

        private static string GetGeneratorParametersJson()
        {
            var stream = typeof(ThemeManager).Assembly.GetManifestResourceStream("MahApps.Metro.Styles.Themes.GeneratorParameters.json");
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static ResourceDictionary GetThemeResourceDictionary(string baseColorScheme, Color accentBaseColor, Color? highlightColor = null, string AccentName = null, bool NoTransparentAccent = false)
        {
            var generatorParameters = GetGeneratorParameters();
            var themeTemplateContent = GetThemeTemplateContent();

            var variant = generatorParameters.BaseColorSchemes.First(x => x.Name == baseColorScheme);
            var colorScheme = new XamlColorSchemeGenerator.ColorScheme
            {
                Name = AccentName
            };
            var values = colorScheme.Values;

            var baseColor = baseColorScheme == "Dark" ? Color.FromRgb(15,15,15) : Colors.White;

            values.Add("MahApps.Colors.AccentBase", accentBaseColor.ToString());
            values.Add("MahApps.Colors.Accent", NoTransparentAccent ? accentBaseColor.AddColor(baseColor, 0.2).ToString() : accentBaseColor.ChangeTransparency((byte)(255*0.8)).ToString());
            values.Add("MahApps.Colors.Accent2", NoTransparentAccent ? accentBaseColor.AddColor(baseColor, 0.4).ToString() : accentBaseColor.ChangeTransparency((byte)(255 * 0.6)).ToString());
            values.Add("MahApps.Colors.Accent3", NoTransparentAccent ? accentBaseColor.AddColor(baseColor, 0.6).ToString() : accentBaseColor.ChangeTransparency((byte)(255 * 0.4)).ToString());
            values.Add("MahApps.Colors.Accent4", NoTransparentAccent ? accentBaseColor.AddColor(baseColor, 0.8).ToString() : accentBaseColor.ChangeTransparency((byte)(255 * 0.2)).ToString());

            values.Add("MahApps.Colors.Highlight", accentBaseColor.ToString());
            values.Add("MahApps.Colors.IdealForeground", IdealTextColor(accentBaseColor).ToString());

            // Strings
            values.Add("ColorScheme", AccentName);

            var xamlContent = new XamlColorSchemeGenerator.ColorSchemeGenerator().GenerateColorSchemeFileContent(generatorParameters, variant, colorScheme, themeTemplateContent, $"{baseColorScheme}.{AccentName}", $"{AccentName} ({baseColorScheme})");

            var resourceDictionary = (ResourceDictionary)XamlReader.Parse(xamlContent);

            return resourceDictionary;
        }


        private static string GetThemeTemplateContent()
        {
            var stream = typeof(ThemeManager).Assembly.GetManifestResourceStream("MahApps.Metro.Styles.Themes.Theme.Template.xaml");
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }


        /// <summary>
        ///     Determining Ideal Text Color Based on Specified Background Color
        ///     http://www.codeproject.com/KB/GDI-plus/IdealTextColor.aspx
        /// </summary>
        /// <param name="color">The bg.</param>
        /// <returns></returns>
        private static Color IdealTextColor(Color color)
        {
            const int nThreshold = 105;
            var bgDelta = Convert.ToInt32((color.R * 0.299) + (color.G * 0.587) + (color.B * 0.114));
            var foreColor = 255 - bgDelta < nThreshold
                ? Colors.Black
                : Colors.White;
            return foreColor;
        }


        private static Color AddColor(this Color baseColor, Color ColorToAdd, double? Factor)
        {
            var firstColorAlpha = baseColor.A;
            var secondColorAlpha = Factor.HasValue ? Convert.ToByte(Factor * 255) : ColorToAdd.A;

            var alpha = CompositeAlpha(firstColorAlpha, secondColorAlpha);

            var r = CompositeColorComponent(baseColor.R, firstColorAlpha, ColorToAdd.R, secondColorAlpha, alpha);
            var g = CompositeColorComponent(baseColor.G, firstColorAlpha, ColorToAdd.G, secondColorAlpha, alpha);
            var b = CompositeColorComponent(baseColor.B, firstColorAlpha, ColorToAdd.B, secondColorAlpha, alpha);

            return Color.FromArgb(alpha, r, g, b);
        }

        /// <summary>
        /// For a single R/G/B component. a = precomputed CompositeAlpha(a1, a2)
        /// </summary>
        private static byte CompositeColorComponent(byte c1, byte a1, byte c2, byte a2, byte a)
        {
            // Handle the singular case of both layers fully transparent.
            if (a == 0)
            {
                return 0;
            }

            return System.Convert.ToByte((((255 * c2 * a2) + (c1 * a1 * (255 - a2))) / a) / 255);
        }

        private static byte CompositeAlpha(byte a1, byte a2)
        {
            return System.Convert.ToByte(255 - ((255 - a2) * (255 - a1)) / 255);
        }

        public static Color ChangeTransparency (this Color color, byte NewA)
        {
            return Color.FromArgb(NewA, color.R, color.G, color.B);
        }

    }
}
