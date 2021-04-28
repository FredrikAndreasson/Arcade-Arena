using Arcade_Arena.Server.Util;

namespace Arcade_Arena.Server.MyEventArgs
{
    class LogMessageEventArgs
    {
        public LogMessage LogMessage { get; set; }

        public LogMessageEventArgs(LogMessage logMessage)
        {
            LogMessage = logMessage;
        }
    }
}
