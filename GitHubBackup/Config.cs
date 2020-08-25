using System;
using System.Collections.Generic;
using System.Text;

namespace GitHubBackup
{
    public class Config
    {
        public string GitDir { get; set; }
        public string ContainerRepoDir { get; set; }
        public string GitHubToken { get; set; }
        public string GitHubOrganizacao { get; set; }
    }
}
