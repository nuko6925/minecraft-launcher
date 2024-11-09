using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.Sessions;
using XboxAuthNet.Game.Accounts;

namespace Minecraft_Launcher;

public class Temp
{
    private static async void Any()
    {
        var loginHandler = JELoginHandlerBuilder.BuildDefault();
        var accounts = loginHandler.AccountManager.GetAccounts();
        var number = 1;
        foreach (var account in accounts)
        {
            if (account is JEGameAccount jeAccount)
            {
                Console.WriteLine($"[{number}] {account.Identifier}");
                Console.WriteLine($"    LastAccess: {jeAccount.LastAccess}");
                Console.WriteLine($"    Username: {jeAccount.Profile?.Username}");
                Console.WriteLine($"    UUID: {jeAccount.Profile?.UUID}");
            }
            else
            {
                Console.WriteLine($"[{number}] {account.Identifier} (NOT JEGameAccount)");
            }
            number++;
        }
            
        IXboxGameAccount selectedAccount;
        if (accounts.Count == 0)
            selectedAccount = loginHandler.AccountManager.NewAccount();
        else
            selectedAccount = accounts.ElementAt(0);
        Console.WriteLine();
        var session = await loginHandler.Authenticate(selectedAccount);

        // show result
        Console.WriteLine(
            "Login result: \n" +
            $"Username: {session.Username}\n" +
            $"UUID: {session.UUID}");
        Console.WriteLine("Done");
    }
}