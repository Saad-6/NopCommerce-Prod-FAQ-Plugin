using System.Reflection;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Data.Migrations;
using Nop.Plugin.F.A.Q.Components;
using Nop.Plugin.F.A.Q.Domain;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;




public class FAQPlugin : BasePlugin, IWidgetPlugin, IAdminMenuPlugin
{
    private readonly Assembly _assembly;
    private readonly IWebHelper _webHelper;
    private readonly IMigrationManager _migrationManager;
    private readonly ISettingService _settings;
    public FAQPlugin(IMigrationManager migrationManager, IWebHelper webHelper, ISettingService settings)
    {
        _assembly = Assembly.GetAssembly(typeof(Nop.Web.Framework.Infrastructure.Extensions.ApplicationBuilderExtensions));
        _migrationManager = migrationManager;
        _webHelper = webHelper;
        _settings = settings;
    }
    public override async Task InstallAsync()
    {
        _migrationManager.ApplyUpMigrations(_assembly);
        //Add Plugin Supported Languages
        var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
        var languageService = EngineContext.Current.Resolve<ILanguageService>();
        var fileProvider = EngineContext.Current.Resolve<INopFileProvider>();
        await LanguageSettings.ImportLanguagesAsync(languageService, localizationService, fileProvider);

        await base.InstallAsync();
    }
    public override async Task UninstallAsync()
    {
        var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
        var languageService = EngineContext.Current.Resolve<ILanguageService>();
        try
        {
           await LanguageSettings.RemovePluginResourcesAsync(languageService, localizationService);
             _migrationManager.ApplyDownMigrations(_assembly);

        }
        catch (Exception ex)
        {
         
        }
        await base.UninstallAsync();
    }
  

    public bool HideInWidgetList => false;

    public override string GetConfigurationPageUrl()
    {

        return $"{_webHelper.GetStoreLocation()}Admin/Questions/Configure";
    }

    public Type GetWidgetViewComponent(string widgetZone)
    {
        return typeof(ProductViewComponent);
  
    }

    public Task<IList<string>> GetWidgetZonesAsync()
    {
        var settings = _settings.LoadSetting<FAQSettings>();
        var widgetZone = settings.ActiveWidgetZone;
        return Task.FromResult<IList<string>>(new List<string> { widgetZone });
    }


    public Task ManageSiteMapAsync(SiteMapNode rootNode)
    {
        var menuItem = new SiteMapNode()
        {
            SystemName = "FAQPlugin",
            Title = "Manage Questions",
            ControllerName = "Questions",
            ActionName = "Index",
            IconClass = "fa fa-dot-circle",
            Visible = true,
            RouteValues = new RouteValueDictionary() { { "area", AreaNames.ADMIN } },
        };
        var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Third party plugins");
        if (pluginNode != null)
            pluginNode.ChildNodes.Add(menuItem);
        else
            rootNode.ChildNodes.Add(menuItem);

        return Task.CompletedTask;
    }
}
