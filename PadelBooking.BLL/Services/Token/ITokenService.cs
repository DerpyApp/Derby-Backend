using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelBooking.BLL.Services.Token
{
    public interface ITokenService
    {
        string GenerateAccessToken(DAL.Models.User user, IList<string> roles); 
        // دي هتعمل الـJWT Token اللي المستخدم هيستخدمه في الـAPI request
        string GenerateRefreshToken();
        // دي هتعمل الـRefresh Token اللي المستخدم هيستخدمه عشان يجدد الـAccess Token لما يخلص
        DateTime GetRefreshTokenExpiryTime(); // بتجيب تاريخ انتهاء ال Refresh Token
    }
}
