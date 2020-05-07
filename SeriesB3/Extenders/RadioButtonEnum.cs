using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SeriesB3.Extenders
{
    public class RadioButtonEnum : RadioButton
    {
        public RadioButtonEnum()
        {
            this.Loaded += new RoutedEventHandler(EnumLoad);
            this.Checked += new RoutedEventHandler(EnumChecked);
        }

        private void EnumChecked(object sender, RoutedEventArgs e)
        {
            if (this.IsChecked == true)
            {
                object binding = EnumBinding;

                if ((binding is Enum) && (EnumValue != null))
                {
                    EnumBinding = Enum.Parse(binding.GetType(), EnumValue);
                }
            }
        }

        private void EnumLoad(object sender, RoutedEventArgs e)
        {
            SetChecked();
        }

        private void SetChecked()
        {
            object binding = EnumBinding;

            if ((binding is Enum) && (EnumValue != null))
            {
                object value = Enum.Parse(binding.GetType(), EnumValue);
                IsChecked = ((Enum)binding).CompareTo(value) == 0;
            }
        }

        public object EnumBinding
        {
            set { SetValue(EnumBindingProperty, value); }
            get { return GetValue(EnumBindingProperty); }
        }
        public static readonly DependencyProperty EnumBindingProperty;

        public string EnumValue
        {
            set { SetValue(EnumValueProperty, value); }
            get { return (string)GetValue(EnumValueProperty); }
        }
        public static readonly DependencyProperty EnumValueProperty;

        static RadioButtonEnum()
        {
            EnumBindingProperty = DependencyProperty.Register("EnumBinding",
                                                               typeof(object),
                                                               typeof(RadioButtonEnum),
                                                               new FrameworkPropertyMetadata(OnEnumBindingChanged) { BindsTwoWayByDefault = true });

            EnumValueProperty = DependencyProperty.Register("EnumValue",
                                                               typeof(string),
                                                               typeof(RadioButtonEnum));
        }

        private static void OnEnumBindingChanged(DependencyObject dependency_object, DependencyPropertyChangedEventArgs event_arguments)
        {
            if (dependency_object is RadioButtonEnum)
            {
                ((RadioButtonEnum)dependency_object).SetChecked();
            }
        }
    }
}
