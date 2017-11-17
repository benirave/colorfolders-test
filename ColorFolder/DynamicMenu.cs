using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ColorFolder.Properties;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;


namespace ColorFolder
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.AllFiles)]
    [COMServerAssociation(AssociationType.Directory)]
    class DynamicMenu : SharpContextMenu
    {
        private ContextMenuStrip menu = new ContextMenuStrip();

        protected override bool CanShowMenu()
        {
            //  We can show the item only for a single selection.
            if (SelectedItemPaths.Count() == 1)
            {
                this.UpdateMenu();
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override ContextMenuStrip CreateMenu()
        {
            menu.Items.Clear();
            FileAttributes attr = File.GetAttributes(SelectedItemPaths.First());

            // check if the selected item is a directory
            if (attr.HasFlag(FileAttributes.Directory))
            {
                this.MenuDirectory();
            }
            else
            {
                //EErosrothis.MenuFiles();
            }

            // return the menu item
            return menu;
        }

        // <summary>
        // Updates the context menu. 
        // </summary>
        private void UpdateMenu()
        {
            // release all resources associated to existing menu
            menu.Dispose();
            menu = CreateMenu();
        }

        protected void MenuDirectory()
        {
            ToolStripMenuItem MainMenu;
            MainMenu = new ToolStripMenuItem
            {
                Text = "Color",
                //Image = Properties.Resources.blue
            };

            ToolStripMenuItem SubMenu1;
            SubMenu1 = new ToolStripMenuItem
            {
                Text = "Green",
                Image = Properties.Resources.folder__55_
            };

            var SubMenu2 = new ToolStripMenuItem
            {
                Text = "Yellow",
                Image = Properties.Resources.folder__62_
            };
            SubMenu2.DropDownItems.Clear();
            SubMenu1.Click += (sender, args) => ApplyFolderIcon(53);
            SubMenu2.Click += (sender, args) => ApplyFolderIcon(60);
            MainMenu.DropDownItems.Add(SubMenu1);
            MainMenu.DropDownItems.Add(SubMenu2);

            menu.Items.Clear();
            menu.Items.Add(MainMenu);
        }

        private void ApplyFolderIcon(int icon)
        {
            foreach (var SelectedItem in SelectedItemPaths)
            {
                var iniPath = Path.Combine(SelectedItem, "desktop.ini");


                if (File.Exists(iniPath))
                {
                    //remove hidden and system attributes to make ini file writable
                    File.SetAttributes(
                        iniPath,
                        File.GetAttributes(iniPath) &
                        ~(FileAttributes.Hidden | FileAttributes.System));
                }

                //create new ini file with the required contents
                var iniContents = new StringBuilder()
                    .AppendLine("[.ShellClassInfo]")
                    .AppendLine($"IconResource={@"C:\Users\Beni\Documents\CFIcons.dll"},{icon}")
                    .ToString();
                File.WriteAllText(iniPath, iniContents);

                //hide the ini file and set it as system
                File.SetAttributes(
                    iniPath,
                    File.GetAttributes(iniPath) | FileAttributes.Hidden | FileAttributes.System);
                //set the folder as system
                File.SetAttributes(
                    SelectedItem,
                    File.GetAttributes(SelectedItem) | FileAttributes.System);

                //refreshUI.Refresh();
            }
        }
    }
}
