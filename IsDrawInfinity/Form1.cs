using IsDrawInfinity.Scripts.DrawController;
using IsDrawInfinity.Scripts.Processing;
using IsDrawInfinity.Scripts.Common;
using IsDrawInfinity.Scripts.Data;
using System;
using System.Windows.Forms;
using Accord.Math;
using System.IO;

namespace IsDrawInfinity
{
    public partial class Form1 : Form
    {
        private DrawController _drawController;
        private DataSet _dataSet = new DataSet();

        public new int Scale { get; set; } = 50;
        private BinaryMatrix _currentBinaryMatrix = BinaryMatrix.Empty();
        private const int BINARYZE_THRESHOLD = 100;

        public Form1()
        {
            InitializeComponent();

            _dataSet.LoadFile();

            _drawController = new DrawController(picDigit);
        }

        private void ResultButton_Click(object sender, EventArgs e)
        {
            if (!_drawController.CanLoadResult)
                return;
            _currentBinaryMatrix = BinaryMatrix.Create(new ImageProcessing(_drawController.Drawing)
               .Grayscale()
               .Binarize(BINARYZE_THRESHOLD)
               .Invert()
               .CropBlob()
               .Resize(Scale, Scale)
               .Binarize(BINARYZE_THRESHOLD)
               .Image,
               0.5f);

            _currentBinaryMatrix.Flatten(vector => ResultLabel.Text = (_dataSet.Predict(vector, Distance.Euclidean) == "0") ? "Это знак бесконечности!" : "Это не знак бесконечности");
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            _drawController.Clear();
            _drawController.CanLoadResult = false;
        }
    }
}
