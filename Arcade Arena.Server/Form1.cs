using Arcade_Arena.Server.Managers;
using Arcade_Arena.Server.MyEventArgs;
using System;

using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arcade_Arena.Server
{
    public partial class Form1 : Form
    {
        private Task task;
        private Server server;
        private ManagerLogger managerLogger;
        private CancellationTokenSource cancellationTokenSource;

        public Form1()
        {
            managerLogger = new ManagerLogger();
            managerLogger.NewLogMessageEvent += NewLogMessageEvent;
            server = new Server(managerLogger);
            server.NewPlayer += NewPlayerEvent;
            InitializeComponent();
        }

        void NewPlayerEvent(object sender, NewPlayerEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler<NewPlayerEventArgs>(NewPlayerEvent), sender, e);
                return;
            }

            //lstPlayers.Items.Add(e.Username);
        }

        private void NewLogMessageEvent(object sender, LogMessageEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler<LogMessageEventArgs>(NewLogMessageEvent), sender, e);
                return;
            }
            dgwServerStatusLog.Rows.Add(new[] { e.LogMessage.Id, e.LogMessage.Message });
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnStop.Enabled = true;

            cancellationTokenSource = new CancellationTokenSource();
            task = new Task(server.Run, cancellationTokenSource.Token);
            task.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (task != null && cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                btnStart.Enabled = true;
                btnStop.Enabled = false;
            }
        }
    }
}
