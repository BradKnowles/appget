﻿using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using AppGet.CommandLine;
using AppGet.Update;
using Console = Colorful.Console;

namespace AppGet.Commands.Outdated
{
    public class OutdatedCommandHandler : ICommandHandler
    {
        private readonly UpdateService _updateService;

        public OutdatedCommandHandler(UpdateService updateService)
        {
            _updateService = updateService;
        }

        public bool CanExecute(AppGetOption commandOptions)
        {
            return commandOptions is OutdatedOptions;
        }

        public async Task Execute(AppGetOption commandOptions)
        {
            var matches = await _updateService.GetUpdates();

            var updates = matches.Where(c => c.Status == UpdateStatus.Available).ToList();
            if (updates.Any())
            {
                Console.WriteLine("{0} Available Updates:", updates.Count);
                updates.ShowTable();
            }
            else
            {
                Console.WriteLine("No updates were found.");
            }


            if (commandOptions.Verbose)
            {
                Console.WriteLine("");
                Console.WriteLine("Latest Version Already Installed:", Color.Green);

                var upToDate = matches.Where(c => c.Status == UpdateStatus.UpToDate);

                upToDate.ShowTable();
            }
        }
    }
}