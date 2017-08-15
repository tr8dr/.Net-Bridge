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
using bridge.common.time;
using System.Collections.Generic;
using System.Threading;

namespace bridge.common.parsing.dates
{
	/// <summary>
	/// Parses Dates / Times
	/// <p>
	/// The date formats are as follows:
	/// <ul>
	/// 	<li>MMMYY			- ie MAR07</li>
	/// 	<li>MMMYYYY			- ie MAR2007</li>
	/// 	<li>dd-MMM-YY		- ie 23-Dec-07</li>
	/// 	<li>dd-MMM-YYYY		- ie 23-Dec-2007</li>
	/// 	<li>YYYY-MM-dd		- ie 2007-11-03</li>
	/// 	<li>YYYYMMDD		- ie 20071103</li>
	/// 	<li>MM/dd/YY		- ie 12/01/07</li>
	/// 	<li>MM/dd/YYYY		- ie 12/01/2007</li>
	/// 	<li>dd/MM/YY		- ie 01/12/07  (british)</li>
	/// 	<li>dd/MM/YYYY		- ie 01/12/2007 (british)</li>
	/// </ul>
	/// The dates can be followed by a space or 'T' separator and can have the following formats:
	/// <ul>
	/// 	<li>HH:mm:ss		- ie 23:10:00</li>
	/// 	<li>HH:mm:ss:SSS	- ie 23:10:00:100</li>
	/// 	<li>HH:mm:ss.SSS	- ie 23:10:00:100</li>
	/// </ul>
	/// Each of these formats can have a Z (for GMT) or timezone (EST) at the end of the 
	/// time string.
	/// </summary>
	public class DateParser
	{
		public enum Convention
			{ American, British }

		public DateParser (Convention convention = Convention.American)
		{
			_convention = convention;
			_lexer = new DateLexer ();
		}

			   
		static DateParser()
	    {
			_months = new Dictionary<string,int> (StringComparer.OrdinalIgnoreCase);
	    	_months["JAN"] = 1;
	    	_months["FEB"] = 2;
	    	_months["MAR"] = 3;
	    	_months["APR"] = 4;
	    	_months["MAY"] = 5;
	    	_months["JUN"] = 6;
	    	_months["JUL"] = 7;
	    	_months["AUG"] = 8;
	    	_months["SEP"] = 9;
	    	_months["OCT"] = 10;
	    	_months["NOV"] = 11;
	    	_months["DEC"] = 12;
	    	
	    	_months["SUN"] = -1;
	    	_months["MON"] = -2;
	    	_months["TUE"] = -3;
	    	_months["WED"] = -4;
	    	_months["THU"] = -5;
	    	_months["FRI"] = -6;
	    	_months["SAT"] = -7;
			
			_parser = new ThreadLocal<DateParser>(() => new DateParser());
	    }
			
		
		// Properties
		
		
		public static DateParser DefaultParser
			{ get { return _parser.Value; } }
				

		
		// Functions
		

		/// <summary>
		/// Parse date/time string
		/// </summary>
		/// <param name='sdate'>
		/// date.
		/// </param>
		/// <param name='default_zone'>
		/// default time zone
		/// </param>
		public ZDateTime Parse (string sdate, ZTimeZone default_zone)
		{
			List<DateToken> tokens = _lexer.Parse (sdate);
			
			return ParseDateTime (sdate, tokens, default_zone);
		}
		
		
		// Classes
		
		
		/**
		 * Date information
		 */
		private struct DateInfo 
		{
			public ZTimeZone		Zone;
			public int				DayOfMonth;
			public int				Month;
			public int 				Year;
			public int				Hours;
			public int				Minutes;
			public int				Seconds;
			public int				Milliseconds;
		}
		
		
		// Implementation
		
		
		private ZDateTime ParseDateTime (string sdate, List<DateToken> tokens, ZTimeZone default_zone)
		{
			var len = tokens.Count;
			if (len < 1)
				throw new ArgumentException ("failed to parse date, as unknown form: " + sdate);
	
			// special-case, may be a timestamp
			var first = tokens[0];
			if (len == 1 && first.Type == DateToken.TType.NUMERIC)
			{
				long stamp = (long)first;

				// determine whether YYYYMMDD date or whether timestamp
				if (stamp > 19000000 && stamp < 90000000)
					return new ZDateTime ((int)(stamp / 10000), (int)(stamp / 100) % 100, (int)stamp % 100, 0, 0, 0, 0, default_zone); 
				else
					return new ZDateTime(stamp, default_zone);
			}
			
			// otherwise straight date-time
			else
			{
				DateInfo idate = new DateInfo();
				int Itime = ParseDate (sdate, ref idate, tokens);
				ParseTime (sdate, ref idate, tokens, Itime);
				
				if (idate.Zone == null)
					idate.Zone = default_zone;
				
				return CreateDateTime (ref idate);
			}		
		}
		
		
		private ZDateTime ParseJustTime (string sdate, List<DateToken> tokens, ZTimeZone zone)
		{
			DateInfo idate = new DateInfo();
			ParseTime (sdate, ref idate, tokens, 0);
			
			return CreateTime (ref idate, zone);
		}
	
		
		
		/**
		 * Parse date part and return index to next token (for time)
		 * <ul>
		 * 	<li>MMMYY			- ie MAR07</li>
		 * 	<li>MMMYYYY			- ie MAR2007</li>
		 * 	<li>dd-MMM-YY		- ie 23-Dec-07</li>
		 * 	<li>dd-MMM-YYYY		- ie 23-Dec-2007</li>
		 * 	<li>YYYY-MM-dd		- ie 2007-11-03</li>
		 * 	<li>MM/dd/YY		- ie 12/01/07</li>
		 * 	<li>MM/dd/YYYY		- ie 12/01/2007</li>
		 * 	<li>dd/MM/YY		- ie 01/12/07  (british)</li>
		 * 	<li>dd/MM/YYYY		- ie 01/12/2007 (british)</li>
		 * </ul>
		 */
		private int ParseDate (string sdate, ref DateInfo info, List<DateToken> tokens)
		{
			switch (tokens[0].Type)
			{
				case DateToken.TType.ALPHA:
					int mod = ToMonthOrDay ((string)tokens[0]);
					if (mod > 0)
						return ParseDateMMMYY (sdate, ref info, tokens);
					else
						return ParseDateHTTP (sdate, ref info, tokens);
						
				case DateToken.TType.NUMERIC:
					return ParseDateNN (sdate, ref info, tokens);
				
				default:
					throw new ArgumentException ("failed to parse date, invalid date format: " + sdate);
			}
		}
		
		
		/**
		 * Parse date part and return index to next token (for time)
		 * <ul>
		 * 	<li>MMMYY			- ie MAR07</li>
		 * 	<li>MMMYYYY			- ie MAR2007</li>
		 * </ul>
		 */
		private int ParseDateMMMYY (string sdate, ref DateInfo info, List<DateToken> tokens)
		{
			info.Month = ToMonthOrDay ((string)tokens[0]);
			
			switch (tokens[1].Type)
			{
				case DateToken.TType.NUMERIC:
					info.Year = ToYear ((int)tokens[1]);
					return 2;
	
				case DateToken.TType.WHITESPACE:
				case DateToken.TType.DASH:
					return ParseDateMMMDDYYYY (sdate, ref info, tokens);
					
				default:
					throw new ArgumentException ("failed to parse date, invalid date format: " + sdate);
			}
			
		}
		
		
		/**
		 * Parse date part and return index to next token (for time)
		 * <ul>
		 * 	<li>MMM DD, YYYY	- Jan 31, 2011</li>
		 * </ul>
		 */
		private int ParseDateMMMDDYYYY (string sdate, ref DateInfo info, List<DateToken> tokens)
		{
			info.DayOfMonth = ToNumeric(sdate, tokens[2]);
			
			// locate year
			for (int i = 3 ; i < tokens.Count ; i++)
			{
				DateToken token = tokens[i];
				switch (token.Type)
				{
					case DateToken.TType.NUMERIC:
						info.Year = ToYear ((int)token);
						return i+1;
						
					case DateToken.TType.COMMA:
					case DateToken.TType.DASH:
					case DateToken.TType.WHITESPACE:
					case DateToken.TType.SLASH:
						break;
						
					default:
						throw new ArgumentException ("failed to parse date, invalid date format: " + sdate);
				}		
			}
			
			throw new ArgumentException ("failed to parse date, invalid date format: " + sdate);
		}
	
		
		
		/**
		 * Parse date part and return index to next token (for time)
		 * <ul>
		 * 	<li>DAY, dd MMM YYYY	- ie Fri, 25 Jul 2008 10:38:41 GMT</li>
		 * </ul>
		 */
		private int ParseDateHTTP (string sdate, ref DateInfo info, List<DateToken> tokens)
		{
			int Inow = 0;
			DateToken T2 = tokens[Inow = SkipSeparator (tokens, Inow+1)];
			DateToken T3 = tokens[Inow = SkipSeparator (tokens, Inow+1)];
			DateToken T4 = tokens[Inow = SkipSeparator (tokens, Inow+1)];
			
			// dd-MMM-yy case
			if (T3.Type == DateToken.TType.ALPHA)
			{
				info.Month = ToMonthOrDay ((string)T3);
				info.DayOfMonth = Bound (sdate, ToNumeric (sdate, T2), 1, 31);
				info.Year = ToYear (sdate, T4);
				return Inow+1;
			}
			else
			{
				info.Month = Bound (sdate, ToNumeric (sdate, T3), 1, 12);
				info.DayOfMonth = Bound (sdate, ToNumeric (sdate, T2), 1, 31);
				info.Year = ToYear (sdate, T4);
				return Inow+1;			
			}
		}
	
		
		
		/**
		 * Parse date part and return index to next token (for time)
		 * <ul>
		 * 	<li>YYYYMMDD		- ie 20080401</li>
		 * 	<li>dd-MMM-YY		- ie 23-Dec-07</li>
		 * 	<li>dd-MMM-YYYY		- ie 23-Dec-2007</li>
		 * 	<li>YYYY-MM-dd		- ie 2007-11-03</li>
		 * 	<li>MM/dd/YY		- ie 12/01/07</li>
		 * 	<li>MM/dd/YYYY		- ie 12/01/2007</li>
		 * 	<li>dd/MM/YY		- ie 01/12/07  (british)</li>
		 * 	<li>dd/MM/YYYY		- ie 01/12/2007 (british)</li>
		 * </ul>
		 */
		private int ParseDateNN (string sdate, ref DateInfo info, List<DateToken> tokens)
		{
			var len = tokens.Count;
	
			int Inow = 0;
			DateToken T1 = tokens[0];
			
			// YYYYMMDD case
			if (len == 1)
			{
				int yyyymmdd = (int)T1;
				info.Year = Bound (sdate, yyyymmdd / 10000, 1950, 9999);
				info.Month = Bound (sdate, (yyyymmdd / 100) % 100, 1, 12);
				info.DayOfMonth = Bound (sdate, yyyymmdd % 100, 1, 31);
				return 1;
			}
			
			DateToken T2 = tokens[Inow = SkipSeparator (tokens, Inow+1)];
			DateToken T3 = tokens[Inow = SkipSeparator (tokens, Inow+1)];
			
			// dd-MMM-yy case
			if (T2.Type == DateToken.TType.ALPHA)
			{
				info.Month = ToMonthOrDay ((string)T2);
				info.DayOfMonth = Bound (sdate, ToNumeric (sdate, T1), 1, 31);
				info.Year = ToYear (sdate, T3);
				return Inow+1;
			}
			
			//  yyyy-mm-dd case
			else if ((int)T1 > 1000)
			{
				info.Year = ToYear (sdate, T1);
				info.Month = Bound (sdate, ToNumeric (sdate, T2), 1, 12);
				info.DayOfMonth = Bound (sdate, ToNumeric (sdate, T3), 1, 31);			
				return Inow+1;
			}
			// mm/dd/yy case
			else if (_convention == Convention.American)
			{
				info.DayOfMonth = Bound (sdate, ToNumeric (sdate, T2), 1, 31);
				info.Month = Bound (sdate, ToNumeric (sdate, T1), 1, 12);
				info.Year = ToYear (sdate, T3);
				return Inow+1;
			}
			// dd/mm/yy case
			else
			{
				info.DayOfMonth = Bound (sdate, ToNumeric (sdate, T1), 1, 31);
				info.Month = Bound (sdate, ToNumeric (sdate, T2), 1, 12);
				info.Year = ToYear (sdate, T3);
				return Inow+1;
			}
		}
	
		
		/**
		 * Parse date part and return index to next token (for time)
		 * <ul>
		 * 	<li>HH:mm:ss		- ie 23:10:00</li>
		 * 	<li>HH:mm:ss:SSS	- ie 23:10:00:100</li>
		 * 	<li>HH:mm:ss.SSS	- ie 23:10:00:100</li>
		 * </ul>
		 * Each of these formats can have a Z (for GMT) or timezone (EST) at the end of the 
		 * time string.
		 */
		private void ParseTime (string sdate, ref DateInfo info, List<DateToken> tokens, int Istart)
		{
			var len = tokens.Count;
			
			if (Istart == len)
				return;
			
			DateToken separator = tokens[Istart];
			DateToken last = tokens[len-1];
			DateToken plast = tokens[len-2];
	
			if (separator.Type != DateToken.TType.WHITESPACE && separator.Type != DateToken.TType.T)
				throw new ArgumentException ("failed to parse date, as unknown time form: " + sdate);
	
			
			int cap = 0;
			
			if (last.Type == DateToken.TType.NUMERIC)
				cap = len;
			else if (plast.Type == DateToken.TType.NUMERIC)
				cap = len - 1;
			else
				cap = len - 2;
			
			int Inow = Istart + 1;
			
			// get hours
			if (Inow < cap)
				info.Hours = Bound (sdate, ToNumeric (sdate, tokens[Inow]), 0, 23);
			
			// get minutes
			Inow = SkipSeparator (tokens, Inow+1);
			if (Inow < cap)
				info.Minutes = Bound (sdate, ToNumeric (sdate, tokens[Inow]), 0, 59);
						
			// get seconds
			Inow = SkipSeparator (tokens, Inow+1);
			if (Inow < cap)
				info.Seconds = Bound (sdate, ToNumeric (sdate, tokens[Inow]), 0, 59);
			
			// get milli seconds
			Inow = SkipSeparator (tokens, Inow+1);
			if (Inow < cap)
			{
				var fraction = Bound (sdate, ToNumeric (sdate, tokens [Inow]), 0, 999999999);
				if (fraction <= 999)
					info.Milliseconds = fraction;
				else if (fraction <= 999999)
					info.Milliseconds = fraction / 1000;
				else
					info.Milliseconds = fraction / 1000000;
			}
			// get timezone
			info.Zone = GetTimezone (last);
		}
	
		
		
		// Implementation: Aux Functions
	
		
		/**
		 * Convert string month to numeric
		 */
		private int ToMonthOrDay (string smonth)
		{
			int month = 0;
			if (_months.TryGetValue(smonth, out month))
				return month;
			else
				throw new ArgumentException ("failed to parse date, invalid month or day: " + smonth);
		}
	
		
		/**
		 * convert numeric token
		 */
		private int ToNumeric (string sdate, DateToken tok)
		{
			if (tok.Type != DateToken.TType.NUMERIC)
				throw new ArgumentException ("failed to parse date, as unknown form: " + sdate);
			else
				return (int)tok;
		}
	
		
		/**
		 * Normalize year
		 */
		private int ToYear (string sdate, DateToken tok)
		{
			if (tok.Type != DateToken.TType.NUMERIC)
				throw new ArgumentException ("failed to parse date, as unknown form: " + sdate);
			else
				return ToYear ((int)tok);
		}
	
		
		/**
		 * Normalize year
		 */
		private int ToYear (int yy)
		{
			if (yy > 1000)
				return yy;
			if (yy > 50)
				return 1900 + yy;
			else
				return 2000 + yy;
		}
		
		
		/**
		 * skip separators in token stream
		 */
		private int SkipSeparator (List<DateToken> tokens, int pos)
		{
			var len = tokens.Count;
			for (int i = pos ; i < len ; i++)
			{
				switch (tokens[i].Type)
				{
					case DateToken.TType.T:
					case DateToken.TType.WHITESPACE:
					case DateToken.TType.COLON:
					case DateToken.TType.COMMA:
					case DateToken.TType.DOT:
					case DateToken.TType.SLASH:
					case DateToken.TType.DASH:
						continue;
					
					default:
						return i;
				}
			}
			
			return len;
		}
	
		
		/**
		 * Do bounds checking
		 */
		private int Bound (string sdate, int value, int min, int max)
		{
			if (value < min || value > max)
				throw new ArgumentException ("invalid date, field out of bounds: " + sdate + ", field: " + value);
			
			return value;
		}
						
		
		/**
		 * Determine timezone for this date
		 */
		private ZTimeZone GetTimezone (DateToken tok)
		{
			switch (tok.Type)
			{
				case DateToken.TType.Z:
					return ZTimeZone.GMT;
					
				case DateToken.TType.ALPHA:
					return new ZTimeZone ((string)tok);
					
				default:
					return default(ZTimeZone);
			}
		}
		
		
		/**
		 * Create date/time from given info
		 */
		private ZDateTime CreateDateTime (ref DateInfo info)
		{
			return new ZDateTime (info.Year, info.Month, Math.Max(info.DayOfMonth, 1), info.Hours, info.Minutes, info.Seconds, info.Milliseconds, info.Zone);
		}
			
	
		/**
		 * Create time from given info, relative to current date
		 */
		private ZDateTime CreateTime (ref DateInfo info, ZTimeZone zone)
		{
			ZDateTime now = ZDateTime.TimeNowFor (zone);
			return new ZDateTime (now.Year, now.Month, Math.Max(now.Day, 1), info.Hours, info.Minutes, info.Seconds, info.Milliseconds, info.Zone);
		}
		
		
		// Static initialization
		
		static IDictionary<string,int> _months;
		
		// Variables

		private Convention					_convention;
		private DateLexer					_lexer;
		static ThreadLocal<DateParser>		_parser;
	}


	/// <summary>
	/// British date parser.
	/// </summary>
	public class BritishDateParser : DateParser
	{
		public static DateParser INSTANCE = new BritishDateParser();

		public BritishDateParser ()
			: base (Convention.British)
		{
		}
	}


	/// <summary>
	/// American date parser.
	/// </summary>
	public class AmericanDateParser : DateParser
	{
		public static DateParser INSTANCE = new AmericanDateParser();

		public AmericanDateParser ()
			: base (Convention.American)
		{
		}
	}
}

