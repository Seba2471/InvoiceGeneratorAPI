using IT.Pdf.DinkToPdf;

namespace InvoiceGenerator.Services
{
    public static class TryAddPdfConverter
    {
        private static WkHtmlToPdfDotNet.GlobalSettings GetGlobalSettings()
        {
            return new WkHtmlToPdfDotNet.GlobalSettings() { Margins = new WkHtmlToPdfDotNet.MarginSettings { } };
        }

        private static LocalSettings GetLocalSettings()
        {
            return new IT.Pdf.DinkToPdf.LocalSettings() { WebSettings = new WkHtmlToPdfDotNet.WebSettings { UserStyleSheet = "WkHtmlToPdf.css" } };
        }

        public static IServiceCollection AddPdfConverter(this IServiceCollection services)
        {
            static IT.Pdf.DinkToPdf.PdfConverter? Factory(IServiceProvider services)
            {
                var logger = services.GetRequiredService<ILogger<IT.Pdf.DinkToPdf.PdfConverter>>();
                try
                {
                    return new IT.Pdf.DinkToPdf.SynchronizedPdfConverter(null, GetGlobalSettings, GetLocalSettings, logger);
                }
                catch (Exception ex)
                {
                    if (logger.IsEnabled(LogLevel.Warning))
                        logger.LogWarning(ex, "[Pdf]");

                    return null;
                }
            }
            services.AddSingleton<IT.Pdf.IPdfConverter>(Factory);
            return services;
        }
    }
}
