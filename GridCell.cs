using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VP_Sudoku
{
    [Serializable]
    public class GridCell : Button
    {
        private int _x;
        private int _y;
        private int _value;
        private bool _isLocked;

        public GridCell()
        {
            this._isLocked = false;
            this._value = 0;
        }

        public void Clear()
        {
            this.Text = "";
            this._value = 0;
        }

        #region Properties
        public int X
        {
            get => _x;
            set => _x = value;
        }

        public int Y
        {
            get => _y;
            set => _y = value;
        }

        public int Value
        {
            get => _value;
            set => _value = value;
        }

        public bool IsLocked
        {
            get => _isLocked;
            set => _isLocked = value;
        }
        #endregion
    }
}
