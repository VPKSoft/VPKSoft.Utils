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

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
                              /// <summary>
                              /// A name space for the VPKSoft.TimeTaker.TimeTaker class.
                              /// </summary>
namespace VPKSoft.TimeTaker
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
{
    /// <summary>
    /// A simple library to help measure times for a specified code block execution.
    /// </summary>
    public static class TimeTaker
    {
        // a list of saved named time counters..
        private static List<Tuple<DateTime, double, string>> timeList = new List<Tuple<DateTime, double, string>>();

        /// <summary>
        /// Gets or sets a value indicating whether the timing is disabled. This to avoid any excess stress for the program if this class is user for debugging purposes.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the timing is disabled; otherwise, <c>false</c>.
        /// </value>
        public static bool TimingDisabled { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the class will raise an exception if a timer operation is requested while the <see cref="TimingDisabled"/> is set to true.
        /// </summary>
        /// <value>
        ///   <c>true</c> if exceptions will be raised on a timer operation call while the <see cref="TimingDisabled"/> is set to true; otherwise, <c>false</c>.
        /// </value>
        public static bool RaiseExceptionsOnTimingDisabled { get; set; } = false;

        /// <summary>
        /// Starts the time counter with an optional name with the value of DateTime.Now. 
        /// If a timer with the given name doesn't exist, a new instance will be created.
        /// </summary>
        /// <param name="name">The name of the timer of which to start.</param>
        public static void StartTimeCounter(string name = "")
        {
            if (AddTime(name)) // ensure, that a timer with a given name exists..
            {
                int idx = timeList.FindIndex(f => f.Item3 == name); // find an index for the timer..

                // save the DateTime.Now, time span and the name of the timer..
                timeList[idx] = new Tuple<DateTime, double, string>(DateTime.Now, timeList[idx].Item2, name);
            }
        }

        /// <summary>
        /// Stops the time counter with an optional name.
        /// If a timer with the given name doesn't exist, a new instance will be created.
        /// </summary>
        /// <param name="name">The name of the timer of which to stop.</param>
        public static void StopTimeCounter(string name = "")
        {
            if (AddTime(name)) // ensure, that a timer with a given name exists..
            {
                int idx = timeList.FindIndex(f => f.Item3 == name); // find an index for the timer..

                // save the DateTime.Now, time span and the name of the timer..
                timeList[idx] = new Tuple<DateTime, double, string>(DateTime.Now,
                    timeList[idx].Item2 + (DateTime.Now - timeList[idx].Item1).TotalSeconds, // the spend time to total seconds..
                    name);
            }
        }

        /// <summary>
        /// Resets the time counter with an optional name to a zero time span.
        /// If a timer with the given name doesn't exist, a new instance will be created.
        /// </summary>
        /// <param name="name">The name of the timer of which to reset.</param>
        /// <returns></returns>
        public static double ResetTimeCounter(string name = "")
        {
            if (AddTime(name)) // ensure, that a timer with a given name exists..
            {
                int idx = timeList.FindIndex(f => f.Item3 == name); // find an index for the timer..
                double retval = timeList[idx].Item2; // save the previous time span, so it can be returned..

                // save the DateTime.Now, time span of 0 and the name of the timer..
                timeList[idx] = new Tuple<DateTime, double, string>(DateTime.Now, 0, name);
                return retval; // return the previous time span in seconds if any..
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Adds a new timer to the internal list of timers of the class.
        /// <note type="note">If the <see cref="TimingDisabled"/> property is set to true, no timer will be added.</note>
        /// </summary>
        /// <param name="name">The name of the timer to add.</param>
        /// <exception cref="System.Exception">If the <see cref="RaiseExceptionsOnTimingDisabled"/> and the <see cref="TimingDisabled"/> are 
        /// set to true an Exception with message 'Timing disabled' is thrown.</exception>
        /// <returns>True if the <see cref="TimingDisabled"/> is set to false; otherwise true.</returns>
        internal static bool AddTime(string name)
        {
            // check if an exception should be thrown..
            if (TimingDisabled && RaiseExceptionsOnTimingDisabled)
            {
                throw new Exception("Timing disabled.");
            }
            else if (TimingDisabled) // if disabled return false..
            {
                return false;
            }

            // create a timer if it doesn't exist..
            if (!timeList.Exists(f => f.Item3 == name))
            {
                timeList.Add(new Tuple<DateTime, double, string>(DateTime.Now, 0, name));
            }

            // indicate a successful operation..
            return true;
        }

        /// <summary>
        /// Returns the current time span of a timer with an optional name.
        /// <note type="note">Current time is not taken into account.</note>
        /// </summary>
        /// <param name="name">The name of the timer of which time span to get.</param>
        /// <returns>An amount counted by the timer with a given name in seconds.</returns>
        public static double CounterSeconds(string name = "")
        {
            if (AddTime(name)) // ensure, that a timer with a given name exists..
            {
                AddTime(name);
                return timeList.First(f => f.Item3 == name).Item2;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Deletes the timer with an optional name from the internal list of this class.
        /// </summary>
        /// <param name="name">An optional name of a timer to delete.</param>
        public static void DeleteTimer(string name = "")
        {
            int idx = timeList.FindIndex(f => f.Item3 == name); // find an index for the timer..
            if (idx != -1) // if an index was found..
            {
                // remove the timer..
                timeList.RemoveAt(idx);
            }
        }

        /// <summary>
        /// Deletes all timers from the internal list of this class.
        /// </summary>
        public static void Clear()
        {
            timeList.Clear(); // deleting all is much easier..
        }

        /// <summary>
        /// Gets all the names of the timers from the internal list of this class.
        /// </summary>
        /// <returns>A list of timer names which are currently saved to this class.</returns>
        public static List<string> GetTimersByName()
        {
            List<string> retVal = new List<string>(); // create a return value..
            foreach (Tuple<DateTime, double, string> timer in timeList) // loop through all the timers saved to this class..
            {
                retVal.Add(timer.Item3); // add the timer name to the return list..
            }
            return retVal; // return the return value..
        }
    }
}
