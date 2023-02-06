using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Step3_WebApiConsumerConsole_Jwt.Models;

namespace Step3_WebApiConsumerConsole_Jwt.Services
{
    public class FriendsHttpService : BaseHttpService, IFriendsHttpService
    {
        readonly Uri _baseUri;
        readonly IDictionary<string, string> _headers;

        public FriendsHttpService(Uri baseUri)
        {
            _baseUri = baseUri;
            _headers = new Dictionary<string, string>();
        }

        public async Task<WebApiID> GetInfoAsync()
        {
            var url = new Uri(_baseUri, "/id");
            var response = await SendRequestAsync<WebApiID>(url, HttpMethod.Get);

            return response;
        }


        public async Task<JwtUserToken> LoginUserAsync(UserCredentials user)
        {
            var url = new Uri(_baseUri, "/api/login/loginuser");
            var response = await SendRequestAsync<UserCredentials, JwtUserToken>(url, HttpMethod.Post, user);

            return response;
        }

        public async Task<IEnumerable<Friend>> GetFriendsAsync(JwtUserToken userToken, int? count = null)
        {
            var qp = (count.HasValue) ? $"?count={count}" : null;
            var url = new Uri(_baseUri, $"/api/friends{qp}");

            var response = await SendRequestAsync<List<Friend>>(url, HttpMethod.Get, userToken.EncryptedToken);
            return response;
        }

        public async Task<Friend> GetFriendAsync(JwtUserToken userToken, Guid Id)
        {
            var url = new Uri(_baseUri, $"/api/friends/{Id}");
            var response = await SendRequestAsync<Friend>(url, HttpMethod.Get, userToken.EncryptedToken);

            return response;
        }

        public async Task<Friend> UpdateFriendAsync(JwtUserToken userToken, Friend friend)
        {
            var url = new Uri(_baseUri, $"/api/friends/{friend.FriendID}");

            //Confirm friend exisit in Database
            var itemToUpdate = await SendRequestAsync<Friend>(url, HttpMethod.Get, userToken.EncryptedToken);
            if (itemToUpdate == null)
                return null;  //friend does not exist

            //Update Friend, always gives null response, NonSuccess response errors are thrown in BaseHttpService
            await SendRequestAsync<Friend, Friend>(url, HttpMethod.Put, friend, userToken.EncryptedToken);

            return friend;
        }

        public async Task<Friend> CreateFriendAsync(JwtUserToken userToken, Friend friend)
        {
            var url = new Uri(_baseUri, "/api/friends");
            var response = await SendRequestAsync<Friend, Friend>(url, HttpMethod.Post, friend, userToken.EncryptedToken);

            return response;
        }

        public async Task<Friend> DeleteFriendAsync(JwtUserToken userToken, Guid Id)
        {
            var url = new Uri(_baseUri, $"/api/friends/{Id}");

            //Confirm customer exisit in Database
            var itemToDel = await SendRequestAsync<Friend>(url, HttpMethod.Get, userToken.EncryptedToken);
            if (itemToDel == null)
                return null;  //friend does not exist

            //Delete Customer, always gives null response, NonSuccess response errors are thrown
            await SendRequestAsync<Friend>(url, HttpMethod.Delete, userToken.EncryptedToken);
            return itemToDel;
        }


        public async Task<IEnumerable<GoodQuote>> GetQuotesAsync(JwtUserToken userToken, int? count = null)
        {
            var qp = (count.HasValue) ? $"?count={count}" : null;
            var url = new Uri(_baseUri, $"/api/quotes{qp}");

            var response = await SendRequestAsync<List<GoodQuote>>(url, HttpMethod.Get, userToken.EncryptedToken);
            return response;
        }
    }
}
