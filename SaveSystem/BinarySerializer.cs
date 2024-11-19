using System;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace SaveSystem
{
    public class BinarySerializer
    {
        private delegate void DelWriter(object toWrite, byte[] bytes, ref int cursor);

        private List<DelWriter> _writers = new List<DelWriter>();
        private List<object> _objects = new List<object>();
        private int _size = 0;

        // Premitive
        public void WriteInt(int value)       => AddWriter(IntWriter,    value, 4);
        public void WriteBool(bool value)     => AddWriter(BoolWriter,   value, 1);
        public void WriteFloat(float value)   => AddWriter(FloatWriter,  value, 4);
        public void WriteString(string value)
        {
            if (value == null) value = "";
            AddWriter(StringWriter, value, 4 + value.Length);
        }

        // Generic
        public void WriteEnum<T>(T value)   where T : Enum          => AddWriter(IntWriter,    Convert.ToInt32(value), 4);
        
        // System
        public void WriteDateTime(DateTime value) => AddWriter(LongWriter, value.Ticks, 8);
        public void WriteTimeSpan(TimeSpan value) => AddWriter(LongWriter, value.Ticks, 8);
        
        // Unity
        public void WriteColor(Color value)           => AddWriter(ColorWriter,   value, 4);
        public void WriteVector2(Vector2 value)       => AddWriter(NFloatsWriter, new float[] { value.x, value.y }, 8);
        public void WriteVector3(Vector3 value)       => AddWriter(NFloatsWriter, new float[] { value.x, value.y, value.z }, 12);
        public void WriteQuaternion(Quaternion value) => AddWriter(NFloatsWriter, new float[] { value.x, value.y, value.z, value.w }, 16);

        // Premitive Arrays
        public void WriteIntArray(IList<int> value)       => AddWriter(ArrayWriter<int>,    value, 4 + value.Count * 4);
        public void WriteBoolArray(IList<bool> value)     => AddWriter(ArrayWriter<bool>,   value, 4 + value.Count);
        public void WriteFloatArray(IList<float> value)   => AddWriter(ArrayWriter<float>,  value, 4 + value.Count * 4);
        public void WriteStringArray(IList<string> value) => AddWriter(ArrayWriter<string>, value, 4 + value.Sum(x => x.Length + 4));

        public void Save(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogError("ID Is Empty");
                return;
            }
            byte[] bytes = new byte[_size];
            int cursor = 0;
            for (int index = 0; index < _writers.Count; index++)
            {
                var writer = _writers[index];
                var value = _objects[index];
                writer(value, bytes, ref cursor);
            }

            var filePath = SirHe.GetPath(id);
            File.WriteAllBytes(filePath, bytes);
        }

        private void AddWriter(DelWriter writer, object value, int size)
        {
            _writers.Add(writer);
            _objects.Add(value);
            _size += size;
        }

        private static void IntWriter(object toWrite, byte[] bytes, ref int cursor)
        {
            var value = (int)toWrite;
            bytes[cursor++] = (byte)(value         & 0xFF);
            bytes[cursor++] = (byte)((value >> 8)  & 0xFF);
            bytes[cursor++] = (byte)((value >> 16) & 0xFF);
            bytes[cursor++] = (byte)((value >> 24) & 0xFF);
        }

        private static void LongWriter(object toWrite, byte[] bytes, ref int cursor)
        {
            var value = (long)toWrite;
            bytes[cursor++] = (byte)(value         & 0xFF);         // Least significant byte
            bytes[cursor++] = (byte)((value >> 8)  & 0xFF);
            bytes[cursor++] = (byte)((value >> 16) & 0xFF);
            bytes[cursor++] = (byte)((value >> 24) & 0xFF);
            bytes[cursor++] = (byte)((value >> 32) & 0xFF);
            bytes[cursor++] = (byte)((value >> 40) & 0xFF);
            bytes[cursor++] = (byte)((value >> 48) & 0xFF);
            bytes[cursor++] = (byte)((value >> 56) & 0xFF); // Most significant byte
        }

        private static void BoolWriter(object toWrite, byte[] bytes, ref int cursor)
        {
            var value = (bool)toWrite;
            bytes[cursor++] = (byte)(value ? 1 : 0);
        }

        private static void FloatWriter(object toWrite, byte[] bytes, ref int cursor)
        {
            byte[] valueBytes = BitConverter.GetBytes((float)toWrite);
            Array.Copy(valueBytes, 0, bytes, cursor, valueBytes.Length);
            cursor += valueBytes.Length;
        }

        private static void StringWriter(object toWrite, byte[] bytes, ref int cursor)
        {
            byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes((string)toWrite);
            IntWriter(valueBytes.Length, bytes, ref cursor);
            Array.Copy(valueBytes, 0, bytes, cursor, valueBytes.Length);
            cursor += valueBytes.Length;
        }

        private static void NFloatsWriter(object toWrite, byte[] bytes, ref int cursor)
        {
            var array = (IEnumerable<float>)toWrite;
            foreach (var value in array)
            {
                FloatWriter(value, bytes, ref cursor);
            }
        }

        private static void ColorWriter(object toWrite, byte[] bytes, ref int cursor)
        {
            var color = (Color)toWrite;
            bytes[cursor++] = (byte)(Mathf.Clamp01(color.r) * 255);
            bytes[cursor++] = (byte)(Mathf.Clamp01(color.g) * 255);
            bytes[cursor++] = (byte)(Mathf.Clamp01(color.b) * 255);
            bytes[cursor++] = (byte)(Mathf.Clamp01(color.a) * 255);
        }

        private static void ArrayWriter<T>(object toWrite, byte[] bytes, ref int cursor) 
        {
            DelWriter writer;
            var type = typeof(T);
            if (type == typeof(int)) writer = IntWriter;
            else if (type == typeof(bool)) writer = BoolWriter;
            else if (type == typeof(float)) writer = FloatWriter;
            else if (type == typeof(string)) writer = StringWriter;
            else return;

            var array = (IList<T>)toWrite;
            IntWriter(array.Count, bytes, ref cursor);
            foreach (var value in array)
                writer(value, bytes, ref cursor);
        }
    }
}