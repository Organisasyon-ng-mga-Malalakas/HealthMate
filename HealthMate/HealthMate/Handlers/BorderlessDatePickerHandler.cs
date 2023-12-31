﻿using Android.Graphics.Drawables;
using HealthMate.Controls;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace HealthMate.Handlers;
public partial class BorderlessDatePickerHandler : DatePickerHandler
{
	private static readonly PropertyMapper<BorderlessDatePicker, BorderlessDatePickerHandler> PropertyMapper = new(ViewMapper)
	{
		[nameof(BorderlessDatePicker.FontFamily)] = MapFont,
		[nameof(BorderlessDatePicker.FontSize)] = MapFont,
		[nameof(BorderlessDatePicker.TextColor)] = MapTextColor,
		[nameof(BorderlessDatePicker.Date)] = MapDate,
		[nameof(BorderlessDatePicker.Format)] = MapFormat,
		[nameof(BorderlessDatePicker.MaximumDate)] = MapMaximumDate,
		[nameof(BorderlessDatePicker.MinimumDate)] = MapMinimumDate

	};

	public BorderlessDatePickerHandler() : base(PropertyMapper) { }

	protected override void ConnectHandler(MauiDatePicker platformView)
	{
		base.ConnectHandler(platformView);
		//platformView.Background = null;
		//platformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
		//platformView.SetPadding(0, 0, 0, 0);
		var gd = new GradientDrawable();
		gd.SetColor(Android.Graphics.Color.Transparent);
		platformView.SetBackground(gd);

	}
}