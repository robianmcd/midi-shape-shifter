using System.Collections.Generic;

namespace MidiShapeShifter.Mss
{
    public delegate void EditorCommandStrategy();

    /// <summary>
    /// This class is uesed to keep track of processing that needs to be done. Commands waiting to 
    /// be executed are stored in a queue. To execute the commands in the queue call 
    /// DoAllCommands(). CommandIdType is used to determine whether a command being added to the
    /// queue already exists.
    /// 
    /// This class is useful for delaying processing until the host is idle. By
    /// delaying processing redundant calls to the same methods can also be avoided.
    /// 
    /// This class is thread safe.
    /// </summary>
    public class CommandQueue<CommandIdType>
    {
        //Used to ensure that multiple threads do not simultaneously access commandList
        protected object commandListLock;

        //Used as the ID for commands that don't need to check for duplicate commands of the same 
        //type.
        protected CommandIdType genericId;

        //List of pending commands. This is usually used as a queue.
        protected List<EditorCommandEntry> commandList;

        public CommandQueue()
        {
            commandListLock = new object();

            this.commandList = new List<EditorCommandEntry>();
        }

        public void Init(CommandIdType genericId)
        {
            this.genericId = genericId;
        }

        /// <summary>
        /// Add a command to the queue. When the command is executed commandExecutor will be called.
        /// </summary>
        public void EnqueueCommand(EditorCommandStrategy commandExecutor)
        {
            lock (commandListLock)
            {
                //Add entry to end of list
                this.commandList.Add(new EditorCommandEntry(this.genericId, 0, commandExecutor));
            }
        }

        /// <summary>
        /// Add a command to the queue. If a command with the same commandId and customSubId already
        /// exist then replace it with this one.
        /// </summary>
        /// <param name="customSubId">
        /// If commandId is not enough to determine whether another command is the same as this one 
        /// then specify a customSubId. For Example: If you had a commandId specifying some 
        /// operation on a parameter then customSubId could specify which parameter.
        /// </param>
        public void EnqueueCommandOverwriteDups(CommandIdType commandId, int customSubId, EditorCommandStrategy commandExecutor)
        {
            lock (commandListLock)
            {
                //Search for duplicates
                int duplicateIndex = this.commandList.FindIndex(
                        entry => entry.CommandId.Equals(commandId) && entry.CustomSubId == customSubId);

                if (duplicateIndex != -1)
                {
                    //Replace Duplicate
                    this.commandList[duplicateIndex] =
                        new EditorCommandEntry(commandId, customSubId, commandExecutor);
                }
                else
                {
                    //Add entry to end of list
                    this.commandList.Add(new EditorCommandEntry(commandId, customSubId, commandExecutor));
                }

            }
        }

        /// <summary>
        /// Add a command to the queue. If a command with the same commandId exist then replace it 
        /// with this one.
        /// </summary>
        public void EnqueueCommandOverwriteDups(CommandIdType commandId, EditorCommandStrategy commandExecutor)
        {
            EnqueueCommandOverwriteDups(commandId, 0, commandExecutor);
        }

        /// <summary>
        /// Execute commands in the queue until it is empty.
        /// </summary>
        public void DoAllCommands()
        {
            while (true)
            {
                EditorCommandEntry curEntry = null;

                lock (commandListLock)
                {
                    if (this.commandList.Count != 0)
                    {
                        curEntry = this.commandList[0];
                        this.commandList.RemoveAt(0);
                    }
                    else
                    {
                        break;
                    }
                }

                curEntry.CommandExecutor();
            }
        }

        /// <summary>
        /// Used to represent a command in the queue.
        /// </summary>
        protected class EditorCommandEntry
        {
            public CommandIdType CommandId;
            public int CustomSubId;
            public EditorCommandStrategy CommandExecutor;

            public EditorCommandEntry(CommandIdType commandId, int customSubId, EditorCommandStrategy commandExecutor)
            {
                this.CommandId = commandId;
                this.CustomSubId = customSubId;
                this.CommandExecutor = commandExecutor;
            }

        } //End Class EditorCommandId

    }
}
