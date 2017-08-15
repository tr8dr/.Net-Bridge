// 
// General:
//      This file is pary of .NET Bridge
//
// Copyright:
//      2010 Jonathan Shore
//      2017 Jonathan Shore and Contributors
//
// License:
//      Licensed under the Apache License, Version 2.0 (the "License");
//      you may not use this file except in compliance with the License.
//      You may obtain a copy of the License at:
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//      Unless required by applicable law or agreed to in writing, software
//      distributed under the License is distributed on an "AS IS" BASIS,
//      WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//      See the License for the specific language governing permissions and
//      limitations under the License.
//

using System;
using System.Collections.Generic;
using System.Text;

using bridge.common.time;


namespace bridge.common.utils
{
	/// <summary>
	/// Logging facility
	/// </summary>
	public class Logger
	{	
		/// <summary>
		/// message severity
		/// </summary>
		public enum Severity
			{ None = 0, Severe = 1, Warn = 2, Info = 3, Debug = 5, Debug2 = 6, Debug3 = 7 };

		/// <summary>
		/// Log decorations
		/// </summary>
		[Flags] 
		public enum Show
			{ Severity = 1, Time = 2, Module = 4 }
		


		/// <summary>
		/// Creates a logging facility for some category of the application identified by string
		/// </summary>
		/// <param name='category'>
		/// Category assigned to this logger
		/// </param>
		/// <param name='severity'>
		/// what level of severity passes through for reporting
		/// </param>
		protected Logger (string category, Severity severity)
		{
			_category = category;
			_severity = severity;
		}
		
		
		// Properties
		
		/// <summary>
		/// Gets the logger category
		/// </summary>
		/// <value>
		/// The category.
		/// </value>
		public string Category
			{ get { return _category; } }
		
		
		/// <summary>
		/// Gets or sets the local verbosity level.
		/// </summary>
		/// <value>
		/// The level.
		/// </value>
		public Severity Level
			{ get { return _severity; } set { _severity = value; } }
		
		
		/// <summary>
		/// Gets or sets the master verbosity level.
		/// </summary>
		/// <value>
		/// The master level.
		/// </value>
		public static Severity MasterLevel
		{
			get 
				{ return _masterseverity; }
			set 
			{
				_masterseverity = value;
				foreach (var log in _loggers.Values)
					log.Level = value;
			}
		}
		
		
		/// <summary>
		/// Gets or sets the reporter.
		/// </summary>
		/// <value>
		/// The reporter.
		/// </value>
		public static IReporter Reporter
			{ get { return _sink; } set { _sink = value; } }
		
		
		/// <summary>
		/// Sets the master level from numeric value
		/// </summary>
		/// <param name='level'>
		/// Level.
		/// </param>
		public static void SetMasterLevel (int level)
		{
			MasterLevel = (Severity)level;
		}
		
		/// <summary>
		/// Sets the master level from numeric value
		/// </summary>
		/// <param name='level'>
		/// Level.
		/// </param>
		public void SetLocalAndMasterLevel (Severity level_local, Severity level_global)
		{
			MasterLevel = level_global;
			Level = level_local;
		}

		
		/// <summary>
		/// Sets the master level from numeric value
		/// </summary>
		/// <param name='level'>
		/// Level.
		/// </param>
		public static void SetMasterLevel (Severity level)
		{
			MasterLevel = level;
		}
		
		
		// Predicates
		
		
		public bool IsInfoEnabled
			{ get { return _severity >= Severity.Info; } }

		public bool IsDebugEnabled
			{ get { return _severity >= Severity.Debug; } }
		
		public bool IsDebug2Enabled
			{ get { return _severity >= Severity.Debug2; } }
		
		public bool IsDebug3Enabled
			{ get { return _severity >= Severity.Debug3; } }
		
		
		// Operations
		
		
		/// <summary>
		/// Get or create logger for category
		/// </summary>
		/// <param name='category'>
		/// Category.
		/// </param>
		public static Logger Get (string category)
		{
			// try to get existing logger
			Logger log = null;
			if (_loggers.TryGetValue(category, out log))
				return log;
			
			// otherwise create new one
			log = new Logger (category, _masterseverity);
			_loggers[category] = log;
			return log;
		}

		
		/// <summary>
		/// Report the specified msg if severity passes filter
		/// </summary>
		/// <param name='severity'>
		/// Severity fo msg
		/// </param>
		/// <param name='msg'>
		/// Message.
		/// </param>
		public void Post (Severity severity, string msg)
		{
			if (severity <= _severity)
			{
				long Tnow = Clock.Now;
				_sink.Post (new Report (this, Tnow, severity, msg));
			}
		}
	
		
		/// <summary>
		/// Report the specified msg as fatal. 
		/// </summary>
		/// <param name='msg'>
		/// Message.
		/// </param>
		public void Fatal (string msg, bool exit = false)
		{
			Post (Severity.Severe, msg);
			if (exit)
				Environment.Exit (1);
		}
	
		
		/// <summary>
		/// Report the specified msg as warning
		/// </summary>
		/// <param name='msg'>
		/// Message.
		/// </param>
		public void Warn (string msg)
		{
			Post (Severity.Warn, msg);
		}
	
		
		/// <summary>
		/// Report the specified msg as info
		/// </summary>
		/// <param name='msg'>
		/// Message.
		/// </param>
		public void Info (string msg)
		{
			Post (Severity.Info, msg);
		}
	
		
		/// <summary>
		/// Report the specified msg as debug
		/// </summary>
		/// <param name='msg'>
		/// Message.
		/// </param>
		public void Debug (string msg)
		{
			Post (Severity.Debug, msg);
		}
		
		
		/// <summary>
		/// Report the specified msg as debug
		/// </summary>
		/// <param name='msg'>
		/// Message.
		/// </param>
		public void Debug2 (string msg)
		{
			Post (Severity.Debug2, msg);
		}

		
		/// <summary>
		/// Add and parse logging related arguments.
		/// </summary>
		/// <param name='args'>
		/// Arguments.
		/// </param>
		public static void Parse (ArgumentParser args)
		{
			args.Register ("warn", false, false, "will show warnings and higher level messages");
			args.Register ("info", false, false, "will show information and higher level messages");
			args.Register ("debug", false, false, "will show debug and higher level messages");
			args.Register ("debug2", false, false, "will show debug2 and higher level messages");
			args.Register ("log:level", true, false, "sets the logging level between [0,7]");
			args.Register ("log:showall", false, false, "shows logging with all decorations");

			if (args.Contains("warn"))
				MasterLevel = Severity.Warn;
			if (args.Contains("log:showall"))
				Reporter.Decoration = (int)(Show.Time | Show.Module | Show.Severity);
			if (args.Contains("info"))
				MasterLevel = Severity.Info;
			if (args.Contains("debug"))
				MasterLevel = Severity.Debug;
			if (args.Contains("debug2"))
				MasterLevel = Severity.Debug2;
			if (args.Contains("log:level"))
				MasterLevel = (Severity)((int)args["log:level"]);
		}
		
		
		#region Classes
		
		
		/// <summary>
		/// Reporting implementation
		/// </summary>
		public interface IReporter
		{
			/// <summary>
			/// What to show
			/// </summary>
			/// <value>The show.</value>
			int						Decoration 		{ get; set; }

			/// <summary>
			/// Post the specified msg.
			/// </summary>
			/// <param name="msg">Message.</param>
			void					Post (Report msg);
		}
		
		
		/// <summary>
		/// Report structure
		/// </summary>
		public struct Report
		{
			public Logger			Source;
			public long				Time;
			public Severity			Level;
			public string			Message;
			
			
			public Report (Logger src, long time, Severity severity, string msg)
			{
				Source = src;
				Time = time;
				Level = severity;
				Message = msg;
			}
		}
		
		
		
		// Variables
		
		private string						_category;
		private Severity					_severity;
		
		static IReporter					_sink = new StderrReporter();
		static IDictionary<string,Logger>	_loggers = new Dictionary<string,Logger>();
		static Severity						_masterseverity = Severity.Warn;
	}


	#endregion

	#region Logger Reporters


	/// <summary>
	/// Reporter that writes to stderr
	/// </summary>
	public class StderrReporter : Logger.IReporter
	{
		public StderrReporter (int show = (int)(Logger.Show.Severity | Logger.Show.Time))
		{
			_show = show;
		}


		// Properties

		/// <summary>
		/// What to show
		/// </summary>
		/// <value>The show.</value>
		public int Decoration
			{ get { return _show; } set { _show = value; } }


		// Functions

		
		/// <summary>
		/// Post the msg with formatting to stderr
		/// </summary>
		/// <param name='level'>
		/// Level.
		/// </param>
		/// <param name='msg'>
		/// Message.
		/// </param>
		public void Post (Logger.Report report)
		{
			StringBuilder s = new StringBuilder (report.Message.Length + 64);

			bool skip = false;
			if ((_show & (int)Logger.Show.Severity) != 0)
			{
				s.Append (SeverityOf(report.Level));
				skip = true;
			}
			
			if ((_show & (int)Logger.Show.Time) != 0)
			{
				if (skip) s.Append (' ');

				ZDateTime time = new ZDateTime(report.Time, ZTimeZone.Local);
				s.Append (string.Format(" [{0:D4}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2}.{6:D3}]", 
					time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second, time.Millisecond));
				
				skip = true;
			}
			
			if ((_show & (int)Logger.Show.Module) != 0)
			{
				if (skip) s.Append (' ');

				s.Append ('[');
				s.Append (report.Source.Category);
				s.Append (']');
				
				skip = true;
			}
			
			if (skip)
				s.Append (": ");
			
			s.Append (report.Message);
			
			Console.Error.WriteLine (s);
		}
		
		
		// Implementation

		
		private static string SeverityOf (Logger.Severity level)
		{
			switch (level)
			{
			case Logger.Severity.Severe:
				return "FATAL";
			case Logger.Severity.Warn:
				return "WARN";
			case Logger.Severity.Info:
				return "INFO";
			default:
				return "DEBUG";
			}
		}

		// Variables
		
		private int		_show;
	}

	#endregion
}

