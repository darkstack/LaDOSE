﻿using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace LaDOSE.DesktopApp.Utils
{
    /// <summary>
    ///     PhpSerializer Class.
    /// </summary>
    public class PhpSerializer
    {
        private readonly NumberFormatInfo nfi;

        private int pos; //for unserialize

        private Dictionary<ArrayList, bool> seenArrayLists; //for serialize (to infinte prevent loops) lol
        //types:
        // N = null
        // s = string
        // i = int
        // d = double
        // a = array (hashtable)

        private Dictionary<Hashtable, bool> seenHashtables; //for serialize (to infinte prevent loops)
        //http://www.w3.org/TR/REC-xml/#sec-line-ends

        public Encoding StringEncoding = new UTF8Encoding();

        public bool
            XMLSafe = true; //This member tells the serializer wether or not to strip carriage returns from strings when serializing and adding them back in when deserializing

        public PhpSerializer()
        {
            nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = "";
            nfi.NumberDecimalSeparator = ".";
        }

        public string Serialize(object obj)
        {
            seenArrayLists = new Dictionary<ArrayList, bool>();
            seenHashtables = new Dictionary<Hashtable, bool>();

            return Serialize(obj, new StringBuilder()).ToString();
        } //Serialize(object obj)

        private StringBuilder Serialize(object obj, StringBuilder sb)
        {
            if (obj == null) return sb.Append("N;");

            if (obj is string)
            {
                var str = (string) obj;
                if (XMLSafe)
                {
                    str = str.Replace("\r\n", "\n"); //replace \r\n with \n
                    str = str.Replace("\r", "\n"); //replace \r not followed by \n with a single \n  Should we do this?
                }

                return sb.Append("s:" + StringEncoding.GetByteCount(str) + ":\"" + str + "\";");
            }

            if (obj is bool) return sb.Append("b:" + ((bool) obj ? "1" : "0") + ";");

            if (obj is int)
            {
                var i = (int) obj;
                return sb.Append("i:" + i.ToString(nfi) + ";");
            }

            if (obj is double)
            {
                var d = (double) obj;

                return sb.Append("d:" + d.ToString(nfi) + ";");
            }

            if (obj is ArrayList)
            {
                if (seenArrayLists.ContainsKey((ArrayList) obj))
                    return sb.Append("N;"); //cycle detected
                seenArrayLists.Add((ArrayList) obj, true);

                var a = (ArrayList) obj;
                sb.Append("a:" + a.Count + ":{");
                for (var i = 0; i < a.Count; i++)
                {
                    Serialize(i, sb);
                    Serialize(a[i], sb);
                }

                sb.Append("}");
                return sb;
            }

            if (obj is Hashtable)
            {
                if (seenHashtables.ContainsKey((Hashtable) obj))
                    return sb.Append("N;"); //cycle detected
                seenHashtables.Add((Hashtable) obj, true);

                var a = (Hashtable) obj;
                sb.Append("a:" + a.Count + ":{");
                foreach (DictionaryEntry entry in a)
                {
                    Serialize(entry.Key, sb);
                    Serialize(entry.Value, sb);
                }

                sb.Append("}");
                return sb;
            }

            return sb;
        } //Serialize(object obj)

        public object Deserialize(string str)
        {
            pos = 0;
            return this.deserialize(str);
        } //Deserialize(string str)

        private object deserialize(string str)
        {
            if (str == null || str.Length <= pos)
                return new object();

            int start, end, length;
            string stLen;
            switch (str[pos])
            {
                case 'N':
                    pos += 2;
                    return null;
                case 'b':
                    char chBool;
                    chBool = str[pos + 2];
                    pos += 4;
                    return chBool == '1';
                case 'i':
                    string stInt;
                    start = str.IndexOf(":", pos) + 1;
                    end = str.IndexOf(";", start);
                    stInt = str.Substring(start, end - start);
                    pos += 3 + stInt.Length;
                    return int.Parse(stInt, nfi);
                case 'd':
                    string stDouble;
                    start = str.IndexOf(":", pos) + 1;
                    end = str.IndexOf(";", start);
                    stDouble = str.Substring(start, end - start);
                    pos += 3 + stDouble.Length;
                    return double.Parse(stDouble, nfi);
                case 's':
                    start = str.IndexOf(":", pos) + 1;
                    end = str.IndexOf(":", start);
                    stLen = str.Substring(start, end - start);
                    var bytelen = int.Parse(stLen);
                    length = bytelen;
                    //This is the byte length, not the character length - so we migth  
                    //need to shorten it before usage. This also implies bounds checking
                    if (end + 2 + length >= str.Length) length = str.Length - 2 - end;
                    var stRet = str.Substring(end + 2, length);
                    while (StringEncoding.GetByteCount(stRet) > bytelen)
                    {
                        length--;
                        stRet = str.Substring(end + 2, length);
                    }

                    pos += 6 + stLen.Length + length;
                    if (XMLSafe) stRet = stRet.Replace("\n", "\r\n");
                    return stRet;
                case 'a':
                    //if keys are ints 0 through N, returns an ArrayList, else returns Hashtable
                    start = str.IndexOf(":", pos) + 1;
                    end = str.IndexOf(":", start);
                    stLen = str.Substring(start, end - start);
                    length = int.Parse(stLen);
                    var htRet = new Hashtable(length);
                    var alRet = new ArrayList(length);
                    pos += 4 + stLen.Length; //a:Len:{
                    for (var i = 0; i < length; i++)
                    {
                        //read key
                        var key = deserialize(str);
                        //read value
                        var val = deserialize(str);

                        if (alRet != null)
                        {
                            if (key is int && (int) key == alRet.Count)
                                alRet.Add(val);
                            else
                                alRet = null;
                        }

                        htRet[key] = val;
                    }

                    pos++; //skip the }
                    if (pos < str.Length && str[pos] == ';'
                    ) //skipping our old extra array semi-colon bug (er... php's weirdness)
                        pos++;
                    if (alRet != null)
                        return alRet;
                    else
                        return htRet;
                default:
                    return "";
            } //switch
        } //unserialzie(object)	
    }
} //class PhpSerializer