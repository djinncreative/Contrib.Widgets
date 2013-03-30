using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Contrib.Widgets.Models;
using Contrib.Widgets.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Mvc.Extensions;
using Orchard.UI.Notify;
using Orchard.Widgets.Models;
using Orchard.Widgets.Services;
using Permissions = Orchard.Widgets.Permissions;

namespace Contrib.Widgets.Controllers {
    [ValidateInput(false), OrchardFeature("Contrib.Widgets")]
    public class AdminController : Controller, IUpdateModel {
        private readonly IOrchardServices _services;
        private readonly IWidgetsService _widgetsService;
        private readonly IWidgetManager _widgetManager;

        public AdminController(IOrchardServices services, IWidgetsService widgetsService, IWidgetManager widgetManager) {
            _services = services;
            _widgetsService = widgetsService;
            _widgetManager = widgetManager;
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }

        public ActionResult ListWidgets(int contentId, string zone) {
            var widgetTypes = _widgetsService.GetWidgetTypeNames().OrderBy(x => x).ToList();
            
            var viewModel = _services.New.ViewModel()
                .WidgetTypes(widgetTypes)
                .ContentId(contentId)
                .Zone(zone);

            return View(viewModel);
        }

        public ActionResult AddWidget(int contentId, string widgetType, string zone, string returnUrl) {
            if (!IsAuthorizedToManageWidgets())
                return new HttpUnauthorizedResult();

            var widgetPart = _services.ContentManager.New<WidgetPart>(widgetType);
            if (widgetPart == null)
                return HttpNotFound();

            try {
                var widgetPosition = _widgetManager.GetWidgets(contentId).Count(widget => widget.Zone == widgetPart.Zone) + 1;
                widgetPart.Position = widgetPosition.ToString(CultureInfo.InvariantCulture);
                widgetPart.Zone = zone;
                widgetPart.AvailableLayers = _widgetsService.GetLayers().ToList();
                widgetPart.LayerPart = _widgetManager.GetContentLayer();
                
                var model = _services.ContentManager.BuildEditor(widgetPart).ContentId(contentId);
                return View(model);
            }
            catch (Exception exception) {
                Logger.Error(T("Creating widget failed: {0}", exception.Message).Text);
                _services.Notifier.Error(T("Creating widget failed: {0}", exception.Message));
                return this.RedirectLocal(returnUrl, () => RedirectToAction("Edit", "Admin", new { area = "Contents" }));
            }
        }

        [HttpPost, ActionName("AddWidget")]
        public ActionResult AddWidgetPost(string widgetType, int contentId) {
            if (!IsAuthorizedToManageWidgets())
                return new HttpUnauthorizedResult();

            var layer = _widgetsService.GetLayers().First();
            var widgetPart = _widgetsService.CreateWidget(layer.Id, widgetType, "", "", "");
            if (widgetPart == null)
                return HttpNotFound();

            var contentItem = _services.ContentManager.Get(contentId, VersionOptions.Latest);
            var contentMetadata = _services.ContentManager.GetItemMetadata(contentItem);
            var returnUrl = Url.RouteUrl(contentMetadata.EditorRouteValues);
            var model = _services.ContentManager.UpdateEditor(widgetPart, this);
            var widgetExPart = widgetPart.As<WidgetExPart>();
            try {
                widgetPart.LayerPart = _widgetManager.GetContentLayer();
                widgetExPart.Host = contentItem;
            }
            catch (Exception exception) {
                Logger.Error(T("Creating widget failed: {0}", exception.Message).Text);
                _services.Notifier.Error(T("Creating widget failed: {0}", exception.Message));
                return Redirect(returnUrl);
            }
            if (!ModelState.IsValid) {
                _services.TransactionManager.Cancel();
                return View((object)model);
            }

            _services.Notifier.Information(T("Your {0} has been added.", widgetPart.TypeDefinition.DisplayName));
            return Redirect(returnUrl);
        }

        private bool IsAuthorizedToManageWidgets() {
            return _services.Authorizer.Authorize(Permissions.ManageWidgets, T("Not authorized to manage widgets"));
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage) {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}