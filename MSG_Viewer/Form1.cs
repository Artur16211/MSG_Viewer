using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSG_Viewer
{
    public partial class Form1 : Form
    {
        private string loadedFilePath; // Variable para almacenar la ruta del archivo cargado
        public Form1()
        {
            InitializeComponent();

            // El tamaño maximo de redimensionamiento horizontal es 1500
            this.MaximumSize = new Size(1500, 5000);

            // desabilitar el boton de maximizar
            this.MaximizeBox = false;


            AllowDrop = true;
            DragEnter += Form1_DragEnter;
            DragDrop += Form1_DragDrop;
            this.SizeChanged += Form1_SizeChanged;

            // Suscribir al evento Load para establecer el tamaño predeterminado
            this.Load += Form1_Load;

            // Aplicar modo oscuro a los controles
            ApplyDarkMode();
            InitializeTextBoxes();
        }

        private void InitializeTextBoxes()
        {
            // Crear los TextBox y agregarlos al FlowLayoutPanel
            removelineTextBox = new TextBox();
            removelineTextBox.BackColor = Color.FromArgb(80, 80, 80);
            removelineTextBox.ForeColor = Color.White;
            removelineTextBox.BorderStyle = BorderStyle.None;
            removelineTextBox.Width = 50;

            lastRemoveLineTextBox = new TextBox();
            lastRemoveLineTextBox.BackColor = Color.FromArgb(80, 80, 80);
            lastRemoveLineTextBox.ForeColor = Color.White;
            lastRemoveLineTextBox.BorderStyle = BorderStyle.None;
            lastRemoveLineTextBox.ReadOnly = false;
            // Asignar la fuente creada
            lastRemoveLineTextBox.Font = new Font("Arial", 16, FontStyle.Regular);

            realLastRemoveLineTextBox = new TextBox();
            realLastRemoveLineTextBox.BackColor = Color.FromArgb(80, 80, 80);
            realLastRemoveLineTextBox.ForeColor = Color.White;
            realLastRemoveLineTextBox.BorderStyle = BorderStyle.None;
            realLastRemoveLineTextBox.Width = 50;

            // Agregar los TextBox al FlowLayoutPanel
            flowLayoutPanel.Controls.Add(removelineTextBox);
            flowLayoutPanel.Controls.Add(lastRemoveLineTextBox);
            flowLayoutPanel.Controls.Add(realLastRemoveLineTextBox);

            // Establecer el ancho inicial del lastRemoveLineTextBox
            int availableWidth = flowLayoutPanel.ClientSize.Width - removelineTextBox.Width - realLastRemoveLineTextBox.Width;
            lastRemoveLineTextBox.Width = availableWidth;
            lastRemoveLineTextBox.AutoSize = false; // Deshabilitar el ajuste automático de ancho

        }

        private void TextBox_Enter(object sender, EventArgs e)
        {
            // Cambiar el color de fondo al seleccionar el TextBox
            TextBox textBox = sender as TextBox;
            textBox.BackColor = Color.FromArgb(20, 20, 20); // Color de fondo al seleccionar
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            // Cambiar el color de fondo nuevamente al perder el enfoque
            TextBox textBox = sender as TextBox;
            textBox.BackColor = Color.FromArgb(80, 80, 80); // Color de fondo original
        }


        // Cambiar el evento Form1_Load para llamar al constructor adecuado con el argumento filePath
        private void Form1_Load(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length > 1 && File.Exists(args[1]) && Path.GetExtension(args[1]).Equals(".msg", StringComparison.InvariantCultureIgnoreCase))
            {
                
                string filePath = args[1];
                this.Size = new Size(1500, 600);
                LoadFile(filePath).Wait(); // Usamos Wait para esperar a que termine la carga del archivo antes de continuar
                
            }
            else
            {
                this.Size = new Size(1500, 600);
            }
        }



        private void ApplyDarkMode()
        {
            // Color de fondo oscuro
            this.BackColor = Color.FromArgb(30, 30, 30);

            // Color de texto en modo oscuro
            this.ForeColor = Color.White;

            // Color de fondo oscuro para el FlowLayoutPanel
            flowLayoutPanel.BackColor = Color.FromArgb(40, 40, 40);

            // Color de texto en modo oscuro para el FlowLayoutPanel
            flowLayoutPanel.ForeColor = Color.White;

            // Color de fondo oscuro para los TextBox
            foreach (Control control in flowLayoutPanel.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.BackColor = Color.FromArgb(80, 80, 80);
                    textBox.ForeColor = Color.White;
                }
            }

            // Color de fondo oscuro para los botones
            btnSave.BackColor = Color.FromArgb(80, 80, 80);
            btnSave.ForeColor = Color.White;
        }


        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private async void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length > 0)
            {
                string filePath = files[0];

                // Verificar si es un archivo .msg que en realidad es un archivo de texto
                if (Path.GetExtension(filePath).Equals(".msg", StringComparison.InvariantCultureIgnoreCase))
                {
                    await LoadFile(filePath); // Tratarlo como archivo de texto
                }
                else
                {
                    await LoadFile(filePath); // Cargar y procesar archivos de texto como antes
                }
            }
        }

        // Declarar los tres TextBox como variables de clase
        private TextBox removelineTextBox;
        private TextBox lastRemoveLineTextBox;
        private TextBox realLastRemoveLineTextBox;

        private async Task LoadFile(string filePath)
        {
            Font newFont = new Font("Arial", 16, FontStyle.Regular); // Cambia "Arial" por la fuente que prefieras y 12 por el tamaño deseado.
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    loadedFilePath = filePath; // Establecer la ruta del archivo cargado
                    // Limpiar el contenedor antes de agregar nuevos controles
                    flowLayoutPanel.Controls.Clear();

                    // Mostrar el nombre del archivo
                    Label fileNameLabel = new Label();
                    fileNameLabel.Text = filePath;
                    // tendra un ancho de 500px
                    fileNameLabel.Width = 500;
                    flowLayoutPanel.Controls.Add(fileNameLabel);
                    // Agregar un salto fila
                    flowLayoutPanel.SetFlowBreak(fileNameLabel, true);


                    int columnCount = 0;
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        LineResult lineResult = ProcessLine(line);

                        // Crear los TextBox correspondientes
                        removelineTextBox = new TextBox();
                        removelineTextBox.Text = lineResult.Removeline;
                        removelineTextBox.ReadOnly = true;
                        removelineTextBox.BackColor = Color.FromArgb(80, 80, 80); // Color de fondo para el TextBox
                        removelineTextBox.ForeColor = Color.White; // Color de texto para el TextBox
                        // borde del textbox
                        removelineTextBox.BorderStyle = BorderStyle.None;
                        // sera de 500px de ancho
                        removelineTextBox.Width = 50;
                        flowLayoutPanel.Controls.Add(removelineTextBox);

                        lastRemoveLineTextBox = new TextBox();
                        lastRemoveLineTextBox.Text = lineResult.LastRemoveLine;
                        lastRemoveLineTextBox.BackColor = Color.FromArgb(80, 80, 80); // Color de fondo para el TextBox
                        lastRemoveLineTextBox.ForeColor = Color.White; // Color de texto para el TextBox
                        // borde del textbox
                        lastRemoveLineTextBox.BorderStyle = BorderStyle.None;
                        lastRemoveLineTextBox.ReadOnly = false;
                        lastRemoveLineTextBox.Font = newFont; // Asignar la fuente creada
                        
                        lastRemoveLineTextBox.Width = 1300;

                        flowLayoutPanel.Controls.Add(lastRemoveLineTextBox);

                        realLastRemoveLineTextBox = new TextBox();
                        realLastRemoveLineTextBox.Text = lineResult.RealLastRemoveLine;
                        realLastRemoveLineTextBox.ReadOnly = true;
                        realLastRemoveLineTextBox.BackColor = Color.FromArgb(80, 80, 80); // Color de fondo para el TextBox
                        realLastRemoveLineTextBox.ForeColor = Color.White; // Color de texto para el TextBox
                        // borde del textbox
                        realLastRemoveLineTextBox.BorderStyle = BorderStyle.None;
                        realLastRemoveLineTextBox.Width = 50;
                        flowLayoutPanel.Controls.Add(realLastRemoveLineTextBox);

                        // el ancho del textbox sera el ancho del contenedor menos el ancho de los otros 2 textbox
                        //lastRemoveLineTextBox.Width = flowLayoutPanel.ClientSize.Width - removelineTextBox.Width - realLastRemoveLineTextBox.Width;
                        //lastRemoveLineTextBox.AutoSize = false; // Agrega esta línea para deshabilitar el ajuste automático de ancho

                        // Asignar manejadores de eventos Enter y Leave a cada TextBox
                        removelineTextBox.Enter += TextBox_Enter;
                        removelineTextBox.Leave += TextBox_Leave;

                        lastRemoveLineTextBox.Enter += TextBox_Enter;
                        lastRemoveLineTextBox.Leave += TextBox_Leave;

                        realLastRemoveLineTextBox.Enter += TextBox_Enter;
                        realLastRemoveLineTextBox.Leave += TextBox_Leave;

                        columnCount += 3;
                        if (columnCount >= 3) // Change '3' to the desired number of columns per row
                        {
                            // Wrap to the next row
                            flowLayoutPanel.SetFlowBreak(realLastRemoveLineTextBox, true);
                            columnCount = 0;
                        }
                        // barra de desplazamiento vertical para los textbox
                        flowLayoutPanel.AutoScroll = true;
                        // los 3 texbox abarcaran el ancho del contenedor pero siguiendo estando los 3 en la misma fila
                        AdjustTextBoxWidth();

                    }
                    

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            // Ajustar el ancho del flowLayoutPanel cuando el formulario se redimensiona
            flowLayoutPanel.Width = this.ClientSize.Width - flowLayoutPanel.Margin.Left - flowLayoutPanel.Margin.Right;

            // Ajustar el alto del flowLayoutPanel cuando el formulario se redimensiona
            flowLayoutPanel.Height = this.ClientSize.Height - flowLayoutPanel.Margin.Top - flowLayoutPanel.Margin.Bottom;

            // Ajustar el ancho de los TextBox al cambiar el tamaño del formulario
            AdjustTextBoxWidth();
        }

        private void AdjustTextBoxWidth()
        {
            // Obtenemos el nuevo ancho del flowLayoutPanel
            int newWidth = flowLayoutPanel.ClientSize.Width;

            // Calculamos el ancho disponible para el lastRemoveLineTextBox
            int availableWidth = newWidth - removelineTextBox.Width - realLastRemoveLineTextBox.Width;

            // Establecemos el ancho para el lastRemoveLineTextBox
            //lastRemoveLineTextBox.Width = availableWidth;
        }




        private LineResult ProcessLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("[sel") || line.StartsWith("[msg"))
            {
                // For lines starting with "[sel" or "[msg" or empty lines, set the original line as removeline
                return new LineResult { Removeline = string.Empty, LastRemoveLine = line, RealLastRemoveLine = string.Empty };
            }
            else
            {
                StringBuilder resultLine = new StringBuilder();
                bool insideBrackets = false;
                bool skipNextBrackets = false;

                for (int i = 0; i < line.Length; i++)
                {
                    char character = line[i];

                    if (character == '[')
                    {
                        insideBrackets = true;
                        int endIndex = line.IndexOf(']', i + 1);
                        if (endIndex != -1)
                        {
                            string nextWord = line.Substring(i + 1, endIndex - i - 1).Trim();
                            if (nextWord == "var" || nextWord == "f 4 1" || nextWord == "f 4 2" || nextWord == "clr" || nextWord == "Navi")
                            {
                                skipNextBrackets = true;
                            }
                        }
                    }
                    else if (character == ']')
                    {
                        insideBrackets = false;
                        skipNextBrackets = false;
                    }
                    else if (!insideBrackets && !skipNextBrackets)
                    {
                        resultLine.Append(line.Substring(i));
                        break;
                    }
                }

                string lastRemoveLine = resultLine.ToString();
                string realLastRemoveLine = string.Empty;

                int lastNBracketIndex = lastRemoveLine.LastIndexOf("[n]");
                if (lastNBracketIndex == -1)
                {
                    // Si no se encontró [n], buscar [e]
                    lastNBracketIndex = lastRemoveLine.LastIndexOf("[e]");
                    if (lastNBracketIndex != -1)
                    {
                        lastRemoveLine = lastRemoveLine.Substring(0, lastNBracketIndex + 3); // +3 to keep the [e] in the result
                    }
                }
                else
                {
                    lastRemoveLine = lastRemoveLine.Substring(0, lastNBracketIndex + 3); // +3 to keep the [n] in the result
                }

                // delete the last [n] or [e] if it's the last character in the line
                if (lastRemoveLine.EndsWith("[n]") || lastRemoveLine.EndsWith("[e]"))
                {
                    lastRemoveLine = lastRemoveLine.Substring(0, lastRemoveLine.Length - 3);
                }
                // delete the [w] and [e]
                lastRemoveLine = lastRemoveLine.Replace("[w]", "");
                lastRemoveLine = lastRemoveLine.Replace("[e]", "");

                realLastRemoveLine = resultLine.ToString().Replace(lastRemoveLine, "");

                string removeline = line.Replace(resultLine.ToString(), "");

                Console.WriteLine("removeline: " + removeline);
                Console.WriteLine("lastRemoveLine: " + lastRemoveLine);
                Console.WriteLine("realLastRemoveLine: " + realLastRemoveLine);

                return new LineResult { Removeline = removeline, LastRemoveLine = lastRemoveLine, RealLastRemoveLine = realLastRemoveLine };
            }
        }


        // Clase para almacenar los resultados de ProcessLine
        public class LineResult
        {
            public string Removeline { get; set; }
            public string LastRemoveLine { get; set; }
            public string RealLastRemoveLine { get; set; }
        }

        private void flowLayoutPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void btnSave_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(loadedFilePath))
            {
                try
                {
                    // El contenido de los textbox se pegará de 3 en 3 para formar las líneas
                    StringBuilder sb = new StringBuilder();
                    int textBoxCount = 0;

                    // Recorremos los controles dentro del flowLayoutPanel
                    foreach (Control control in flowLayoutPanel.Controls)
                    {
                        // Solo consideramos los TextBox
                        if (control is TextBox textBox)
                        {
                            // Agregamos el texto del TextBox actual al StringBuilder
                            sb.Append(textBox.Text.Trim());

                            textBoxCount++;

                            if (textBoxCount == 3)
                            {
                                // Agregamos un salto de línea para separar las líneas en el archivo
                                sb.AppendLine();
                                // Reiniciamos el contador para el siguiente conjunto de 3 TextBox
                                textBoxCount = 0;
                            }
                            else
                            {
                                // Si no se han agregado 3 TextBox aún, agregamos espacios en blanco para separarlos en el archivo
                                sb.Append("");
                            }
                        }
                    }

                    // Guardamos el contenido del archivo usando el método SaveToFile
                    await SaveToFile(loadedFilePath, sb.ToString());
                    MessageBox.Show("Archivo guardado exitosamente.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar el archivo: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("No se ha cargado ningún archivo.");
            }
        }





        private async Task SaveToFile(string filePath, string content)
        {
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                await writer.WriteAsync(content);
            }
        }


    }
}
