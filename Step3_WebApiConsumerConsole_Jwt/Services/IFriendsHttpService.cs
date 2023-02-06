using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Step3_WebApiConsumerConsole_Jwt.Models;

namespace Step3_WebApiConsumerConsole_Jwt.Services
{
    public interface IFriendsHttpService
    {
        Task<WebApiID> GetInfoAsync();

        Task<JwtUserToken> LoginUserAsync(UserCredentials user);

        Task<IEnumerable<Friend>> GetFriendsAsync(JwtUserToken userToken, int? count=null);
        Task<Friend> GetFriendAsync(JwtUserToken userToken, Guid Id);

        Task<Friend> UpdateFriendAsync(JwtUserToken userToken, Friend friend);

        Task<Friend> CreateFriendAsync(JwtUserToken userToken, Friend friend);
        Task<Friend> DeleteFriendAsync(JwtUserToken userToken, Guid Id);

        Task<IEnumerable<GoodQuote>> GetQuotesAsync(JwtUserToken userToken, int? count = null);
    }
}
