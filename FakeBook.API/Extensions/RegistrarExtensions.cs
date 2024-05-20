using FakeBook.API.Registrars;

namespace FakeBook.API.Extensions
{
    public static class RegistrarExtensions 
    {
        public static void RegisterServices(this WebApplicationBuilder builder, Type scanningType)
        {
            // to get all classes that implement the IWebAppBuilderRegistrar in the whole assemply (Program)
            var registrars = GetRegistrars<IWebAppBuilderRegistrar>(scanningType);

            foreach (var registrar in registrars)
            {
                registrar.RegisterServices(builder);
            }
        }

        private static IEnumerable<T> GetRegistrars<T>(Type scanningType) where T :IRegistrar
        {
            return scanningType.Assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(T))&& !t.IsAbstract && !t.IsInterface).Select(Activator.CreateInstance)
                .Cast<T>();
        }

        public static void RegisterPipelineComponents(this WebApplication app, Type scanningType)
        {
            // to get all classes that implement the IWebAppRegistrar in the whole assemply (Program)
            var registrars = GetRegistrars<IWebAppRegistrar>(scanningType);
            foreach (var registrar in registrars)
            {
                registrar.RegisterPipelineComponents(app);
            }
        }
    }
}
