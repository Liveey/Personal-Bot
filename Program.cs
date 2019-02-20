using System;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace Okami
{
    class Program
    {
        private DiscordSocketClient Client;
        private CommandService Commands;

        static void Main(string[] args)
      => new Program().mainAsync().GetAwaiter().GetResult();
        private async Task mainAsync()
        {
            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug
            });

            //below ensures commands are case sensitive or not (I chose not)
            Commands = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug
            });

            Client.MessageReceived += Client_MessageReceived;
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);

            Client.Ready += Client_Ready;
            Client.Log += Client_Log;

            string Token = "Cannot put real token here, or else my server will be taken over by monsters"; //this connects the bot to your server
            await Client.LoginAsync(Discord.TokenType.Bot, Token);
            await Client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task Client_Log(LogMessage message)
        {
            Console.WriteLine($"{DateTime.Now} at {message.Source}] {message.Message}"); //information for me
        }

        private async Task Client_Ready()
        {
            await Client.SetGameAsync("with the lives of players!"); //this is a discord status, can easily be changed to anything.
        }

        private async Task Client_MessageReceived(SocketMessage MessageParam)
        {
            var Message = MessageParam as SocketUserMessage;
            var Context = new SocketCommandContext(Client, Message);

            if (Context.Message == null || Context.Message.Content == "") return;
            if (Context.User.IsBot) return;

            int ArgPos = 0;
            if (!(Message.HasStringPrefix("/a ", ref ArgPos) || Message.HasMentionPrefix(Client.CurrentUser, ref ArgPos))) return; //to speak to the bot you need to use /a first

            var Result = await Commands.ExecuteAsync(Context, ArgPos, null);
            if (!Result.IsSuccess)
            {
                Console.WriteLine($"{DateTime.Now} at Commands] Something went wrong with executing a command. Text:{Context.Message.Content} | Error: {Result.ErrorReason}");
                await Context.Channel.SendMessageAsync("What?"); //the console will tell me there is something wrong, and the bot will say "what?" if there is a syntax error
            }
        }
    }
}
