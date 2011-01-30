﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace sgsubdotnet
{
    class SelectCells
    {
        private List<CellPos> m_SelectedCells = new List<CellPos>();

        public DataGridViewRowCollection Rows
        {
            set;
            private get;
        }

        public SelectCells()
        {
            Rows = null;
        }

        public void SelectCell(int Col, int Row)
        {
            CellPos cell = new CellPos(Col, Row);
            if (!m_SelectedCells.Contains(cell)) m_SelectedCells.Add(cell);
            if (Rows != null)
                if (Row < Rows.Count && Col < 3) Rows[Row].Cells[Col].Style.BackColor = Color.SkyBlue;

        }

        public void Deselect(int Col, int Row)
        {
            if (Rows != null)
                if (Row < Rows.Count && Col < 3)
                {
                    Rows[Row].Cells[Col].Style.BackColor = Color.White;
                    CellPos cell = new CellPos(Col, Row);
                    if (m_SelectedCells.Contains(cell)) m_SelectedCells.Remove(cell);
                }
        }

        public void DeselectAll()
        {
            foreach (CellPos cell in m_SelectedCells)
            {
                if (Rows != null)
                {
                    if (cell.Row < Rows.Count && cell.Col < 3)
                        Rows[cell.Row].Cells[cell.Col].Style.BackColor = Color.White;
                }
            }
            m_SelectedCells = new List<CellPos>();

        }

        public void Reset()
        {
            m_SelectedCells = new List<CellPos>();
            if (Rows.Count > 0)
            {
                for (int i = 0; i < Rows.Count; i++)
                {
                    Rows[i].Cells[0].Style.BackColor = Color.White;
                    Rows[i].Cells[1].Style.BackColor = Color.White;
                    Rows[i].Cells[2].Style.BackColor = Color.White;
                }
            }
        }

    }

    class CellPos : IComparable<CellPos>,IEquatable<CellPos>
    {
        public CellPos()
        {
            Col = 0;
            Row = 0;
        }

        public CellPos(int col, int row)
        {
            Col = col;
            Row = row;
        }

       
        public int Col;
        public int Row;

        #region IComparable<CellPos> Members

        int IComparable<CellPos>.CompareTo(CellPos other)
        {
            int val = (Row - other.Row) * 3 + (Col - other.Col);
            return val;
        }

        #endregion

        #region IEquatable<CellPos> Members

        public bool Equals(CellPos other)
        {
            return (Row == other.Row && Col == other.Col);
        }

        #endregion
    }
    
}
