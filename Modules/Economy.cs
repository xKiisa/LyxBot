using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Data.Sqlite;
using System.ComponentModel;

namespace LyxBot.Modules
{
    public class Economy
    {
        private const string ConnectionString = "Data Source=LyxBot.db";

        public class Coins
        {
            [Command("coins")]
            [RequireGuild]
            [Description("Shows the balance of all users in the server")]
            public static async Task BalancesAsync(CommandContext ctx)
            {
                var guild = ctx.Guild;
                if (guild is null)
                {
                    return;
                }

                var userBalances = await GetAllUserBalancesAsync();
                if (userBalances.Count == 0)
                {
                    await ctx.RespondAsync("No user balances found");
                    return;
                }
                var response = new List<string>();

                foreach (var userBalance in userBalances)
                {
                    var members = guild.Members;

                    response.Add(members.TryGetValue(Convert.ToUInt64(userBalance.Key), out var member)
                        ? $"**{member.DisplayName}:**\n {userBalance.Value} Lyx Coins"
                        : $"UserId **{userBalance.Key}:**\n {userBalance.Value} Lyx Coins");
                }

                const int pageSize = 10; // Page entries
                var pages = new List<Page>();
                for (int i = 0; i < response.Count; i += pageSize)
                {
                    var embed = new DiscordEmbedBuilder()
                        .WithTitle("User Balances")
                        .WithDescription(string.Join("\n", response.Skip(i).Take(pageSize)));
                    var page = new Page { Embed = embed };
                    pages.Add(page);
                }
                // Send paginated message
                var interactivity = ctx.Client.GetInteractivity();
                await interactivity.SendPaginatedMessageAsync(ctx.Channel, ctx.User, pages);
            }
            private static async Task<Dictionary<string, int>> GetAllUserBalancesAsync()
            {
                var userBalances = new Dictionary<string, int>();

                await using SqliteConnection connection = new(ConnectionString);
                {
                    connection.Open();
                    string selectQuery = "SELECT UserId, Balance FROM UserBalances";
                    await using SqliteCommand selectCommand = new(selectQuery, connection);
                    await using var reader = await selectCommand.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        string userId = reader.GetString(0);
                        int balance = reader.GetInt32(1);
                        userBalances[userId] = balance;
                    }
                }
                return userBalances;
            }
        }
    }
}