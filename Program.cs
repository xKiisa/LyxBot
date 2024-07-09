using dotenv.net;
using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Commands.Processors.TextCommands.Parsing;
using DSharpPlus.Entities;

namespace LyxBot
{
    class Program
    {
        static async Task Main()
        {
            // Load environment variables from .env file
            DotEnv.Load();
            // Get the token from environment variables
            var token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
            // Check if token is set in .env file
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("The DISCORD_TOKEN is not set, please add it in the .env file");
                return;
            }
            Console.WriteLine($"Bot token in use: {token}\n");

            // Create the Discord client
            DiscordClientBuilder builder = DiscordClientBuilder.CreateDefault(token, DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents);
            DiscordClient client = builder.Build();

            // Set up Commands
            CommandsExtension commandsExtension = client.UseCommands(new CommandsConfiguration()
            {
                RegisterDefaultCommandProcessors = true
            });

            // Register commands
            commandsExtension.AddCommands(typeof(Program).Assembly);
            TextCommandProcessor textCommandProcessor = new(new()
            {
                PrefixResolver = new DefaultPrefixResolver(true, "$").ResolvePrefixAsync
            });

            await commandsExtension.AddProcessorsAsync(textCommandProcessor);



            // Set Bot Status
            DiscordActivity status = new("\"$\" | Lynxbot" , DiscordActivityType.ListeningTo);
            // Connect the Discord client
            await client.ConnectAsync(status, DiscordUserStatus.Online);
            await Task.Delay(-1);
        }
    }
}