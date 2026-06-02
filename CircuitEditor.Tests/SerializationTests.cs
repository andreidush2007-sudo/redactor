using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace CircuitEditor.Tests
{
    public class SerializationTests
    {
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented
        };

        [Fact]
        public void saveLoad_preservesElements()
        {
            var elements = new List<CircuitElement>
            {
                new Resistor(100, 100),
                new PowerSource(200, 200),
                new Fuse(300, 300),
                new Key(400, 400)
            };

            string json = JsonConvert.SerializeObject(elements, _settings);
            var loaded = JsonConvert.DeserializeObject<List<CircuitElement>>(json, _settings);

            Assert.Equal(4, loaded.Count);
            Assert.Equal(100, loaded[0].X);
            Assert.Equal(200, loaded[1].X);
        }

        [Fact]
        public void serialization_preservesResistor()
        {
            var r = new Resistor(100, 100);
            string json = JsonConvert.SerializeObject(r, _settings);
            var loaded = JsonConvert.DeserializeObject<Resistor>(json, _settings);
            Assert.Equal(r.X, loaded.X);
            Assert.Equal(r.Y, loaded.Y);
            Assert.Equal(r.Type, loaded.Type);
        }

        [Fact]
        public void serialization_preservesRotation()
        {
            var r = new Resistor(100, 100);
            r.Rotate();
            r.Rotate();

            string json = JsonConvert.SerializeObject(r, _settings);
            var loaded = JsonConvert.DeserializeObject<Resistor>(json, _settings);
            Assert.Equal(180, loaded.Rotation);
        }
    }
}