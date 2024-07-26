using dotenv.net;
using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Commands.Processors.TextCommands.Parsing;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;

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
            var prefix = Environment.GetEnvironmentVariable("COMMAND_PREFIX");
            // Check if token and command prefix is set in .env file
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("The DISCORD_TOKEN is not set, please add it in the .env file");
                return;
            }
            if (string.IsNullOrEmpty(prefix))
            {
                Console.WriteLine("The COMMAND_PREFIX is not set, please add it in the .env file");
                return;
            }
            // Create the Discord client
            DiscordClientBuilder builder = DiscordClientBuilder.CreateDefault(token, DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents | DiscordIntents.Guilds | DiscordIntents.GuildPresences);
            builder.ConfigureEventHandlers
            (
                b => b.HandleGuildAvailable(async (s, e) =>
                {
                    var guildCount = s.Guilds.Count;
                    e.Guild.GetAllMembersAsync();
                    await Console.Out.WriteLineAsync($"\nBot token in use: {token}");
                    await Console.Out.WriteLineAsync($"Guilds joined: {guildCount}\n");
                })
            );
            DiscordClient client = builder.Build();

            // Set up Commands
            CommandsExtension commandsExtension = client.UseCommands(new CommandsConfiguration()
            {
                RegisterDefaultCommandProcessors = true,
            });
            // Register commands
            commandsExtension.AddCommands(typeof(Program).Assembly);

            TextCommandProcessor textCommandProcessor = new(new()
            {
                PrefixResolver = new DefaultPrefixResolver(true, prefix).ResolvePrefixAsync
            });
            await commandsExtension.AddProcessorsAsync(textCommandProcessor);

            client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(1)
            });

            // Set Bot Status
            DiscordActivity status = new($"\"{prefix}\" | Lyxbot" , DiscordActivityType.ListeningTo);
            // Connect the Discord client
            await client.ConnectAsync(status, DiscordUserStatus.Online);
            await Task.Delay(-1);
        }
    }
}