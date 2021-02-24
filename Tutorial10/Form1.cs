using System;
using System.Windows.Forms;
using System.IO;

namespace Tutorial10
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void openWaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            // This will get the current WORKING directory (i.e. \bin\Debug)
            string workingDirectory = Environment.CurrentDirectory;
            // or: Directory.GetCurrentDirectory() gives the same result

            // This will get the current PROJECT bin directory (ie ../bin/)
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;

            // This will get the current PROJECT directory
            string projectDirectory2 = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

            open.InitialDirectory = projectDirectory + "\\WAV_Files";
            open.Filter = "Wave File (*.wav)|*.wav;";

            if (open.ShowDialog() != DialogResult.OK) return;
            
            waveViewer1.SamplesPerPixel = 23;
            waveViewer1.WaveStream = new NAudio.Wave.WaveFileReader(open.FileName);

            if(chart1.Series.Count > 0)
            {
                chart1.Series.RemoveAt(0);
            }
            chart1.Series.Add("wave");
            chart1.Series["wave"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart1.Series["wave"].ChartArea = "ChartArea1";

            NAudio.Wave.WaveChannel32 wave = new NAudio.Wave.WaveChannel32(new NAudio.Wave.WaveFileReader(open.FileName));

            byte[] buffer = new byte[16384];
            int read = 0;

            while (wave.Position < wave.Length)
            {
                read = wave.Read(buffer, 0, 16384);

                for (int i = 0; i < read / 4; i++)
                {
                    chart1.Series["wave"].Points.Add(BitConverter.ToSingle(buffer, i * 4));
                }
            }
        }
    }
}
