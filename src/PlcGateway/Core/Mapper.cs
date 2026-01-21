using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace PlcGateway.Core
{
    /// <summary>
    /// 一对多双向映射器
    /// 支持 TKey -> List<TValue> 和 TValue -> TKey 的双向查找
    /// </summary>
    /// <typeparam name="TKey">键类型（一的一方）</typeparam>
    /// <typeparam name="TValue">值类型（多的一方）</typeparam>
    public class Mapper<TKey, TValue>
        where TKey : notnull
        where TValue : notnull
    {
        // 正向映射：键 -> 值列表
        private readonly ConcurrentDictionary<TKey, List<TValue>> _keyToValues = new();

        // 反向映射：值 -> 键
        private readonly ConcurrentDictionary<TValue, TKey> _valueToKey = new();

        // 锁对象，用于确保线程安全的批量操作
        private readonly object _syncRoot = new object();

        /// <summary>
        /// 添加映射关系
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>如果成功添加返回true，如果已存在返回false</returns>
        public bool Add(TKey key, TValue value)
        {
            lock (_syncRoot)
            {
                // 检查该值是否已关联到其他键
                if (_valueToKey.TryGetValue(value, out var existingKey))
                {
                    if (Equals(existingKey, key))
                    {
                        // 已存在相同的映射
                        return false;
                    }

                    // 从旧键中移除该值
                    RemoveValueFromKey(existingKey, value);
                }

                // 添加到正向映射
                var values = _keyToValues.GetOrAdd(key, _ => new List<TValue>());
                if (!values.Contains(value))
                {
                    values.Add(value);
                }

                // 添加到反向映射
                _valueToKey[value] = key;
                return true;
            }
        }

        /// <summary>
        /// 批量添加值到同一个键
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="values">值集合</param>
        /// <returns>成功添加的数量</returns>
        public int AddRange(TKey key, IEnumerable<TValue> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            lock (_syncRoot)
            {
                int addedCount = 0;
                foreach (var value in values.Distinct())
                {
                    if (Add(key, value))
                    {
                        addedCount++;
                    }
                }
                return addedCount;
            }
        }

        /// <summary>
        /// 移除指定的映射关系
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>如果成功移除返回true，如果不存在返回false</returns>
        public bool Remove(TKey key, TValue value)
        {
            lock (_syncRoot)
            {
                if (!Contains(key, value))
                {
                    return false;
                }

                // 从正向映射中移除
                if (_keyToValues.TryGetValue(key, out var values))
                {
                    values.Remove(value);

                    // 如果键没有值了，移除整个键
                    if (values.Count == 0)
                    {
                        _keyToValues.TryRemove(key, out _);
                    }
                }

                // 从反向映射中移除
                _valueToKey.TryRemove(value, out _);
                return true;
            }
        }

        /// <summary>
        /// 移除指定键及其所有关联值
        /// </summary>
        /// <param name="key">要移除的键</param>
        /// <returns>被移除的值的数量</returns>
        public int RemoveKey(TKey key)
        {
            lock (_syncRoot)
            {
                if (!_keyToValues.TryGetValue(key, out var values))
                {
                    return 0;
                }

                int removedCount = 0;
                foreach (var value in values.ToList()) // 使用 ToList 避免修改迭代的集合
                {
                    if (Remove(key, value))
                    {
                        removedCount++;
                    }
                }

                return removedCount;
            }
        }

        /// <summary>
        /// 移除指定值
        /// </summary>
        /// <param name="value">要移除的值</param>
        /// <returns>如果成功移除返回true</returns>
        public bool RemoveValue(TValue value)
        {
            lock (_syncRoot)
            {
                if (!_valueToKey.TryGetValue(value, out var key))
                {
                    return false;
                }

                return Remove(key, value);
            }
        }

        /// <summary>
        /// 获取键关联的所有值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值的只读列表，如果键不存在返回空列表</returns>
        public IReadOnlyList<TValue> GetValues(TKey key)
        {
            if (_keyToValues.TryGetValue(key, out var values))
            {
                return values.AsReadOnly();
            }

            return new List<TValue>().AsReadOnly();
        }

        /// <summary>
        /// 获取值关联的键
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="key">找到的键</param>
        /// <returns>如果找到返回true</returns>
        public bool TryGetKey(TValue value, out TKey key)
        {
            return _valueToKey.TryGetValue(value, out key);
        }

        /// <summary>
        /// 获取值关联的键
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>关联的键，如果不存在返回默认值</returns>
        public TKey GetKey(TValue value)
        {
            _valueToKey.TryGetValue(value, out var key);
            return key;
        }

        /// <summary>
        /// 检查是否包含指定的映射关系
        /// </summary>
        public bool Contains(TKey key, TValue value)
        {
            if (!_keyToValues.TryGetValue(key, out var values))
            {
                return false;
            }

            if (!values.Contains(value))
            {
                return false;
            }

            if (!_valueToKey.TryGetValue(value, out var associatedKey))
            {
                return false;
            }

            return Equals(associatedKey, key);
        }

        /// <summary>
        /// 检查是否包含指定的键
        /// </summary>
        public bool ContainsKey(TKey key)
        {
            return _keyToValues.ContainsKey(key);
        }

        /// <summary>
        /// 检查是否包含指定的值
        /// </summary>
        public bool ContainsValue(TValue value)
        {
            return _valueToKey.ContainsKey(value);
        }

        /// <summary>
        /// 获取所有键
        /// </summary>
        public IEnumerable<TKey> Keys => _keyToValues.Keys;

        /// <summary>
        /// 获取所有值
        /// </summary>
        public IEnumerable<TValue> Values => _valueToKey.Keys;

        /// <summary>
        /// 获取键值对数量
        /// </summary>
        public int Count => _valueToKey.Count;

        /// <summary>
        /// 获取不同键的数量
        /// </summary>
        public int KeyCount => _keyToValues.Count;

        /// <summary>
        /// 清空所有映射
        /// </summary>
        public void Clear()
        {
            lock (_syncRoot)
            {
                _keyToValues.Clear();
                _valueToKey.Clear();
            }
        }

        /// <summary>
        /// 更新键关联的所有值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="newValues">新值集合</param>
        public void UpdateKey(TKey key, IEnumerable<TValue> newValues)
        {
            if (newValues == null)
                throw new ArgumentNullException(nameof(newValues));

            lock (_syncRoot)
            {
                // 移除旧值
                RemoveKey(key);

                // 添加新值
                AddRange(key, newValues);
            }
        }

        /// <summary>
        /// 获取所有映射关系的只读副本
        /// </summary>
        public IReadOnlyDictionary<TKey, IReadOnlyList<TValue>> ToDictionary()
        {
            lock (_syncRoot)
            {
                return _keyToValues.ToDictionary(
                    kvp => kvp.Key,
                    kvp => (IReadOnlyList<TValue>)kvp.Value.AsReadOnly()
                );
            }
        }

        /// <summary>
        /// 获取反向映射的只读副本
        /// </summary>
        public IReadOnlyDictionary<TValue, TKey> ToReverseDictionary()
        {
            lock (_syncRoot)
            {
                return _valueToKey.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value
                );
            }
        }

        // 内部辅助方法：从键中移除值
        private void RemoveValueFromKey(TKey key, TValue value)
        {
            if (_keyToValues.TryGetValue(key, out var values))
            {
                values.Remove(value);

                if (values.Count == 0)
                {
                    _keyToValues.TryRemove(key, out _);
                }
            }
        }
    }
}
