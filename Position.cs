using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Position
    {

        public int Row { get; set; }
        public int Col { get; set; }
        public Position(int row, int col)
        {

            Row = row;
            Col = col;
        }
    }

}
