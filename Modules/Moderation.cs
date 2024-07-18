using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Entities;

namespace LyxBot.Modules
{
    public class Moderation
    {
        public class Kick
        {
            [Command("kick")]
            [Description("Kicks a User")]
            [RequirePermissions(DiscordPermissions.KickMembers)]
            public static async Task KickAsync(CommandContext ctx, DiscordMember? member = null)
            {
                if (member is null)
                {
                    return;
                }

                await member.RemoveAsync();
                await ctx.RespondAsync($"Kicked {member.Mention} from the server!");
            }
        }
        public class Ban
        {
            [Command("ban")]
            [Description("Bans a User")]
            [RequirePermissions(DiscordPermissions.KickMembers)]
            public static async Task BanAsync(CommandContext ctx, DiscordMember? member = null)
            {
                if (member is null)
                {
                    return;
                }

                await member.BanAsync();
                await ctx.RespondAsync($"Banned {member.Mention} from the server!");
            }
        }
        public class Purge
        {
            [Command("purge")]
            [Description("Removes a specified amount of messages")]
            [RequirePermissions(DiscordPermissions.ManageMessages)]
            public static async Task PurgeAsync(CommandContext ctx, int amount = 0)
            {
                if (amount == 0)
                {
                    await ctx.RespondAsync($"Please enter amount of Messages to delete!");
                }
                else
                {
                    try
                    {
                        var messages = ctx.Channel.GetMessagesAsync(amount + 1);
                        var now = DateTimeOffset.UtcNow;
                        var messagesToDelete = new List<DiscordMessage>();

                        await ctx.Channel.DeleteMessagesAsync(messages);
                        await ctx.RespondAsync($"Removed {amount} messages!");
                    }
                    catch (ArgumentException)
                    {
                        await ctx.RespondAsync("Error! Messages might be older than 14 days!");
                    }
                }
            }
        }
        public class Timeout
        {
            [Command("timeout")]
            [Description("Timeout a user for a specified duration")]
            [RequirePermissions(DiscordPermissions.Administrator)]
            public static async Task TimeoutAsync(CommandContext ctx, DiscordMember? member, int amount = 0)
            {
                if (member is null)
                {
                    return;
                }
                if (amount == 0)
                {
                    await ctx.RespondAsync($"Please enter a duration!");
                }
                else
                {
                    await member.TimeoutAsync(DateTime.Now + TimeSpan.FromMinutes(amount));
                    if (amount != 1)
                    {
                        await ctx.RespondAsync($"{member.Mention} has been timed out for {amount} minutes.");
                    }
                    else
                    {
                        await ctx.RespondAsync($"{member.Mention} has been timed out for {amount} minute.");
                    }
                }
            }
        }
    }
}