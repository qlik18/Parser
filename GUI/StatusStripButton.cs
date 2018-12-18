using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms.Design;
using System.Windows.Forms;

namespace GUI
{
    [ToolboxBitmapAttribute("image path or use another overload..."),
  ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.MenuStrip |
                                   ToolStripItemDesignerAvailability.ContextMenuStrip |
                                   ToolStripItemDesignerAvailability.StatusStrip)]
    public class StatusStripButton : ToolStripButton
    {
    }
}
