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
    [Summary("Purge Commands")]
    public class Purge : ModuleBase<SocketCommandContext>
    {
        [Command("all")]
        public async Task PurgeAll()
        {
            await ReplyAsync("Purging everything...");
            await ReplyAsync("REEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            await PurgeActions.RolePurge(Context.Guild);
            await PurgeActions.ChannelPurge(Context.Guild);
        }

        [Command("roles")]
        public async Task PurgeRole()
        {
            await ReplyAsync("Purging roles...");
            await ReplyAsync("REEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            await PurgeActions.RolePurge(Context.Guild);
        }

        [Command("channels")]
        public async Task PurgeChannels()
        {
            await ReplyAsync("Purging channels...");
            await ReplyAsync("REEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            await PurgeActions.ChannelPurge(Context.Guild);
        }
    }
}
