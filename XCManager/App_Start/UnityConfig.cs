using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Mvc5;
using XCManager.Controllers;
using XCManager.Services;

namespace XCManager
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here

            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<IUserServices, UserServices>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}