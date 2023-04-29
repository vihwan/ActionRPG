using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SG
{
    [System.Serializable] 
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector] 
        private List<TKey> keys = new List<TKey>();

        [SerializeField, HideInInspector] 
        private List<TValue> values = new List<TValue>();

        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            SyncInspectorFromDictionary();
        }

        /// <summary>
        /// 인스펙터를 딕셔너리로 초기화
        /// </summary>
        public void SyncInspectorFromDictionary()
        {
            //인스펙터 키 밸류 리스트 초기화
            keys.Clear();
            values.Clear();

            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        /// <summary>
        /// 딕셔너리를 인스펙터로 초기화
        /// </summary>
        public void SyncDictionaryFromInspector()
        {
            //딕셔너리 키 밸류 리스트 초기화
            foreach (var key in Keys.ToList())
            {
                base.Remove(key);
            }

            for (int i = 0; i < keys.Count; i++)
            {
                //중복된 키가 있다면 에러 출력
                if (this.ContainsKey(keys[i]))
                {
                    Debug.LogError("중복된 키가 있습니다.");
                    break;
                }
                base.Add(keys[i], values[i]);
            }
        }

        // save the dictionary to lists
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        // load dictionary from lists
        public void OnAfterDeserialize()
        {
            this.Clear();

            for (int i = 0; i < keys.Count && i < values.Count; i++)
            {
                this[keys[i]] = this.values[i];
            }
        }
    }
}



