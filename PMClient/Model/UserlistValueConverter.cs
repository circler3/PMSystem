using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using PMClient.ViewModel;

namespace PMClient.Model
{
    class UserlistValueConverter : IValueConverter
    {
        //value as userdict
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var obj = value as Dictionary<string,string>;
            return obj.Values;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
