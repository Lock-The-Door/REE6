using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace REE6
{
    public class Program
    {
        private DiscordSocketClient _client;
        private CommandService _commands;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug
            });
            _commands = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                LogLevel = LogSeverity.Debug
            });

            Command_Handler command_Handler = new Command_Handler(client: _client, commands: _commands);
            await command_Handler.InstallCommandsAsync();

            _commands.Log += Log;
            _client.Log += Log;

            await _client.LoginAsync(TokenType.Bot,
                Environment.GetEnvironmentVariable("REE6_API_TOKEN", EnvironmentVariableTarget.User));

            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
