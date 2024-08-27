using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Localization;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Data.DataProviders;
using Nop.Services.Localization;

namespace Nop.Plugin.F.A.Q.Domain;
public class LanguageSettings
{
    private readonly IRepository<LocaleStringResource> _repository;

    public LanguageSettings(IRepository<LocaleStringResource> repository)
    {
        _repository = repository;
    }
    public async Task<IEnumerable<LocaleStringResource>> GetLocaleStringResourcesByPrefixAsync(string prefix)
    {
        return await _repository.Table
            .Where(lsr => lsr.ResourceName.StartsWith(prefix))
            .ToListAsync();
    }
    public static async Task ImportLanguagesAsync(ILanguageService languageService, ILocalizationService localizationService, INopFileProvider fileProvider)
    {
        var supportedLanguages = new Dictionary<string, string>
        {
          { "en-US", "~/Plugins/F.A.Q/Localization/Resources.en-US.xml" },
          { "en-UK", "~/Plugins/F.A.Q/Localization/Resources.en-US.xml" },
          { "en-CA", "~/Plugins/F.A.Q/Localization/Resources.en-US.xml" },
          { "en-GB", "~/Plugins/F.A.Q/Localization/Resources.en-US.xml" },
          { "en-AU", "~/Plugins/F.A.Q/Localization/Resources.en-US.xml" },
          { "en-NZ", "~/Plugins/F.A.Q/Localization/Resources.en-US.xml" },
          { "de-DE", "~/Plugins/F.A.Q/Localization/Resources.de-DE.xml" },
          { "aa-DJ", "~/Plugins/F.A.Q/Localization/Resources.de-DE.xml" },
          { "ur-PK", "~/Plugins/F.A.Q/Localization/Resources.ur-PK.xml" },
          { "fr-FR", "~/Plugins/F.A.Q/Localization/Resources.fr-FR.xml" },
          { "it-IT", "~/Plugins/F.A.Q/Localization/Resources.it-IT.xml" },
          { "es-ES", "~/Plugins/F.A.Q/Localization/Resources.es-ES.xml" },


        };

        foreach (var languagePair in supportedLanguages)
        {
            var allLanguages = languageService.GetAllLanguages();
            var language = allLanguages.FirstOrDefault(m => m.LanguageCulture == languagePair.Key);

            if (language != null)
            {
                var localizationPath = fileProvider.MapPath(languagePair.Value);

                if (fileProvider.FileExists(localizationPath))
                {
                    using (var streamReader = new StreamReader(localizationPath))
                    {
                        await localizationService.ImportResourcesFromXmlAsync(language, streamReader, updateExistingResources: true);
                    }
                }
            }
        }
    }
    public  static async Task RemovePluginResourcesAsync(ILanguageService languageService, ILocalizationService localizationService)
    {
        // Define the prefix for your plugin resources
        string prefix = "Plugins.F.A.Q.";
        var repository = EngineContext.Current.Resolve<IRepository<LocaleStringResource>>();
        var languageSettings = new LanguageSettings(repository);
        var resources = await languageSettings.GetLocaleStringResourcesByPrefixAsync(prefix);

        foreach (var resource in resources)
        {
            await localizationService.DeleteLocaleResourceAsync(resource.ResourceName);
        }
    }


}
