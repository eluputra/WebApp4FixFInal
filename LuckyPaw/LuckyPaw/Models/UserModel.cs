using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace LuckyPaw.Models
{
    // User model to hold information about all users and roles and user roles .i.e the relationship
    // between a user and their role
    public class UserModel
    {
        public List<IdentityUser> userList { get; set; }

        public List<IdentityRole> roleList { get; set; }

        public List<IdentityUserRole<string>> userRoleList { get; set; }
    }
}