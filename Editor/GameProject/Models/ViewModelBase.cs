﻿using Editor.GameProject.Models;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Editor.GameProject
{
    [DataContract(IsReference = true)]
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}