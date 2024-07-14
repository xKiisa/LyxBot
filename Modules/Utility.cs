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
        public class UserInfo
        {
            [Command("userinfo")]
            public static async Task AvatarAsync(CommandContext ctx, DiscordUser? member = null)
            {

                member ??= ctx.User;
                var userID      = member.Id;
                var userName    = member.Username;
                var userCreated = member.CreationTimestamp;
                    
                DiscordEmbedBuilder embed = new();
                {
                    embed.WithThumbnail(member.AvatarUrl);
                    embed.AddField("User Name", userName.ToString());
                    embed.AddField("User ID", userID.ToString());
                    embed.AddField("User Created", userCreated.ToString());
                    await ctx.RespondAsync(embed: embed);
                }
            }
        }
    }
}
