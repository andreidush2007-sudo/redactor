using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Windows.Forms;
using System.Xml.Linq;
using JsonFormatting = Newtonsoft.Json.Formatting;

namespace CircuitEditor
{
    public partial class CircuitEditorForm : Form
    {
        private List<CircuitElement> elements = new List<CircuitElement>();
        private List<Connection> connections = new List<Connection>();

        private CircuitElement selectedElement = null;
        private Connection selectedConnection = null;
        private Point dragStart;
        private bool isDragging = false;

        private bool connectionMode = false;
        private CircuitElement connectionStartElement = null;

        public CircuitEditorForm()
        {
            DoubleBuffered = true;
            SetupUI();
            SetupWindow();
        }

        private void SetupWindow()
        {
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.Sizable;
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(800, 600);
        }

        private void SetupUI()
        {
            Text = "Редактор электрических схем";
            BackColor = Color.White;

            var topToolStrip = new ToolStrip();
            topToolStrip.BackColor = SystemColors.Control;
            topToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            topToolStrip.Dock = DockStyle.Top;

            var btnConnect = new ToolStripButton("Соединить");
            var btnDelete = new ToolStripButton("Удалить");
            var btnTurn = new ToolStripButton("Повернуть");
            var btnSave = new ToolStripButton("Сохранить");
            var btnLoad = new ToolStripButton("Загрузить");
            var btnClear = new ToolStripButton("Очистить");
            var separator1 = new ToolStripSeparator();

            btnConnect.ToolTipText = "Соединить два элемента";
            btnDelete.ToolTipText = "Удалить выделенный элемент";
            btnTurn.ToolTipText = "Повернуть выделенный элемент";
            btnSave.ToolTipText = "Сохранить схему";
            btnLoad.ToolTipText = "Загрузить схему";
            btnClear.ToolTipText = "Очистить всё";

            btnConnect.Click += (s, e) => StartConnectionMode();
            btnDelete.Click += (s, e) => DeleteSelected();
            btnTurn.Click += (s, e) => RotateSelected();
            btnSave.Click += (s, e) => SaveCircuit();
            btnLoad.Click += (s, e) => LoadCircuit();
            btnClear.Click += (s, e) => ClearAll();

            topToolStrip.Items.Add(btnConnect);
            topToolStrip.Items.Add(btnDelete);
            topToolStrip.Items.Add(btnTurn);
            topToolStrip.Items.Add(separator1);
            topToolStrip.Items.Add(btnSave);
            topToolStrip.Items.Add(btnLoad);
            topToolStrip.Items.Add(btnClear);

            Controls.Add(topToolStrip);

            var leftToolStrip = new ToolStrip();
            leftToolStrip.Dock = DockStyle.Left;
            leftToolStrip.AutoSize = false;
            leftToolStrip.Width = 110;
            leftToolStrip.BackColor = Color.FromArgb(240, 240, 240);
            leftToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            leftToolStrip.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;

            var titleLabel = new ToolStripLabel("КОМПОНЕНТЫ");
            titleLabel.Font = new Font("Arial", 9, FontStyle.Bold);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.AutoSize = false;
            titleLabel.Size = new Size(90, 30);
            leftToolStrip.Items.Add(titleLabel);
            leftToolStrip.Items.Add(new ToolStripSeparator());

            var btnResistor = new ToolStripButton("Резистор");
            var btnPower = new ToolStripButton("Источник");
            var btnFuse = new ToolStripButton("Предохранитель");
            var btnKey = new ToolStripButton("Ключ");
            var btnCapasitor = new ToolStripButton("Конденсатор");
            var btnInductor = new ToolStripButton("Катушка\nиндуктивности");
            var btnDiode = new ToolStripButton("Диод");
            var btnTransistor = new ToolStripButton("Транзистор");
            var btnTransformer = new ToolStripButton("Трансформатор");
            var btnGround = new ToolStripButton("Заземление");
            var btnLamp = new ToolStripButton("Лампа\nнакаливания");
            var btnBuzz = new ToolStripButton("Звонок");

            foreach (var btn in new[] { btnResistor, btnPower, btnFuse, btnKey, btnCapasitor, btnInductor, btnDiode, btnTransistor, btnTransformer, btnGround, btnLamp, btnBuzz })
            {
                btn.AutoSize = false;
                btn.Size = new Size(100, 40);
                btn.TextAlign = ContentAlignment.MiddleCenter;
                btn.Font = new Font("Arial", 8, FontStyle.Bold);
                btn.Margin = new Padding(5, 2, 5, 2);
            }

            btnResistor.ToolTipText = "Добавить резистор(1000 Ом)";
            btnPower.ToolTipText = "Добавить источник питания(5 В)";
            btnFuse.ToolTipText = "Добавить предохранитель(5 A)";
            btnKey.ToolTipText = "Добавить ключ";
            btnCapasitor.ToolTipText = "Добавить конденсатор(100 мкФ, 16 В)";
            btnInductor.ToolTipText = "Добавить Катушку индуктивности (100 мкГн)";
            btnDiode.ToolTipText = "Добавить диод(1N4007)";
            btnTransistor.ToolTipText = "Добавить транзистор(BC547)";
            btnTransformer.ToolTipText = "Добавить трансформатор(220 В → 12 В)";
            btnGround.ToolTipText = "Добавить заземление(GND)";
            btnLamp.ToolTipText = "Добавить лампу накаливания(12 В, 5 Вт)";
            btnBuzz.ToolTipText = "Добавить звонок(5 В)";

            btnResistor.Click += (s, e) => AddElement(new Resistor(200, 100));
            btnPower.Click += (s, e) => AddElement(new PowerSource(200, 100));
            btnFuse.Click += (s, e) => AddElement(new Fuse(200, 100));
            btnKey.Click += (s, e) => AddElement(new Key(200, 100));
            btnCapasitor.Click += (s, e) => AddElement(new Capacitor(200, 100));
            btnInductor.Click += (s, e) => AddElement(new Inductor(200, 100));
            btnDiode.Click += (s, e) => AddElement(new Diode(200, 100));
            btnTransistor.Click += (s, e) => AddElement(new Transistor(200, 100));
            btnTransformer.Click += (s, e) => AddElement(new Transformer(200, 100));
            btnGround.Click += (s, e) => AddElement(new Ground(200, 100));
            btnLamp.Click += (s, e) => AddElement(new Lamp(200, 100));
            btnBuzz.Click += (s, e) => AddElement(new Buzzer(200, 100));

            leftToolStrip.Items.Add(btnResistor);
            leftToolStrip.Items.Add(btnPower);
            leftToolStrip.Items.Add(btnFuse);
            leftToolStrip.Items.Add(btnKey);
            leftToolStrip.Items.Add(btnCapasitor);
            leftToolStrip.Items.Add(btnInductor);
            leftToolStrip.Items.Add(btnDiode);
            leftToolStrip.Items.Add(btnTransistor);
            leftToolStrip.Items.Add(btnTransformer);
            leftToolStrip.Items.Add(btnGround);
            leftToolStrip.Items.Add(btnLamp);
            leftToolStrip.Items.Add(btnBuzz);

            Controls.Add(leftToolStrip);

            var statusStrip = new StatusStrip();
            var statusLabel = new ToolStripStatusLabel("Редактор готов к работе | Сетка 20px");
            statusStrip.Items.Add(statusLabel);
            Controls.Add(statusStrip);

            MouseDown += CircuitEditorForm_MouseDown;
            MouseMove += CircuitEditorForm_MouseMove;
            MouseUp += CircuitEditorForm_MouseUp;
            Paint += CircuitEditorForm_Paint;
            Resize += (s, e) => Invalidate();
        }

        private void DrawGrid(Graphics g)
        {
            int gridSize = CircuitElement.GridSize;
            int width = ClientSize.Width;
            int height = ClientSize.Height;

            using (Pen pen = new Pen(Color.LightGray, 1))
            {
                pen.DashStyle = DashStyle.Dot;

                for (int x = 0; x < width; x += gridSize)
                {
                    g.DrawLine(pen, x, 0, x, height);
                }

                for (int y = 0; y < height; y += gridSize)
                {
                    g.DrawLine(pen, 0, y, width, y);
                }
            }

            int bigGridSize = gridSize * 5;
            using (Pen pen = new Pen(Color.Gray, 1))
            {
                for (int x = 0; x < width; x += bigGridSize)
                {
                    g.DrawLine(pen, x, 0, x, height);
                }

                for (int y = 0; y < height; y += bigGridSize)
                {
                    g.DrawLine(pen, 0, y, width, y);
                }
            }
        }

        private void AddElement(CircuitElement element)
        {
            element.snapToGrid();
            elements.Add(element);
            Invalidate();
        }

        public void AddConnection(CircuitElement start, CircuitElement end)
        {
            if (start == end) return;

            if (connections.Any(c =>
                (c.StartElement == start && c.EndElement == end) ||
                (c.StartElement == end && c.EndElement == start)))
            {
                MessageBox.Show("Соединение уже существует!", "Предупреждение");
                return;
            }

            var connection = new Connection
            {
                StartElement = start,
                EndElement = end,
                StartElementId = start.Id,
                EndElementId = end.Id
            };

            connections.Add(connection);
            Invalidate();
        }

        private void CircuitEditorForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            DrawGrid(e.Graphics);

            foreach (var conn in connections)
                conn.Draw(e.Graphics);

            foreach (var el in elements)
                el.Draw(e.Graphics);

            if (selectedElement != null)
                selectedElement.DrawSelection(e.Graphics);

            if (selectedConnection != null)
                selectedConnection.DrawSelection(e.Graphics);

            if (connectionMode && connectionStartElement != null)
            {
                Point start = connectionStartElement.GetConnectionPoint();
                Point end = PointToClient(Cursor.Position);

                using (Pen pen = new Pen(Color.Gray, 2))
                {
                    pen.DashStyle = DashStyle.Dash;
                    e.Graphics.DrawLine(pen, start, end);
                }
            }
        }

        private void CircuitEditorForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                connectionMode = false;
                connectionStartElement = null;
                selectedElement = null;
                selectedConnection = null;
                Cursor = Cursors.Default;
                Text = "Редактор электрических схем";
                Invalidate();
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                selectedConnection = connections.LastOrDefault(c => c.IsHit(e.X, e.Y));

                if (selectedConnection != null)
                {
                    selectedElement = null;
                    Text = "Выделено соединение";
                    Invalidate();
                    return;
                }

                selectedElement = elements.LastOrDefault(el => el.IsHit(e.X, e.Y));

                if (selectedElement != null)
                {
                    selectedConnection = null;

                    if (connectionMode)
                    {
                        if (connectionStartElement == null)
                        {
                            connectionStartElement = selectedElement;
                            Text = "Выберите второй элемент для соединения";
                        }
                        else
                        {
                            AddConnection(connectionStartElement, selectedElement);
                            connectionMode = false;
                            connectionStartElement = null;
                            Text = "Редактор электрических схем";
                            Cursor = Cursors.Default;
                        }
                        Invalidate();
                    }
                    else
                    {
                        isDragging = true;
                        dragStart = new Point(e.X - selectedElement.X, e.Y - selectedElement.Y);
                        Text = $"Выделен: {selectedElement.Type}";
                    }
                }
                else
                {
                    selectedElement = null;
                    selectedConnection = null;
                    Text = "Редактор электрических схем";
                    Invalidate();
                }
            }
        }

        private void CircuitEditorForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && selectedElement != null)
            {
                int newX = e.X - dragStart.X;
                int newY = e.Y - dragStart.Y;

                newX = (int)Math.Round((double)newX / CircuitElement.GridSize) * CircuitElement.GridSize;
                newY = (int)Math.Round((double)newY / CircuitElement.GridSize) * CircuitElement.GridSize;

                selectedElement.X = newX;
                selectedElement.Y = newY;
                Invalidate();
            }
        }

        private void CircuitEditorForm_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        public void StartConnectionMode()
        {
            if (selectedElement != null)
            {
                connectionMode = true;
                connectionStartElement = selectedElement;
                Cursor = Cursors.Cross;
                Text = "Режим соединения: выберите второй элемент ";
            }
            else
            {
                MessageBox.Show("Сначала выделите элемент, от которого хотите провести соединение",
                    "Подсказка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void DeleteSelected()
        {
            if (selectedConnection != null)
            {
                connections.Remove(selectedConnection);
                selectedConnection = null;
                Invalidate();
            }
            else if (selectedElement != null)
            {
                connections.RemoveAll(c => c.StartElement == selectedElement || c.EndElement == selectedElement);
                elements.Remove(selectedElement);
                selectedElement = null;
                Invalidate();
            }
        }

        public void RotateSelected()
        {
            if (selectedElement != null)
            {
                selectedElement.Rotate();
                Invalidate();
            }
            else
            {
                MessageBox.Show("Сначала выделите элемент, который хотите повернуть",
                    "Подсказка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SaveCircuit()
        {
            var saveData = new SaveData();
            saveData.Elements = elements;

            foreach (var conn in connections)
            {
                var connData = new ConnectionSaveData
                {
                    StartElementId = conn.StartElement?.Id,
                    EndElementId = conn.EndElement?.Id
                };
                saveData.Connections.Add(connData);
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "JSON files (*.json)|*.json";
                sfd.Title = "Сохранить схему";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var settings = new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                        Formatting = JsonFormatting.Indented
                    };
                    string json = JsonConvert.SerializeObject(saveData, settings);
                    File.WriteAllText(sfd.FileName, json);
                    MessageBox.Show("Схема сохранена", "Успех");
                }
            }
        }

        private void LoadCircuit()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "JSON files (*.json)|*.json";
                ofd.Title = "Загрузить схему";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string json = File.ReadAllText(ofd.FileName);
                        var settings = new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Auto
                        };
                        var saveData = JsonConvert.DeserializeObject<SaveData>(json, settings);

                        if (saveData != null)
                        {
                            elements = saveData.Elements;
                            connections.Clear();

                            foreach (var connData in saveData.Connections)
                            {
                                var startElement = elements.FirstOrDefault(e => e.Id == connData.StartElementId);
                                var endElement = elements.FirstOrDefault(e => e.Id == connData.EndElementId);

                                if (startElement != null && endElement != null)
                                {
                                    var connection = new Connection
                                    {
                                        StartElement = startElement,
                                        EndElement = endElement,
                                        StartElementId = startElement.Id,
                                        EndElementId = endElement.Id
                                    };
                                    connections.Add(connection);
                                }
                            }

                            Invalidate();
                            MessageBox.Show($"Схема загружена", "Успех");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка");
                    }
                }
            }
        }

        private void ClearAll()
        {
            elements.Clear();
            connections.Clear();
            selectedElement = null;
            selectedConnection = null;
            Invalidate();
        }
    }

    [Serializable]
    public class SaveData
    {
        public List<CircuitElement> Elements { get; set; } = new List<CircuitElement>();
        public List<ConnectionSaveData> Connections { get; set; } = new List<ConnectionSaveData>();
    }

    [Serializable]
    public class ConnectionSaveData
    {
        public string StartElementId { get; set; }
        public string EndElementId { get; set; }
    }
}