using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace UrlScraperApp.Services
{
    public class ViewToStringRenderer : IViewToStringRenderer
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        public ViewToStringRenderer(IRazorViewEngine viewEngine,
                                 ITempDataProvider tempDataProvider,
                                 IServiceProvider serviceProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        public async Task<string> RenderViewToString(string viewName, object model, bool isPartialView = false)
        {
            await using var stringWriter = new StringWriter();
            var actionContext = GetActionContext();
            var viewResult = FindView(actionContext, viewName, isPartialView);

            if (viewResult == null)
                throw new ArgumentNullException($"{viewName} was unable to be found.");

            var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), actionContext.ModelState)
            {
                Model = model
            };

            var viewContext = new ViewContext(
                actionContext,
                viewResult,
                viewDictionary,
                new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                stringWriter,
                new HtmlHelperOptions()
            );

            await viewResult.RenderAsync(viewContext);

            return stringWriter.ToString();
        }

        private IView FindView(ActionContext actionContext, string viewName, bool isPartialView = false)
        {
            
            var getViewResult = _viewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: !isPartialView);
            if (getViewResult.Success)
            {
                return getViewResult.View;
            }

            var findViewResult = _viewEngine.FindView(actionContext, viewName, isMainPage: !isPartialView);
            if (findViewResult.Success)
            {
                return findViewResult.View;
            }

            var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
            var errorMessage = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(searchedLocations)); ;

            throw new InvalidOperationException(errorMessage);
        }

        private ActionContext GetActionContext()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.RequestServices = _serviceProvider;
            return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        }
    }
}
