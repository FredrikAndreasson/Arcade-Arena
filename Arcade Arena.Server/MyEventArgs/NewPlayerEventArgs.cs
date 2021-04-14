using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Server.MyEventArgs
{
    class NewPlayerEventArgs
    {
        public string Username { set; get; }

        public NewPlayerEventArgs(string username)
        {
            Username = username;
        }
    }
}
