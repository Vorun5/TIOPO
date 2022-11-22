using System;
using System.Collections.Generic;
using tv;

var channel = new Dictionary<ushort, string>()
{
    { 2, "NTV"},
    { 5, "Five"},
    { 30, "STS"}
};
const string defaultState = "Tv turned on: false\nSelected TV channel: 2-NTV\n";
            
var tv = new TV(channel);

Console.WriteLine(tv.Info());