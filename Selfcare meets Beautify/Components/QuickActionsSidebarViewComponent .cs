using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Selfcare_meets_Beautify.Models;

namespace Selfcare_meets_Beautify.Components
{
    public class QuickActionsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string variant = "default")
        {
            var actions = await GetQuickActionsAsync(variant);
            return View(actions);
        }

        private async Task<List<QuickAction>> GetQuickActionsAsync(string variant)
        {
            // Simulate async operation
            await Task.Delay(1);

            var actions = new List<QuickAction>();

            switch (variant.ToLower())
            {
                case "admin":
                    actions = GetAdminActions();
                    break;
                case "content":
                    actions = GetContentActions();
                    break;
                case "user":
                    actions = GetUserActions();
                    break;
                default:
                    actions = GetDefaultActions();
                    break;
            }

            return actions;
        }

        private List<QuickAction> GetDefaultActions()
        {
            return new List<QuickAction>
        {
            new QuickAction
            {
                Title = "Add User",
                Icon = "fas fa-user-plus",
                Url = "/Admin/Users/Create",
                Description = "Create new user account",
                Badge = "New"
            },
            new QuickAction
            {
                Title = "Create Post",
                Icon = "fas fa-edit",
                Url = "/Admin/Posts/Create",
                Description = "Write new blog post"
            },
            new QuickAction
            {
                Title = "View Reports",
                Icon = "fas fa-chart-bar",
                Url = "/Admin/Reports",
                Description = "Analytics and insights",
                Badge = "5",
                BadgeColor = "warning"
            },
            new QuickAction
            {
                Title = "Settings",
                Icon = "fas fa-cog",
                Url = "/Admin/Settings",
                Description = "System configuration"
            }
        };
        }

        private List<QuickAction> GetAdminActions()
        {
            return new List<QuickAction>
        {
            new QuickAction
            {
                Title = "Manage Users",
                Icon = "fas fa-users",
                Url = "/Admin/Users",
                Description = "View all users",
                Badge = "42",
                BadgeColor = "info"
            },
            new QuickAction
            {
                Title = "System Logs",
                Icon = "fas fa-clipboard-list",
                Url = "/Admin/Logs",
                Description = "View system logs"
            },
            new QuickAction
            {
                Title = "Backup",
                Icon = "fas fa-database",
                Url = "/Admin/Backup",
                Description = "Create system backup"
            },
            new QuickAction
            {
                Title = "Permissions",
                Icon = "fas fa-shield-alt",
                Url = "/Admin/Permissions",
                Description = "Manage user roles"
            }
        };
        }

        private List<QuickAction> GetContentActions()
        {
            return new List<QuickAction>
        {
            new QuickAction
            {
                Title = "New Article",
                Icon = "fas fa-newspaper",
                Url = "/Admin/Articles/Create",
                Description = "Create new article"
            },
            new QuickAction
            {
                Title = "Media Library",
                Icon = "fas fa-images",
                Url = "/Admin/Media",
                Description = "Manage media files",
                Badge = "12",
                BadgeColor = "success"
            },
            new QuickAction
            {
                Title = "Categories",
                Icon = "fas fa-tags",
                Url = "/Admin/Categories",
                Description = "Manage categories"
            },
            new QuickAction
            {
                Title = "Comments",
                Icon = "fas fa-comments",
                Url = "/Admin/Comments",
                Description = "Moderate comments",
                Badge = "3",
                BadgeColor = "danger"
            }
        };
        }

        private List<QuickAction> GetUserActions()
        {
            return new List<QuickAction>
        {
            new QuickAction
            {
                Title = "My Profile",
                Icon = "fas fa-user",
                Url = "/Profile",
                Description = "Update your profile"
            },
            new QuickAction
            {
                Title = "Messages",
                Icon = "fas fa-envelope",
                Url = "/Messages",
                Description = "Check your messages",
                Badge = "3",
                BadgeColor = "warning"
            },
            new QuickAction
            {
                Title = "Notifications",
                Icon = "fas fa-bell",
                Url = "/Notifications",
                Description = "View notifications"
            },
            new QuickAction
            {
                Title = "Settings",
                Icon = "fas fa-cog",
                Url = "/Settings",
                Description = "Account settings"
            }
        };
        }
    }
}
