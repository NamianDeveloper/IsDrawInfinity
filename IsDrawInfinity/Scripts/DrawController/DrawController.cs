using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace IsDrawInfinity.Scripts.DrawController
{
    internal class DrawController
    {
        private PictureBox _picDigit;
        private Pen _drawingPen;
        private Point lastPoint = Point.Empty;
        private bool isMouseDown = false;
        private Bitmap _bitmap;

        public Bitmap Drawing { get => _bitmap; }

        public DrawController(PictureBox picDigit)
        {
            _picDigit = picDigit;
            Clear();

            _drawingPen = new Pen(new SolidBrush(Color.Black), 6);

            _drawingPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            _drawingPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

            _picDigit.MouseDown += _picDigit_MouseDown;
            _picDigit.MouseMove += _picDigit_MouseMove;
            _picDigit.MouseUp += _picDigit_MouseUp;
        }

        public void Clear()
        {
            _bitmap = new Bitmap("blank.png");
            _picDigit.Image = _bitmap;
        }

        private void _picDigit_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            lastPoint = Point.Empty;
        }

        private void _picDigit_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown == true)
            {
                if (lastPoint != null)
                {
                    using (Graphics g = Graphics.FromImage(_picDigit.Image))
                    {
                        g.DrawLine(_drawingPen, lastPoint, e.Location);
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                    }
                    _picDigit.Invalidate();
                    lastPoint = e.Location;
                }
            }
        }

        private void _picDigit_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = e.Location;
            isMouseDown = true;
        }
    }
}