using Accord.Imaging.Filters;
using Accord.Imaging;
using System;
using System.Drawing;

namespace IsDrawInfinity.Scripts.Processing
{
    internal class ImageProcessing
    {
        private Bitmap _image;
        public Bitmap Image { get => _image; }

        public ImageProcessing(Bitmap image)
        {
            _image = image;
        }

        internal ImageProcessing Grayscale()
        {
            Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
            _image = filter.Apply(_image);
            return this;
        }

        internal Rectangle DetectBlob()
        {
            BlobCounter bc = new BlobCounter();
            bc.ProcessImage(_image);
            int x = int.MaxValue;
            int y = int.MaxValue;
            int width = 0;
            int height = 0;
            var blobs = bc.GetObjectsRectangles();
            foreach (var rec in blobs)
            {
                if (rec.X < x)
                    x = rec.X;
                if (rec.Y < y)
                    y = rec.Y;
                if (rec.X + rec.Width > width)
                    width = rec.X + rec.Width;
                if (rec.Y + rec.Height > height)
                    height = rec.Y + rec.Height;
            }
            width = width - x;
            height = height - y;
            return new Rectangle(x, y, width, height);
        }

        internal ImageProcessing CropBlob()
        {
            var rectangle = DetectBlob();
            Crop filter = new Crop(rectangle);
            _image = filter.Apply(_image);
            return this;
        }

        internal ImageProcessing Resize(int width, int height)
        {
            ResizeBilinear filter = new ResizeBilinear(width, height);
            _image = filter.Apply(_image);
            return this;
        }

        internal ImageProcessing Invert()
        {
            Invert invertFilter = new Invert();
            invertFilter.ApplyInPlace(_image);
            return this;
        }

        internal ImageProcessing Binarize(int threshold)
        {
            Threshold filter = new Threshold(threshold);
            filter.ApplyInPlace(_image);
            return this;
        }

        internal ImageProcessing Do(Action<Bitmap> action)
        {
            action(_image);
            return this;
        }
    }
}