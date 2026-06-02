using Xunit;

namespace CircuitEditor.Tests
{
    public class CircuitElementTests
    {
        [Fact]
        public void resistor_creation_ok()
        {
            var r = new Resistor(100, 200);
            Assert.Equal(100, r.X);
            Assert.Equal(200, r.Y);
            Assert.Equal("Resistor", r.Type);
        }

        [Fact]
        public void powerSource_creation_ok()
        {
            var p = new PowerSource(50, 60);
            Assert.Equal(50, p.X);
            Assert.Equal(60, p.Y);
            Assert.Equal("PowerSource", p.Type);
        }

        [Fact]
        public void fuse_creation_ok()
        {
            var f = new Fuse(30, 40);
            Assert.Equal(30, f.X);
            Assert.Equal(40, f.Y);
            Assert.Equal("Fuse", f.Type);
        }

        [Fact]
        public void key_creation_ok()
        {
            var k = new Key(10, 20);
            Assert.Equal(10, k.X);
            Assert.Equal(20, k.Y);
            Assert.Equal("Key", k.Type);
        }

        [Fact]
        public void capacitor_creation_ok()
        {
            var c = new Capacitor(100, 100);
            Assert.Equal("Capacitor", c.Type);
        }

        [Fact]
        public void inductor_creation_ok()
        {
            var i = new Inductor(100, 100);
            Assert.Equal("Inductor", i.Type);
        }

        [Fact]
        public void diode_creation_ok()
        {
            var d = new Diode(100, 100);
            Assert.Equal("Diode", d.Type);
        }

        [Fact]
        public void transistor_creation_ok()
        {
            var t = new Transistor(100, 100);
            Assert.Equal("Transistor", t.Type);
        }

        [Fact]
        public void transformer_creation_ok()
        {
            var t = new Transformer(100, 100);
            Assert.Equal("Transformer", t.Type);
        }

        [Fact]
        public void ground_creation_ok()
        {
            var g = new Ground(100, 100);
            Assert.Equal("Ground", g.Type);
        }

        [Fact]
        public void lamp_creation_ok()
        {
            var l = new Lamp(100, 100);
            Assert.Equal("Lamp", l.Type);
        }

        [Fact]
        public void buzzer_creation_ok()
        {
            var b = new Buzzer(100, 100);
            Assert.Equal("Buzzer", b.Type);
        }

        [Fact]
        public void snapToGrid_works()
        {
            var r = new Resistor(123, 247);
            r.snapToGrid();
            Assert.Equal(120, r.X);
            Assert.Equal(240, r.Y);
        }

        [Fact]
        public void rotate_works()
        {
            var r = new Resistor(100, 100);
            r.Rotate(); Assert.Equal(90, r.Rotation);
            r.Rotate(); Assert.Equal(180, r.Rotation);
            r.Rotate(); Assert.Equal(270, r.Rotation);
            r.Rotate(); Assert.Equal(0, r.Rotation);
        }

        [Fact]
        public void rotate_swapsWH()
        {
            var r = new Resistor(100, 100);
            int w = r.Width, h = r.Height;
            r.Rotate();
            Assert.Equal(h, r.Width);
            Assert.Equal(w, r.Height);
        }

        [Fact]
        public void isHit_true()
        {
            var r = new Resistor(100, 100);
            r.Width = 80; r.Height = 60;
            Assert.True(r.IsHit(120, 120));
            Assert.True(r.IsHit(100, 100));
        }

        [Fact]
        public void isHit_false()
        {
            var r = new Resistor(100, 100);
            r.Width = 80; r.Height = 60;
            Assert.False(r.IsHit(50, 50));
            Assert.False(r.IsHit(200, 200));
        }

        [Fact]
        public void connectionPoint_center()
        {
            var r = new Resistor(100, 100);
            r.Width = 80; r.Height = 60;
            var p = r.GetConnectionPoint();
            Assert.Equal(140, p.X);
            Assert.Equal(130, p.Y);
        }
    }
}