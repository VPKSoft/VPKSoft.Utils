#region License
/*
VPKSoft.Utils

Some utilities by VPKSoft.
Copyright © 2021 VPKSoft, Petteri Kautonen

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
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VPKSoft.Utils
{
    /// <summary>
    /// A class containing information of a single file. This class is used by FileEnumerator.RecurseFiles method.
    /// </summary>
    public class FileEnumeratorFileEntry
    {
        /// <summary>
        /// A file name without path.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// A file name with path.
        /// </summary>
        public string FileNameWithPath { get; set; }

        /// <summary>
        /// A file name without path and without extension.
        /// </summary>
        public string FileNameNoExtension { get; set; }

        /// <summary>
        /// A most upper path of the directory tree.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The full path of the file's location.
        /// </summary>
        public string PathFull { get; set; }

        /// <summary>
        /// Creates a new <see cref="FileEnumeratorFileEntry"/> class instance from a given <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The name and the path of the file.</param>
        /// <returns>An instance to a <see cref="FileEnumeratorFileEntry"/> class with file details.</returns>
        public static FileEnumeratorFileEntry FromFileName(string fileName)
        {
            return new FileEnumeratorFileEntry
            {
                FileName = System.IO.Path.GetFileName(fileName), // .. so the file name..
                FileNameWithPath = fileName, // .. and the file name with path..
                FileNameNoExtension =
                    System.IO.Path
                        .GetFileNameWithoutExtension(fileName), // .. and the file without path and without extension..
                Path = new DirectoryInfo(System.IO.Path.GetDirectoryName(fileName)).Name, // .. and the non-full path..
                PathFull = System.IO.Path.GetDirectoryName(fileName) // .. and the full path..
            };
        }
    }

    /// <summary>
    /// A class containing information of a single directory. This class is used by FileEnumerator.RecurseDirectories method.
    /// </summary>
    public class FileEnumeratorDirectoryEntry
    {
        /// <summary>
        /// A most upper path of the directory tree.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The full path of the file's location.
        /// </summary>
        public string PathFull { get; set; }

        /// <summary>
        /// Creates a new <see cref="FileEnumeratorFileEntry"/> class instance from a given <paramref name="path"/>.
        /// </summary>
        /// <param name="path">The path to use.</param>
        /// <returns>An instance to a <see cref="FileEnumeratorDirectoryEntry"/> class with the path details.</returns>
        public static FileEnumeratorDirectoryEntry FromPath(string path)
        {
            return new FileEnumeratorDirectoryEntry
            {
                Path = new DirectoryInfo(path).Name, // .. so the non-full path..
                PathFull = path // .. and the full path..
            };
        }
    }

    /// <summary>
    /// A class which searches a given directory with given file extensions recursively.
    /// </summary>
    public static class FileEnumerator
    {
        /// <summary>
        /// Video file extensions.
        /// </summary>
        public static readonly string[] FiltersVideoVlc =
            { "*.3g2", "*.3gp", "*.3gp2", "*.3gpp", "*.amv", "*.asf", "*.avi", "*.bik", "*.bin", "*.divx", "*.drc",
              "*.dv", "*.f4v", "*.flv", "*.gvi", "*.gfx", "*.iso", "*.m1v", "*.m2v", "*.m2t", "*.m2ts", "*.m4v", "*.mkv",
              "*.mov", "*.mp2", "*.mp2v", "*.mp4", "*.mp4v", "*.mpe", "*.mpeg", "*.mpeg1", "*.mpeg2", "*.mpeg4", "*.mpg",
              "*.mpv2", "*.mts", "*.mtv", "*.mxf", "*.mxg", "*.nsv", "*.nuv", "*.ogm", "*.ogv", "*.ogx", "*.ps", "*.rec",
              "*.rm", "*.rmvb", "*.rpl", "*.thp", "*.tod", "*.ts", "*.tts", "*.txd", "*.vob", "*.vro", "*.webm", "*.wm",
              "*.wmv", "*.wtv", "*.xesc" };

        /// <summary>
        /// Video file extensions.
        /// </summary>
        public static readonly string[] FiltersVideoVlcNoBinNoIso =
            { "*.3g2", "*.3gp", "*.3gp2", "*.3gpp", "*.amv", "*.asf", "*.avi", "*.bik", "*.divx", "*.drc",
              "*.dv", "*.f4v", "*.flv", "*.gvi", "*.gfx", "*.m1v", "*.m2v", "*.m2t", "*.m2ts", "*.m4v", "*.mkv",
              "*.mov", "*.mp2", "*.mp2v", "*.mp4", "*.mp4v", "*.mpe", "*.mpeg", "*.mpeg1", "*.mpeg2", "*.mpeg4", "*.mpg",
              "*.mpv2", "*.mts", "*.mtv", "*.mxf", "*.mxg", "*.nsv", "*.nuv", "*.ogm", "*.ogv", "*.ogx", "*.ps", "*.rec",
              "*.rm", "*.rmvb", "*.rpl", "*.thp", "*.tod", "*.ts", "*.tts", "*.txd", "*.vob", "*.vro", "*.webm", "*.wm",
              "*.wmv", "*.wtv", "*.xesc" };

        /// <summary>
        /// Audio file extensions.
        /// </summary>
        public static readonly string[] FiltersAudio =
            { "*.mp3", "*.ogg", "*.wav", "*.flac", "*.wma", "*.m4a", "*.aac", "*.aif", "*.aiff" };

        /// <summary>
        /// Image file extensions.
        /// </summary>
        public static readonly string[] FiltersImage =
            { "*.gif", "*.jpg", "*.jpeg", "*.exif", "*.png", "*.tiff", "*.tif" };

        /// <summary>
        /// An error tolerant method to search for files than the System.IO.Directory.GetFiles or System.IO.Directory.EnumerateFiles.
        /// </summary>
        /// <param name="path">The path from which to start the search from.</param>
        /// <param name="searchPattern">The search string to match against the names of subdirectories in path.This parameter can contain a combination of valid literal and wildcard characters, but doesn't support regular expressions.</param>
        /// <param name="noFiles">Indicates if files should be enumerated.</param>
        /// <param name="firstCall">Indicates if the recursion is at first level.</param>
        /// <returns>A list of found files matching the search pattern.</returns>
        private static IEnumerable<string> SafeGetFilesAll(string path, string searchPattern, bool noFiles, bool firstCall = true)
        {
            return SafeGetFilesAllAsync(path, searchPattern, noFiles, firstCall).Result;
        }

        /// <summary>
        /// An error tolerant method to search for files than the System.IO.Directory.GetFiles or System.IO.Directory.EnumerateFiles.
        /// </summary>
        /// <param name="path">The path from which to start the search from.</param>
        /// <param name="searchPattern">The search string to match against the names of subdirectories in path.This parameter can contain a combination of valid literal and wildcard characters, but doesn't support regular expressions.</param>
        /// <param name="noFiles">Indicates if files should be enumerated.</param>
        /// <param name="firstCall">Indicates if the recursion is at first level.</param>
        /// <returns>A list of found files matching the search pattern.</returns>
        private static async Task<IEnumerable<string>> SafeGetFilesAllAsync(string path, string searchPattern, bool noFiles, bool firstCall = true)
        {
            // create a list of file names which is updated by this method recursively..
            List<string> fileList = new List<string>();
            IEnumerable<string> dirs; // first the directories are required..

            if (path == "." || path == "..") // can't recurse self or backwards..
            {
                return fileList;
            }

            try
            {
                // an exception might happen if there is no right to access the subdirectory (also PathTooLongException might occur)..
                dirs = Directory.EnumerateDirectories(path, searchPattern);
            }
            catch
            {
                // the exception occurred, so just return what was "gathered", which is nothing..
                return fileList;
            }

            // the first given path is in the top of the directory tree..
            if (dirs.ToList().Count == 0 && !noFiles && firstCall)
            {
                // so add it to the list..
                dirs = new List<string>(new string[] { path });
            }

            // search through all the subdirectories in the base directory..
            foreach (string baseDir in dirs)
            {
                try
                {
                    if (!noFiles) // if only directories are searched then there is no need to enumerate files..
                    {
                        // try to find the files with a given search pattern..
                        foreach (string file in Directory.EnumerateFiles(baseDir, searchPattern))
                        {
                            fileList.Add(file);
                        }
                    }
                }
                catch
                {
                    // nothing to do here as the loop will continue..
                }

                if (noFiles) // if only directories then only return the directories..
                {
                    fileList.Add(baseDir);
                }

                // recurse the subdirectory..
                fileList.AddRange(await SafeGetFilesAllAsync(baseDir, searchPattern, noFiles, false).ConfigureAwait(false));
            }
            // return what was gotten from the error tolerant loop..
            return fileList;
        }


        /// <summary>
        /// Removes the excess '*' and '.' characters from a filter list as this class uses regular expressions to filter a file list.
        /// </summary>
        /// <param name="filters">An array of file extension filters to "normalize".</param>
        /// <returns>A list of filters with excess characters removed.</returns>
        private static string[] NormalizeFilters(string[] filters)
        {
            // create a task and run it..
            // no foreach clause as the parameter array is being manipulated..
            for (int i = 0; i < filters.Length; i++) // loop through the parameter array..
            {
                // remove the '*' and '.' characters from the filters..
                filters[i] = filters[i].TrimStart('*').TrimStart('.');
            }
            // return the manipulated array..
            return filters;
        }

        /// <summary>
        /// Recurses all the directories for files of a given base path with given filters.
        /// </summary>
        /// <param name="path">A path to start the recursion from.</param>
        /// <param name="filters">A list of filter to use in the search. The filters are case-insensitive.</param>
        /// <returns>A collection of FileEnumeratorFileEntry class instances which matched the given filters.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the <paramref name="filters"/>is an empty array.</exception>
        public static IEnumerable<FileEnumeratorFileEntry> RecurseFiles(string path, params string[] filters)
        {
            return RecurseFilesAsync(path, filters).Result;
        }

        /// <summary>
        /// Recurses all the directories for files of a given base path with given filters.
        /// </summary>
        /// <param name="path">A path to start the recursion from.</param>
        /// <param name="filters">A list of filter to use in the search. The filters are case-insensitive.</param>
        /// <returns>A collection of FileEnumeratorFileEntry class instances which matched the given filters.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the <paramref name="filters"/>is an empty array.</exception>
        public static async Task<IEnumerable<FileEnumeratorFileEntry>> RecurseFilesAsync(string path, params string[] filters)
        {
            // as this recursion is based on file extensions a zero amount of them will throw an exception..
            if (filters.Length == 0)
            {
                throw new InvalidOperationException("At least one filter must be given.");
            }

            // remove the excess '*' and '.' characters from a filter list..
            filters = NormalizeFilters(filters);

            // create a regular exception of the file extensions ignoring the case and the culture..
            var regex = new Regex(
                @"$(?<=\.(" + string.Join("|", filters) + "))", // separate the filters with or ('|') character..
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

            // start the recursive loop to find the files..
            var files = await SafeGetFilesAllAsync(path, "*", false).ConfigureAwait(false);
            files = files.Where(f => regex.IsMatch(f)) // ..where matching the regular expression..
                    .ToList(); // ..and make it to a list..

            // initialize the result list..
            List<FileEnumeratorFileEntry> result = new List<FileEnumeratorFileEntry>();

            // enumerate the results..
            foreach (string file in files)
            {
                // hopefully this is all the data one would require of a file listing..
                result.Add(new FileEnumeratorFileEntry()
                {
                    FileName = Path.GetFileName(file), // .. so the file name..
                    FileNameWithPath = file, // .. and the file name with path..
                    FileNameNoExtension = Path.GetFileNameWithoutExtension(file), // .. and the file without path and without extension..
                    Path = new DirectoryInfo(Path.GetDirectoryName(file)).Name, // .. and the non-full path..
                    PathFull = Path.GetDirectoryName(file) // .. and the full path..
                });
            }
            return result;
        }

        /// <summary>
        /// Recurses all the directories for files of a given base path.
        /// </summary>
        /// <param name="path">>A path to start the recursion from.</param>
        /// <returns>A collection of FileEnumeratorFileEntry class instances of found files.</returns>
        public static IEnumerable<FileEnumeratorFileEntry> RecurseFilesAll(string path)
        {
            return RecurseFilesAllAsync(path).Result;
        }

        /// <summary>
        /// Recurses all the directories for files of a given base path.
        /// </summary>
        /// <param name="path">>A path to start the recursion from.</param>
        /// <returns>A collection of FileEnumeratorFileEntry class instances of found files.</returns>
        public static async Task<IEnumerable<FileEnumeratorFileEntry>> RecurseFilesAllAsync(string path)
        {
            // start the recursive loop to find the files..
            var files = await SafeGetFilesAllAsync(path, "*", false).ConfigureAwait(false);
            files = files.Distinct();

            // initialize the result list..
            List<FileEnumeratorFileEntry> result = new List<FileEnumeratorFileEntry>();
            // enumerate the results..
            foreach (string file in files)
            {
                // hopefully this is all the data one would require of a file listing..
                result.Add(new FileEnumeratorFileEntry()
                {
                    FileName = Path.GetFileName(file), // .. so the file name..
                    FileNameWithPath = file, // .. and the file name with path..
                    FileNameNoExtension = Path.GetFileNameWithoutExtension(file), // .. and the file without path and without extension..
                    Path = new DirectoryInfo(Path.GetDirectoryName(file)).Name, // .. and the non-full path..
                    PathFull = Path.GetDirectoryName(file) // .. and the full path..
                });
            }
            return result;
        }

        /// <summary>
        /// Recurses all the directories of a given base path.
        /// </summary>
        /// <param name="path">A path to start the recursion from.</param>
        /// <returns>
        /// A collection of FileEnumeratorDirectoryEntry class instances of the directories.
        /// </returns>
        public static IEnumerable<FileEnumeratorDirectoryEntry> RecurseDirectoriesAll(string path)
        {
            return RecurseDirectoriesAllAsync(path).Result;
        }

        /// <summary>
        /// Recurses all the directories of a given base path.
        /// </summary>
        /// <param name="path">A path to start the recursion from.</param>
        /// <returns>
        /// A collection of FileEnumeratorDirectoryEntry class instances of the directories.
        /// </returns>
        public static async Task<IEnumerable<FileEnumeratorDirectoryEntry>> RecurseDirectoriesAllAsync(string path)
        {
            // start the recursive loop to find the files..
            var directories = await SafeGetFilesAllAsync(path, "*", true).ConfigureAwait(false);

            // initialize the result list..
            List<FileEnumeratorDirectoryEntry> result = new List<FileEnumeratorDirectoryEntry>();

            // enumerate the results..
            foreach (string dir in directories)
            {
                // hopefully this is all the data one would require of a directory listing..
                result.Add(new FileEnumeratorDirectoryEntry()
                {
                    Path = new DirectoryInfo(dir).Name, // .. so the non-full path..
                    PathFull = dir // .. and the full path..
                });
            }
            return result;
        }

        /// <summary>
        /// Recurses all the directories of a given base path with given filters.
        /// </summary>
        /// <param name="path">A path to start the recursion from.</param>
        /// <param name="filters">A list of filter to use in the search. The filters are case-insensitive.</param>
        /// <returns>
        /// A collection of FileEnumeratorDirectoryEntry class instances of the directories containing the files
        /// matching the search pattern.
        /// </returns>
        /// <exception cref="InvalidOperationException">Thrown when the <paramref name="filters"/>is an empty array.</exception>
        public static IEnumerable<FileEnumeratorDirectoryEntry> RecurseDirectories(string path, params string[] filters)
        {
            return RecurseDirectoriesAsync(path, filters).Result;
        }

        /// <summary>
        /// Recurses the files of a given path with given filters. This method doesn't go deeper than the given <paramref name="path"/>.
        /// </summary>
        /// <param name="path">The path search files from.</param>
        /// <param name="filters">A list of filter to use in the search. The filters are case-insensitive.</param>
        /// <returns>A collection of FileEnumeratorFileEntry class instances of found files.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the <paramref name="filters"/>is an empty array.</exception>
        public static IEnumerable<FileEnumeratorFileEntry> GetFilesPath(string path, params string[] filters)
        {
            // as this recursion is based on file extensions a zero amount of them will throw an exception..
            if (filters.Length == 0)
            {
                throw new InvalidOperationException("At least one filter must be given.");
            }

            // remove the excess '*' and '.' characters from a filter list..
            filters = NormalizeFilters(filters);

            // create a regular exception of the file extensions ignoring the case and the culture..
            var regex = new Regex(
                @"$(?<=\.(" + string.Join("|", filters) + "))",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

            string[] filesArray = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);

            var files = filesArray.Where(f => regex.IsMatch(f)); // ..and match the regular expression..

            // initialize the result list..
            List<FileEnumeratorFileEntry> result = new List<FileEnumeratorFileEntry>();

            // enumerate the results..
            foreach (string file in files)
            {
                // hopefully this is all the data one would require of a directory listing..
                result.Add(new FileEnumeratorFileEntry()
                {
                    FileName = Path.GetFileName(file), // .. so the file name..
                    FileNameWithPath = file, // .. and the file name with path..
                    FileNameNoExtension = Path.GetFileNameWithoutExtension(file), // .. and the file without path and without extension..
                    Path = new DirectoryInfo(Path.GetDirectoryName(file)).Name, // .. and the non-full path..
                    PathFull = Path.GetDirectoryName(file) // .. and the full path..                
                });
            }
            return result;
        }

        /// <summary>
        /// Recurses all the directories of a given base path with given filters.
        /// </summary>
        /// <param name="path">A path to start the recursion from.</param>
        /// <param name="filters">A list of filter to use in the search. The filters are case-insensitive.</param>
        /// <returns>
        /// A collection of FileEnumeratorDirectoryEntry class instances of the directories containing the files
        /// matching the search pattern.
        /// </returns>
        /// <exception cref="InvalidOperationException">Thrown when the <paramref name="filters"/>is an empty array.</exception>
        public static async Task<IEnumerable<FileEnumeratorDirectoryEntry>> RecurseDirectoriesAsync(string path, params string[] filters)
        {
            // as this recursion is based on file extensions a zero amount of them will throw an exception..
            if (filters.Length == 0)
            {
                throw new InvalidOperationException("At least one filter must be given.");
            }

            // remove the excess '*' and '.' characters from a filter list..
            filters = NormalizeFilters(filters);


            // create a regular exception of the file extensions ignoring the case and the culture..
            var regex = new Regex(
                @"$(?<=\.(" + string.Join("|", filters) + "))",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

            // start the recursive loop to find the files..
            var directories = await SafeGetFilesAllAsync(path, "*", false).ConfigureAwait(false);

            directories = directories
                .Where(f => regex.IsMatch(f)) // ..and match the regular expression..
                .Select(f => Path.GetDirectoryName(f)) // ..and only select directories..
                .Distinct(); // ..and distinct..

            // initialize the result list..
            List<FileEnumeratorDirectoryEntry> result = new List<FileEnumeratorDirectoryEntry>();

            // enumerate the results..
            foreach (string dir in directories)
            {
                // hopefully this is all the data one would require of a directory listing..
                result.Add(new FileEnumeratorDirectoryEntry()
                {
                    Path = new DirectoryInfo(dir).Name, // .. so the non-full path..
                    PathFull = dir // .. and the full path..
                });
            }
            return result;
        }
    }
}
