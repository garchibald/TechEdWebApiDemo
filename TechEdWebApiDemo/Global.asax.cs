using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TechEdWebApiDemo.App_Start;
using TechEdWebApiDemo.DAL;

namespace TechEdWebApiDemo
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Register Database
            Database.SetInitializer(new TechEdInit());

            // Register Security
            SecurityConfig.ConfigureGlobal(GlobalConfiguration.Configuration);

            // Register Automapper
            AutoMapper.Mapper.CreateMap<Models.Member, TechEd.Integration.ServiceModels.Member>();
            AutoMapper.Mapper.CreateMap<TechEd.Integration.ServiceModels.Member, Models.Member>();
        }
    }
}