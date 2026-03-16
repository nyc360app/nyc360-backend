using System.Reflection;

namespace NYC360.Domain.Constants;

public static class Permissions
{
    public const string PermissionClaimType = "Permission";
    
    public static class Dashboard
    {
        public const string View = "Permissions.Dashboard.View";
    }
    public static class Roles
    {
        public const string View = "Permissions.Roles.View";
        public const string Create = "Permissions.Roles.Create";
        public const string Edit = "Permissions.Roles.Edit";
        public const string Delete = "Permissions.Roles.Delete";
    }
    
    public static class Users
    {
        public const string View = "Permissions.Users.View";
        public const string Edit = "Permissions.Users.Edit";
        public const string Delete = "Permissions.Users.Delete";
        public const string UpdateRoles = "Permissions.Users.UpdateRoles";
        public const string UpdatePermissions = "Permissions.Users.UpdatePermissions";
    }

    public static class Posts
    {
        public const string View = "Permissions.Posts.View";
        public const string Create = "Permissions.Posts.Create";
        public const string Edit = "Permissions.Posts.Edit";
        public const string Delete = "Permissions.Posts.Delete";
        public const string AutoApproval = "Permissions.Posts.AutoApproval";
        
        public const string Comment = "Permissions.Posts.Comment";
        public const string Interact = "Permissions.Posts.Interact";
        public const string Share = "Permissions.Posts.Share";
        public const string Report = "Permissions.Posts.Report";
        public const string ViewReport = "Permissions.Posts.Report.View";
    }

    public static class Topics
    {
        public const string View = "Permissions.Topics.View";
        public const string Create = "Permissions.Topics.Create";
        public const string Edit = "Permissions.Topics.Edit";
        public const string Delete = "Permissions.Topics.Delete";
    }

    public static class RssFeeds
    {
        public const string View = "Permissions.RssFeeds.View";
        public const string Create = "Permissions.RssFeeds.Create";
        public const string Edit = "Permissions.RssFeeds.Edit";
        public const string Delete = "Permissions.RssFeeds.Delete";
    }

    public static class PostFlags
    {
        public const string View = "Permissions.Flags.Posts.View";
        public const string TakeAction = "Permissions.Flags.Posts.TakeAction";
    }
    
    public static class Events
    {
        public const string View = "Permissions.Events.View";
        public const string Create = "Permissions.Events.Create";
        public const string Update = "Permissions.Events.Update";
        public const string Delete = "Permissions.Events.Delete";
        public const string Purchase = "Permissions.Events.Purchase";
    }
    
    public static class Communities
    {
        public const string Create = "Permissions.Communities.Create";
    }
    
    public static class Tags
    {
        public const string View = "Permissions.Tags.View";
        public const string Create = "Permissions.Tags.Create";
        public const string Edit = "Permissions.Tags.Edit";
        public const string Delete = "Permissions.Tags.Delete";
        public const string Verify = "Permissions.Tags.Verify";
    }
    
    public static class SupportTickets
    {
        public const string View = "Permissions.SupportTickets.View";
        public const string Reply = "Permissions.SupportTickets.Reply";
        public const string Close = "Permissions.SupportTickets.Close";
    }

    public static class Housing
    {
        public const string Create = "Permissions.Housing.Create";
        public const string View = "Permissions.Housing.View";
        public const string Edit = "Permissions.Housing.Edit";
    }
    
    public static class Forums
    {
        public const string Create = "Permissions.Forums.Create";
        public const string View = "Permissions.Forums.View";
        public const string Edit = "Permissions.Forums.Edit";
        public const string Delete = "Permissions.Forums.Delete";
    }
    
    public static List<string> GetAllPermissions()
    {
        var allPermissions = new List<string>();

        // Get all nested public static classes within the Permissions class
        var nestedTypes = typeof(Permissions).GetNestedTypes(BindingFlags.Public | BindingFlags.Static);

        foreach (var type in nestedTypes)
        {
            // Get all public constant string fields from the nested class
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string));

            // Add the constant values to our list
            allPermissions.AddRange(fields.Select(fi => (string)fi.GetRawConstantValue()!));
        }

        return allPermissions;
    }
}