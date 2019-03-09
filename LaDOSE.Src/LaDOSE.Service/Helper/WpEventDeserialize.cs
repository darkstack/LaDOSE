using System;
using System.Collections;
using System.Collections.Generic;

namespace LaDOSE.Business.Helper
{
    public static class WpEventDeserialize
    {
        public static readonly List<String> EventManagerField =new List<string>(new []{"HR3", "HR2", "COMMENT", "BOOKING_COMMENT"});
        public static List<Reservation> Parse(string meta)
        {
            if (meta == null) return new List<Reservation>();
            PhpSerializer p = new PhpSerializer();
            var b = p.Deserialize(meta);
            Hashtable Wpbook = b as Hashtable;

            var games = new List<Reservation>();
            if (Wpbook != null)
            {
                Hashtable reg2 = Wpbook["booking"] as Hashtable;
                foreach (string reg2Key in reg2.Keys)
                {
                    if (!EventManagerField.Contains(reg2Key.ToUpperInvariant()))
                        games.Add(new Reservation()
                        {
                            Name = reg2Key,
                            Valid = (string)reg2[reg2Key] == "1"
                        });
                }

                return games;
            }

            return null;
        }
    }
}