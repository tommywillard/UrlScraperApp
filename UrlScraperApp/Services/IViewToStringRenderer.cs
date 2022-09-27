namespace UrlScraperApp.Services
{
    public interface IViewToStringRenderer
    {
        Task<string> RenderViewToString(string viewName, object model, bool isPartialView);
    }
}
