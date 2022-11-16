using System;
using System.Collections.Generic;
using NUnit.Framework;
using tv;

namespace tv_tests
{
    public class TvInitializationTest
    {
        [Test]
        public void Exception_if_there_are_less_than_one_channels()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var channel = new Dictionary<ushort, string>();
                var tv = new Tv(channel);
            });
        }
        
        
        [Test]
        public void Exception_if_adding_a_channel_with_an_empty_name()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var channel = new Dictionary<ushort, string>()
                {
                    { 2, "NTV"},
                    { 5, ""},
                    { 30, "STS"},
                };
                var tv = new Tv(channel);
            });
            
            Assert.Throws<ArgumentException>(() =>
            {
                var channel = new Dictionary<ushort, string>()
                {
                    { 2, "NTV"},
                    { 30, "STS"},
                    { 23, null}
                };
                var tv = new Tv(channel);
            });
        }

        
        [Test]
        public void Checking_the_default_state_on_successful_TV_creation()
        {
            var channel = new Dictionary<ushort, string>()
            {
                { 2, "NTV"},
                { 5, "Five"},
                { 30, "STS"}
            };
            var defaultState = $"Tv turned on: False\nSelected TV channel: 2-NTV";
            
            var tv = new Tv(channel);
            
            Assert.That(defaultState, Is.EqualTo(tv.Info()).NoClip);
        }
    }
}