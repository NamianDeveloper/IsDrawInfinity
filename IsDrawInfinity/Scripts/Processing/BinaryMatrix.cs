using IsDrawInfinity.Scripts.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsDrawInfinity.Scripts.Processing
{
    internal class BinaryMatrix
    {
        private double[,] _matrix;

        private BinaryMatrix() { }

        private BinaryMatrix(Bitmap image, float threshold)
        {
            if (image.PixelFormat != System.Drawing.Imaging.PixelFormat.Format8bppIndexed) throw new ArgumentException("Image must be 8bpIndexed format", nameof(image));
            if (threshold > 1.0 || threshold < 0.0) throw new ArgumentException("Threshold can be between 0.0 and 1.0", nameof(threshold));

            _matrix = image.CreateMatrix(threshold);
        }

        public void Flatten(Action<double[]> onValid)
        {
            if (_matrix == null) return;

            onValid(_matrix.Flatten());
        }

        public override string ToString() => _matrix.CreateString();

        public static BinaryMatrix Create(Bitmap image, float threshold) => new BinaryMatrix(image, threshold);
        public static BinaryMatrix Empty() => new BinaryMatrix();
    }
}