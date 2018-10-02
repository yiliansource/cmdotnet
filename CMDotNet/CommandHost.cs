using System.Reflection;

namespace YSource.Commands
{
    public abstract class CommandHost
    {
        protected CommandExecuter Executer { get; set; }

        public void Initialize()
        {
            Executer = new CommandExecuter();
            InstallCommands();
        }

        protected virtual void InstallCommands ()
        {
            Executer.InstallModules(Assembly.GetExecutingAssembly());
        }
    }
}
