using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

using Discord;
using Discord.Commands;

namespace Okami.Core.Commands
{
    public class HelloWorld : ModuleBase<SocketCommandContext> //these are basic commands for testing, typing /a hello will prompt this
    {
        [Command("Hello"), Alias("Hello World", "hallo", "Hi", "hey", "hello", "yo", "hai", "sup","wazzup", "ello"), Summary("Hello command")]
        public async Task Liveey()
        {
            await Context.Channel.SendMessageAsync("Hello! I am still being programmed! Sorry I dont have many things I can do or say yet.");
        }

        [Command("embed"), Summary("Labeled text command")] //this is used to create an embed. Since I have no use for one this is a sample command.
        public async Task Embed([Remainder] string input = "none")
        {
            EmbedBuilder Embed = new EmbedBuilder();
            Embed.WithAuthor("Liveeys Test Embed", Context.User.GetAvatarUrl());
            Embed.WithColor(40, 200, 150);
            Embed.WithFooter("This is a footer", Context.Guild.Owner.GetAvatarUrl());
            Embed.WithDescription("This is a description for your **test** Embed, with a stupid link. \n" +
                "[My favorite _website_](https://github.com/Liveey/Personal-Bot) ");
            Embed.AddField("User input", input);
            await Context.Channel.SendMessageAsync("", false, Embed.Build());
        }

        [Command("help"), Alias("Codes", "Give Codes", "list", "Commands", "help please"), Summary("Help command to show list of player commands (or not)")] 
        //this is the help command, this is where I should tell people how to use the commands. However that is not something I am working on until finished.
        public async Task Help()
        {
            await Context.Channel.SendMessageAsync("Ew gross. You dont know my commands? You must be new..");
        }
  
    }

}
