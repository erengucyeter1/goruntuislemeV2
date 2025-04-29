using goruntuislemeV2.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace goruntuislemeV2.components
{
    internal class CutPanel : OptionsPanel, ISideEffectRemove
    {
        private int maxPoints = 4;
        private List<Point> points = new List<Point>(4);

        private Bitmap tempImage = new Bitmap(MainForm.originalImage);
        public CutPanel()
        {

            NonePanel nonePanel = new NonePanel();
            nonePanel.Apply(null, null);

            if (!MainForm.rbSetNormal.Checked)
            {
                MainForm.rbSetNormal.Checked = true;
            }



            AddPointObserverToPictureBoxes();
        }

        private void AddPointObserverToPictureBoxes()
        {
            MainForm.selectedPictureBox.MouseDown += AddPoint;
        }

        public void RemoveSideEffects()
        {
            MainForm.selectedPictureBox.MouseDown -= AddPoint;
            if(points.Count > 0)
            {
                ResetPictureBox();
                MainForm.selectedPictureBox.Image = new Bitmap(MainForm.originalImage);
            }
        }

        public void AddPoint(Object sender, MouseEventArgs e )
        {
            
                if (e.Button == MouseButtons.Left && points.Count < maxPoints)
                {
                    points.Add(new Point(e.X, e.Y));
                    Label pointer = new Label();
                    pointer.BackColor = Color.Red;
                    pointer.Location = new Point(e.X - 5, e.Y - 5);
                    pointer.Size = new Size(10, 10);
                    pointer.BorderStyle = BorderStyle.FixedSingle;
                    pointer.MouseClick += new MouseEventHandler((s, ev) =>
                    {
                        if (ev.Button == MouseButtons.Right)
                        {
                            MainForm.selectedPictureBox.Controls.Remove(pointer);
                            points.Remove(new Point(e.X, e.Y));
                            drawPoints();
                        }
                    });

                    pointer.BringToFront();
                    MainForm.selectedPictureBox.Controls.Add(pointer);

                    drawPoints();





                    MainForm.selectedPictureBox.Invalidate();
                }

          
        }

        private void drawPoints()
        {
            Bitmap bitmap = new Bitmap(tempImage); // direkt orijinalden kopya al

            if (points.Count > 1)
            {
               

                for (int i = 0; i < points.Count - 1; i++)
                {
                    drawLine(points[i], points[i + 1], bitmap);
                }

                if(points.Count == maxPoints)
                {
                    drawLine(points[points.Count - 1], points[0], bitmap); // son noktayı ilk noktayla birleştir
                }
                

            }
            else if(points.Count == 1)
            {
                
                drawLine(points[0], points[0], bitmap);
            }
        }

        

        private void drawLine(Point p1, Point p2, Bitmap  bitmap)
        {

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawLine(new Pen(Color.Red, 10), p1, p2); // sadece yeni çizgiyi çiz
            }
            MainForm.selectedPictureBox.Image = bitmap;

        }
        
        private void ResetPictureBox()
        {
            MainForm.selectedPictureBox.Controls.Clear();
            points.Clear();
            //MainForm.selectedPictureBox.Image = new Bitmap(MainForm.originalImage);
        }
        internal override void InitializeComponents()
        {
            base.InitializeComponents();

        }

        internal override  async Task<Bitmap> ApplyFilter()
        {
            if(points.Count != maxPoints)
            {
                MessageBox.Show("Lütfen 4 nokta seçin.");
                return null;
            }

            Point[] pointsArray = new Point[4];
            pointsArray = points.ToArray();


            ResetPictureBox();

            return await Task.Run(() =>
            {
                Bitmap cropped =  Filters.CutRegion(tempImage, pointsArray[0], pointsArray[1], pointsArray[2], pointsArray[3]);
                tempImage = cropped;
                return cropped;
            });
        }
    }
}
