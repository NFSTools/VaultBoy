using System.Diagnostics.CodeAnalysis;
using GalaSoft.MvvmLight.Messaging;
using VaultLib.Core.DB;

namespace VaultBoy.Messages
{
    public class CurrentDatabaseChangedMessage : MessageBase
    {
        public CurrentDatabaseChangedMessage([NotNull] Database database)
        {
            Database = database;
        }

        public Database Database { get; }
    }
}