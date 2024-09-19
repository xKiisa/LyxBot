using System.ComponentModel;
using dotenv.net;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;

namespace LyxBot.Modules
{
    public class Utility
    {
        public class Avatar
        {
            [Command("avatar")]
            [Description("Display a user's avatar")]
            public static async Task AvatarAsync(CommandContext ctx, DiscordUser? member = null)
            {
                member ??= ctx.User;
                var avatar = member.AvatarUrl;

                DiscordEmbedBuilder embed = new();
                {
                    embed.ImageUrl = avatar;
                    await ctx.RespondAsync(embed: embed);
                }
            }
        }
        public class UserInfo
        {
            [Command("userinfo")]
            [Description("Retrieve a user's profile information")]
            public static async Task AvatarAsync(CommandContext ctx, DiscordMember? member = null)
            {
                member ??= ctx.Member;
                if (member is null)
                {
                    return;
                }

                // Get member info
                var userId      = member.Id;
                var userName    = member.Username;
                var userCreated = member.CreationTimestamp;
                var joinedAt    = member.JoinedAt;

                // Respond with collected member info using an embed
                DiscordEmbedBuilder embed = new();
                {
                    embed.WithThumbnail(member.AvatarUrl);
                    embed.AddField("User Name", userName);
                    embed.AddField("User ID", userId.ToString());
                    embed.AddField("User Created", userCreated + " UTC");
                    embed.AddField("Joined Guild At", joinedAt + " UTC");
                    await ctx.RespondAsync(embed: embed);
                }
            }
        }
        public class GuildInfo
        {
            [Command("guildinfo")]
            [Description("Shows information about the server")]
            public static async Task AvatarAsync(CommandContext ctx)
            {
                var guild = ctx.Guild;
                if (guild is null)
                {
                    return;
                }

                // Collect Guild info
                var guildId          = guild.Id;
                var guildName        = guild.Name;
                var guildCreated     = guild.CreationTimestamp;
                var guildMemberCount = guild.MemberCount;
                var guildOwner       = guild.Owner;

                DiscordEmbedBuilder embed = new();
                {
                    embed.WithThumbnail(guild.IconUrl);
                    embed.AddField("Guild Name", guildName);
                    embed.AddField("Guild ID", guildId.ToString());
                    embed.AddField("Guild Creation", guildCreated + " UTC");
                    embed.AddField("Guild Member Count", guildMemberCount.ToString());
                    embed.AddField("Guild Owner Name", guildOwner.DisplayName);
                    embed.AddField("Guild Owner ID", guildOwner.Id.ToString());
                    await ctx.RespondAsync(embed: embed);
                }
            }
        }
        public class Addcoins
        {
            [Command("addcoins")]
            [Description("Adds Lyx Coins to select user")]
            [RequireApplicationOwner] // Only for bot owner
            public static async Task AddcoinsAsync(CommandContext ctx, DiscordMember? member = null, int amount = 0)
            {
                if (member is null)
                {
                    await ctx.RespondAsync("Please tag a user to add coins to!");
                    return;
                }
                if (amount <= 0)
                {
                    await ctx.RespondAsync("Please enter a positive amount of coins to add to the user!");
                    return;
                }
                // Add entered amount to members balance
                await DatabaseConnection.UpdateUserBalance(ctx.User.Id.ToString(), amount);
                if (amount != 1)
                {
                    await ctx.RespondAsync($"Added {amount} Lyx coins to {member.Mention}");
                }
                else
                {
                    await ctx.RespondAsync($"Added {amount} Lyx coin to {member.Mention}");
                }
            }
        }
        public class Give
        {
            [Command("give")]
            [Description("Give select amount of own Lyx coins to another user")]
            public static async Task GiveAsync(CommandContext ctx, DiscordMember? member = null, int amount = 0)
            {
                if (member is null || member == ctx.User)
                {
                    await ctx.RespondAsync("Please tag another user to give Lyx coins to!");
                    return;
                }
                if (amount <= 0)
                {
                    await ctx.RespondAsync("Please enter a positive amount of Lyx coins to add to the user!");
                    return;
                }
                try
                {
                    // Reduce select amount of coins from giver
                    await DatabaseConnection.UpdateUserBalance(ctx.User.Id.ToString(), -amount);

                    // Add select amount of coins to recipient
                    await DatabaseConnection.UpdateUserBalance(member.Id.ToString(), amount);


                    if (amount != 1)
                    {
                        await ctx.RespondAsync($"{ctx.User.Mention} Gave {amount} Lyx coins to {member.Mention}");
                    }
                    else
                    {
                        await ctx.RespondAsync($"{ctx.User.Mention} Gave {amount} Lyx coin to {member.Mention}");
                    }
                }
                catch (InvalidOperationException ex)
                {
                    await ctx.RespondAsync(ex.Message);
                }
            }
        }
        public class Help
        {
            [Command("help")]
            [Description("Displays all available commands")]
            public static async Task HelpAsync(CommandContext ctx)
            {
                DotEnv.Load();
                var prefix = Environment.GetEnvironmentVariable("COMMAND_PREFIX");  // Get command prefix for use in embed

                
                // Create a list with the help pages
                var embeds = new List<DiscordEmbedBuilder>
                {
                    new DiscordEmbedBuilder()
                        .WithTitle("Bot Commands - Page 1")
                        .AddField($"{prefix}help", "Displays all available commands")
                        .AddField($"{prefix}rate", "Rate someone")
                        .AddField($"{prefix}coinflip", "Flips a coin!")
                        .AddField($"{prefix}rps", "Enter rock, paper or scissors")
                        .AddField($"{prefix}dice", "Enter amount of dice and sides"),  // e.g $dice 1 6,

                    new DiscordEmbedBuilder()
                        .WithTitle("Bot Commands - Page 2")
                        .AddField($"{prefix}avatar", "Display a user's avatar")
                        .AddField($"{prefix}userinfo", "Retrieve a user's profile information")
                        .AddField($"{prefix}guildinfo", "Shows information about the server")
                        .AddField($"{prefix}kick", "Kicks a user")
                        .AddField($"{prefix}ban", "Bans a user"),

                    new DiscordEmbedBuilder()
                        .WithTitle("Bot Commands - Page 3")
                        .AddField($"{prefix}timeout", "Timeout a user for a specified duration")
                        .AddField($"{prefix}purge", "Removes a specified amount of messages")
                        .AddField($"{prefix}coins", "Shows the balance of all users in the server")
                        .AddField($"{prefix}give", "Give select amount of own coins to another user")
                        .AddField($"{prefix}addcoins", "Adds Lyx Coins to select user"),

                    new DiscordEmbedBuilder()
                        .WithTitle("Bot Commands - Page 4")
                        .AddField($"{prefix}flip", "Gambles an amount of Lyx coins in a coinflip"),
                };
                // Turns the embeds list into pages
                var pages = new List<Page>();
                foreach (var embed in embeds)
                {
                    pages.Add(new Page { Embed = embed });
                }

                var interactivity = ctx.Client.GetInteractivity();
                await interactivity.SendPaginatedMessageAsync(ctx.Channel, ctx.User, pages);
            }
        }
    }
}