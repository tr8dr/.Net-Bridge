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
using System.Text;
using System.Collections.Generic;
using bridge.common.time;


namespace bridge.common.utils
{	
	/// <summary>
	/// command-line parser (understands unix and dos conventions).  In particular:
	/// <ul>
	///		<li>/argname:value</li>
	///		<li>-argname value</li>
	///		<li>--argname=value</li>
	/// </ul>
	/// If the value is comma "," separated will assume is a list and parse accordingly.  If
	/// multiple instances of a switch occur, will aggregate into a list likewise.
	/// </summary>
	public class ArgumentParser
	{
		/// <summary>
		/// provide a help function
		/// </summary>
		public delegate void HelpDelegate ();


		/// <summary>
		/// create parser
		/// </summary>
		public ArgumentParser (string[] args, HelpDelegate help)
		{
			_pending = args;
			_help = help;

			Register ("help", false, false, "shows command help");
		}


		/// <summary>
		/// create parser
		/// </summary>
		public ArgumentParser (string[] args)
			: this (args, null)
		{
		}


		/// <summary>
		/// indicate arguments to parse up-front
		/// </summary>
		/// <param name="name">name of argument</param>
		/// <param name="arguments">indicate whether argument has a trailing value</param>
		/// <param name="mandatory">optional or mandatory</param>
		public void Register (string name, bool arguments, bool mandatory, string description)
		{
			_templates [name] = new Template (name, arguments, mandatory, description);
		}


		// accessors

		/// <summary>
		/// get argument corresponding to name
		/// </summary>
		public Argument this [string argname]
		{
			get 
			{ 
				Parse();
				Argument val = null;
				if (_switches.TryGetValue (argname, out val))
					return val;
				else
					return new Argument (argname);
			}
			set 
			{ 
				_switches[argname] = value; 
			}
		}
		
		
		/// <summary>
		/// Returns argument value or default if missing
		/// </summary>
		/// <param name='name'>
		/// Name of argument
		/// </param>
		/// <param name='def'>
		/// Default value
		/// </param>
		public int Or (string name, int def)
		{
			if (Contains (name))
				return this[name].Value;
			else
				return def;
		}
				
		/// <summary>
		/// Returns argument value or default if missing
		/// </summary>
		/// <param name='name'>
		/// Name of argument
		/// </param>
		/// <param name='def'>
		/// Default value
		/// </param>
		public double Or (string name, double def)
		{
			if (Contains (name))
				return this[name].Value;
			else
				return def;
		}
				
		/// <summary>
		/// Returns argument value or default if missing
		/// </summary>
		/// <param name='name'>
		/// Name of argument
		/// </param>
		/// <param name='def'>
		/// Default value
		/// </param>
		public string Or (string name, string def)
		{
			if (Contains (name))
				return this[name].Value;
			else
				return def;
		}
				
		/// <summary>
		/// Returns argument value or default if missing
		/// </summary>
		/// <param name='name'>
		/// Name of argument
		/// </param>
		/// <param name='def'>
		/// Default value
		/// </param>
		public bool Or (string name, bool def)
		{
			if (Contains (name))
				return this[name].Value;
			else
				return def;
		}
				
		/// <summary>
		/// Returns argument value or default if missing
		/// </summary>
		/// <param name='name'>
		/// Name of argument
		/// </param>
		/// <param name='def'>
		/// Default value
		/// </param>
		public ZDateTime Or (string name, ZDateTime def)
		{
			if (Contains (name))
				return new ZDateTime (this[name].Value);
			else
				return def;
		}

		/// <summary>
		/// get remaining non-switch arguments
		/// </summary>
		public IList<Any> Remainder
			{ get { Parse(); return _remainder; } }
		

		/// <summary>
		/// contains arg?
		/// </summary>
		public bool Contains (string argname)
			{ Parse(); return _switches.ContainsKey (argname); }


		/// <summary>
		/// parse arguments
		/// </summary>
		public void Parse ()
		{
			if (_pending != null)
			{
				Parse(_pending);
				Check ();
				_pending = null;
			}
		}


		
		/// <summary>
		/// help
		/// </summary>
		public void Help ()
		{ 
			if (_help != null) 
				_help ();
			else
				HelpInfo ();
		}


		// Implementation


		/// <summary>
		/// parse argument stream
		/// </summary>
		private void Parse (string[] args)
		{
			// parse arguments
			for (int i = 0 ; i < args.Length ; i++)
			{
				// get next argument name (possibly null)
				Argument arg = Parse (args, ref i);

				if (arg != null)
					Add (arg);
				else
					_remainder.Add (new Any(args[i]));
			}
		}


		/// <summary>
		/// check arguments to see if they conform
		/// </summary>
		private void Check ()
		{
			foreach (Template entry in _templates.Values)
			{
				// check to see if arg
				var arg = _switches [entry.Name];

				if (arg == null && entry.Mandatory)
					ArgumentError ("could not find mandatory argument: " + entry.Name);
				if (arg == null && !entry.Mandatory)
					continue;
				else if (arg.Type == Argument.ValueType.None && entry.HasValue)
					ArgumentError ("argument: " + entry.Name + " does not have value");
			}

			if (_switches ["help"] != null)
				{ Help(); System.Environment.Exit (1); }
		}
		

		/// <summary>
		/// an argument
		/// </summary>
		private Argument Parse (string[] args, ref int i)
		{
			string first = args[i];
			
			// unix style argument "--argname=value"
			if (first.StartsWith("--")) 
			{
				string name = StringUtils.Field (first.Substring(2), 0, '=');
				if (!_templates.ContainsKey (name))
					ArgumentError ("unknown switch: " + name);
				
				IList<Any> vallist = ParseValue (_templates[name], StringUtils.LTrimField (first.Substring(2), 1, '='));
				return new Argument (name, vallist);
			}
			
			// unix style argument "-argname value"
			else if (first.StartsWith("-"))
			{
				string name = first.Substring(1);
				if (!_templates.ContainsKey (name))
					ArgumentError ("unknown switch: " + name);
				
				IList<Any> vallist = ParseValue (_templates[name], (args.Length > (i+1)) ? args[i+1] : null);
				i += (vallist.Count == 0) ? 0 : 1;
				return new Argument (name, vallist);
			}

			else
				return null;
		}



		/// <summary>
		/// parse value, returning single value or array
		/// </summary>
		private IList<Any> ParseValue (Template arginfo, string val)
		{
			// determine if supposed to have argument
			bool hasvalue = arginfo.HasValue;

			// if not supposed to have value, return
			if (!hasvalue || val == null)
				return new Any[] {};

			// otherwise process value
			string[] vallist = val.Split (new char[] {','});
			
			IList<Any> list = new List<Any>();
			foreach (string single in vallist)
			{
				string nsingle = single.Trim ();
				if (nsingle.Length == 0)
					continue;
				else
					list.Add (new Any (nsingle));
			}

			return list;
		}



		/// <summary>
		/// add argument
		/// </summary>
		private void Add (Argument arg)
		{
			if (_switches[arg.Name] != null)
				((Argument)_switches[arg.Name]).Append (arg.ValueList);
			else
				_switches[arg.Name] = arg;
		}


		/// <summary>
		/// display error
		/// </summary>
		private void ArgumentError (string msg)
		{
			Console.WriteLine ("{0}: {1}\n", Application(), msg);
			Help();
			System.Environment.Exit (1);
		}


		/// <summary>
		/// display error
		/// </summary>
		private string Application ()
		{
			string fullname = StringUtils.Field (System.Environment.CommandLine, 0, ' ');
			string[] parts = fullname.Split (new char[] {'/','\\'});
			return StringUtils.Field (parts	[parts.Length-1], 0, '.');
		}


		/// <summary>
		/// help info
		/// </summary>
		private void HelpInfo ()
		{
			Console.WriteLine ("{0}: has the following parameters:\n", Application());
			int len = 0;
			foreach (var tmpl in _templates.Values)
			{
				len = (int)Math.Max (len, tmpl.Name.Length);
			}
	
			foreach (Template tmpl in _templates.Values)
			{
				// check to see if arg
				if (tmpl.HasValue)
					Console.WriteLine ("   -{0}\t: {1} ({2})", StringField (tmpl.Name + " <value>",len+10), tmpl.Description, tmpl.Mandatory ? "required" : "optional");
				else
					Console.WriteLine ("   -{0}\t: {1} ({2})", StringField (tmpl.Name,len+10), tmpl.Description, tmpl.Mandatory ? "required" : "optional");
			}
		}


		/// <summary>
		/// format string into field length (left aligned)
		/// </summary>
		private string StringField (string s, int len)
		{
			StringBuilder build = new StringBuilder (len);
			build.Append (s);
			for (int i = 0 ; i < (len - s.Length) ; i++) build.Append (' ');
			return build.ToString();
		}
		
		
		// Classes
		
		
		/// <summary>
		/// Parameter template
		/// </summary>
		private struct Template
		{
			public string		Name;
			public bool			HasValue;
			public bool			Mandatory;
			public string		Description;
			
			
			public Template (string name, bool hasval, bool mandatory, string description)
			{
				Name = name;
				HasValue = hasval;
				Mandatory = mandatory;
				Description = description;
			}
		}
		

		// variables

		private string[]						_pending;
		private Dictionary<string,Argument>		_switches	= new Dictionary<string, Argument>();
		private IList<Any>						_remainder	= new List<Any>();
		private Dictionary<string,Template>		_templates	= new Dictionary<string, Template>();
		private HelpDelegate					_help;
	}
	
	
	
	/// <summary>
	/// command-line argument, containing
	/// <ul>
	///		<li>switch</li>
	///		<li>argument type</li>
	///		<li>value or array of values</li>
	/// </ul>
	/// </summary>
	public class Argument
	{
		/// <summary>
		/// type of value
		/// </summary>
		public enum ValueType
			{ None, Single, List };


		internal Argument (string name)
		{
			_name = name;
			_valuelist = new List<Any> ();
		}

		internal Argument (string name, Any svalue)
		{
			_name = name;
			_valuelist = new List<Any> ();
			_valuelist.Add (svalue);
		}

		internal Argument (string name, IList<Any> valuelist)
		{
			_name = name;
			_valuelist = new List<Any> (valuelist);
		}

		internal Argument (string name, object val)
		{
			_name = name;
			_valuelist = new List<Any> ();
			Append (val);
		}



		/// <summary>
		/// type of value
		/// </summary>
		public ValueType Type
			{ get { return _valuelist.Count > 1 ? ValueType.List :  ValueType.Single; } }

		/// <summary>
		/// name
		/// </summary>
		public string Name
			{ get { return _name; } }

		/// <summary>
		/// value (only valid for single values)
		/// </summary>
		public Any Value
			{ get { return (Any)_valuelist[0]; } }

		/// <summary>
		/// value list (only valid for single values)
		/// </summary>
		public IList<Any> ValueList
			{ get { return _valuelist; } }


		/// <summary>
		/// append another value
		/// </summary>
		public void Append (Any v)
			{ _valuelist.Add (v); }

		/// <summary>
		/// append another value list
		/// </summary>
		public void Append (ICollection<Any> list)
		{
			foreach (Any v in list) _valuelist.Add (v);
		}

		/// <summary>
		/// append another value list
		/// </summary>
		public void Append (object v)
		{
			if (v.GetType().IsArray)
				Append ((ICollection<Any>)v);
			else
				Append ((Any)v);
		}


		/// <summary>
		/// Returns argument value or default if missing
		/// </summary>
		/// <param name='def'>
		/// Default value
		/// </param>
		public int Or (int def)
		{
			if (_valuelist.Count > 0)
				return (int)Value;
			else
				return def;
		}

		/// <summary>
		/// Returns argument value or default if missing
		/// </summary>
		/// <param name='def'>
		/// Default value
		/// </param>
		public double Or (double def)
		{
			if (_valuelist.Count > 0)
				return (double)Value;
			else
				return def;
		}

		/// <summary>
		/// Returns argument value or default if missing
		/// </summary>
		/// <param name='def'>
		/// Default value
		/// </param>
		public string Or (string def)
		{
			if (_valuelist.Count > 0)
				return (string)Value;
			else
				return def;
		}

		/// <summary>
		/// Returns argument value or default if missing
		/// </summary>
		/// <param name='def'>
		/// Default value
		/// </param>
		public bool Or (bool def)
		{
			if (_valuelist.Count > 0)
				return (bool)Value;
			else
				return def;
		}

		/// <summary>
		/// Returns argument value or default if missing
		/// </summary>
		/// <param name='def'>
		/// Default value
		/// </param>
		public ZDateTime Or (ZDateTime def)
		{
			if (_valuelist.Count > 0)
				return new ZDateTime ((string)Value);
			else
				return def;
		}

		
		// Comversions
		
		
		public static implicit operator string (Argument arg)
			{ return (string)arg.Value; }
		
		public static implicit operator double (Argument arg)
			{ return (double)arg.Value; }
		
		public static implicit operator bool (Argument arg)
			{ return (bool)arg.Value; }
		
		public static implicit operator int (Argument arg)
			{ return (int)arg.Value; }

		public override string ToString ()
			{ return (string)Value; }

		// variables

		string			_name;
		IList<Any>		_valuelist;
	}
	
	
}
