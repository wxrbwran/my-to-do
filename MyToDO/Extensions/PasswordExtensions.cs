﻿using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace MyToDO.Extensions
{
	public class PasswordExtensions
	{
		public static string GetPassWord(DependencyObject obj)
		{
			return (string)obj.GetValue(PassWordProperty);
		}

		public static void SetPassWord(DependencyObject obj, string value)
		{
			obj.SetValue(PassWordProperty, value);
		}

		public static readonly DependencyProperty PassWordProperty =
				DependencyProperty.RegisterAttached(
					"PassWord",
					typeof(string),
					typeof(PasswordExtensions),
					new PropertyMetadata(string.Empty, OnPassWordPropertyChanged)
				);

		private static void OnPassWordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var passWordBox = sender as PasswordBox;
			string password = (string)e.NewValue;
			if (passWordBox != null && passWordBox.Password != password) 
			{
				passWordBox.Password = password;
			}
		}

	}
	public class PasswordBehavior: Behavior<PasswordBox>
	{
		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.PasswordChanged += AssociatedObject_PasswordChanged;
		}

		protected override void OnDetaching()
		{
			AssociatedObject.PasswordChanged -= AssociatedObject_PasswordChanged;
		}

		private void AssociatedObject_PasswordChanged(object sender, RoutedEventArgs e)
		{
			PasswordBox passwordBox = sender as PasswordBox;
			string password = PasswordExtensions.GetPassWord(passwordBox);
			if(passwordBox != null && passwordBox.Password != password) 
			{
				PasswordExtensions.SetPassWord(passwordBox, passwordBox.Password);
			}
		}
	}
}
