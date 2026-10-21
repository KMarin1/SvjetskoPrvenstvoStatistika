using System.Globalization;

namespace SvjetskoPrvenstvoPobjedeForm
{
    /// <summary>Switches the application between the languages in <see cref="PodatkovniSloj.Config.UserSettings.SupportedCultures"/>.</summary>
    internal static class UiCulture
    {
        /// <summary>
        /// Sets both the UI language (which resources are read) and the formatting culture,
        /// for the UI thread and for any thread started afterwards.
        /// </summary>
        public static void Apply(string cultureName)
        {
            var culture = CultureInfo.GetCultureInfo(cultureName);
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}
