using System;
using System.Diagnostics;
using System.IO;

namespace GitHubBackup
{
    class Program
    {
        static void Main(string[] args)
        {
            Backup bkp = new Backup();
            bkp.Do();
        }
    }
}
