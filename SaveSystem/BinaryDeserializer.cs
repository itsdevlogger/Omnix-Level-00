using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace SaveSystem
{
    public class BinaryDeserializer
    {
        private int _cursor;
        private byte[] _bytes;

        public BinaryDeserializer(string id)
        {
            var filePath = SirHe.GetPath(id);
            if (File.Exists(filePath))
            {
                _bytes = File.ReadAllBytes(filePath);
                _cursor = 0;
            }
            else
            {
                _bytes = new byte[0];
                _cursor = -1;
            }
        }

        // Premitive
        public int ReadInt(int defaultTo = default)
        {
            if (_cursor < 0) return defaultTo;
            return (_bytes[_cursor++]) 
                 | (_bytes[_cursor++] << 8) 
                 | (_bytes[_cursor++] << 16) 
                 | (_bytes[_cursor++] << 24);
        }

        public bool ReadBool(bool defaultTo = default)
        {
            if (_cursor < 0) return defaultTo;
            return _bytes[_cursor++] == 1;
        }

        public float ReadFloat(float defaultTo = default)
        {
            if (_cursor < 0) return defaultTo;
            float value = BitConverter.ToSingle(_bytes, _cursor);
            _cursor += 4;
            return value;
        }

        public string ReadString(string defaultTo = default)
        {
            if (_cursor < 0) return defaultTo;
            int length = ReadInt();
            string value = System.Text.Encoding.UTF8.GetString(_bytes, _cursor, length);
            _cursor += length;
            return value;
        }

        // Generic
        public T ReadEnum<T>(T defaultTo = default)
        {
            if (_cursor < 0) return defaultTo;
            return (T)Enum.ToObject(typeof(T), ReadInt());
        }

        // System
        public DateTime ReadDateTime(DateTime defaultTo = default)
        {
            if (_cursor < 0) return defaultTo;
            return new DateTime(ReadLong());
        }

        public TimeSpan ReadTimeSpan(TimeSpan defaultTo = default)
        {
            if (_cursor < 0) return defaultTo;
            return new TimeSpan(ReadLong());
        }

        // Unity
        private float ByteToFloat             => _bytes[_cursor++] / 255f;
        public Color ReadColor(Color defaultTo = default)
        {
            if (_cursor < 0) return defaultTo;
            return new Color(ByteToFloat, ByteToFloat, ByteToFloat, ByteToFloat);
        }

        public Vector2 ReadVector2(Vector2 defaultTo = default)
        {
            if (_cursor < 0) return defaultTo;
            return new Vector2(ReadFloat(), ReadFloat());
        }

        public Vector3 ReadVector3(Vector3 defaultTo = default)
        {
            if (_cursor < 0) return defaultTo;
            return new Vector3(ReadFloat(), ReadFloat(), ReadFloat());
        }

        public Quaternion ReadQuaternion(Quaternion defaultTo = default)
        {
            if (_cursor < 0) return defaultTo;
            return new Quaternion(ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat());
        }

        // Premitive Arrays
        public int[]        ReadIntArray(int[] defaultTo = default)    => ArrayReader(defaultTo, ReadInt);
        public bool[]       ReadBoolArray(bool[] defaultTo = default)   => ArrayReader(defaultTo, ReadBool);
        public float[]      ReadFloatArray(float[] defaultTo = default)  => ArrayReader(defaultTo, ReadFloat);
        public string[]     ReadStringArray(string[] defaultTo = default) => ArrayReader(defaultTo, ReadString);

        // Premitive Lists
        public List<int>    ReadIntList(List<int> defaultTo = default)     => ListReader(defaultTo, ReadInt);
        public List<bool>   ReadBoolList(List<bool> defaultTo = default)    => ListReader(defaultTo, ReadBool);
        public List<float>  ReadFloatList(List<float> defaultTo = default)   => ListReader(defaultTo, ReadFloat);
        public List<string> ReadStringList(List<string> defaultTo = default)  => ListReader(defaultTo, ReadString);


        private long ReadLong()
        {
            long l = ((long)_bytes[_cursor++])
                   | ((long)_bytes[_cursor++] << 8)
                   | ((long)_bytes[_cursor++] << 16)
                   | ((long)_bytes[_cursor++] << 24)
                   | ((long)_bytes[_cursor++] << 32)
                   | ((long)_bytes[_cursor++] << 40)
                   | ((long)_bytes[_cursor++] << 48)
                   | ((long)_bytes[_cursor++] << 56);
            
            return l;
        }

        private T[] ArrayReader<T>(T[] defaultTo, Func<T, T> reader)
        {
            if (_cursor < 0) return defaultTo;

            int count = ReadInt();
            T[] array = new T[count];
            var def = default(T);
            for (int i = 0; i < count; i++)
                array[i] = reader(def);
            return array;
        }

        private List<T> ListReader<T>(List<T> defaultTo, Func<T, T> reader)
        {
            if (_cursor < 0) return defaultTo;

            int count = ReadInt();
            var list = new List<T>(count);
            var def = default(T);
            for (int i = 0; i < count; i++)
                list.Add(reader(def));
            return list;
        }
    }
}