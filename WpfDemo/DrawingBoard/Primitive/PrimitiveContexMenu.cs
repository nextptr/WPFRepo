using System.Windows.Controls;

namespace DrawingBoard.Primitive
{
    public class PrimitiveContexMenu : ContextMenu
    {
        public PrimitiveContexMenu()
        {
            MenuItem mi = new MenuItem();
            mi.Header = "Move to Origin";
            Items.Add(mi);
        }

        public MenuItem GetItem(int i)
        {
            return Items[i] as MenuItem;
        }
    }
}
