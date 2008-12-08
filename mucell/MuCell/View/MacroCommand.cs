using System;
using System.Collections.Generic;
using System.Text;

namespace MuCell.View
{
    public class MacroCommand : ICommand
    {
        private List<ICommand> commands;

        public MacroCommand()
        {
            commands = new List<ICommand>();
        }
        public void addCommand(ICommand command)
        {
            commands.Add(command);
        }
        public int countCommands()
        {
            return commands.Count;
        }

        #region ICommand Members

        /// <summary>
        /// Performs all the commands inside the macro
        /// </summary>
        public void doAction()
        {
            foreach (ICommand command in commands)
            {
                command.doAction();
            }
        }

        /// <summary>
        /// Un-performs all the commands inside the macro but in reverse order
        /// </summary>
        public void undoAction()
        {
            commands.Reverse();
            foreach (ICommand command in commands)
            {
                command.undoAction();
            }
            commands.Reverse();
        }

        #endregion
    }
}
