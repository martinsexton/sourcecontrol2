﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Helpers;
using doneillspa.Models;

namespace doneillspa.Factories
{
    public static class TimesheetFactory
    {
        public static Timesheet CreateTimesheet(string username, DateTime startOfWeek, string role, Guid owner)
        {
            Timesheet timesheet = new Timesheet();
            timesheet.DateCreated = DateTime.UtcNow;
            timesheet.Username = username;
            timesheet.WeekStarting = startOfWeek;
            timesheet.Role = role;
            timesheet.Owner = owner;
            timesheet.TimesheetEntries = new List<TimesheetEntry>();
            timesheet.Status = TimesheetStatus.New;

            return timesheet;
        }

        public static TimesheetEntry CreateFullDayEntryForDay(string code, DayOfWeek day)
        {
            TimesheetEntry tse = new TimesheetEntry();
            tse.StartTime = Constants.Strings.Timesheets.StartTime;
            tse.EndTime = Constants.Strings.Timesheets.EndTime;
            tse.Code = code;
            tse.DateCreated = DateTime.UtcNow;
            tse.Day = TranslateDayOfWeek(day.ToString());
            tse.Chargeable = tse.IsEntryChargeable();

            return tse;
        }

        private static string TranslateDayOfWeek(string dayOfWeek)
        {
            if (dayOfWeek.Equals("Monday"))
            {
                return "Mon";
            }
            else if (dayOfWeek.Equals("Tuesday"))
            {
                return "Tue";
            }
            else if (dayOfWeek.Equals("Wednesday"))
            {
                return "Wed";
            }
            else if (dayOfWeek.Equals("Thursday"))
            {
                return "Thurs";
            }
            else if (dayOfWeek.Equals("Friday"))
            {
                return "Fri";
            }
            else
            {
                return "Sat";
            }
        }
    }
}
