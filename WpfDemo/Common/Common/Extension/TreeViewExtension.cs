using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace  Common.Extension
{
    public static class TreeViewExtension
    {
        public static TreeViewItem GetParent(this TreeViewItem tvi)
        {
            ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(tvi);
            TreeViewItem parent = ic as TreeViewItem;
            return parent;
        }

        public static List<TreeViewItem> GetParents(this TreeViewItem tvi)
        {
            List<TreeViewItem> parents = new List<TreeViewItem>();
            ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(tvi);
            TreeViewItem parent = ic as TreeViewItem;
            while (parent != null)
            {
                parents.Add(parent);
                ic = ItemsControl.ItemsControlFromItemContainer(parent);
                parent = ic as TreeViewItem;
            }
            return parents;
        }

        public static void ExpandOnly(this TreeViewItem tvi, TreeViewItem selected)
        {
            for (int i = 0; i < tvi.Items.Count; ++i)
            {
                TreeViewItem item = tvi.Items[i] as TreeViewItem;
                if (item != selected)
                {
                    item.IsExpanded = false;
                }
            }
        }

        public static void ExpandOnly(this TreeView tv, TreeViewItem selected)
        {
            for (int i = 0; i < tv.Items.Count; ++i)
            {
                TreeViewItem item = tv.Items[i] as TreeViewItem;
                if (item != selected)
                {
                    item.IsExpanded = false;
                }
            }
        }
    }
}
