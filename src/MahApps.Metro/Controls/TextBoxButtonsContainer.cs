using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace MahApps.Metro.Controls
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(Button))]
    public class TextBoxButtonsContainer : ItemsControl
    {
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is Button;
        }

    }
}
