using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Entities;

namespace LyxBot.Modules
{
    public class Fun
    {
        public class CoinFlip
        {
            [Command("coinflip")]
            [Description("Flips a coin!")]
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
            [Description("Rate someone")]
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
        public class Dice
        {
            [Command("dice")]
            [Description("Enter amount of dice and sides")]
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