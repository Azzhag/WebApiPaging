using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http.Controllers;
using System.Web.Http.ValueProviders;

namespace WebApiPaging.Infrastructure
{
    /// <remarks>
    /// From Pro ASP.NET Web API: HTTP Web Services in ASP.NET (http://www.amazon.com/gp/search?index=books&linkCode=qs&keywords=9781430247265)
    /// </remarks>>
    public class XHeaderValueProvider : IValueProvider
    {
        private readonly HttpRequestHeaders _headers;
        private const string XHeaderPrefix = "X-";

        public XHeaderValueProvider(HttpActionContext actionContext)
        {
            _headers = actionContext.ControllerContext.Request.Headers;
        }
        public bool ContainsPrefix(string prefix)
        {
            return _headers.Any(header => header.Key.Contains(XHeaderPrefix + prefix));
        }

        public ValueProviderResult GetValue(string key)
        {
            IEnumerable<string> values;

            return _headers.TryGetValues(XHeaderPrefix + key, out values)
                ? new ValueProviderResult(values.First(), values.First(), CultureInfo.CurrentCulture)
                : null;
        }
    }

    public class XHeaderValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(HttpActionContext actionContext)
        {
            return new XHeaderValueProvider(actionContext);
        }
    }

}