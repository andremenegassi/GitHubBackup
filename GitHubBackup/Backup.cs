using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Octokit;
using System;
using System.Diagnostics;
using System.IO;


namespace GitHubBackup
{
    public class Backup
    {
        Config _config = new Config();
        public Backup()
        {
            Config config = new Config();

            IConfiguration Configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables()
              .Build();
            Configuration.Bind("Config", _config);
       
        }

        public void Do()
        {
            string gitDir = _config.GitDir;
            string containerRepoDir = _config.ContainerRepoDir;
            string githubToken = _config.GitHubToken;
            string organizacao = _config.GitHubOrganizacao;

            if (!Directory.Exists(containerRepoDir))
            {
                Directory.CreateDirectory(containerRepoDir);
            }

            var client = new GitHubClient(new ProductHeaderValue(organizacao));
            client.Credentials = new Credentials(githubToken);

            var repos = client.Repository.GetAllForCurrent().Result;
            Console.WriteLine("Syncing repos...");

            int i = 0;
            foreach (var r in repos)
            {
                i++;
                Console.WriteLine(i + " - " + r.FullName.PadRight(50, ' '));
                string repo = containerRepoDir + r.Name;

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.RedirectStandardOutput = true;
                psi.FileName = gitDir;

                string command = "";

                if (!Directory.Exists(repo))
                {
                    Directory.SetCurrentDirectory(containerRepoDir);
                    command = $"clone {r.CloneUrl}";
                }
                else
                {
                    Directory.SetCurrentDirectory(repo);
                    command = $"pull";
                }

                psi.Arguments = command;

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit();
                    Console.WriteLine(process.StandardOutput.ReadToEnd());
                }

                Directory.SetCurrentDirectory(containerRepoDir);
            }

            Console.WriteLine("End");
            Console.ReadKey();


        }
    }
}
