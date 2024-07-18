using System.ComponentModel;
using dotenv.net;
using DSharpPlus.Commands;
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
                var userID      = member.Id;
                var userName    = member.Username;
                var userCreated = member.CreationTimestamp;
                var joinedAt    = member.JoinedAt;

                // Respond with collected member info using an embed
                DiscordEmbedBuilder embed = new();
                {
                    embed.WithThumbnail(member.AvatarUrl);
                    embed.AddField("User Name", userName.ToString());
                    embed.AddField("User ID", userID.ToString());
                    embed.AddField("User Created", userCreated.ToString() + " UTC");
                    embed.AddField("Joined Guild At", joinedAt.ToString() + " UTC");
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
                var guildID          = guild.Id;
                var guildName        = guild.Name;
                var guildCreated     = guild.CreationTimestamp;
                var guildMemberCount = guild.MemberCount;
                var guildOwner       = guild.Owner;

                DiscordEmbedBuilder embed = new();
                {
                    embed.WithThumbnail(guild.IconUrl);
                    embed.AddField("Guild Name", guildName.ToString());
                    embed.AddField("Guild ID", guildID.ToString());
                    embed.AddField("Guild Creation", guildCreated.ToString() + " UTC");
                    embed.AddField("Guild Member Count", guildMemberCount.ToString());
                    embed.AddField("Guild Owner Name", guildOwner.DisplayName.ToString());
                    embed.AddField("Guild Owner ID", guildOwner.Id.ToString());
                    await ctx.RespondAsync(embed: embed);
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
                        .AddField($"{prefix}purge", "Removes a specified amount of messages"),
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