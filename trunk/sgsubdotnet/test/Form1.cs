﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SGSDatatype;

namespace test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private SubStationAlpha _sub = null;
        private SSAIndex _subindex = null;
        private void button1_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            if(dlg.ShowDialog()!=DialogResult.OK) return;
            //  _sub = SubStationAlpha.Load(@"E:\test\Haruhi_14.ass");
            _sub = SubStationAlpha.Load(dlg.FileName);
            _subindex = new SSAIndex {Subtitle = _sub};
            _subindex.CreateIndex(600);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (_subindex == null) return;
            label1.Text = _subindex.GetSubtitle(trackBar1.Value);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var filestream = new FileStream(@"E:\test\default.xml", FileMode.Open, FileAccess.Read);
            _sub = SubStationAlpha.FromXml(filestream);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _sub.EventsSection.NewLine("testNewline");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _sub.Save(@"E:\test\testsave.txt",Encoding.Unicode);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();
            if (dlg.ShowDialog() != DialogResult.OK) return;
            var stream = new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write);
            _sub.WriteXml(stream);
            stream.Flush();
        }
    }
}
