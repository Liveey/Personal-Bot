using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

using Discord.Commands;
using Discord;
using Discord.WebSocket;
using Okami.Core.Currency;
using Okami.Resources.Database;
using System.Linq;

namespace Okami.Core.Currency
{
    public class GP : ModuleBase<SocketCommandContext>
    {
        [Group("gel"), Alias("G", "Money", "Currency", "gp"), Summary("Gold group, manages G that players have.")] //Global for money (using with sqlite)
        public class GoldGroup : ModuleBase<SocketCommandContext>
        {
            [Command(""), Alias("Me", "My", "a", "money", "amount"), Summary("Shows your Gel currently")] //using /a gel and then nothing will prompt this command
            public async Task Me(IUser User = null)
            {
                if (User == null) //if the user isnt specified it will look for your ID in the database

                    await Context.Channel.SendMessageAsync($"{Context.User.Username}, you have {Data.DataGP.GetGold(Context.User.Id)} gel!"); 

                else //if the user is specified it will look for their ID in the data base

                    await Context.Channel.SendMessageAsync($"{User.Username}, they have {Data.DataGP.GetGold(User.Id)} gel!");
            }

            [Command("give"), Alias("Gift"), Summary("Used to give people gel")] //this is for giving gel/currency
            public async Task Give(IUser User = null, int Amount = 0)
            {
                if (User == null) //if user does not specify who, the bot will tell them to specify them
                {
                    await Context.Channel.SendMessageAsync("Who? You didnt mention giving it to anyone...(/a g give **<@user>** <amount>)");
                    return;
                }

                if (User.IsBot) //you cannot give bots currency, but I want to make it so the money disappears anyway.
                {
                    await Context.Channel.SendMessageAsync("Bots don't need your money, but if you want to give it so bad.. I'll take it from you! :)");
                    return;
                }

                if (Amount == 0) //no money means you arent giving anything. The bot will respond sarcastically here just because.
                {
                    await Context.Channel.SendMessageAsync($"Wow... you're charitable arent you? That good ole' donation of nothing! {User.Username} will appreciate it I am sure.");
                    return;
                }
                SocketGuildUser User1 = Context.User as SocketGuildUser; //this give command spawns money, so it wont work for anyone who isnt an admin currently
                if (!User1.GuildPermissions.Administrator)
                {
                    await Context.Channel.SendMessageAsync(":x: You need admin permissions for this command!");
                    return;
                }

                await Context.Channel.SendMessageAsync($":tada: {User.Mention} you have received **{Amount}** Gel from {Context.User.Username}");

                await Data.DataGP.SaveGold(User.Id, Amount); //this saves the gold in our database
            }
            [Command("reset"), Alias("del", "delete"), Summary("Resets the user progress")]
            public async Task Reset(IUser User = null)
            {
                if (User == null) //another admin command, to delete the user from the database entirely.
                {
                    await Context.Channel.SendMessageAsync($":x: You need to specify the user you wish to delete...");
                    return;
                }
                if (User.IsBot)
                {
                    await Context.Channel.SendMessageAsync("I dont have any progress... Tell Aki to let me play too! :robot:");
                    return;
                }
                SocketGuildUser User1 = Context.User as SocketGuildUser;
                if (!User1.GuildPermissions.Administrator)
                {
                    await Context.Channel.SendMessageAsync(":x: You need admin permissions for this command! :x:");
                    return;
                }

                await Context.Channel.SendMessageAsync($":skull: {User.Mention}, you have been terminated by {Context.User.Username}! This means you lost all of your gold! Boo Hoo!");

                using (var DbContext = new SqliteDbContext())
                {
                    DbContext.Golds.RemoveRange(DbContext.Golds.Where(x => x.UserId == User.Id));
                    await DbContext.SaveChangesAsync();
                }
            }
        }
    }
}
