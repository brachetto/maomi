﻿// <copyright file="ReadJsonHelper.cs" company="Maomi">
// Copyright (c) Maomi. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/whuanle/maomi
// </copyright>

using System.Buffers;
using System.Text.Json;

namespace Maomi.I18n;

/// <summary>
/// JSON 读取帮助类.
/// </summary>
public static class ReadJsonHelper
{
    /// <summary>
    /// 读取 json 字节流.
    /// </summary>
    /// <param name="sequence"></param>
    /// <param name="jsonReaderOptions"></param>
    /// <returns>字典集合.</returns>
    public static Dictionary<string, object> Read(ReadOnlySequence<byte> sequence, JsonReaderOptions jsonReaderOptions)
    {
        var reader = new Utf8JsonReader(sequence, jsonReaderOptions);
        var map = new Dictionary<string, object>();
        BuildJsonField(ref reader, map, null);
        return map;
    }

    // 解析 json 对象
    private static void BuildJsonField(ref Utf8JsonReader reader, Dictionary<string, object> map, string? baseKey)
    {
        while (reader.Read())
        {
            // 顶级数组 "[123,123]"
            if (reader.TokenType is JsonTokenType.StartArray)
            {
                ParseArray(ref reader, map, baseKey);
            }
            else if (reader.TokenType is JsonTokenType.EndObject)
            {
                break;
            }
            else if (reader.TokenType is JsonTokenType.PropertyName)
            {
                var key = reader.GetString()!;
                var newkey = baseKey is null ? key : $"{baseKey}:{key}";

                reader.Read();
                if (reader.TokenType is JsonTokenType.StartArray)
                {
                    ParseArray(ref reader, map, newkey);
                }
                else if (reader.TokenType is JsonTokenType.StartObject)
                {
                    BuildJsonField(ref reader, map, newkey);
                }
                else
                {
                    map[newkey] = ReadObject(ref reader) ?? string.Empty;
                }
            }
        }
    }

    // 解析数组
    private static void ParseArray(ref Utf8JsonReader reader, Dictionary<string, object> map, string? baseKey)
    {
        int i = 0;
        while (reader.Read())
        {
            if (reader.TokenType is JsonTokenType.EndArray)
            {
                break;
            }

            var newkey = baseKey is null ? $"[{i}]" : $"{baseKey}[{i}]";
            i++;

            switch (reader.TokenType)
            {
                // [...,null,...]
                case JsonTokenType.Null:
                    map[newkey] = default!;
                    break;

                // [...,123.666,...]
                case JsonTokenType.Number:
                    map[newkey] = reader.GetDouble();
                    break;

                // [...,"123",...]
                case JsonTokenType.String:
                    map[newkey] = reader.GetString()!;
                    break;

                // [...,true,...]
                case JsonTokenType.True:
                    map[newkey] = reader.GetBoolean();
                    break;

                case JsonTokenType.False:
                    map[newkey] = reader.GetBoolean();
                    break;

                // [...,{...},...]
                case JsonTokenType.StartObject:
                    BuildJsonField(ref reader, map, newkey);
                    break;

                // [...,[],...]
                case JsonTokenType.StartArray:
                    ParseArray(ref reader, map, newkey);
                    break;
                default:
                    map[newkey] = JsonValueKind.Null;
                    break;
            }
        }
    }

    // 读取字段值
    private static object? ReadObject(ref Utf8JsonReader reader)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null or JsonTokenType.None:
                return null;
            case JsonTokenType.False:
                return reader.GetBoolean();
            case JsonTokenType.True:
                return reader.GetBoolean();
            case JsonTokenType.Number:
                return reader.GetDouble();
            case JsonTokenType.String:
                return reader.GetString() ?? string.Empty;
            default: return null;
        }
    }
}