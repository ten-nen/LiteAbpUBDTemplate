using Volo.Abp.Reflection;

namespace LiteAbpUBD.Example.Business
{
    public static class ExamplePermissions
    {
        public const string GroupName = "Example";
        public static class OpenCustomers
        {
            public const string Default = GroupName + ".OpenCustomers";

            public const string Create = Default + ".Create";
        }
        public static class Activitys
        {
            public const string Default = GroupName + ".Activitys";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(ExamplePermissions));
        }
    }
}
