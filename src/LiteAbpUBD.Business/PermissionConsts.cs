using Volo.Abp.Reflection;

namespace LiteAbpUBD.Business
{
    public static class Permissions
    {
        public const string GroupName = "Admin";
        public static class Indexs
        {
            public const string Default = GroupName + ".Indexs";
        }
        public static class Roles
        {
            public const string Default = GroupName + ".Roles";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }
        public static class Users
        {
            public const string Default = GroupName + ".Users";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }
        public static class Menus
        {
            public const string Default = GroupName + ".Menus";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(Permissions));
        }
    }
}
