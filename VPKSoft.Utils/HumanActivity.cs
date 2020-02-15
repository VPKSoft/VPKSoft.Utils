#region License
/*
VPKSoft.Utils

Some utilities by VPKSoft.
Copyright © 2020 VPKSoft, Petteri Kautonen

Contact: vpksoft@vpksoft.net

This file is part of VPKSoft.Utils.

VPKSoft.Utils is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

VPKSoft.Utils is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with VPKSoft.Utils.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using TTimer = System.Threading.Timer;

namespace VPKSoft
{
    namespace Utils
    {
        /// <summary>
        /// Event arguments passed with UserWake event from HumanActivity class instance.
        /// </summary>
        public class UserWakeEventArgs : EventArgs
        {
            /// <summary>
            /// The time the user has been idle.
            /// </summary>
            public TimeSpan UserSlept = new TimeSpan(0);

            /// <summary>
            /// A UserWakeEventArgs constructor.
            /// </summary>
            public UserWakeEventArgs()
                : base()
            {

            }
        }

        /// <summary>
        /// Event arguments passed with UserSleep event from HumanActivity class instance.
        /// </summary>
        public class UserSleepEventArgs : EventArgs
        {
            /// <summary>
            /// Stop the idle UserIdle event from occurring continuously. 
            /// It will start again when user activity is detected.
            /// </summary>
            public bool StopIdle = true;

            /// <summary>
            /// A UserSleepEventArgs constructor.
            /// </summary>
            public UserSleepEventArgs()
                : base()
            {

            }
        }

        /// <summary>
        /// A class that monitors user activity (idle time) within a Windows Forms application.
        /// </summary>
        public class HumanActivity : IMessageFilter
        {
            /// <summary>
            /// A static list of process id's for applications that use this class.
            /// </summary>
            private static List<int> apps = new List<int>();

            /// <summary>
            /// A delegate for the UserSleep event.
            /// </summary>
            /// <param name="sender">A sender of the event.</param>
            /// <param name="e">Event arguments passed with UserSleep event.</param>
            public delegate void OnUserSleep(object sender, UserSleepEventArgs e);

            /// <summary>
            /// A delegate for the UserWake event.
            /// </summary>
            /// <param name="sender">A sender of the event.</param>
            /// <param name="e">Event arguments passed with UserWake event.</param>
            public delegate void OnUserWake(object sender, UserWakeEventArgs e);

            /// <summary>
            /// A DateTime used internally to measure time spans.
            /// </summary>
            private DateTime dt1 = new DateTime(0);

            /// <summary>
            /// Another DateTime used internally to measure time spans.
            /// </summary>
            private DateTime dt2 = new DateTime(0);

            /// <summary>
            /// Returns true if the user is sleeping..
            /// </summary>
            public bool Sleeping
            {
                get
                {
                    return tm == null;
                }
            }

            /// <summary>
            /// Destroys the timer that measures the user idle time.
            /// <para/>Same as Enabled = false.
            /// </summary>
            public void Stop()
            {
                DestructTimer();
            }

            /// <summary>
            /// An event that is fired when user activity has been idle for a given amount of seconds.
            /// </summary>
            public event OnUserSleep UserSleep = null;

            /// <summary>
            /// An event that is fired when user activity has occurred.
            /// </summary>
            public event OnUserWake UserWake = null;

            /// <summary>
            /// An idle time in seconds assigned by the class constructor.
            /// </summary>
            private readonly int idleTime = 0;

            /// <summary>
            /// A constructor for the HumanActivity class.
            /// </summary>
            /// <param name="idleTimeSeconds">An idle time in seconds to measure user activity before the UserSleep event is fired.</param>
            public HumanActivity(int idleTimeSeconds)
                : base()
            {
                if (apps.Contains(Process.GetCurrentProcess().Id))
                {
                    throw new Exception("This application has already added HumanActivity class instance.");
                }
                Application.AddMessageFilter(this);
                idleTime = idleTimeSeconds * 1000;
                ConstructTimer();
            }

            /// <summary>
            /// A callback method that is given to a System.Threading.Timer
            /// <para/>class instance to fire the UserSleep event after a certain time span.
            /// </summary>
            /// <param name="stateInfo">An object that is passed to this method by a System.Threading.Timer class instance.</param>
            private void TTimerCall(object stateInfo)
            {
                if (UserSleep != null)
                {
                    UserSleepEventArgs e = new UserSleepEventArgs();
                    DestructTimer();
                    UserSleep(this, e);
                    if (!e.StopIdle)
                    {
                        RebuildTimer();
                    }
                }
            }

            /// <summary>
            /// Creates a new System.Threading.Timer class instance with an interval in seconds
            /// <para/>given the class constructor.
            /// </summary>
            private void ConstructTimer()
            {
                if (tm == null)
                {
                    dt1 = DateTime.Now;
                    tm = new TTimer(TTimerCall, null, idleTime, Timeout.Infinite);
                }
            }

            /// <summary>
            /// Destructs a System.Threading.Timer class instance if one exists with an instance of this class.
            /// </summary>
            private void DestructTimer()
            {
                if (tm != null)
                {
                    tm.Dispose();
                    tm = null;
                    dt2 = DateTime.Now;
                }
            }

            /// <summary>
            /// Initiates subsequent calls to DestructTimer() and ConstructTimer() methods.
            /// </summary>
            private void RebuildTimer()
            {
                DestructTimer();
                ConstructTimer();
            }

            /// <summary>
            /// Gets or sets a value indicating if a user's idle time should be monitored.
            /// </summary>
            public bool Stopped
            {
                get
                {
                    return tm == null;
                }

                set
                {
                    if (value)
                    {
                        RebuildTimer();
                    }
                    else
                    {
                        DestructTimer();
                    }
                }
            }

            /// <summary>
            /// A value indicating if the monitoring of user activity is enabled.
            /// </summary>
            private bool enabled = true;

            /// <summary>
            /// Gets or sets a value indicating if the monitoring of user activity should be enabled.
            /// </summary>
            public bool Enabled
            {
                get
                {
                    return enabled;
                }

                set
                {
                    enabled = value;
                    if (!value)
                    {
                        DestructTimer();
                    }
                }
            }

            /// <summary>
            /// Constant for a message that the IMessageFilter is monitoring.
            /// </summary>
            private const int WM_LBUTTONDOWN = 0x0201;

            /// <summary>
            /// Constant for a message that the IMessageFilter is monitoring.
            /// </summary>
            private const int WM_MBUTTONDOWN = 0x0207;

            /// <summary>
            /// Constant for a message that the IMessageFilter is monitoring.
            /// </summary>
            private const int WM_MOUSEHWHEEL = 0x020E;

            /// <summary>
            /// Constant for a message that the IMessageFilter is monitoring.
            /// </summary>
            private const int WM_RBUTTONDOWN = 0x0204;

            /// <summary>
            /// Constant for a message that the IMessageFilter is monitoring.
            /// </summary>
            private const int WM_XBUTTONDOWN = 0x020B;

            /// <summary>
            /// Constant for a message that the IMessageFilter is monitoring.
            /// </summary>
            private const int WM_MOUSEWHEEL = 0x020A;

            /// <summary>
            /// Constant for a message that the IMessageFilter is monitoring.
            /// </summary>
            private const int WM_KEYDOWN = 0x0100;

            /// <summary>
            /// Constant for a message that the IMessageFilter is monitoring.
            /// </summary>
            private const int WM_MOUSEMOVE = 0x0200; // added 03.06.2018..


            /// <summary>
            /// A System.Threading.Timer class instance to fire the UserSleep event
            /// <para/>if a given time span of inactivity has occurred.
            /// </summary>
            private TTimer tm = null;

            /// <summary>
            /// An implementation for IMessageFilter.PreFilterMessage method.
            /// <para/>This sees if mouse or keyboard activity within the application is happening.
            /// </summary>
            /// <param name="m">A reference to a System.Windows.Forms.Message class instance.</param>
            /// <returns>True to filter the message and stop it from being dispatched; 
            /// <para/>false to allow the message to continue to the next filter or control.
            /// <para/>Note: In this case false is returned always.</returns>
            public bool PreFilterMessage(ref Message m)
            {
                if (!enabled)
                {
                    return false;
                }

                if (m.Msg == WM_LBUTTONDOWN ||
                    m.Msg == WM_MBUTTONDOWN ||
                    m.Msg == WM_MOUSEHWHEEL ||
                    m.Msg == WM_RBUTTONDOWN ||
                    m.Msg == WM_XBUTTONDOWN ||
                    m.Msg == WM_MOUSEWHEEL ||
                    m.Msg == WM_KEYDOWN ||
                    m.Msg == WM_MOUSEMOVE) // added 03.06.2018..
                {
                    DestructTimer();
                    if (UserWake != null)
                    {
                        UserWakeEventArgs e = new UserWakeEventArgs();
                        e.UserSlept = dt2 - dt1;
                        dt1 = new DateTime(0);
                        dt2 = new DateTime(0);
                        UserWake(this, e);
                    }
                    ConstructTimer();
                }
                return false;
            }

            /// <summary>
            /// The class destructor. 
            /// <para/>The application's process id is removed from the internal list of process id's and
            /// <para/>the System.Threading.Timer class instance is disposed.
            /// </summary>
            ~HumanActivity()
            {
                DestructTimer();
                int index = apps.IndexOf(Process.GetCurrentProcess().Id);
                if (index != -1)
                {
                    apps.RemoveAt(index);
                }
            }
        }
    }
}