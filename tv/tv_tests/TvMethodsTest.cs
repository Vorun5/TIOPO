using System;
using System.Collections.Generic;
using NUnit.Framework;
using tv;

namespace tv_tests
{
    public class TvMethodsTest
    {
        private TV _tv;
        private string _defaultStateTurnedOff;
        private string _defaultStateTurnedOn;
        private Dictionary<ushort, string> _channels;
        private KeyValuePair<ushort, string> _channel;
        private KeyValuePair<ushort, string> _channel2;

        [SetUp]
        public void Setup()
        {
            _channel = new KeyValuePair<ushort, string>(1, "First");
            _channel2 = new KeyValuePair<ushort, string>(12, "STS");
            _channels = new Dictionary<ushort, string>()
            {
                { _channel.Key, _channel.Value },
            };

            _defaultStateTurnedOff = $"Tv turned on: False\nSelected TV channel: {_channel.Key}-{_channel.Value}";
            _defaultStateTurnedOn = $"Tv turned on: True\nSelected TV channel: {_channel.Key}-{_channel.Value}";
            _tv = new TV(_channels);
        }

        [Test]
        public void TurnOn_should_return_true_TV_was_turned_off()
        {
            var turnedOn = _tv.TurnOn();
            
            Assert.True(turnedOn);
            Assert.That(_defaultStateTurnedOn, Is.EqualTo( _tv.Info()).NoClip);
        }
        
        [Test]
        public void TurnOn_should_return_true_on_the_TV_if_it_was_turned_off()
        {
            var turnOn =_tv.TurnOn();

            Assert.True(turnOn);
            Assert.That(_defaultStateTurnedOn, Is.EqualTo( _tv.Info()).NoClip);
        }

        [Test]
        public void TurnOn_should_return_false_on_the_TV_if_it_was_turned_on()
        {
            _tv.TurnOn();
            var turnOn = _tv.TurnOn();

            Assert.False(turnOn);
            Assert.That(_defaultStateTurnedOn, Is.EqualTo(_tv.Info()).NoClip);
        }
        
        [Test]
        public void TurnOff_should_return_true_TV_was_turned_on()
        {
            _tv.TurnOn();
            
            var turnedOff = _tv.TurnOff();
            
            Assert.True(turnedOff);
            Assert.That(_defaultStateTurnedOff, Is.EqualTo(_tv.Info()).NoClip);
        }
        
        [Test]
        public void TurnOff_should_true_on_the_TV_if_it_was_turned_on()
        {
            _tv.TurnOn();
            
            var turnedOff=  _tv.TurnOff();
            var info = _tv.Info();
            
            Assert.True(turnedOff);
            Assert.That(_defaultStateTurnedOff, Is.EqualTo(info).NoClip);
        }
        
        
        [Test]
        public void TurnOff_should_return_false_on_the_TV_if_it_was_turned_off()
        {
            var turnOff = _tv.TurnOff();

            Assert.False(turnOff);
            Assert.That(_defaultStateTurnedOff, Is.EqualTo(_tv.Info()).NoClip);
        }
        
        [Test]
        public void AddChannel_the_channel_will_not_be_added_if_the_number_is_busy()
        {
            _tv.TurnOn();

            var channelAdded = _tv.AddChannel(_channel);
            
            Assert.False(channelAdded);
        }

        [Test]
        public void AddChannel_the_channel_will_not_be_added_if_the_TV_is_turned_off()
        {
            var channelAdded = _tv.AddChannel(_channel);
            
            Assert.False(channelAdded);
        }
        
        [Test]
        public void AddChannel_there_will_be_an_exception_when_trying_to_add_a_channel_with_an_empty_name()
        {
            _tv.TurnOn();
            var channel = new KeyValuePair<ushort, string>(12, "");

            Assert.Throws<ArgumentException>(() => { _tv.AddChannel(channel); });
        }

        [Test]
        public void AddChannel_there_will_be_an_exception_when_trying_to_add_a_channel_without_a_name()
        {
            _tv.TurnOn();
            var channel = new KeyValuePair<ushort, string>(12, null);

            Assert.Throws<ArgumentException>(() => { _tv.AddChannel(channel); });
        }
        
        [Test]
        public void AddChannel_on_successful_channel_addition_should_return_true()
        {
            _tv.TurnOn();

            var channelAdded = _tv.AddChannel(_channel2);
            
            Assert.True(channelAdded);
        }

        [Test]
        public void SelectChannel_should_return_false_if_there_is_no_channel()
        {
            _tv.TurnOn();
            var channelSelected = _tv.SelectChannel(_channel2.Key);
            
            Assert.False(channelSelected);
            Assert.That(_defaultStateTurnedOn, Is.EqualTo(_tv.Info()).NoClip);
        }

        [Test]
        public void SelectChannel_should_return_false_if_the_TV_is_turned_off()
        {
            var channelSelected = _tv.SelectChannel(_channel.Key);
            
            Assert.False(channelSelected);
            Assert.That(_defaultStateTurnedOff, Is.EqualTo(_tv.Info()).NoClip);
        }
        
        [Test]
        public void SelectChannel_should_return_true_if_the_channel_is_available_and_the_TV_is_on()
        {
            _tv.TurnOn();
            
            var channelSelected = _tv.SelectChannel(_channel.Key);
            
            Assert.True(channelSelected);
            Assert.That(_defaultStateTurnedOn, Is.EqualTo(_tv.Info()).NoClip);
        }



        [Test]
        public void RemoveChannel_the_channel_should_not_be_deleted_if_the_TV_is_turned_off()
        {
            _tv.AddChannel(_channel2);
            
            var channelRemoved = _tv.RemoveChannel(_channel2.Key);
            
            Assert.False(channelRemoved);
            Assert.That(_defaultStateTurnedOff, Is.EqualTo(_tv.Info()).NoClip);
        }
        
        [Test]
        public void RemoveChannel_the_channel_should_not_be_deleted_if_it_is_not_in_the_list()
        {
            _tv.TurnOn();
            
            var channelRemoved = _tv.RemoveChannel(_channel2.Key);
            
            Assert.False(channelRemoved);
            Assert.That(_defaultStateTurnedOn, Is.EqualTo(_tv.Info()).NoClip);
        }
        
        [Test]
        public void RemoveChannel_channel_should_not_be_deleted_if_currently_selected()
        {
            _tv.TurnOn();
            
            var channelRemoved = _tv.RemoveChannel(_channel.Key);
            
            Assert.False(channelRemoved);
            Assert.That(_defaultStateTurnedOn, Is.EqualTo(_tv.Info()).NoClip);
        }

        [Test]
        public void RemoveChannel_the_channel_should_be_deleted_if_the_TV_is_on_and_not_selected()
        {
            _tv.TurnOn();
            _tv.AddChannel(_channel2);
            
            var channelRemoved = _tv.RemoveChannel(_channel2.Key);
            
            Assert.True(channelRemoved);
            Assert.That(_defaultStateTurnedOn, Is.EqualTo(_tv.Info()).NoClip);
        }
        
        // NEW
        [Test]
        public void RemoveChannel_the_old_channel_can_be_deleted()
        {
            _tv.TurnOn();
            _tv.AddChannel(_channel2);
            _tv.SelectChannel(_channel2.Key);
            
            var channelRemoved = _tv.RemoveChannel(_channel.Key);
            
            Assert.True(channelRemoved);
            Assert.That($"Tv turned on: True\nSelected TV channel: {_channel2.Key}-{_channel2.Value}", Is.EqualTo(_tv.Info()).NoClip);
        }

        // NEW
        [Test]
        public void SelectChannel_the_newly_created_channel_can_be_selected()
        {
            _tv.TurnOn();
            _tv.AddChannel(_channel2);
            
            var channelSelected = _tv.SelectChannel(_channel2.Key);
            Assert.True(channelSelected);
            Assert.That($"Tv turned on: True\nSelected TV channel: {_channel2.Key}-{_channel2.Value}", Is.EqualTo(_tv.Info()).NoClip);
        }
    }
}