using RadioMeti.Application.DTOs.Enums;

namespace RadioMeti.Site.Utilities.Permission
{
    public static class SitePermissions
    {
        public static string IndexUser = PermissionsSite.IndexUsers.ToString().ToUpper();
        public static string CreateUser = PermissionsSite.CreateUser.ToString().ToUpper();
        public static string EditUser = PermissionsSite.EditUser.ToString().ToUpper();
        public static string DeleteUser = PermissionsSite.DeleteUser.ToString().ToUpper();
        public static string IndexRoles = PermissionsSite.IndexRoles.ToString().ToUpper();
        public static string CreateRole = PermissionsSite.CreateRole.ToString().ToUpper();
        public static string EditRole = PermissionsSite.EditRole.ToString().ToUpper();
        public static string DeleteRole = PermissionsSite.DeleteRole.ToString().ToUpper();
    }
}
