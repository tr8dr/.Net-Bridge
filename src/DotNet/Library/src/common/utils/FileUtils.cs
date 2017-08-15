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
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Diagnostics;

using bridge.common.utils;
using System.Threading;
using System.Reflection;
using System.Text.RegularExpressions;


namespace bridge.common.io
{
	/// <summary>
	/// Various file utilities
	/// </summary>
	public class FileUtils
	{
		// Constants

		public static readonly uint		Mode_rwx = Convert.ToUInt32 ("0777", 8);
		public static readonly uint		Mode_rw = Convert.ToUInt32 ("0666", 8);

		public readonly static int		Recursive 	= 1;
	    public readonly static int		Single		= 0;
	    

		/// <summary>
		/// Length of the file
		/// </summary>
		/// <param name="path">Path.</param>
		public static long LengthOf (string path)
		{
			return (new FileInfo(path)).Length;
		}


		/// <summary>
		/// Return directory component of file path.
		/// </summary>
		/// <returns>directory component.</returns>
		/// <param name="path">Path.</param>
	    public static string DirOf (string path)
	    {
			return Path.GetDirectoryName (path);
	    }

			    
		/// <summary>
		/// Return file component of file path
		/// </summary>
		/// <returns>
		/// The of.
		/// </returns>
		/// <param name='path'>
		/// Path.
		/// </param>
		/// <param name="remove_extension">
		/// Indicates whether file extension should be removed
		/// </param>
	    public static string FileOf (string path, bool remove_extension = false)
	    {
			var filename = Path.GetFileName (path);
			if (remove_extension)
				return StringUtils.Field (filename, 0, '.');
			else
				return filename;
	    }
		

		/// <summary>
		/// Find file in given directory path matching regular expression
		/// </summary>
		/// <returns>The in directory.</returns>
		/// <param name="dir">Dir.</param>
		/// <param name="matching">Matching.</param>
		public static List<string> FilesInDirectory (string dir, string matching = null)
		{
			var files = new List<string> ();
			var rex = new Regex (matching ?? ".+");

			foreach (var filename in Directory.GetFiles (dir))
			{
				if (rex.IsMatch (filename))
					files.Add (filename);
			}

			return files;
		}


		/// <summary>
		/// Find files with extension.
		/// </summary>
		/// <returns>The with extension.</returns>
		/// <param name="dir">Dir.</param>
		/// <param name="extension">Extension.</param>
		public static List<string> FilesWithExtension (string dir, string extension)
		{
			var files = new List<string> ();

			foreach (var filename in Directory.GetFiles (dir))
			{
				if (filename.EndsWith(extension))
					files.Add (filename);
			}

			return files;
		}



		/// <summary>
		/// Provide N levels of the tail of the path
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		/// <param name='n'>
		/// N.
		/// </param>
		/// <param name='separator'>
		/// Separator.
		/// </param>
		public static string TailOf (string path, int n, char separator = '/')
		{
			int Istart = path.Length;
			while (n-- > 0 && Istart > 0)
				Istart = path.LastIndexOf (separator, Istart-1);

			if (Istart < 0)
				return path;
			else
				return path.Substring (Istart+1);
		}

	    
	    /**
	     * Creates a temporary filename guaranteed to be unique
	     *
	     * @param prefix	tmp file prefix
	     * @return			unique temporary file name
	     */
	    public static string TmpFile (String prefix)
	    {
	    	String tmpdir = Path.DirectorySeparatorChar + "tmp";
	    	long timestamp = SystemUtils.Ticks;
	    	
	    	return tmpdir + Path.DirectorySeparatorChar + prefix + "-" + timestamp;
	    }
	    
		
		/// <summary>
		/// Gets the home directory
		/// </summary>
		public static string HomeDir
		{
			get 
			{
				var platform = Environment.OSVersion.Platform;
				return (platform == PlatformID.Unix || platform == PlatformID.MacOSX) ? 
					Environment.GetEnvironmentVariable("HOME") : 
    				Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
			}
		}
		
		
		/// <summary>
		/// Determines whether the given directory can be written to (i.e. create a new file)
		/// </summary>
		/// <param name='dir'>
		/// directory path
		/// </param>
		public static bool IsDirectoryWritable (string dir)
		{
			var info = new DirectoryInfo (dir);
			if (!info.Exists)
				return false;
			
			try
			{
				var testfile = dir + Path.DirectorySeparatorChar + ".dummy";
				WriteFile (testfile, "test");
				DeleteFile (testfile);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}


		/// <summary>
		/// Chmod the specified path and mask.
		/// </summary>
		/// <param name="path">Path.</param>
		/// <param name="mask">Mask.</param>
		public static void Chmod (string path, uint mask)
		{
			try
			{
				InitializePosix ();
				if (_Mchmod != null)
					_Mchmod.Invoke (null, new object[] { path, mask });
			}
			catch (Exception e)
			{
				_log.Warn ("failed to chmod file: " + path);
			}
		}
	
		
		/// <summary>
		/// Determines whether the file is likely on a local filesystem (tests latency
		/// </summary>
		/// <param name='filename'>
		/// ilenane
		/// </param>
		public static bool IsLocalFile (string filename)
		{
			FileInfo info = new FileInfo (filename);
			var dir = info.Directory.FullName;
			
			lock (_local)
			{
				var local = false;
				if (_local.TryGetValue(dir, out local))
					return local;
			}
			
			var timing = TimeFileAccess (filename);
			
			lock (_local)
			{
				if (timing.Milliseconds > 50)
					return _local[dir] = false;
				else
					return _local[dir] = true;
			}
		}
		
	
	    /**
	     * Convert ~ to home dir
	     *
	     * @param dir		directory with home alias (possibly)
	     * @return			expanded director path
	     */
	    public static string NormalizeDir (String dir)
	    {
	    	if (dir.StartsWith("~/"))
	    		return HomeDir + dir.Substring(1);
	    	else
	    		return dir;
	    }

		
        /// <summary>
        /// Get the nth ancestor of the given directory
        /// </summary>
        /// <param name="adir"></param>
        /// <param name="up"></param>
        /// <returns></returns>
        public static string DirAncestorOf(DirectoryInfo adir, int up)
        {
            for (int i = 0; i < up; i++)
                adir = adir.Parent;

            return adir.FullName;
        }

		
	    /// <summary>
	    /// Copy file
	    /// </summary>
	    /// <param name='file'>
	    /// File.
	    /// </param>
	    /// <param name='basefile'>
	    /// Basefile.
	    /// </param>
	    public static void Copy (string srcfile, string dstfile)
	    {
	    	Stream fin = CreateInputStream (srcfile);
	    	Stream fout = CreateOutputStream (dstfile);
	    	
    		IOUtils.Copy (fin, fout);
    		fin.Close();
    		fout.Close ();
	    }


		/// <summary>
		/// Determine # of lines in file (that contain non-whitespace)
		/// </summary>
		/// <returns>The # of lines in file.</returns>
		/// <param name="file">Filename.</param>
		/// <param name="nonwhitespace">If true, only counts lines that contain text.</param>
		public static int LinesInFile (string file, bool nonwhitespace = true)
		{
			var fin = CreateInputStream (file);
			var buffer = new byte [8192];

			int nrows = 0;
			bool solidline = false;

			while (true) {
				// read into buffer
				int n = fin.Read (buffer, 0, 8192);
				// error, reached EOS prematurely
				if (n <= 0) {
					fin.Close ();
					return solidline ? nrows + 1 : nrows;
				}

				// scan for \n
				for (int i = 0; i < n; i++) {
					switch (buffer [i]) {
					case 10: // LN
						if (solidline || !nonwhitespace) nrows++;
						solidline = false;
						break;
					case 0:
					case 9:  // tab
					case 11: // VT
					case 12: // FF
					case 13: // CR
					case 32: // space
						break;
					default:
						solidline = true;
						break;
					}
				}
			}
		}

	    
		/// <summary>
		/// Decompress the specified file and basefile.
		/// </summary>
		/// <param name='file'>
		/// File.
		/// </param>
		/// <param name='basefile'>
		/// Basefile.
		/// </param>
	    public static void Decompress (string file, string basefile = null)
	    {
			var defile_dst = (basefile != null) ? basefile : StringUtils.RTrimField (file, 1, '.');
			var defile_tmp = DirOf (defile_dst) + Path.DirectorySeparatorChar + "." + FileOf (defile_dst);

	    	Stream fin = CreateInputStream (file);
	    	Stream fout = CreateOutputStream (defile_tmp);
	    	
	    	try
	    	{
	    		IOUtils.Copy (fin, fout);
	    		fin.Close();
	    		fout.Close ();
	    	}
	    	catch
	    	{
	    		FileUtils.DeleteFile (defile_tmp);
	    		throw;
	    	}

			if (File.Exists (defile_dst))
				File.Delete (defile_dst);

			File.Move (defile_tmp, defile_dst);
	    }
		
	    
		/// <summary>
		/// Decompress the specified file and basefile, with a file lock.  We poll the file
		/// being decompressed to see that progress is occurring, otherwise delete the
		/// decompressed file and redo.
		/// </summary>
		/// <param name='file'>
		/// File.
		/// </param>
		/// <param name='basefile'>
		/// Basefile.
		/// </param>
		/// <param name='Tpolling'>
		/// Polling period for progress in milliseconds
		/// </param>
	    public static string DecompressWithLock (string file, string basefile = null, int Tpolling = 5000)
	    {
			var defile_dst = (basefile != null) ? basefile : StringUtils.RTrimField (file, 1, '.');
			var defile_tmp = DirOf (defile_dst) + Path.DirectorySeparatorChar + "." + FileOf (defile_dst);
	    	
	    	if (!File.Exists (defile_tmp))
			{
				Decompress (file, defile_dst);
				return defile_dst;
			}

			var info = new FileInfo (defile_tmp);
			var psize = info.Length;

			_log.Info ("decompress: waiting for another process to finish decompressing: " + file);
			while (File.Exists (defile_tmp))
			{
				Thread.Sleep (Tpolling);
				var nsize = info.Length;

				if (nsize == psize)
				{
					_log.Info ("decompress: other process dead, will redo decompression: " + file);
					File.Delete (defile_tmp);
					Decompress (file, defile_dst);
					return defile_dst;
				}
			}

			return defile_dst;
	    }
	
	        
	    
	    /**
	     * Compress file
	     *
	     * @param file			filename to be compressed
	     */
	    public static void Compress (string basefile)
	    {
	    	Stream fin = CreateInputStream (basefile);
	    	Stream fout = CreateOutputStream (basefile + ".gz");
	    	
	    	try
	    	{
	    		IOUtils.Copy (fin, fout);
	    		fin.Close();
	    		fout.Close ();
	    	}
	    	catch
	    	{
	    		FileUtils.DeleteFile (basefile + ".gz");
	    		throw;
	    	}
	    }
	    
	    
	    /**
	     * Get last modified timestamp for a given filename
	     * 
	     * @param filename		file path
	     * @return				date stamp of last modification
	     */
	    public static DateTime GetLastModified (String filename)
	    {
	    	FileInfo file = new FileInfo (filename);
	    	return file.LastWriteTime;
	    }
	    
	    
	    /**
	     * Create output stream, possibly compressed if compression extension used
	     *
	     * @param filename		filename
	     * @return				output stream
	     */
		public static Stream CreateOutputStream (string filename, int compresslevel = 5, int buffersize = 8*4096)
	    {
	    	filename = NormalizeDir (filename);
	
	    	var fout = new BufferedWriteStream (
				new FileStream (filename, FileMode.Create),
				buffersize);
	    	
	    	if (filename.EndsWith (".gz"))
	    		return new GZipStream(fout, CompressionMode.Compress);
	    	else
	    		return fout;
	    }
	    
	    
	    
	    /**
	     * Create input stream, possibly compressed if compression extension used
	     *
	     * @param filename		filename
	     * @return				input stream
	     */
	    public static Stream CreateInputStream (string filename, int buffersize = 8*4096)
	    {
	    	filename = NormalizeDir (filename);
	    	
	    	var fin = new FileStream (filename, FileMode.Open, FileAccess.Read, FileShare.Read, 8*8192, FileOptions.SequentialScan);
	    	
	    	if (filename.EndsWith (".gz"))
	    		return new BufferedReadStream (new GZipStream(fin, CompressionMode.Decompress));
	    	else
	    		return fin;
	    }
	    
		
		/**
		 * Read file to completion, returning as buffer
		 * 
		 * @param filename		path to filename
		 * @return				buffer containing file
		 * @throws 			IOException
		 */
		public static Blob ReadFile (string filename)
		{
	    	filename = NormalizeDir (filename);
			Blob buffer = new Blob();
			var stream = new FileStream (filename, FileMode.Open, FileAccess.Read);
			
			while (true)
			{
				// acquire buffer for read
				Blob.Region region = buffer.Acquire (4096, false);
	
				int n = stream.Read (region.Bytes, region.Offset, region.Span);
				if (n <= 0)
					return buffer;
				else
					region.Used += n;
			}
		}
		
		
		/**
		 * Write content to file
		 * 
		 * @param filename		name of file
		 * @param content		content to be written
		 */
		public static void WriteFile (string filename, string content)
		{
	    	filename = NormalizeDir (filename);
			var stream = new BufferedWriteStream (new FileStream (filename,FileMode.Create));
		    
		    IOUtils.Write (stream, content);
		    stream.Close ();
		}
		
	
		/// <summary>
		/// Deletes the directory recursively.
		/// </summary>
		/// <param name="path">Path.</param>
		public static void DeleteDirectory (string path)
		{
			path = NormalizeDir (path);
			string[] files = Directory.GetFiles(path);
			string[] dirs = Directory.GetDirectories(path);

			foreach (string file in files)
			{
				File.SetAttributes(file, FileAttributes.Normal);
				File.Delete(file);
			}

			foreach (string dir in dirs)
			{
				DeleteDirectory (dir);
			}

			Directory.Delete(path, false);
		}


		/// <summary>
		/// Deletes the directory recursively.
		/// </summary>
		/// <param name="path">Path.</param>
		public static void DeleteFile (string path)
		{
			path = NormalizeDir (path);

			File.SetAttributes(path, FileAttributes.Normal);
			File.Delete(path);
		}
	
			
		
		/**
		 * move file or dir
		 */
		public static void Move (string frompath, string topath)
		{
			File.Move (frompath, topath);			
		}
	
		
		
		/**
		 * Create directory and any intervening missing directories in path
		 * 
		 * @param path		directory path
		 */
		public static void MkDirs (string path)
		{
			path = NormalizeDir(path);
	        Directory.CreateDirectory (path);
			Chmod (path, Mode_rwx);
		}
		
		
		
		/**
		 * Determine if given file/path exists
		 * 
		 * @param path		path/file to check
		 */
		public static bool Exists (string path)
		{
		    FileInfo file = new FileInfo (path);
		    return file.Exists;
		}
		
		
		/// <summary>
		/// Find the nth ancestor of given directory
		/// </summary>
		/// <param name='dir'>
		/// Directory path
		/// </param>
		/// <param name='nth'>
		/// Nth.
		/// </param>
		public static string AncestorOf (string dir, int nth)
		{
			DirectoryInfo idir = new DirectoryInfo (dir);
			for (int i = 0 ; i < nth ; i++)
				idir = idir.Parent;
			
			return idir.FullName;
		}
		
		
		/// <summary>
		/// Finds the given directory by name in descendent paths from the base path
		/// </summary>
		/// <returns>
		/// A sequence of directories matching name
		/// </returns>
		/// <param name='basedir'>
		/// Base directory
		/// </param>
		/// <param name='dirname'>
		/// Directory name
		/// </param>
		public static IEnumerable<string> FindDir (string basedir, string dirname, int maxdepth = int.MaxValue)
		{
			foreach (var subdir in SubDirsOf (basedir))
			{
				var name = subdir.Name;
				var path = subdir.FullName;
				if (name != dirname)
				{
					foreach (var result in FindDir (path, dirname, maxdepth-1)) 
						yield return result;
				} else
					yield return path;
			}
		}

		
		#region Implementation


        /// <summary>
        /// Provide an enumeration of subdirs
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static IEnumerable<DirectoryInfo> SubDirsOf(string path)
        {
            DirectoryInfo[] dirs = null;
            try
            {
			    DirectoryInfo bdir = new DirectoryInfo (path);
			    dirs = bdir.GetDirectories();
            }
            catch (Exception)
            {
            }

            if (dirs != null)
            {
                foreach (var dir in dirs)
                    yield return dir;
            }
        }

		
		private static TimeSpan TimeFileAccess (string filename)
		{
			var buffer = new byte[4096];
			var random = new Random ();
			
			
			Stopwatch watch = new Stopwatch();
			watch.Start();
			
			var fileinfo = new FileInfo (filename);
			var filelen = fileinfo.Length;
			var stream = new FileStream (filename, FileMode.Open, FileAccess.Read);
			
			for (int i = 0 ; i < 8 ; i++)
			{
				var Istart = (long)(random.NextDouble() * Math.Max(filelen - 4096, 0));
				stream.Seek (Istart, SeekOrigin.Begin);
				stream.Read (buffer, 0, buffer.Length);
			}
			
			stream.Close();
			
			watch.Stop ();
			return watch.Elapsed;
		}


		private static void InitializePosix ()
		{
			if (_syscall == typeof(string))
				return;
			else if (_syscall != null)
				return;

			SystemUtils.AddLibPath ("/opt/mono-3.0/lib");

			try
			{
				var assembly = Assembly.Load ("Mono.Posix, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756");
				_syscall = assembly.GetType ("Mono.Unix.Native.Syscall");
				_Mchmod = _syscall.GetMethod ("chmod");
			}
			catch (Exception)
			{
				_syscall = typeof(string);
				_log.Warn ("various posix functions not supported on this platform (chmod(), etc)");
			}
		}
	
		
		#endregion

		// Variables
		
		static Dictionary<string,bool>		_local = new Dictionary<string, bool>();
		static Type							_syscall;
		static MethodInfo					_Mchmod;

		static Logger						_log = Logger.Get ("DATA");
	}
}

