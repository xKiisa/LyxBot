using DSharpPlus.Commands;
using DSharpPlus.Entities;
using DSharpPlus.Commands.ContextChecks;

namespace LyxBot.Modules
{
    public class Moderation
    {
        public class Kick
        {
            [Command("kick")]
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
    }
}
