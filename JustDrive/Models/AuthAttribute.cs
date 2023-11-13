using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace JustDrive.Models
{
    public class AuthAttribute : TypeFilterAttribute
    {
        public AuthAttribute(string actionName, string roleType) : base(typeof(Authorization))
        {
            Arguments = new object[] {
            actionName,
            roleType
        };
        }
    }


    public class AuthorizeAction : IAuthorizationFilter
    {
        private readonly string _actionName;
        private readonly string _roleType;
        public AuthorizeAction(string actionName, string roleType)
        {
            _actionName = actionName;
            _roleType = roleType;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string _roleType = context.HttpContext.Request?.Headers["role"].ToString();
            switch (_actionName)
            {
                case "Login":
                    if (!_roleType.Contains("user")) context.Result = new JsonResult("Permission denined!");
                    break;
            }
        }
    }


}
