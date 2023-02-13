using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using Step3_WebApiConsumerConsole_Jwt.Models;
using Step3_WebApiConsumerConsole_Jwt.Services;

namespace Step3_WebApiConsumerConsole_Jwt;
class Program
{
    static async Task Main(string[] args)
    {
        //var ServiceUri = "https://localhost:7249";
        var ServiceUri = "https://hellofrommartin.azurewebsites.net";

        var service = new FriendsHttpService(new Uri(ServiceUri));

        Console.WriteLine("Testing GetInfoAsync()");
        var webApi = await service.GetInfoAsync();
        Console.WriteLine($"Title: {webApi?.Title}");
        Console.WriteLine($"Author: {webApi?.Author}");
        Console.WriteLine($"Version: {webApi?.Version}");


        Console.WriteLine("\nTesting JwtLoginAsync()");
        var userToken = await service.LoginUserAsync(new UserCredentials { UserName = "User1", Password= "pa$$W0rd"});
        if (userToken == null) throw new Exception("Error testing JwtLoginAsync()");

        Console.WriteLine("Logged in User");
        Console.WriteLine($"Id: {userToken.UserId}");
        Console.WriteLine($"UserName: {userToken.UserName}");
        Console.WriteLine($"Email: {userToken.UserEmail}");


        Console.WriteLine("\nTesting GetFriendsAsync()");
        var friends = await service.GetFriendsAsync(userToken);
        if (friends == null) throw new Exception("Error testing GetFriendsAsync()");

        Console.WriteLine($"Nr of friends: {friends?.Count()}");
        Console.WriteLine($"First 5 friends:");
        friends?.Take(5).ToList().ForEach(c => Console.WriteLine(c));


        Console.WriteLine("\nTesting GetFriendsAsync(5)");
        friends = await service.GetFriendsAsync(userToken, 5);
        if (friends == null) throw new Exception("Error testing GetFriendsAsync(5)");

        Console.WriteLine($"Nr of friends: {friends?.Count()}");
        friends?.ToList().ForEach(c => Console.WriteLine(c));


        Console.WriteLine("\nTesting GetFriendAsync(Guid Id)");
        var friend = await service.GetFriendAsync(userToken, friends!.First().FriendID);
        if (friend == null) throw new Exception("Error testing GetFriendAsync(Guid Id)");

        Console.WriteLine(friend);


        Console.WriteLine("\nTesting CreateFriendAsync(Friend friend)");
        friend = await service.CreateFriendAsync(userToken, Friend.Factory.CreateRandom());
        if (friend == null) throw new Exception("Error testing CreateFriendAsync(Friend friend)");

        Console.WriteLine(friend);


        Console.WriteLine("\nTesting UpdateFriendAsync(Friend friend)");
        friend.FirstName += "_Updated";
        friend.LastName += "_Updated";
        friend = await service.UpdateFriendAsync(userToken, friend);
        if (friend == null) throw new Exception("Error testing UpdateFriendAsync(Friend friend)");

        Console.WriteLine(friend);

        Console.WriteLine("\nTesting DeleteFriendAsync(Guid Id)");
        friend = await service.DeleteFriendAsync(userToken, friend.FriendID);
        if (friend == null) throw new Exception("Error testing DeleteFriendAsync(Guid Id)");

        Console.WriteLine(friend);


        Console.WriteLine("\nTesting GetQuotesAsync()");
        var quotes = await service.GetQuotesAsync(userToken);
        if (quotes == null) throw new Exception("Error testing GetQuotesAsync()");

        Console.WriteLine($"Nr of quotes: {quotes?.Count()}");
        Console.WriteLine($"First 5 quotes:");
        quotes?.Take(5).ToList().ForEach(c => Console.WriteLine($"{c.Quote}\n- {c.Author}"));


        Console.WriteLine("\nTesting GetQuotesAsync(5)");
        quotes = await service.GetQuotesAsync(userToken, 5);
        if (quotes == null) throw new Exception("Error testing GetQuotesAsync(5)");

        Console.WriteLine($"Nr of quotes: {quotes?.Count()}");
        quotes?.ToList().ForEach(c => Console.WriteLine($"{c.Quote}\n- {c.Author}"));

    }
}

