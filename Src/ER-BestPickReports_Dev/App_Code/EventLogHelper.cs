using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Configuration;

namespace ER_BestPickReports_Dev.App_Code
{
    public class EventLogHelper
    {
        public static int eventID = 234;              //DO NOT CHANGE

        //##############################################################
        //              Custom Application Settings
        //
        //  Change these per the application you are using.
        //##############################################################
        public static string sSource = "BestPickReports_Site";    //use this to enter the name that will be used for the application source
        public static string applicationName = "Best Pick Reports Website"; //use this for a format friendly version of the application name in the log entries

        public static void WriteInformation(string message)
        {
            Write("Information for application " + applicationName + ":    " + message, EventLogEntryType.Information);
        }

        public static void WriteError(string message)
        {
            Write("ERROR in application " + applicationName + ":    " + message, EventLogEntryType.Error);
        }

        private static void Write(string message, EventLogEntryType type)
        {
            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, "Application");

            EventLog.WriteEntry(sSource, message, type, eventID);
        }
    }
}