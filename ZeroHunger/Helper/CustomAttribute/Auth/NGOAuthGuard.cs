﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace E_Commerce_Web_Application.Helper.CustomAttribute.Auth
{
    public class NGOAuthGuard : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["userId"] != null && httpContext.Session["userEmail"] != null && httpContext.Session["userType"].ToString() == "NGO")
            {
                return true;
            }
            
          
            return false;
        }
    }
}