﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Config;

namespace sgsubdotnet
{
    public partial class SGSMainForm : Form
    {
        private string m_Appdata;
        public SGSMainForm()
        {
            InitializeComponent();
            m_Appdata 
                = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) 
                + "\\" + Application.CompanyName;
            if (!System.IO.Directory.Exists(m_Appdata))
            {
                System.IO.Directory.CreateDirectory(m_Appdata);
            }
            if (!System.IO.Directory.Exists(m_Appdata+@"\config"))
            {
                System.IO.Directory.CreateDirectory(m_Appdata + @"\config");
            }
            if (!System.IO.File.Exists(m_Appdata + @"\config\sgscfg.xml"))
            {
                m_Config = SGSConfig.FromFile(Application.StartupPath + @"\config\sgscfg.xml");
                m_Config.Save(m_Appdata + @"\config\sgscfg.xml");
            }
            else
            {
                m_Config = SGSConfig.FromFile(m_Appdata + @"\config\sgscfg.xml");
            }

            WaveReader.WaveForm.FFmpegpath = Application.StartupPath + @"\ffmpeg.exe";
            
            SetDefaultValues();

            DataGridViewColumn column;

            subtitleGrid.AutoGenerateColumns = false;
            subtitleGrid.AutoSize = false;

            subtitleGrid.DataSource = m_CurrentSub.SubItems;

            column = new DataGridViewTextBoxColumn();
            column.HeaderText = "Begin Time";
            column.DataPropertyName = "StartTime";
            subtitleGrid.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.HeaderText = "End Time";
            column.DataPropertyName = "EndTime";
            subtitleGrid.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.HeaderText = "Text";
            column.DataPropertyName = "Text";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            subtitleGrid.Columns.Add(column);

            m_selectCells.Rows = subtitleGrid.Rows;
        }

        /// <summary>
        /// 把与AssSub有关的参数从m_Config中搬到m_CurrentSub中
        /// </summary>
        private void SetDefaultValues()
        {
            m_CurrentSub.DefaultAssHead = m_Config.DefaultAssHead;
            m_CurrentSub.DefaultFormatLine = m_Config.DefaultFormatLine;
            m_CurrentSub.DefaultFormat = m_Config.DefaultFormat;
            m_CurrentSub.DefaultLayer = m_Config.DefaultLayer;
            m_CurrentSub.DefaultMarked = m_Config.DefaultMarked;
            m_CurrentSub.DefaultStart = m_Config.DefaultStart;
            m_CurrentSub.DefaultEnd = m_Config.DefaultEnd;
            m_CurrentSub.DefaultStyle = m_Config.DefaultStyle;
            m_CurrentSub.DefaultName = m_Config.DefaultName;
            m_CurrentSub.DefaultActor = m_Config.DefaultActor;
            m_CurrentSub.DefaultMarginL = m_Config.DefaultMarginL;
            m_CurrentSub.DefaultMarginR = m_Config.DefaultMarginR;
            m_CurrentSub.DefaultMarginV = m_Config.DefaultMarginV;
            m_CurrentSub.DefaultEffect = m_Config.DefaultEffect;
        }

        /// <summary>
        /// 字幕
        /// </summary>
        private Subtitle.AssSub m_CurrentSub = new Subtitle.AssSub();

        /// <summary>
        /// 字幕的时间索引己生成
        /// </summary>
        private bool m_TrackLoaded = false;

        /// <summary>
        /// 视频文件己打开
        /// </summary>
        private bool m_VideoOpened = false;

        /// <summary>
        /// 己正常开始播放视频
        /// </summary>
        private bool m_VideoPlaying = false;

        /// <summary>
        /// 字幕己读取
        /// </summary>
        private bool m_SubLoaded = false;

        /// <summary>
        /// 暂停
        /// </summary>
        private bool m_Paused = false;

        /// <summary>
        /// 字幕文件名
        /// </summary>
        private string m_AssFilename = null;

        /// <summary>
        /// 字幕被修改
        /// </summary>
        private bool m_Edited = false;

        private void timer_Tick(object sender, EventArgs e)
        {
            if (m_VideoOpened)
            {
             
                waveScope.CurrentPosition = dxVideoPlayer.CurrentPosition;
                waveScope.Redraw();
                //显示字幕内容
                if (m_TrackLoaded)
                    subLabel.Text = m_CurrentSub.GetSubtitle(dxVideoPlayer.CurrentPosition);
            }
        }


        /// <summary>
        /// 如果字幕己修改，询问是否保存
        /// </summary>
        /// <returns>true:正常, false:取消操作</returns>
        private bool AskSave()
        {
            if (m_Edited && m_SubLoaded)
            {
                DialogResult result = MessageBox.Show("当前字幕己修改" + Environment.NewLine + "想保存文件吗",
                    "SGSUB.Net", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                switch (result)
                {
                    case DialogResult.Yes:
                        return SaveAssSub();
                    case DialogResult.No:
                        return true;
                    case DialogResult.Cancel:
                        return false;
                    default:
                        return false;
                }
            }
            return true;
        }
        
        private void OpenVideo_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Video File (*.mp4;*.mkv;*.avi;*.mpg)|*.mp4;*.mkv;*.avi;*.mpg|All files (*.*)|*.*||";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                OpenVideo(dlg.FileName);
            }
        }
        /// <summary>
        /// 打开视频
        /// </summary>
        /// <param name="file"></param>
        private void OpenVideo(string filename)
        {
            waveScope.Wave = null;
            try
            {
                dxVideoPlayer.OpenVideo(filename);
                //生成字幕的时间索引
                m_TrackLoaded = false;
                if (m_SubLoaded)
                {
                    m_CurrentSub.CreateIndex(dxVideoPlayer.Duration);
                    m_TrackLoaded = true;
                }
                dxVideoPlayer.Play();
                m_VideoOpened = true;
                m_VideoPlaying = true;
                m_Paused = false;
                timer.Start();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        /// 打开ass文件按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenSub_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "ASS Subtitle (*.ass)|*.ass||";
            if (AskSave() && dlg.ShowDialog() == DialogResult.OK)
            {
                OpenAss(dlg.FileName);
            }
        }

        /// <summary>
        /// 打开TXT文件按钮。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenTxt_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Text File (*.txt)|*.txt||";

            if (AskSave() && dlg.ShowDialog() == DialogResult.OK)
            {
                OpenTxt(dlg.FileName);
            }
        }

        /// <summary>
        /// 保存字幕文件，如果之前未保存过，提示用户输入文件名
        /// </summary>
        /// <returns>是否保存 true为保存 false为cancel</returns>
        private bool SaveAssSub()
        {
            if (m_AssFilename == null)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.AddExtension = true;
                dlg.DefaultExt = "ass";
                dlg.Filter = "ASS Subtitle (*.ass)|*.ass||";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    m_AssFilename = dlg.FileName;
                }
                else
                {
                    return false;
                }
            }
            m_CurrentSub.WriteAss(m_AssFilename, Encoding.Unicode);
            m_Edited = false;
            return true;
        }

        /// <summary>
        /// 打开ASS文件
        /// </summary>
        /// <param name="filename"></param>
        private void OpenAss(string filename)
        {
            m_CurrentSub.LoadAss(filename);
            m_SubLoaded = true;
            m_AssFilename = filename;
            m_Edited = false;
            if (m_VideoOpened)
            {
                m_CurrentSub.CreateIndex(dxVideoPlayer.Duration);
                m_TrackLoaded = true;
            }
            m_undoRec.Reset();
            m_selectCells.Reset();
        }

        /// <summary>
        /// 打开TXT文件
        /// </summary>
        /// <param name="filename"></param>
        private void OpenTxt(string filename)
        {
            m_CurrentSub.LoadText(filename);
            m_SubLoaded = true;
            m_AssFilename = null;
            m_Edited = false;
            if (m_VideoOpened)
            {
                m_CurrentSub.CreateIndex(dxVideoPlayer.Duration);
                m_TrackLoaded = true;
            }
            m_undoRec.Reset();
            m_selectCells.Reset();
        }


        private void SaveSub_Click(object sender, EventArgs e)
        {

            if (m_SubLoaded)
            {
                SaveAssSub();
            }
        }

        private double oldS = 0, oldE = 0;
        private string oldString;
        private void subtitleGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            oldString = subtitleGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            if (m_VideoPlaying)
            {
                m_Paused = true;
                dxVideoPlayer.Pause();
            }
            if (e.ColumnIndex != 2)
            {
                Subtitle.AssItem item = (Subtitle.AssItem)subtitleGrid.Rows[e.RowIndex].DataBoundItem;
                oldS = item.Start.TimeValue;
                oldE = item.End.TimeValue;
            }

        }

        private void subtitleGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string newString = subtitleGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            if (!oldString.Equals(newString))
            {
                m_undoRec.Edit(e.RowIndex, e.ColumnIndex, oldString);//比较单元格内容，如改变，记录undo
            }

            if (e.ColumnIndex != 2)
            {
                Subtitle.AssItem item = (Subtitle.AssItem)subtitleGrid.Rows[e.RowIndex].DataBoundItem;
                m_CurrentSub.ItemEdited(item, oldS, oldE);
            }
            m_Edited = true;
        }

        /// <summary>
        /// 插入开始时间，并移动当前单元格
        /// </summary>
        private void addStartTime()
        {
            if (subtitleGrid.CurrentRow != null)
            {
                
                int rowindex = subtitleGrid.CurrentRow.Index;
                if (m_VideoPlaying && m_TrackLoaded)
                {
                    double time = dxVideoPlayer.CurrentPosition + m_Config.StartOffset;
                    addStartTimeToRow(rowindex, dxVideoPlayer.CurrentPosition + m_Config.StartOffset);
                    subtitleGrid.CurrentCell = subtitleGrid.CurrentRow.Cells[1];
                    if (m_Config.AutoOverlapCorrection && rowindex > 0)
                    {
                        Subtitle.AssItem lastitem = ((Subtitle.AssItem)(subtitleGrid.Rows[rowindex - 1].DataBoundItem));
                        if (lastitem.End.TimeValue - time > 0 &&
                            lastitem.End.TimeValue - time < Math.Max(Math.Abs(m_Config.StartOffset), Math.Abs(m_Config.EndOffset)))
                        {
                            addEndTimeToRow(rowindex - 1, time - 0.01);
                        }
                    }
                    m_Edited = true;
                }
            }
        }

        /// <summary>
        /// 插入结束时间，并移动当前单元格
        /// </summary>
        private void addEndTime()
        {
            if (subtitleGrid.CurrentRow != null)
            {
                int rowindex = subtitleGrid.CurrentRow.Index;
                if (m_VideoOpened && m_TrackLoaded)
                {
                    double time = dxVideoPlayer.CurrentPosition + m_Config.EndOffset;
                    addEndTimeToRow(rowindex, time);

                    if (rowindex < subtitleGrid.Rows.Count - 1)
                        subtitleGrid.CurrentCell = subtitleGrid.Rows[rowindex + 1].Cells[0];
                    if (rowindex - subtitleGrid.FirstDisplayedScrollingRowIndex > m_Config.SelectRowOffset)
                        subtitleGrid.FirstDisplayedScrollingRowIndex = rowindex - m_Config.SelectRowOffset;
                }
            }
        }

        /// <summary>
        /// 编辑某行的起始时间
        /// </summary>
        /// <param name="time"></param>
        private void addStartTimeToRow(int rowIndex, double time)
        {

            if (subtitleGrid.CurrentRow != null && m_VideoOpened && m_TrackLoaded)
            {
                Subtitle.AssItem item = (Subtitle.AssItem)(subtitleGrid.Rows[rowIndex].DataBoundItem);
                m_undoRec.Edit(rowIndex, 0, item.StartTime);//记录Undo
                double os = item.Start.TimeValue;
                item.Start.TimeValue = time > 0 ? time : 0;
                m_CurrentSub.ItemEdited(item, os, item.End.TimeValue);
                subtitleGrid.UpdateCellValue(0, rowIndex);
                m_Edited = true;

            }
        }

        /// <summary>
        /// 编辑某行的结束时间
        /// </summary>
        /// <param name="time"></param>
        private void addEndTimeToRow(int rowIndex, double time)
        {

            if (subtitleGrid.CurrentRow != null && m_VideoPlaying && m_TrackLoaded)
            {

                Subtitle.AssItem item = (Subtitle.AssItem)(subtitleGrid.Rows[rowIndex].DataBoundItem);
                m_undoRec.Edit(rowIndex, 1, item.EndTime);//记录Undo
                double oe = item.End.TimeValue;
                item.End.TimeValue = time > 0 ? time : 0; ;
                m_CurrentSub.ItemEdited(item, item.Start.TimeValue, oe);
                subtitleGrid.UpdateCellValue(1, rowIndex);
                m_Edited = true;
            }
        }

        /// <summary>
        /// Record Key Status
        /// </summary>
        private bool[] m_keyhold = new bool[256];
        private void subtitleGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if ((int)e.KeyCode >= 0 && (int)e.KeyCode < 256)
            {
                #region Timeing Keys
                if (e.KeyCode == m_Config.AddTimePoint && !m_keyhold[(int)e.KeyCode])
                {
                    addStartTime();
                }
                else if (e.KeyCode == m_Config.AddStartTime && !m_keyhold[(int)e.KeyCode])
                {
                    addStartTime();
                }
                else if (e.KeyCode == m_Config.AddEndTime && !m_keyhold[(int)e.KeyCode])
                {
                    addEndTime();
                }
                else if (e.KeyCode == m_Config.AddContTimePoint && !m_keyhold[(int)e.KeyCode])
                {
                    addEndTime();
                    if (subtitleGrid.CurrentRow.Index < subtitleGrid.Rows.Count - 1)
                    {
                        addStartTime();
                    }
                }
                else if (e.KeyCode == m_Config.AddCellTime && !m_keyhold[(int)e.KeyCode])
                {
                    if (subtitleGrid.CurrentCell.ColumnIndex == 0)
                    {
                        addStartTime();
                    }
                    else if (subtitleGrid.CurrentCell.ColumnIndex == 1)
                    {
                        addEndTime();
                    }
                }
                #endregion
                #region Seek Keys
                else if (e.KeyCode == m_Config.Pause && !m_keyhold[(int)e.KeyCode])
                {
                    if (m_VideoPlaying)
                    {
                        if (m_Paused)
                        {
                            dxVideoPlayer.Play();
                            m_Paused = false;
                        }
                        else
                        {
                            dxVideoPlayer.Pause();
                            m_Paused = true;
                        }

                    }
                }
                else if (e.KeyCode == m_Config.SeekBackword && !m_keyhold[(int)e.KeyCode])
                {
                    if (m_VideoPlaying)
                    {
                        dxVideoPlayer.CurrentPosition -= m_Config.SeekStep;
                    }
                }
                else if (e.KeyCode == m_Config.SeekForward && !m_keyhold[(int)e.KeyCode])
                {
                    if (m_VideoPlaying)
                    {
                        dxVideoPlayer.CurrentPosition += m_Config.SeekStep;
                    }
                }
                else if (e.KeyCode == m_Config.GotoCurrent && !m_keyhold[(int)e.KeyCode])
                {
                    if (m_VideoPlaying && subtitleGrid.CurrentRow != null)
                    {
                        double position = ((Subtitle.AssItem)(subtitleGrid.CurrentRow.DataBoundItem)).Start.TimeValue;
                        dxVideoPlayer.CurrentPosition = position;


                    }
                }
                else if (e.KeyCode == m_Config.GotoPrevious && !m_keyhold[(int)e.KeyCode])
                {
                    if (m_VideoPlaying && subtitleGrid.CurrentRow != null)
                    {
                        int rowindex = subtitleGrid.CurrentRow.Index;
                        double position;
                        if (rowindex >= 1)
                        {
                            position = ((Subtitle.AssItem)(subtitleGrid.Rows[rowindex - 1].DataBoundItem)).Start.TimeValue;
                            dxVideoPlayer.CurrentPosition = position;
                        }
                    }
                }

                #endregion
                #region Edit Keys
                //复制，支持多个单元格的复制和粘贴
                else if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control && !m_keyhold[(int)e.KeyCode])
                {
                    if (subtitleGrid.CurrentCell != null)
                    {
                        //行，列的取值范围
                        int cmin, cmax, rmin, rmax;
                        //行，列的个数
                        int nr, nc;
                        //内容
                        string[,] content;
                        string cb = "";
                        cmin = subtitleGrid.SelectedCells[0].ColumnIndex;
                        cmax = cmin;
                        rmin = subtitleGrid.SelectedCells[0].RowIndex;
                        rmax = rmin;
                        foreach (DataGridViewCell cell in subtitleGrid.SelectedCells)
                        {
                            if (cell.ColumnIndex < cmin) cmin = cell.ColumnIndex;
                            if (cell.ColumnIndex > cmax) cmax = cell.ColumnIndex;
                            if (cell.RowIndex < rmin) rmin = cell.RowIndex;
                            if (cell.RowIndex > rmax) rmax = cell.RowIndex;
                        }
                        nr = rmax - rmin + 1;
                        nc = cmax - cmin + 1;
                        content = new string[nr, nc];
                        foreach (DataGridViewCell cell in subtitleGrid.SelectedCells)
                        {
                            content[cell.RowIndex - rmin, cell.ColumnIndex - cmin] = cell.Value.ToString();
                        }
                        for (int r = 0; r < nr; r++)
                        {
                            for (int c = 0; c < nc; c++)
                            {
                                cb += content[r, c];
                                if (c != nc - 1) cb += "\t";
                            }
                            cb += Environment.NewLine;
                        }
                        Clipboard.SetText(cb);
                    }

                }
                //粘贴，支持多个单元格的复制和粘贴
                else if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control && !m_keyhold[(int)e.KeyCode])
                {
                    if (subtitleGrid.CurrentCell != null && Clipboard.ContainsText())
                    {
                        int cC, cR;
                        cC = subtitleGrid.CurrentCell.ColumnIndex;
                        cR = subtitleGrid.CurrentCell.RowIndex;
                        string[] cells;
                        char[] spliter = { '\t' };
                        StringReader strReader = new StringReader(Clipboard.GetText());
                        string line = strReader.ReadLine();
                        m_undoRec.BeginMultiCells(); //开始Undo记录
                        while (line != null)
                        {
                            cells = line.Split(spliter, 3 - cC);
                            for (int i = 0; i < cells.Length; i++)
                            {
                                if (cells[i].Length != 0)
                                {
                                    m_undoRec.EditMultiCells(cR, cC + i, subtitleGrid.Rows[cR].Cells[cC + i].Value.ToString());
                                    subtitleGrid.Rows[cR].Cells[cC + i].Value = cells[i];
                                }
                            }
                            cR++;
                            if (cR >= subtitleGrid.Rows.Count) break;
                            line = strReader.ReadLine();
                        }
                        m_undoRec.EndEditMultiCells();//结束Undo记录
                        m_Edited = true;
                        m_CurrentSub.RefreshIndex();
                    }
                }

                else if (e.KeyCode == m_Config.EnterEditMode)
                {
                    if (subtitleGrid.CurrentCell != null)
                    {
                        subtitleGrid.BeginEdit(true);
                    }
                }
                else if (e.KeyCode == Keys.Delete && e.Modifiers != Keys.Control && !m_keyhold[(int)e.KeyCode])
                {
                    if (subtitleGrid.SelectedCells.Count != 0)
                    {
                        m_undoRec.BeginMultiCells(); //开始Undo记录
                        foreach (DataGridViewCell cell in subtitleGrid.SelectedCells)
                        {
                            m_undoRec.EditMultiCells(cell.RowIndex, cell.ColumnIndex, cell.Value.ToString());
                            subtitleGrid.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Value = "";
                        }
                        m_undoRec.EndEditMultiCells();
                        m_Edited = true;
                    }
                }
                else if (e.KeyCode == Keys.Delete && e.Modifiers == Keys.Control && !m_keyhold[(int)e.KeyCode])
                {
                    if (subtitleGrid.CurrentRow != null)
                    {
                        List<DataGridViewRow> deleteRow = new List<DataGridViewRow>();
                        foreach (DataGridViewCell cell in subtitleGrid.SelectedCells)
                        {
                            if (!deleteRow.Contains(subtitleGrid.Rows[cell.RowIndex]))
                                deleteRow.Add(subtitleGrid.Rows[cell.RowIndex]);
                        }
                        foreach (DataGridViewRow row in deleteRow)
                        {
                            DeleteRow(row);
                        }
                    }
                }
                #endregion
                #region File Keys
                else if (e.KeyCode == m_Config.SaveAss && !m_keyhold[(int)e.KeyCode])
                {
                    if (m_SubLoaded)
                    {
                        SaveAssSub();
                    }
                }
                #endregion
                m_keyhold[(int)e.KeyCode] = true;
            }

        }

        private void subtitleGrid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==m_Config.AddTimePoint)
            {
                addEndTime();
            }
            if ((int)e.KeyCode >= 0 && (int)e.KeyCode < 256)
            {
                m_keyhold[(int)e.KeyCode] = false;
            }
        }

        private SGSConfig m_Config;

        private void toolStripPause_Click(object sender, EventArgs e)
        {
            if (m_VideoPlaying)
            {

                dxVideoPlayer.Pause();
                m_Paused = true;
            }
        }

        private void toolStripPlay_Click(object sender, EventArgs e)
        {
            if (m_VideoPlaying)
            {
                dxVideoPlayer.Play();
                m_Paused = false;
            }
        }

        private void toolStripJumpto_Click(object sender, EventArgs e)
        {
            if (m_VideoPlaying && subtitleGrid.CurrentRow != null)
            {
                double position = ((Subtitle.AssItem)(subtitleGrid.CurrentRow.DataBoundItem)).Start.TimeValue;
                dxVideoPlayer.CurrentPosition = position;
            }
        }




        private void KeyCfgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeyConfigForm keycfg = new KeyConfigForm(m_Config);
            if (keycfg.ShowDialog() == DialogResult.OK)
            {
                m_Config.Save();
            }
        }

        private void SaveAsSub_Click(object sender, EventArgs e)
        {
            if (m_SubLoaded)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.AddExtension = true;
                dlg.DefaultExt = "ass";
                dlg.Filter = "ASS Subtitle (*.ass)|*.ass||";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    m_CurrentSub.WriteAss(dlg.FileName,Encoding.Unicode);
                    m_AssFilename = dlg.FileName;
                    m_Edited = false;
                }
            }
        }

        private void toolStripDuplicate_Click(object sender, EventArgs e)
        {
            if (subtitleGrid.CurrentRow != null)
            {
                Subtitle.AssItem i = ((Subtitle.AssItem)(subtitleGrid.CurrentRow.DataBoundItem)).Clone();
                m_CurrentSub.SubItems.Insert(subtitleGrid.CurrentRow.Index + 1, i);
                m_undoRec.InsertRow(subtitleGrid.CurrentRow.Index + 1);//为Undo记录插入操作
                m_selectCells.Reset(); //清空选中的单元格
                subtitleGrid.Refresh();
                m_CurrentSub.RefreshIndex();
                m_Edited = true;
            }
        }

        private void toolStripDeleteItem_Click(object sender, EventArgs e)
        {
            if (subtitleGrid.CurrentRow != null)
            {
                DeleteRow(subtitleGrid.CurrentRow);
            }
        }

        private void DeleteRow(DataGridViewRow row)
        {
            m_undoRec.DeleteRow(row.Index, row);//为Undo记录删除操作
            Subtitle.AssItem i = ((Subtitle.AssItem)(row.DataBoundItem));
            m_CurrentSub.SubItems.Remove(i);
            m_selectCells.Reset(); //清空选中的单元格
            subtitleGrid.Refresh();
            m_CurrentSub.RefreshIndex();
            m_Edited = true;
        }

        private void InsertNewRow(int index, DataGridViewRow currentRow)
        {
            Subtitle.AssItem i = ((Subtitle.AssItem)(currentRow.DataBoundItem)).Clone();
            i.Text = "";
            i.Start.TimeValue = 0;
            i.End.TimeValue = 0;
            m_CurrentSub.SubItems.Insert(index, i);
            m_undoRec.InsertRow(index);//为Undo记录插入操作
            m_selectCells.Reset(); //清空选中的单元格
            subtitleGrid.Refresh();
            m_Edited = true;
        }

        private void toolStripInsertAfter_Click(object sender, EventArgs e)
        {
            if (subtitleGrid.CurrentRow != null)
            {
                InsertNewRow(subtitleGrid.CurrentRow.Index + 1, subtitleGrid.CurrentRow);
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AboutSgsubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutbox = new AboutBox();
            aboutbox.Show();
        }

        private void SGSMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (!AskSave())
            {
                e.Cancel = true;
            }
           
        }

        private void subtitleGrid_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
            if (subtitleGrid.CurrentRow != null)
            {
                Subtitle.AssItem i = (Subtitle.AssItem)(subtitleGrid.Rows[e.Cell.RowIndex].DataBoundItem);
                waveScope.Start = i.Start.TimeValue;
                waveScope.End = i.End.TimeValue;
                labelcurrent.Text = i.Text;
                Subtitle.AssTime time = new Subtitle.AssTime();
                time.TimeValue = i.End.TimeValue - i.Start.TimeValue;
                if (time.TimeValue >= 0) labelThisDuration.Text = time.ToString();
                    else labelThisDuration.Text = "Error";
                if (e.Cell.RowIndex > 0)
                {
                    i = (Subtitle.AssItem)(subtitleGrid.Rows[e.Cell.RowIndex - 1].DataBoundItem);
                    waveScope.LastStart = i.Start.TimeValue;
                    waveScope.LastEnd = i.End.TimeValue;
                    labellastline.Text = i.Text;
                    time.TimeValue = i.End.TimeValue - i.Start.TimeValue;
                    if (time.TimeValue >= 0) labelLastDuration.Text = time.ToString();
                    else labelLastDuration.Text = "Error";
                }
                else
                {
                    labellastline.Text = "";
                    labelLastDuration.Text = "-:--:--.--";
                }
                if (e.Cell.RowIndex < subtitleGrid.Rows.Count - 1)
                {
                    i = (Subtitle.AssItem)(subtitleGrid.Rows[e.Cell.RowIndex + 1].DataBoundItem);
                    labelnextline.Text = i.Text;
                    time.TimeValue = i.End.TimeValue - i.Start.TimeValue;
                    if (time.TimeValue >= 0) labelNextDuration.Text = time.ToString();
                    else labelNextDuration.Text = "Error";
                }
                else
                {
                    labelnextline.Text = "";
                    labelNextDuration.Text = "-:--:--.--";
                }
            }
        }

        private void waveScope_WSMouseDown(object sender, WaveReader.WFMouseEventArgs e)
        {
            if (subtitleGrid.CurrentRow != null)
            {
                int rowindex = subtitleGrid.CurrentRow.Index;
                Subtitle.AssItem item = (Subtitle.AssItem)(subtitleGrid.Rows[rowindex].DataBoundItem);
                if (e.Button == MouseButtons.Left)
                {
                    addStartTimeToRow(subtitleGrid.CurrentRow.Index, e.Time);
                }
                if (e.Button == MouseButtons.Right)
                {
                    addEndTimeToRow(subtitleGrid.CurrentRow.Index, e.Time);

                    if (rowindex < subtitleGrid.Rows.Count - 1)
                    {
                        subtitleGrid.CurrentCell = subtitleGrid.Rows[rowindex + 1].Cells[0];
                        rowindex++;
                    }
                }
                item = (Subtitle.AssItem)(subtitleGrid.Rows[rowindex].DataBoundItem);
                waveScope.Start = item.Start.TimeValue;
                waveScope.End = item.End.TimeValue;
                if (rowindex > 0)
                {
                    item = (Subtitle.AssItem)(subtitleGrid.Rows[rowindex - 1].DataBoundItem);
                    waveScope.LastStart = item.Start.TimeValue;
                    waveScope.LastEnd = item.End.TimeValue;
                }
            }
            subtitleGrid.Focus();
        }

        private void tsBtnFFT_Click(object sender, EventArgs e)
        {
            if (m_VideoPlaying)
            {
                dxVideoPlayer.Pause();
                WaveReader.WaveForm wf = WaveReader.WaveForm.ExtractWave(dxVideoPlayer.Filename);
                waveScope.Wave = wf;
            }
        }

        private enum TimeCheckStatus { OK = 0, OVERLAP, ERROR};
        /// <summary>
        /// 时间轴重叠和错误检查
        /// </summary>
        private void tsBtnOLScan_Click(object sender, EventArgs e)
        {
            if (m_SubLoaded)
            {
                bool overlap = false;
                bool timeerror = false;
                TimeCheckStatus[] itemStatus = new TimeCheckStatus[subtitleGrid.Rows.Count];
                subtitleGrid.Rows[0].Cells[0].Style.ForeColor = Color.Red;
                for (int i = 0; i < subtitleGrid.Rows.Count - 1; i++)
                {
                    Subtitle.AssItem itema = (Subtitle.AssItem)(subtitleGrid.Rows[i].DataBoundItem);
                    if (itema.Start.TimeValue > itema.End.TimeValue)
                    {
                        itemStatus[i] = TimeCheckStatus.ERROR;
                        continue;
                    }
                    for (int j = i + 1; j < subtitleGrid.Rows.Count; j++)
                    {
                        Subtitle.AssItem itemb = (Subtitle.AssItem)(subtitleGrid.Rows[j].DataBoundItem);
                        if (itemb.Start.TimeValue > itemb.End.TimeValue)
                        {
                            itemStatus[j] = TimeCheckStatus.ERROR;
                            continue;
                        }
                        if (
                            itema.End.TimeValue > itemb.Start.TimeValue && itema.Start.TimeValue < itemb.Start.TimeValue ||
                            itemb.End.TimeValue > itema.Start.TimeValue && itemb.Start.TimeValue < itema.Start.TimeValue
                        )
                        {
                            itemStatus[i] = TimeCheckStatus.OVERLAP;
                            itemStatus[j] = TimeCheckStatus.OVERLAP;
                        }
                    }

                }
                for (int i = 0; i < subtitleGrid.Rows.Count - 1; i++)
                {
                    switch (itemStatus[i])
                    {
                        case TimeCheckStatus.OVERLAP:
                            subtitleGrid.Rows[i].Cells[0].Style.ForeColor = Color.Red;
                            subtitleGrid.Rows[i].Cells[1].Style.ForeColor = Color.Red;
                            overlap = true;
                            break;
                        case TimeCheckStatus.OK:
                            subtitleGrid.Rows[i].Cells[0].Style.ForeColor = Color.Black;
                            subtitleGrid.Rows[i].Cells[1].Style.ForeColor = Color.Black;
                            break;
                        case TimeCheckStatus.ERROR:
                            subtitleGrid.Rows[i].Cells[0].Style.ForeColor = Color.Blue;
                            subtitleGrid.Rows[i].Cells[1].Style.ForeColor = Color.Blue;
                            timeerror = true;
                            break;
                    }
                }
                string msg =
                    timeerror && overlap ? "发现时间轴重叠和错误时间点" :
                    timeerror && !overlap ? "发现错误时间点" :
                    !timeerror && overlap ? "发现时间轴重叠" : "未发现时间轴重叠和错误时间点";

                MessageBox.Show(msg, "时间轴检查");

            }
        }

        private UndoRecord m_undoRec = new UndoRecord();

        private void tsBtnUndo_Click(object sender, EventArgs e)
        {
            m_undoRec.Undo(m_CurrentSub);
            subtitleGrid.Refresh();
            m_CurrentSub.RefreshIndex();
            m_Edited = true;
        }

        SelectCells m_selectCells = new SelectCells();

        private void SelectCell_Click(object sender, EventArgs e)
        {
            if (subtitleGrid.SelectedCells != null)
            {
                foreach (DataGridViewCell cell in subtitleGrid.SelectedCells)
                {
                    m_selectCells.SelectCell(cell.ColumnIndex, cell.RowIndex);
                }
            }
        }

        private void tsBtnDeselectAll_Click(object sender, EventArgs e)
        {
            m_selectCells.DeselectAll();
        }

        private void DeselectCell_Click(object sender, EventArgs e)
        {
            if (subtitleGrid.SelectedCells != null)
            {
                foreach (DataGridViewCell cell in subtitleGrid.SelectedCells)
                {
                    m_selectCells.Deselect(cell.ColumnIndex, cell.RowIndex);
                }
            }
        }

        private void TimeOffset_Click(object sender, EventArgs e)
        {
            TimeOffsetDialog toDlg = new TimeOffsetDialog();
            if (toDlg.ShowDialog() == DialogResult.OK)
            {
                m_selectCells.TimeOffset(toDlg.TimeOffset, m_undoRec);
                subtitleGrid.Refresh();
            }
        }

        /// <summary>
        /// 双击单元格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void subtitleGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (subtitleGrid.CurrentCell != null)
            {
                subtitleGrid.BeginEdit(true);
            }
        }
        private string[] allowedexts = { "avi", "mkv", "mp4", "rmvb", "wmv", "ass", "txt" };

        private void SGSMainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])(e.Data.GetData(DataFormats.FileDrop));
                if (files.Length == 1)
                {
                    string ext = (files[0].Substring(files[0].LastIndexOf('.') + 1)).ToLower();
                    if (allowedexts.Contains(ext))
                    {
                    e.Effect = DragDropEffects.Copy;
                    }
                }
            }
        }


        private void SGSMainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])(e.Data.GetData(DataFormats.FileDrop));
                if (files.Length == 1)
                {
                    string ext = (files[0].Substring(files[0].LastIndexOf('.') + 1)).ToLower();
                    switch (ext)
                    {
                        case "txt":
                            if (AskSave())
                            {
                                OpenTxt(files[0]);
                            }
                            break;
                        case "ass":
                            if (AskSave())
                            {
                                OpenAss(files[0]);
                            }
                            break;
                        default:
                            OpenVideo(files[0]);
                            break;
                    }
                    if (allowedexts.Contains(ext))
                    {
                        e.Effect = DragDropEffects.Copy;
                    }
                }
            }
        }

        private void tsBtnInsBefore_Click(object sender, EventArgs e)
        {
            if (subtitleGrid.CurrentRow != null)
            {
                InsertNewRow(subtitleGrid.CurrentRow.Index, subtitleGrid.CurrentRow);
            }
        }
        
    }
}