/*
 * -----------------------------------------------------------------------------
 *	 
 *   Filename		:   LogHelper.cs
 *   Date			:   2020-06-22 13:19:59
 *   All rights reserved
 * 
 * -----------------------------------------------------------------------------
 * @author     Patrick Robin <support@rietrob.de>
 * @Version      1.0.0
 */

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Eventing;
using System.IO;
using System.Runtime.CompilerServices;
using Caliburn.Micro;
using CarRefuelTracker.UI.DataAccess;
using CarRefuelTracker.UI.Enums;
using CarRefuelTracker.UI.Models;
using static CarRefuelTracker.UI.Enums.LogState;
using ILog = log4net.ILog;
using LogManager = log4net.LogManager;

namespace CarRefuelTracker.UI.Helper
{
    public static class LogHelper
    {


        #region Fields
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger
        //      (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly log4net.ILog Log = LogHelper.GetLogger();

        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Methods

        public static ILog GetLogger([CallerFilePath] string filename = "")
        {
            return LogManager.GetLogger(filename);
        }

        public static void  WriteToLog(string logMessage, LogState logState)
        {
            string logFileName =
                Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            logFileName += @"\CarRefuelTracker\Logs\" + DateTime.Now.ToShortDateString() + @"_log";
            log4net.GlobalContext.Properties["LogFile"] = logFileName;
            string s = new Uri(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.
                GetExecutingAssembly().CodeBase), "log4net.config")).LocalPath;
            log4net.Config.XmlConfigurator.Configure();

            switch (logState)
            {
                case LogState.Debug:
                {
                    Log.Debug(logMessage);
                    break;
                }
                case LogState.Info:
                {
                    Log.Info(logMessage);
                    break;
                }
                case LogState.Warn:
                {
                    Log.Warn(logMessage);
                    break;
                }
                case LogState.Error:
                {
                    Log.Error(logMessage);
                    break;
                }
                case LogState.Fatal:
                {
                    Log.Fatal(logMessage);
                    break;
                }
                default:
                {
                    Log.Debug(logMessage);
                    break;
                }
            }
        }

        #endregion
    }
}