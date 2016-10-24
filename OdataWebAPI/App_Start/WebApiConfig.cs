using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using ODataWebAPI.Data;

namespace OdataWebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Employee>("Employees");
            builder.EntitySet<Address>("Addresses");
            builder.EntitySet<Company>("Companies");

            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: "odata",
                model: builder.GetEdmModel());
        }
    }
}
