using System;
using System.Collections.Generic;
using NUnit.Framework;
using tv;

namespace tv_tests
{
    public class TvMethodsTest
    {
        private Tv _tv;
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
            _tv = new Tv(_channels);
        }

        [Test]
        public void TurnOn_should_turn_on_the_TV_and_return_true_if_it_is_on_and_false_otherwise()
        {
            Assert.True(_tv.TurnOn());
            Assert.That(_defaultStateTurnedOn, Is.EqualTo(_tv.Info()).NoClip);
            Assert.False(_tv.TurnOn());
            Assert.That(_defaultStateTurnedOn, Is.EqualTo(_tv.Info()).NoClip);
        }

        [Test]
        public void TurnOff_should_turn_off_the_TV_and_return_true_if_it_is_off_and_false_otherwise()
        {
            Assert.False(_tv.TurnOff());
            Assert.That(_defaultStateTurnedOff, Is.EqualTo(_tv.Info()).NoClip);
            Assert.True(_tv.TurnOn());
            Assert.That(_defaultStateTurnedOn, Is.EqualTo(_tv.Info()).NoClip);
            Assert.True(_tv.TurnOff());
            Assert.That(_defaultStateTurnedOff, Is.EqualTo(_tv.Info()).NoClip);
        }

        [Test]
        public void AddChannel_the_channel_will_not_be_added_if_the_number_is_busy_or_the_TV_is_turned_off()
        {
            Assert.False(_tv.AddChannel(_channel));
            Assert.True(_tv.TurnOn());
            Assert.False(_tv.AddChannel(_channel));
        }

        [Test]
        public void AddChannel_there_will_be_an_exception_when_trying_to_add_a_channel_with_an_empty_name()
        {
            var channel = new KeyValuePair<ushort, string>(12, "");

            var channel2 = new KeyValuePair<ushort, string>(12, null);

            
            Assert.True(_tv.TurnOn());
            Assert.That(_defaultStateTurnedOn, Is.EqualTo(_tv.Info()).NoClip);
            Assert.Throws<ArgumentException>(() => { _tv.AddChannel(channel); });
            Assert.Throws<ArgumentException>(() => { _tv.AddChannel(channel2); });
        }

        [Test]
        public void AddChannel_the_channel_will_be_added_if_the_number_is_not_busy_and_the_name_is_not_empty()
        {
            Assert.True(_tv.TurnOn());
            Assert.True(_tv.AddChannel(_channel2));
        }

        [Test]
        public void SelectChannel_should_return_false_if_there_is_no_channel_or_the_TV_is_turned_off()
        {
            Assert.False(_tv.SelectChannel(_channel2.Key));
            Assert.True(_tv.TurnOn());
            Assert.False(_tv.SelectChannel(_channel2.Key));
        }

        [Test]
        public void SelectChannel_should_return_true_if_there_is_no_channel_or_the_TV_is_turned_off()
        {
            Assert.False(_tv.SelectChannel(_channel2.Key));
            Assert.True(_tv.TurnOn());
            Assert.False(_tv.SelectChannel(_channel2.Key));
        }

        [Test]
        public void SelectChannel_should_return_true_if_the_channel_is_available()
        {
            Assert.True(_tv.TurnOn());
            Assert.True(_tv.SelectChannel(_channel.Key));
            Assert.True(_tv.AddChannel(_channel2));
            Assert.True(_tv.SelectChannel(_channel2.Key));
            Assert.That($"Tv turned on: True\nSelected TV channel: {_channel2.Key}-{_channel2.Value}",
                Is.EqualTo(_tv.Info()).NoClip);
        }

        [Test]
        public void RemoveChannel_the_channel_should_not_be_deleted_if_the_TV_is_turned_off_or_it_is_not_in_the_list()
        {
            Assert.False(_tv.RemoveChannel(_channel.Key));
            Assert.True(_tv.TurnOn());
            Assert.False(_tv.SelectChannel(_channel2.Key));
        }

        [Test]
        public void RemoveChannel_channel_should_not_be_deleted_if_currently_selected()
        {
            Assert.True(_tv.TurnOn());
            Assert.False(_tv.RemoveChannel(_channel.Key));
            Assert.True(_tv.AddChannel(_channel2));
            Assert.False(_tv.RemoveChannel(_channel.Key));
            Assert.True(_tv.SelectChannel(_channel2.Key));
            Assert.False(_tv.RemoveChannel(_channel2.Key));
        }

        [Test]
        public void RemoveChannel_the_channel_should_be_deleted_if_the_TV_is_on_and_not_selected()
        {
            Assert.True(_tv.TurnOn());
            Assert.True(_tv.AddChannel(_channel2));
            Assert.True(_tv.SelectChannel(_channel2.Key));
            Assert.True(_tv.RemoveChannel(_channel.Key));
            Assert.True(_tv.AddChannel(_channel));
            Assert.True(_tv.SelectChannel(_channel.Key));
            Assert.True(_tv.RemoveChannel(_channel2.Key));
        }
    }
}