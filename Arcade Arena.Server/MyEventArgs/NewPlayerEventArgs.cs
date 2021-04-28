
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
