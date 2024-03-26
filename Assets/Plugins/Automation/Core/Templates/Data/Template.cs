using System.Text.RegularExpressions;
using Automation.Git;

namespace Automation.Core.Templates.Data
{
    public class Template
    {
        private const string specialCharsToDisplay = @"[a-zA-Z\{\}\!\^\s]";
        private const string specialChars = @"[\{\}\!\^\s]";
        
        public readonly string Name;
        public readonly string Version;
        public readonly string LastVersion;

        public readonly bool Installed;
        public readonly GitInfo GitInfo;
        
        public bool IsLastVersionInstalled => LastVersion == Version;

        public Template(string name, string version, string lastVersion, bool installed, GitInfo gitInfo)
        {
            Name = name;
            Installed = installed;

            Version = Regex.Replace(version, specialCharsToDisplay, string.Empty);

            GitInfo = gitInfo;

            if (string.IsNullOrEmpty(lastVersion))
            {
                LastVersion = "unknown";
                return;
            }
            
            Regex.Replace(lastVersion, specialChars, string.Empty);
            LastVersion = Regex.Replace(lastVersion, specialCharsToDisplay, string.Empty);

            
        }
    }
}