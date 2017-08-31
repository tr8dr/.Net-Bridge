// 
// General:
//      This file is part of .NET Bridge
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
using System.IO;
using System.Reflection;

using bridge.common.io;


namespace bridge.common.utils
{	
	/// <summary>
	/// Load resource (indicated by path) from local filesystem
	/// </summary>
	public class ResourceLoader
	{
		static ResourceLoader()
		{
			// add one level down from execution directory as a basedir
			Uri url = new Uri (Assembly.GetAssembly(typeof(ResourceLoader)).CodeBase);
			string dllpath = Path.GetDirectoryName(url.AbsolutePath);
			
			string last = StringUtils.RField(dllpath, 0, Path.DirectorySeparatorChar,1);
			if (last == "Debug" || last == "Release")
				Paths.Add (StringUtils.RTrimField (dllpath, 2, Path.DirectorySeparatorChar));
			else
				Paths.Add (StringUtils.RTrimField (dllpath, 1, Path.DirectorySeparatorChar));
			
			// add elements in path
            var pathvar = (Environment.OSVersion.Platform == PlatformID.Win32NT) ? "Path" : "PATH";
            var PATH = Environment.GetEnvironmentVariable(pathvar);
			var elems = StringUtils.Split (PATH, Path.PathSeparator, true);
			
			foreach (string path in elems)
				Paths.Add (path);

			// add elements in library path
			var libpath = Environment.GetEnvironmentVariable("LD_LIBRARY_PATH");
			if (libpath != null)
			{
				var lelems = StringUtils.Split (libpath, Path.PathSeparator, true);
			
				foreach (string path in lelems)
					Paths.Add (path);
			}
			
			var installdir = FindInstallDir (Path.GetDirectoryName (typeof(ResourceLoader).Assembly.Location));
			if (installdir != null)
			{	
				// add install dir path
				Paths.Add (installdir);

				// add development paths
				var srcdir = installdir + Path.DirectorySeparatorChar + "src";
				if (Directory.Exists (srcdir))
				{	
					foreach (var path in FileUtils.FindDir (srcdir, "etc", 3))
						Paths.Add (FileUtils.AncestorOf(path, 1));
				}
			}
			
			Paths.Add (".");
		}

		
		// Operations
		
		
		/// <summary>
		/// Load the file at the specified relative path (can also be absolute path), returning as buffer
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		/// <param name='cache'>
		/// Cache.
		/// </param>
		public static Blob Load (string path, bool cache = false)
		{
			Blob resource = null;
			if (Resources.TryGetValue (path, out resource))
				return resource;
			
			// if is an absolute path, load
			if (Path.IsPathRooted (path))
				resource = LoadFromFilesystem (path);
			else
				resource = LoadAndFindInFilesystem (path);
			
			if (cache && resource != null)
				Resources[path] = resource;
			
			return resource;
		}
		
		
		/// <summary>
		/// find resource file in path
		/// </summary>
		/// <param name='path'>
		/// relative path to file
		/// </param>
		public static string Find (string path)
		{
			path = FileUtils.NormalizeDir (path);
			try
			{
				FileInfo ifile = new FileInfo (path);
				if (ifile.Exists)
					return path;
			}
			catch (Exception)
				{ }

			foreach (string rootdir in Paths)
			{
				try
				{
					FileInfo file = new FileInfo (rootdir + Path.DirectorySeparatorChar + path);
					if (!file.Exists)
						continue;
				
					return file.FullName;
				}
				catch (Exception)
					{ }
			}
			
			return null;
		}
		
		
		
		/// <summary>
		/// Finds directory within paths
		/// </summary>
		/// <param name='path'>
		/// relative sub-path
		/// </param>
		public static string FindDir (string path)
		{
            path = path.Replace('/', Path.DirectorySeparatorChar);
			foreach (string rootdir in Paths)
			{
                DirectoryInfo dir = new DirectoryInfo(rootdir + Path.DirectorySeparatorChar + path);
                if (dir.Exists)
				    return dir.FullName;
			}
			
			string cparent = Path.GetDirectoryName(Assembly.GetAssembly (typeof(ResourceLoader)).Location);
			
			DirectoryInfo pdir = null;
			DirectoryInfo cdir = new DirectoryInfo (cparent);
			
			while ((cdir = cdir.Parent) != pdir)
			{
				pdir = cdir;

                DirectoryInfo dir = new DirectoryInfo(cdir.FullName + Path.DirectorySeparatorChar + path);
                if (dir.Exists)
					return dir.FullName;
			}
			
			return null;
		}
	
		
		
		/// <summary>
		/// find resource parent directory containing file in classpath
		/// </summary>
		/// <returns>
		/// The dir containing given resource file
		/// </returns>
		/// <param name='path'>
		/// Path.
		/// </param>
		public static string FindDirWith (string path)
		{
			string full = Find (path);
			if (full != null)
				return Path.GetDirectoryName(full);
			else
				return null;
		}
		
		
		/// <summary>
		/// Add path to list that loader will scan through
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		public static void AddPath (string path)
		{
			Paths.Add (path);
		}
		
		
		
		// Implementation
		
		
		/**
		 * Cycle through classpath entries to locate a given file
		 * 
		 * @param path		path
		 */
		private static Blob LoadAndFindInFilesystem (string path)
		{
			foreach (string rootdir in Paths)
			{
				_log.Debug2 ("searching path: " + rootdir + ", for: " + path);
				string file = rootdir + Path.DirectorySeparatorChar + path;
				FileInfo info = new FileInfo (file);
				
				if (!info.Exists)
					continue;
				
				return LoadFromFilesystem (info.FullName);
			}
			
			return null;
		}
		
		
		/**
		 * Load file from path
		 * 
		 * @param path		path
		 */
		private static Blob LoadFromFilesystem (string path)
		{
			return FileUtils.ReadFile (path);
		}
		
		
		/// <summary>
		/// Finds the installation dir.
		/// </summary>
		private static string FindInstallDir (string basedir)
		{
			var installdir = Environment.GetEnvironmentVariable("GFLIBDIR");
			if (installdir != null)
				return installdir;
			
			DirectoryInfo pdir = null;
			DirectoryInfo cdir = new DirectoryInfo(basedir);
			while (pdir != cdir && cdir != null)
			{
				var cpath = cdir.FullName;
				if (IsRoot (cpath))
					return null;
				
				var finddir = cpath + Path.DirectorySeparatorChar + ".installdir";
				var info = new FileInfo (finddir);
				if (info.Exists)
					return cdir.FullName;
				
				pdir = cdir;
				cdir = cdir.Parent;
			}
			
			return null;
		}


		/// <summary>
		/// Determines if the path root is reached
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		private static bool IsRoot (string path)
		{
			if (!SystemUtils.IsWindows)
				return path == "/";

			if (path[1] == ':' && path[2] == '\\')
				return true;
			else
				return false;
		}

		
		// Variables

		static IList<string>				Paths = new List<string>();
		static IDictionary<string,Blob>		Resources = new Dictionary<string,Blob>();

		static Logger						_log = Logger.Get ("CONFIG");
	}
}

