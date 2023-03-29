using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SpecialityCatalogWebAPI.Classes
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer";
        public const string AUDIENCE = "MyAuthClient";
        const string KEY = "mSR@wT7_mvbgR9yB3gz+2_pY5GxK9AhsZYz?92YfP=8BwUjHL?@3?5M5-y4xU8tH";
        public const int LIFETIMEDAYS = 1;
        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
