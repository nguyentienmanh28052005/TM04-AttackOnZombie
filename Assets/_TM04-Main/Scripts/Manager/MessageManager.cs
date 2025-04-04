using System;
using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using UnityEngine;


    public enum ManhMessageType
    {
        OnGameStart,
        OnRound1,
        
        /// <summary>
        /// When the game is over
        /// </summary>
        OnGameLose,
        OnGameWin,
        OnButtonClick,
        OnHitEnemy,
        OnEnemyDie,
        OnCollectCoin,
        OnDataChanged,
        
    }
    public class Message
    {
        public ManhMessageType type;
        public object[] data;
        
        public Message(ManhMessageType type)
        {
            this.type = type;
        }
        
        public Message(ManhMessageType type, object[] data)
        {
            this.type = type;
            this.data = data;
        }
    }
    public interface IMessageHandle
    {
        void Handle(Message message);
    }
    public class MessageManager : Singleton<MessageManager>, ISerializationCallbackReceiver
    {
        // private static MessageManager instance = null;
        
        //Stores information when Serialize data in the subcribers-Dictionary
        [HideInInspector] public List<ManhMessageType> _keys = new List<ManhMessageType>();
        [HideInInspector] public List<List<IMessageHandle>> _values = new List<List<IMessageHandle>>();
        
        
        private Dictionary<ManhMessageType, List<IMessageHandle>> subcribers = new Dictionary<ManhMessageType, List<IMessageHandle>>();
        /*public static MessageManager Instance { get { return instance; } }
        void Start()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }*/
        public void AddSubcriber(ManhMessageType type, IMessageHandle handle)
        {
            if (!subcribers.ContainsKey(type))
                subcribers[type] = new List<IMessageHandle>();
            if (!subcribers[type].Contains(handle))
                subcribers[type].Add(handle);
            Debug.Log("Add: " + handle);
        }
        
        public void RemoveSubcriber(ManhMessageType type, IMessageHandle handle)
        {
            if (subcribers.ContainsKey(type))
                if (subcribers[type].Contains(handle))
                    subcribers[type].Remove(handle);
        }
        
        public void SendMessage(Message message)
        {
            if (subcribers.ContainsKey(message.type))
                for (int i = subcribers[message.type].Count - 1; i > -1; i--)
                    subcribers[message.type][i].Handle(message);
        }
        
        public void SendMessageWithDelay(Message message, float delay)
        {
            StartCoroutine(_DelaySendMessage(message, delay));
        }
        
        private IEnumerator _DelaySendMessage(Message message, float delay)
        {
            yield return new WaitForSeconds(delay);
            SendMessage(message);
        }
        
        public void OnBeforeSerialize()
        {
            _keys.Clear();
            _values.Clear();
            foreach (var element in subcribers)
            {
                _keys.Add(element.Key);
                _values.Add(element.Value);
            }
        }
        
        public void OnAfterDeserialize()
        {
            subcribers = new Dictionary<ManhMessageType, List<IMessageHandle>>();
            for (int i = 0; i < _keys.Count; i++)
            {
                subcribers.Add(_keys[i], _values[i]);
            }
        }
    }
