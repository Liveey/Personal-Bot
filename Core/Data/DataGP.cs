using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Okami.Resources.Database;
using System.Linq;
using System.Diagnostics;




namespace Okami.Core.Data
{
    public static class DataGP
    {
        public static int GetGold(ulong UserId) //loading and getting gold amount from database
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Golds.Where(x => x.UserId == UserId).Count() < 1)
                    return 0;
                return DbContext.Golds.Where(x => x.UserId == UserId).Select(x => x.Amount).FirstOrDefault();
            }

        }

        public static async Task SaveGold(ulong UserId, int Amount) //Saving a players gold
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Golds.Where(x => x.UserId == UserId).Count() < 1)
                {
                    //user doesnt have any yet make one for them
                    Debug.WriteLine("Yes Sir!"); //I had issues here, gold wasnt saving so this command was not being executed. Fixed now.
                    
                    DbContext.Golds.Add(new Gold
                    {
                        UserId = UserId, 
                        Amount = Amount
                    });
                }
                else
                {
                    Gold Current = DbContext.Golds.Where(x => x.UserId == UserId).FirstOrDefault();
                    Current.Amount += Amount;
                    DbContext.Golds.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }
    }
}
