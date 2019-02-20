using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Okami.Resources.Database
{
    public class Gold //this is for our database, this will alow me to get and set values of gold
    {
        [Key]
        public ulong UserId { get; set; }
        public int Amount { get; set; }
    }
}
