using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Data.Migrations;
using Nop.Plugin.F.A.Q.Components;
using Nop.Services.Cms;
using Nop.Services.Plugins;
using Nop.Web.Framework;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;




public class FAQPlugin : BasePlugin, IWidgetPlugin, IAdminMenuPlugin
{
    private readonly Assembly _assembly;
    private readonly IWebHelper _webHelper;
    private readonly IMigrationManager _migrationManager;
    public FAQPlugin(IMigrationManager migrationManager, IWebHelper webHelper)
    {
        _assembly = Assembly.GetAssembly(typeof(Nop.Web.Framework.Infrastructure.Extensions.ApplicationBuilderExtensions));
        _migrationManager = migrationManager;
        _webHelper = webHelper;
    }
    public override Task InstallAsync()
    {
        _migrationManager.ApplyUpMigrations(_assembly);
        return base.InstallAsync();
    }
    public override Task UninstallAsync()
    {
        try
        {
            _migrationManager.ApplyDownMigrations(_assembly);
        }
        catch (Exception ex)
        {

        }
        return base.UninstallAsync();
    }
  

    public bool HideInWidgetList => false;

    public override string GetConfigurationPageUrl()
    {

        return $"{_webHelper.GetStoreLocation()}Admin/Dashboard/Configure";
    }

    public Type GetWidgetViewComponent(string widgetZone)
    {
        return typeof(ProductViewComponent);
  
    }

    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.ProductDetailsBottom });
    }


    public Task ManageSiteMapAsync(SiteMapNode rootNode)
    {
        var menuItem = new SiteMapNode()
        {
            SystemName = "FAQPlugin",
            Title = "Manage FAQs",
            ControllerName = "Dashboard",
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
