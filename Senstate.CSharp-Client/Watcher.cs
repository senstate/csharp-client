using System;

namespace Senstate.CSharp_Client
{
    // Register a Watcher
    //  
    // Send the WatchData to that Watcher

    public enum WatcherType
    {
        String = 0, 
        Number, 
        Json
    }

    public class WatcherMeta
    {
        public string WatchId { get; set; } = Guid.NewGuid().ToString();
        public string Tag { get; set; }
        public string Group { get; set; }
        public WatcherType Type { get; set; }
    }

    /// <summary>
    /// Probably the Handler to Register / Send Data
    /// 
    /// TODO? make it generic? 
    /// </summary>
    public class Watcher
    {
        private bool m_registered = false;
        private WatcherMeta m_watchMeta;
        public Watcher(WatcherMeta watcherMeta)
        {
            m_watchMeta = watcherMeta;
        }

        public void SendData(object data)
        {
            RegisterThisWatcher();

            // TODO Send the Values to 
        }

        private void RegisterThisWatcher()
        {
            if (!m_registered)
            {
                // TODO Do the WebSocket Stuff 
                m_registered = true;
            }
        }
    }

}
