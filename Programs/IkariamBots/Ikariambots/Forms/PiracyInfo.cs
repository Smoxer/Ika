using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Library;

namespace IkariamBots.Forms
{
    public partial class PiracyInfo : Form
    {
        public int id;

        private Languages lang = Languages.Instance;
        private DateTime endTime;
        private List<Label> topLabels;
        private bool timeUpdated;

        public PiracyInfo(string server, int id)
        {
            InitializeComponent();

            #region Translate
            Text = lang.Get("Misc", "Info");
            this.server.Text = lang.Get("Misc", "Server") + ": " + server;
            #endregion

            this.id = id;
            timeUpdated = false;

            topLabels = new List<Label>();
            topLabels.Add(top1);
            topLabels.Add(top2);
            topLabels.Add(top3);
            topLabels.Add(top4);
            topLabels.Add(top5);
            topLabels.Add(top6);
            topLabels.Add(top7);
            topLabels.Add(top8);
            topLabels.Add(top9);
            topLabels.Add(top10);
        }

        private void PiracyInfo_Load(object sender, EventArgs e)
        {
            UpdateTimeLeft();
        }

        public void UpdateTimeEnd(DateTime endTime, string server)
        {
            this.endTime = endTime;
            if (!timeEnd.InvokeRequired)
                timeEnd.Text = endTime.ToString(Config.DATE_TIME_FORMAT);
            else
                timeEnd.BeginInvoke(new Action(() => { timeEnd.Text = endTime.ToString(Config.DATE_TIME_FORMAT); }));
            timeUpdated = true;
        }

        private void UpdateTimeLeft()
        {
            Thread t = new Thread(delegate ()
            {
                string days = lang.Get("Misc", "Days");
                while (true)
                {
                    try
                    {
                        TimeSpan timeSpanLeft = (endTime - DateTime.Now);
                        string span = string.Format("{0} {1}, {2}:{3}:{4}", timeSpanLeft.Days, days, timeSpanLeft.Hours, timeSpanLeft.Minutes, timeSpanLeft.Seconds);
                        if (!timeLeft.InvokeRequired)
                            timeLeft.Text = span;
                        else
                            timeLeft.BeginInvoke(new Action(() => { timeLeft.Text = span; }));

                        if(timeSpanLeft <= new TimeSpan(0, 0, 0, 5) && timeUpdated)
                        {
                            PhoneConnection phone = new PhoneConnection(Settings.Instance.Get("Pushbullet", ""));
                            phone.SendPush(Config.PROGRAM_NAME, lang.Get("Piracy", "RotationEnd"));
                            break;
                        }
                    } catch (Exception e) { }
                    Thread.Sleep(1000);
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        public void UpdateTop(List<Ikariam.PiracyTopUser> text)
        {
            int count = 0;
            
            foreach (Label topLabel in topLabels)
            {
                if (!topLabel.InvokeRequired)
                {
                    //MessageBox.Show("s");
                    SetText(
                        (count < text.Count) ? text[count] : null,
                        topLabel
                    );
                }
                else
                {
                    topLabel.BeginInvoke(new Action(() =>
                    {
                        SetText(
                            (count < text.Count) ? text[count] : null,
                            topLabel
                        );
                    }
                    ));
                }
                count++;
            }
        }

        private void SetText(Ikariam.PiracyTopUser user, Label topLabel)
        {
            if (null == user)
            {
                topLabel.Text = "";
                return;
            }
            ToolTip infoToolTip = new ToolTip();
            infoToolTip.ToolTipIcon = ToolTipIcon.Info;
            infoToolTip.IsBalloon = true;
            infoToolTip.ShowAlways = true;
            infoToolTip.UseAnimation = true;
            infoToolTip.ToolTipTitle = user.name;
            topLabel.Text = "#" + user.place +  " - " + user.name;
            infoToolTip.SetToolTip(topLabel, string.Format("#{0}\n{1}\n{2} {3}\n{6}: {4} {5}\n{7}: {8}", 
                user.place, 
                user.name, 
                user.capturePoints, 
                Languages.Instance.Get("Piracy", "CapturePoints"), 
                (user.distance.Equals(string.Empty) ? "---" : user.distance),
                Languages.Instance.Get("Misc", "Islands"), 
                Languages.Instance.Get("Misc", "Distance"),
                Languages.Instance.Get("Misc", "Server"),
                user.server
           ));

            if (!server.InvokeRequired)
                server.Text = lang.Get("Misc", "Server") + ": " + user.server;
            else
                server.BeginInvoke(new Action(() => { server.Text = lang.Get("Misc", "Server") + ": " + user.server; }));
            
        }
    }
}

