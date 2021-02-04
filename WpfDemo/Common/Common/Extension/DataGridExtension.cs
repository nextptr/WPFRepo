using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace  Common.Extension
{
    public static class DataGridExtension
    {
        public static DataGridRow GetDataGridRow(this DataGrid me, int i)
        {
            return me.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
        }

        public static bool CellEditCompare<T>(this T item, DataGridCellEditEndingEventArgs e)
        {
            PropertyInfo prop = typeof(T).GetProperty(e.Column.SortMemberPath);

            if (prop == null)
            {
                return false;
            }

            string v1 = prop.GetValue(item).ToString();
            string v2 = ((TextBox)e.EditingElement).Text;

            double oldValue = 0, newValue = 0;

            if (!double.TryParse(v1, out oldValue))
            {
                return false;
            }
            if (!double.TryParse(v2, out newValue))
            {
                return false;
            }

            return (oldValue != newValue);
        }

        public static DataGridCell GetCell(DataGridCellInfo dataGridCellInfo)
        {
            if (!dataGridCellInfo.IsValid)
            {
                return null;
            }

            var cellContent = dataGridCellInfo.Column.GetCellContent(dataGridCellInfo.Item);
            if (cellContent != null)
            {
                return (DataGridCell)cellContent.Parent;
            }
            else
            {
                return null;
            }
        }

        public static int GetRowIndex(DataGridCell dataGridCell)
        {
            // Use reflection to get DataGridCell.RowDataItem property value.
            PropertyInfo rowDataItemProperty = dataGridCell.GetType().GetProperty("RowDataItem", BindingFlags.Instance | BindingFlags.NonPublic);

            DataGrid dataGrid = GetDataGridFromChild(dataGridCell);

            return dataGrid.Items.IndexOf(rowDataItemProperty.GetValue(dataGridCell, null));
        }

        public static DataGrid GetDataGridFromChild(DependencyObject dataGridPart)
        {
            if (VisualTreeHelper.GetParent(dataGridPart) == null)
            {
                throw new NullReferenceException("Control is null.");
            }
            if (VisualTreeHelper.GetParent(dataGridPart) is DataGrid)
            {
                return (DataGrid)VisualTreeHelper.GetParent(dataGridPart);
            }
            else
            {
                return GetDataGridFromChild(VisualTreeHelper.GetParent(dataGridPart));
            }
        }

        public static DataGridCell GetCell(this DataGridRow me, int column)
        {
            DataGridCellsPresenter presenter = FindChild<DataGridCellsPresenter>(me);

            if (presenter != null)
            {
                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                return cell;
            }
            else
            {
                return null;
            }
        }

        public static DataGridCell GetCell(this DataGrid me, int row, int column)
        {
            DataGridRow rowContainer = (DataGridRow)me.ItemContainerGenerator.ContainerFromIndex(row);

            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = FindChild<DataGridCellsPresenter>(rowContainer);

                if (presenter != null)
                {
                    DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                    return cell;
                }
                else
                {
                    return null;
                }
            }

            return null;
        }

        public static T FindChild<T>(this DependencyObject parent) where T : DependencyObject
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; ++i)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    T childOfChild = FindChild<T>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }

            return null;
        }

        public static T FindChild<T>(this DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null)
            {
                return null;
            }

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                T childType = child as T;

                if (childType == null)
                {
                    foundChild = FindChild<T>(child, childName);

                    if (foundChild != null)
                        break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;

                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        foundChild = (T)child;
                        break;
                    }
                    else
                    {
                        foundChild = FindChild<T>(child, childName);

                        if (foundChild != null)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        public static List<T> FindChild<T>(this DependencyObject parent, Type typename) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(parent) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(parent, i);

                if (child is T && (((T)child).GetType() == typename))
                {
                    childList.Add((T)child);
                }
                childList.AddRange(FindChild<T>(child, typename));
            }
            return childList;
        }
    }
}
