using DSharpPlus.Commands;
using LyxBot.DBConnection;
using System.ComponentModel;

namespace LyxBot.Modules
{
    public class Games
    {
        public class RPS
        {

            [Command("rps")]
            [Description("Enter rock, paper or scissors")]
            public static async Task RpsAsync(CommandContext ctx, string? memberChoice = null)
            {
                Random random = new();
                string[] choices = ["rock", "paper", "scissors"];
                string botChoice = choices[random.Next(choices.Length)];

                if (string.IsNullOrEmpty(memberChoice))
                {
                    await ctx.RespondAsync($"{ctx.User.Mention} Please enter one of the following: `rock`, `paper`, `scissors`");
                    return;
                }

                memberChoice = memberChoice.ToLower();

                if (memberChoice == botChoice)
                {
                    await ctx.RespondAsync($"{ctx.User.Mention} it's a tie! We both chose **{botChoice}**.");
                }
                else if ((memberChoice == "rock" && botChoice == "scissors") ||
                         (memberChoice == "scissors" && botChoice == "paper") ||
                         (memberChoice == "paper" && botChoice == "rock"))
                {
                    int awardCoin = random.Next(1, 5);
                    if (awardCoin != 1)
                    {
                        await ctx.RespondAsync($"{ctx.User.Mention} You chose **{memberChoice}** and I chose **{botChoice}**!\nYou won and earned {awardCoin} Lyx Coins!");
                    }
                    else
                    {
                        await ctx.RespondAsync($"{ctx.User.Mention} You chose **{memberChoice}** and I chose **{botChoice}**!\nYou won and earned {awardCoin} Lyx Coin!");
                    }
                    // Add random amount of won currency to DB
                    await DatabaseConnection.UpdateUserBalance(ctx.User.Id.ToString(), awardCoin);
                }
                else if ((memberChoice == "scissors" && botChoice == "rock") ||
                         (memberChoice == "paper" && botChoice == "scissors") ||
                         (memberChoice == "rock" && botChoice == "paper"))
                {
                    await ctx.RespondAsync($"{ctx.User.Mention} You chose **{memberChoice}** and I chose **{botChoice}**!\nYou lost!");
                }
                else
                {
                    await ctx.RespondAsync($"{ctx.User.Mention} Something went wrong. Please enter one of the following: `rock`, `paper`, `scissors`");
                }
            }
        }
    }
}