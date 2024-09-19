using System.ComponentModel;
using DSharpPlus.Commands;


namespace LyxBot.Modules
{
    public class Flip
    {
        [Command("flip")]
        [Description("Gambles an amount of Lyx coins in a coinflip")]
        public static async Task FlipAsync(CommandContext ctx, int amount = 0)
        {
            Random random = new();
            int result = random.Next(2);
            string coinFlip = result == 0 ? "Heads" : "Tails";

            if (amount <= 0)
            {
                await ctx.RespondAsync("Please enter a positive amount of Lyx coins to gamble!");
                return;
            }
            try
            {
                if (result == 0)
                {
                    await DatabaseConnection.UpdateUserBalance(ctx.User.Id.ToString(), amount);
                    await ctx.RespondAsync($"{ctx.User.Mention} {coinFlip}! You win **{amount}** Lyx coins!");
                }
                else
                {
                    await DatabaseConnection.UpdateUserBalance(ctx.User.Id.ToString(), -amount);
                    await ctx.RespondAsync($"{ctx.User.Mention} {coinFlip}! You lost **{amount}** Lyx coins!");
                }
            }
            catch (InvalidOperationException ex)
            {
                await ctx.RespondAsync(ex.Message);
            }
        }
    }
}