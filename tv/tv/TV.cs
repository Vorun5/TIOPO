using System;
using System.Collections.Generic;
using System.Linq;

namespace tv
{
    public class TV
    {
        private bool _turnedOn;
        private ushort _selectedChannel;
        private readonly Dictionary<ushort, string> _channels;

        public TV(Dictionary<ushort, string> channels)
        {
            if (channels.Count <= 0)
            {
                throw new ArgumentException("Need at least one channel");
            }

            if (channels.Any(channel => string.IsNullOrEmpty(channel.Value)))
            {
                throw new ArgumentException("Channel name must not be empty");
            }
            
            _channels = channels;
            _selectedChannel = channels.First().Key;
            _turnedOn = false;
        }
        
        public bool TurnOn()
        {
            if (_turnedOn) return false;
            _turnedOn = true;
            return _turnedOn;
        }
        
        public bool TurnOff()
        {
            if (!_turnedOn) return false;
            _turnedOn = false;
            return !_turnedOn;
        }

        public bool AddChannel(KeyValuePair<ushort, string> channel)
        {
            if (!_turnedOn)
            {
                return false;
            }
            
            if (string.IsNullOrEmpty(channel.Value))
            {
                throw new ArgumentException("Channel name must not be empty");
            }
            
            return _channels.TryAdd(channel.Key, channel.Value);
        }

        public bool SelectChannel(ushort channelNumber)
        {
            
            if (!_turnedOn || !_channels.ContainsKey(channelNumber)) return false;
            _selectedChannel = channelNumber;
            return true;
        }

        public bool RemoveChannel(ushort channelNumber)
        {
            if (_selectedChannel == channelNumber)
            {
                return false;
            }
            
            return _turnedOn && _channels.Remove(channelNumber);
        }
        
        public string Info()
        {
            return $"Tv turned on: {_turnedOn}\nSelected TV channel: {_selectedChannel}-{_channels[_selectedChannel]}";
        }
        
    }
}