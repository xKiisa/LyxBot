using DSharpPlus.Commands;
using DSharpPlus.Entities;

namespace LyxBot.Modules
{
    public class Fun
    {
        public class CoinFlip
        {
            [Command("coinflip")]
            public static async Task CoinFlipAsync(CommandContext ctx)
            {
                Random random = new();
                int result = random.Next(2);
                string coinFlip = result == 0 ? "Heads" : "Tails";

                await ctx.RespondAsync($"{ctx.User.Mention} {coinFlip}!");
            }
        }
        public class Rate
        {
            [Command("rate")]
            public static async Task RateAsync(CommandContext ctx, DiscordUser? user = null)
            {
                user ??= ctx.User;
                Random random = new();
                int rateResult = random.Next(1, 10);
                if (rateResult != 8)
                {
                    await ctx.RespondAsync($"{user.Mention} is a {rateResult} out of 10!");
                }
                else
                {
                    await ctx.RespondAsync($"{user.Mention} is an {rateResult} out of 10!");
                }
            }
        }
        public class Help
        {
            [Command("help")]
            public static async Task HelpAsync(CommandContext ctx)
            {
                DiscordEmbedBuilder embed = new();
                embed.WithTitle("Bot Commands");
                embed.AddField("$help", "Displays this commands message");
                embed.AddField("$rate", "Rate someone");
                embed.AddField("$coinflip", "Flips a coin!");
                embed.AddField("$rps", "Enter rock, paper or scissors");
                embed.AddField("$dice", "Enter amount of dice and sides");  // e.g $dice 1 6
                embed.AddField("$avatar", "Display a user's avatar");
                embed.AddField("$userinfo", "Retrieve a user's profile information");
                embed.AddField("$guildinfo", "Shows information about the guild");
                await ctx.RespondAsync(embed: embed);
            }
        }
        public class Dice
        {
            [Command("dice")]
            public static async Task DiceAsync(CommandContext ctx, int numberOfDice, int numberOfSides)
            {
                if (numberOfDice <= 0 || numberOfSides <= 0)
                {
                    await ctx.RespondAsync("Number of dice and number of sides must be greater than zero.");
                    return;
                }

                Random random = new();
                var dice = new List<string>();
                for (int i = 0; i < numberOfDice; i++)
                {
                    int roll = random.Next(1, numberOfSides + 1);
                    dice.Add(roll.ToString());
                }
                var diceResult = string.Join(", ", dice);
                await ctx.RespondAsync($"{ctx.User.Mention} rolled: {diceResult}!");
            }
        }
    }
}
