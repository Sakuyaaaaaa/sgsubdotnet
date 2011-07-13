﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SGSControls
{
    public partial class FFTForm : Form
    {
        public WaveReader.WaveForm Waveform;
        public static string FFMpegPath;
        private Thread _extractThread;
        private string _filename;
        public FFTForm()
        {
            InitializeComponent();
        }
        public void ExtractWave(string filename)
        {
            _filename = filename;
            _extractThread = new Thread(WaveExtractFunction);
            _extractThread.Start();
            btnOK.Enabled = false;
        }
        private void WaveExtractFunction()
        {
            WaveReader.WaveForm.FFmpegpath = FFMpegPath;
            Waveform = WaveReader.WaveForm.ExtractWave(_filename);
            EnableOk();

        }

        private delegate void SetButton();

        private void EnableOk()
        {
            if(btnOK.InvokeRequired)
            {
                btnOK.Invoke(new SetButton(EnableOk));
            }
            else
            {
                btnOK.Enabled = true;
                labelInfo.Text = "已完成。";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
