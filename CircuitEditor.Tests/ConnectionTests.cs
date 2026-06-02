using Xunit;

namespace CircuitEditor.Tests
{
    public class ConnectionTests
    {
        [Fact]
        public void connection_creation_ok()
        {
            var start = new Resistor(100, 100);
            var end = new PowerSource(200, 200);

            var conn = new Connection
            {
                StartElement = start,
                EndElement = end,
                StartElementId = start.Id,
                EndElementId = end.Id
            };

            Assert.Equal(start, conn.StartElement);
            Assert.Equal(end, conn.EndElement);
        }

        [Fact]
        public void isHit_onLine_true()
        {
            var start = new Resistor(100, 100);
            var end = new Resistor(300, 100);

            var conn = new Connection { StartElement = start, EndElement = end };
            var sp = start.GetConnectionPoint();
            int midX = (sp.X + 300) / 2;
            Assert.True(conn.IsHit(midX, sp.Y));
        }

        [Fact]
        public void isHit_far_false()
        {
            var start = new Resistor(100, 100);
            var end = new Resistor(200, 100);
            var conn = new Connection { StartElement = start, EndElement = end };
            Assert.False(conn.IsHit(50, 50));
            Assert.False(conn.IsHit(300, 300));
        }

        [Fact]
        public void isHit_null_false()
        {
            var conn = new Connection();
            Assert.False(conn.IsHit(100, 100));
        }
    }
}