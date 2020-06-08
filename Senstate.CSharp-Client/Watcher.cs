using System;
using System.ComponentModel;

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

            SenstateContext.SendEventData(SenstateEventConstants.InputEvent, new { 
               watchId = m_watchMeta.WatchId,
               data
            });
        }

        private void RegisterThisWatcher()
        {
            if (!m_registered)
            {
                SenstateContext.SendEventData(SenstateEventConstants.AddWatcher, new { 
                    watchId = m_watchMeta.WatchId,
                    tag = m_watchMeta.Tag,
                    group = m_watchMeta.Group,
                    type = (int)m_watchMeta.Type
                });
                m_registered = true;
            }
        }
    }

}
