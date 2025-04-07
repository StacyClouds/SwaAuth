using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace StacyClouds.SwaAuth.Tests.Api.Mocks;
public class MockHeaderDictionary : IHeaderDictionary
{
    private readonly Dictionary<string, StringValues> _headers = new();

    public StringValues this[string key]
    {
        get => _headers[key];
        set => _headers[key] = value;
    }

    public long? ContentLength
    {
        get => _headers.ContainsKey("Content-Length") ? long.Parse(_headers["Content-Length"]) : null;
        set
        {
            if (value.HasValue)
            {
                _headers["Content-Length"] = value.Value.ToString();
            }
            else
            {
                _headers.Remove("Content-Length");
            }
        }
    }

    public ICollection<string> Keys => _headers.Keys;

    public ICollection<StringValues> Values => _headers.Values;

    public int Count => _headers.Count;

    public bool IsReadOnly => false;

    public void Add(string key, StringValues value)
    {
        _headers.Add(key, value);
    }

    public void Add(KeyValuePair<string, StringValues> item)
    {
        _headers.Add(item.Key, item.Value);
    }

    public void Clear()
    {
        _headers.Clear();
    }

    public bool Contains(KeyValuePair<string, StringValues> item)
    {
        return _headers.ContainsKey(item.Key) && _headers[item.Key].Equals(item.Value);
    }

    public bool ContainsKey(string key)
    {
        return _headers.ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<string, StringValues>[] array, int arrayIndex)
    {
        ((IDictionary<string, StringValues>)_headers).CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<string, StringValues>> GetEnumerator()
    {
        return _headers.GetEnumerator();
    }

    public bool Remove(string key)
    {
        return _headers.Remove(key);
    }

    public bool Remove(KeyValuePair<string, StringValues> item)
    {
        return _headers.Remove(item.Key);
    }

    public bool TryGetValue(string key, out StringValues value)
    {
        return _headers.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _headers.GetEnumerator();
    }
}
