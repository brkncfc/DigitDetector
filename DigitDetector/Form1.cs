using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DigitDetector
{
    public partial class Form1 : Form
    {
        public Bitmap pic = new Bitmap(28, 28);
        Bitmap Drawing = new Bitmap(28, 28);
        float[,] drawMat = new float[28, 28];
        NeuralNetwork NN;
        Graphics graph;
        int[,] colormat = new int[28, 28];
        
        public Form1()
        {
            InitializeComponent();
            graph = drawBox.CreateGraphics();
            graph.InterpolationMode = InterpolationMode.NearestNeighbor;
            NN = new NeuralNetwork(this, 784, 20, 20, 10);
            InitColor();
        }
        public void InitColor()
        {
            for (int i = 0; i < colormat.GetLength(0); i++)
            {
                for (int j = 0; j < colormat.GetLength(1); j++)
                {
                    colormat[i, j] = 255;
                }

            }
        }

        private void Clear(object sender, EventArgs e)
        {
            Drawing = new Bitmap(28, 28);
            drawMat = new float[28, 28];
            InitColor();
            drawBox.Image = Drawing;
            textBox2.Text = "?";
        }
        private void Draw(object sender, MouseEventArgs e)
        {
            int[] ClickPos = { e.Y / 20, e.X / 20 };
            if (Control.MouseButtons == MouseButtons.Left && e.X < 550 && e.Y < 550)
            {

                colormat[ClickPos[0], ClickPos[1]] -= 120;
                if (colormat[e.Y / 20, e.X / 20] < 0)
                    colormat[e.Y / 20, e.X / 20] = 0;

                drawMat[e.Y / 20, e.X / 20] += 120f;
                Drawing.SetPixel(e.X / 20, e.Y / 20, Color.FromArgb(colormat[ClickPos[0], ClickPos[1]], colormat[ClickPos[0], ClickPos[1]], colormat[ClickPos[0], ClickPos[1]]));
                graph.DrawImage(Drawing, new Rectangle(0, 0, 560, 560));

                int a = NN.GuessDrawing(drawMat);
                textBox2.Text = a > 15 ? "?" : a.ToString();

            }

        }
    }
}
