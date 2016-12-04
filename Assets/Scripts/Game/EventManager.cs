using System.Collections.Generic;
using Assets.Utility;
using Assets.Utility.Tuple;
using UnityEngine.Events;

namespace Assets.Game
{
    [System.Serializable]
    public class StringEvent : UnityEvent<string> { }

    [System.Serializable]
    public class LongEvent : UnityEvent<long> { }

    /// <summary>
    /// Manages event system
    /// </summary>
	public class EventManager : Singleton<EventManager>
    {
        public static bool Destroyed = false;

        private const int MaxQueuedActions = 100;
	    private Dictionary<string, LongEvent> _eventsLong;
	    private Dictionary<string, UnityEvent> _eventsVoid;
        private Queue<Tuple<string, UnityAction<long>>> _longDisposalQueue;
        private Queue<Tuple<string, UnityAction>> _voidDisposalQueue;
        // TODO: Listeners are not always called when events are triggered
         
        protected EventManager()
        {
            Initialize();
        }

		protected void Start()
		{
		}
		
		protected void Update()
		{
		    while (_voidDisposalQueue.Count > MaxQueuedActions)
		    {
		        Tuple<string, UnityAction> toRemove = _voidDisposalQueue.Dequeue();
                StopListening(toRemove.Item1, toRemove.Item2);
		    }
		    while (_longDisposalQueue.Count > MaxQueuedActions)
		    {
		        Tuple<string, UnityAction<long>> toRemove = _longDisposalQueue.Dequeue();
                StopListening(toRemove.Item1, toRemove.Item2);
		    }
		    if (_voidDisposalQueue.Count > 0)
		    {
		        Tuple<string, UnityAction> toRemove = _voidDisposalQueue.Dequeue();
                StopListening(toRemove.Item1, toRemove.Item2);
		    }
		    if (_longDisposalQueue.Count > 0)
		    {
		        Tuple<string, UnityAction<long>> toRemove = _longDisposalQueue.Dequeue();
                StopListening(toRemove.Item1, toRemove.Item2);
		    }
		}

        /// <summary>
        /// Initializes storage
        /// </summary>
	    public void Initialize()
	    {
	        if (_eventsLong == null)
	        {
	            _eventsLong = new Dictionary<string, LongEvent>();
	        }
	        if (_eventsVoid == null)
	        {
	            _eventsVoid = new Dictionary<string, UnityEvent>();
	        }
            if (_voidDisposalQueue == null)
            {
                _voidDisposalQueue = new Queue<Tuple<string, UnityAction>>();
            }
            if (_longDisposalQueue == null)
            {
                _longDisposalQueue = new Queue<Tuple<string, UnityAction<long>>>();
            }
	    }

        /// <summary>
        /// Drops all event listeners and uninitializes storage
        /// </summary>
	    public void Uninitialize()
	    {
	        _eventsLong.Clear();
	        _eventsLong = null;
            _eventsVoid.Clear();
	        _eventsVoid = null;
	    }

        /// <summary>
        /// Adds listener for given event name to given action
        /// </summary>
        /// <param name="eventName">
        /// Event to listen for
        /// </param>
        /// <param name="listener">
        /// Function to trigger when event occurs
        /// </param>
	    public void StartListening(string eventName, UnityAction listener)
	    {
	        UnityEvent thisEvent = null;
            if (_eventsVoid.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            } 
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                _eventsVoid.Add(eventName, thisEvent);
            }
        }

        /// <summary>
        /// Adds listener for given event name to given action
        /// </summary>
        /// <param name="eventName">
        /// Event to listen for
        /// </param>
        /// <param name="listener">
        /// Function to trigger when event occurs
        /// </param>
	    public void StartListening(string eventName, UnityAction<long> listener)
	    {
	        LongEvent thisEvent = null;
            if (_eventsLong.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            } 
            else
            {
                thisEvent = new LongEvent();
                thisEvent.AddListener(listener);
                _eventsLong.Add(eventName, thisEvent);
            }
        }

        /// <summary>
        /// Removes listener for given event name from given action
        /// </summary>
        /// <param name="eventName">
        /// Event to stop listening for
        /// </param>
        /// <param name="listener">
        /// Function to remove listener from
        /// </param>
        public void StopListening(string eventName, UnityAction listener)
        {
            UnityEvent thisEvent = null;
            if (_eventsVoid.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        /// <summary>
        /// Removes listener for given event name from given action
        /// </summary>
        /// <param name="eventName">
        /// Event to stop listening for
        /// </param>
        /// <param name="listener">
        /// Function to remove listener from
        /// </param>
        public void StopListening(string eventName, UnityAction<long> listener)
        {
            LongEvent thisEvent = null;
            if (_eventsLong.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        /// <summary>
        /// Trigger all listeners for given event
        /// </summary>
        /// <param name="eventName">
        /// Event to trigger
        /// </param>
        public void TriggerEvent(string eventName)
        {
            UnityEvent thisEvent = null;
            if (_eventsVoid.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke();
            }
        }

        /// <summary>
        /// Trigger all listeners for given event
        /// </summary>
        /// <param name="eventName">
        /// Event to trigger
        /// </param>
        /// <param name="param">
        /// Parameter to pass to listeners
        /// </param>
	    public void TriggerEvent(string eventName, long param)
	    {
	        LongEvent thisEvent = null;
            // Regular listeners
	        if (_eventsLong.TryGetValue(eventName, out thisEvent))
	        {
	            thisEvent.Invoke(param);
	        }
            // Value listeners
	        if (_eventsLong.TryGetValue(eventName + param.ToString(), out thisEvent))
	        {
	            thisEvent.Invoke(param);
	        }
	    }

        /// <summary>
        /// Adds listener for given event name to given action to trigger when event's value matches
        /// given value
        /// </summary>
        /// <param name="eventName">
        /// Event to listen for
        /// </param>
        /// <param name="listener">
        /// Function to trigger when event occurs and value matches
        /// </param>
        /// <param name="value">
        /// Value to listen for
        /// </param>

	    public void StartListeningForValue(string eventName, UnityAction<long> listener, long value)
	    {
	        LongEvent thisEvent = null;
	        eventName += value.ToString();
            if (_eventsLong.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            } 
            else
            {
                thisEvent = new LongEvent();
                thisEvent.AddListener(listener);
                _eventsLong.Add(eventName, thisEvent);
            }
	    }

        /// <summary>
        /// Removes listener for given event name and value from given action
        /// </summary>
        /// <param name="eventName">
        /// Event to stop listening for value on
        /// </param>
        /// <param name="listener">
        /// Function to remove listener from
        /// </param>
        /// <param name="value">
        /// Value to stop listening for
        /// </param>
        public void StopListeningForValue(string eventName, UnityAction<long> listener, long value)
        {
            LongEvent thisEvent = null;
            eventName += value.ToString();
            if (_eventsLong.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        /// <summary>
        /// Queues the listener for removal from listening for the given event. Does not remove immediately
        /// </summary>
        /// <param name="eventName">
        /// Event to queue removal from
        /// </param>
        /// <param name="listener">
        /// Listener to queue removal for
        /// </param>
        public void QueueToStopListening(string eventName, UnityAction listener)
        {
            _voidDisposalQueue.Enqueue(new Tuple<string, UnityAction>(eventName, listener));
        }

        /// <summary>
        /// Queues the listener for removal from listening for the given event. Does not remove immediately
        /// </summary>
        /// <param name="eventName">
        /// Event to queue removal from
        /// </param>
        /// <param name="listener">
        /// Listener to queue removal for
        /// </param>
        public void QueueToStopListening(string eventName, UnityAction<long> listener)
        {
            _longDisposalQueue.Enqueue(new Tuple<string, UnityAction<long>>(eventName, listener));
        }

        /// <summary>
        /// Queues the listener for removal from listening for the given event. Does not remove immediately
        /// </summary>
        /// <param name="eventName">
        /// Event to queue removal from
        /// </param>
        /// <param name="listener">
        /// Listener to queue removal for
        /// </param>
        /// <param name="value">
        /// Value to stop listening for
        /// </param>
        public void QueueToStopListeningForValue(string eventName, UnityAction<long> listener, long value)
        {
            eventName += value;
            _longDisposalQueue.Enqueue(new Tuple<string, UnityAction<long>>(eventName, listener));
        }

	    public new void OnDestroy()
	    {
	        Destroyed = true;
            base.OnDestroy();
	    }
	}
}