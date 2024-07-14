using DSharpPlus.Commands;
using DSharpPlus.Entities;

namespace LyxBot.Modules
{
    public class Utility
    {
        public class Avatar
        {
            [Command("avatar")]
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
    }
}
