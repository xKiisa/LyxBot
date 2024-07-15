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
    }
}
