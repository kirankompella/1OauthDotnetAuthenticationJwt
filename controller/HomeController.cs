using System.Text;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Server{
    public class HomeController: Controller
{
    public IActionResult Index(){
        return View();
    }

    [Authorize]
    public IActionResult Secret(){
        return View();
    }

    public IActionResult Authenticate(){
        var claims = new[]{
            new Claim(JwtRegisteredClaimNames.Sub,"some_user_id")
        };

        var SecretBytes= Encoding.UTF8.GetBytes(Constants.Secret);
        var symmentricKey = new SymmetricSecurityKey(SecretBytes);
        
        var algorithm =  SecurityAlgorithms.HmacSha256;

        var signingCredentials = new SigningCredentials(symmentricKey,algorithm);

        var token = new JwtSecurityToken(
            Constants.Issuer,
            Constants.Audience,
            claims,
            notBefore: DateTime.Now,
            expires:DateTime.Now.AddDays(1),
            signingCredentials
        );
       
        var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);


        return Ok(new {access_token=tokenJson});
    }

}

}
