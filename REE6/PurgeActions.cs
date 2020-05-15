using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;

namespace REE6
{
    public static class PurgeActions
    {
        public static async Task ChannelPurge(IGuild guild)
        {
            ITextChannel statusChannel = await guild.CreateTextChannelAsync("REE6 Purge Progress", channel => channel.Position = 1);

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
                if (channel == statusChannel)
                    break;
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
            System.Random random = new System.Random();

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
                ulong randomCategoryId = categoryIds[random.Next(categoryNum)];

                ITextChannel textChannel = await guild.CreateTextChannelAsync("REEEEEEEEEE! text channelEEEEEEEEEEEEEEEEE!!!!");
                try
                {
                    await textChannel.ModifyAsync(channel => channel.CategoryId = randomCategoryId);
                }
                catch
                {
                    await textChannel.SendMessageAsync("This channel could not be put into a category");
                }
            }
            // Recreate Voice Channels
            await statusMessage.ModifyAsync(msg => msg.Content = "REEEEEEEEEE! Creating voice channels");
            for (int i = voiceChannelNum; i > 0; i--)
            {
                ulong randomCategoryId = categoryIds[random.Next(categoryNum)];

                IVoiceChannel voiceChannel = await guild.CreateVoiceChannelAsync("REEEEEEEEEE! voice channelEEEEEEEEEEEEEEEEE!!!!");
                try
                {
                    await voiceChannel.ModifyAsync(channel => channel.CategoryId = randomCategoryId);
                }
                catch
                {
                    await statusChannel.SendMessageAsync("A voice channel could not be put in a category");
                }
            }

            await statusMessage.ModifyAsync(msg => msg.Content = "REEEEEEEEEE! Channel Purge Operation Complete!");
            await statusChannel.SendMessageAsync("Deleting channel in 3 seconds...");
            System.Timers.Timer terminate = new System.Timers.Timer(3000);
            terminate.AutoReset = false;
            terminate.Elapsed += new System.Timers.ElapsedEventHandler(delegate (object sender, System.Timers.ElapsedEventArgs e) { statusChannel.DeleteAsync(); });
            terminate.Start();
        }
    }
}
