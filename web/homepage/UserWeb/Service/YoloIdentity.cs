using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNet.Identity;

namespace DBLIB
{
    public class YoloIdentity : ClaimsIdentity
    {
        YoloIdentity(List<Claim> claims)
            :base(claims, DefaultAuthenticationTypes.ApplicationCookie)
        {

        }
        
        public static YoloIdentity CreateIdentity(string  id, string  name, string tel)
        {
            var list = new List<Claim>();
            list.Add(new Claim( ClaimTypes.NameIdentifier, id, ClaimValueTypes.String));
            list.Add(new Claim( ClaimTypes.Name, name, ClaimValueTypes.String));
            list.Add(new Claim(ClaimTypes.HomePhone, tel));

            return new YoloIdentity(list);
        }

    }
}