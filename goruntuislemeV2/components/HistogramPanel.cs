using goruntuislemeV2.utils;
using System.Drawing.Imaging;



namespace goruntuislemeV2.components
{
    internal class HistogramPanel:OptionsPanel
    {
        private NumericUpDown InputMin { get; set; } = new NumericUpDown();
        private NumericUpDown InputMax { get; set; } = new NumericUpDown();
        private NumericUpDown OutpuMin { get; set; } = new NumericUpDown();
        private NumericUpDown OutputMax { get; set; } = new NumericUpDown();

        private ComboBox processTypeComboBox { get; set; } = new ComboBox();

        private Panel InputPanel { get; set; } = new Panel();


        internal async override Task<Bitmap> ApplyFilter()
        {
            string processType = processTypeComboBox.Text;

            return await Task.Run(() =>
            {
                if (processType == "Germe")
                {
                    return Filters.HistogramGerme(MainForm.originalImage, (int)InputMin.Value, (int)InputMax.Value, (int)OutpuMin.Value, (int)OutputMax.Value);
                }
                else if (processType == "Genişletme")
                {
                    return Filters.HistogramGenisletme(MainForm.originalImage);
                }
                else
                {
                    throw new Exception("Geçersiz işlem türü");
                }
            });
        }
        private void InitNumUD(NumericUpDown nud)
        {
            nud.Minimum = 0;
            nud.Maximum = 255;
            nud.DecimalPlaces = 0;
            nud.Increment = 1;
        }

        private void DrawHistGraphicic()
        {
            Bitmap img = MainForm.originalImage;
            if (img == null)
            {
                MessageBox.Show("Lütfen bir resim seçin.");
                return;
            }

            int[] histogram = new int[256];

            // SENİN METODUN: resmi griye çevir
            Bitmap grayImg = Filters.GrayScale(img);

            BitmapData data = grayImg.LockBits(new Rectangle(0, 0, grayImg.Width, grayImg.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            int stride = data.Stride;
            int width = grayImg.Width;
            int height = grayImg.Height;
            int bytesPerPixel = 3;
            byte[] pixelData = new byte[stride * height];
            System.Runtime.InteropServices.Marshal.Copy(data.Scan0, pixelData, 0, pixelData.Length);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int i = y * stride + x * bytesPerPixel;
                    int intensity = pixelData[i]; // grayscale olduğu için R=G=B
                    histogram[intensity]++;
                }
            }

            grayImg.UnlockBits(data);

            // Histogramı çizmek için yeni bir bitmap oluştur
            int graphWidth = 600;
            int graphHeight = 600;
            int margin = 50;
            Bitmap histogramGraph = new Bitmap(graphWidth, graphHeight);
            using (Graphics g = Graphics.FromImage(histogramGraph))
            {
                g.Clear(Color.White);

                Pen axisPen = new Pen(Color.Black, 2);
                Font labelFont = new Font("Arial", 8);
                Brush labelBrush = Brushes.Black;

                // Sadece X ekseni çiz
                g.DrawLine(axisPen, margin, graphHeight - margin, graphWidth - margin, graphHeight - margin);

                // Maksimum değeri al (ölçekleme için)
                int maxCount = histogram.Max();

                // Çubukları çiz
                for (int i = 0; i < 256; i++)
                {
                    float x = margin + i * ((graphWidth - 2 * margin) / 255f);
                    float barHeight = ((histogram[i] / (float)maxCount) * (graphHeight - 2 * margin));
                    float y = graphHeight - margin - barHeight;

                    g.DrawLine(Pens.Blue, x, graphHeight - margin, x, y);
                }

                // X ekseni etiketleri (0 - 255 arası)
                for (int i = 0; i <= 255; i += 51)
                {
                    float x = margin + i * ((graphWidth - 2 * margin) / 255f);
                    g.DrawString(i.ToString(), labelFont, labelBrush, x - 10, graphHeight - margin + 5);
                }
            }

            MainForm.selectedPictureBox.Image = histogramGraph;
        }



        internal override void InitializeComponents()
        {
            Button showHistGraphBtn = new Button();
            showHistGraphBtn.Text = "Histogram Grafiği\nGöster";
            showHistGraphBtn.Location = new Point(10, 50);
            showHistGraphBtn.Size = new Size(200, 60);
            showHistGraphBtn.Click += (s, e) =>
            {
                DrawHistGraphicic();
            };
            this.Controls.Add(showHistGraphBtn);



            //
            processTypeComboBox.Items.Add("Germe");
            processTypeComboBox.Items.Add("Genişletme");
            processTypeComboBox.SelectedIndex = 0;
            processTypeComboBox.Location = new Point(10, 10);
            processTypeComboBox.Size = new Size(200, 30);
            processTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            processTypeComboBox.SelectedIndexChanged += (s, e) =>
            {
                if (processTypeComboBox.SelectedIndex == 0)
                {
                    InputPanel.Visible = true;
                }
                else
                {
                    InputPanel.Visible = false;
                }
            };

            this.Controls.Add(processTypeComboBox);

            

           

            InitNumUD(InputMin);
            InitNumUD(InputMax);
            InitNumUD(OutpuMin);
            InitNumUD(OutputMax);

            InputPanel.AutoSize = true;
            InputPanel.BorderStyle = BorderStyle.FixedSingle;
            InputPanel.Location = new Point(240, 10);

            // Sol sütun (Giriş)
            Label l1 = new Label();
            l1.Text = "Giriş Min";
            l1.Location = new Point(10, 10);
            l1.Size = new Size(100, 30);
            InputMin.Location = new Point(10, 40);
            InputMin.Size = new Size(100, 30);

            Label l2 = new Label();
            l2.Text = "Giriş Max";
            l2.Location = new Point(150, 10); 
            l2.Size = new Size(100, 30);
            InputMax.Location = new Point(150, 40);
            InputMax.Size = new Size(100, 30);

            // Sağ sütun (Çıkış)
            Label l3 = new Label();
            l3.Text = "Çıkış Min";
            l3.Location = new Point(10, 80);
            l3.Size = new Size(100, 30);
            OutpuMin.Location = new Point(10, 110);
            OutpuMin.Size = new Size(100, 30);

            Label l4 = new Label();
            l4.Text = "Çıkış Max";
            l4.Location = new Point(150, 80);
            l4.Size = new Size(100, 30);
            OutputMax.Location = new Point(150, 110);
            OutputMax.Size = new Size(100, 30);


            InputPanel.Controls.Add(l1);
            InputPanel.Controls.Add(l2);
            InputPanel.Controls.Add(l3);
            InputPanel.Controls.Add(l4);
            InputPanel.Controls.Add(InputMin);
            InputPanel.Controls.Add(InputMax);
            InputPanel.Controls.Add(OutpuMin);
            InputPanel.Controls.Add(OutputMax);

            this.Controls.Add(InputPanel);
            



        }
    }
}
