using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using CerberusWareV3.Configuration;
using Hexa.NET.ImGui;
[CompilerGenerated]
public static class UiSettingsRenderer
{
	public static void Render()
	{
		bool flag = !CerberusConfig.Settings.UiCustomizable;
		if (!flag)
		{
			ImGui.PushFont(FontManager.GetFont("global"));
			ImGui.SetNextWindowSize(new Vector2(340f, 480f));
			ImGui.Begin(LocalizationManager.GetString("UiSettings_Title"), ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse);
			ImGuiWidgets.Toggle(LocalizationManager.GetString("UiSettings_EnableNotifications"), ref CerberusConfig.Notifications.Enabled);
			ImGuiWidgets.SliderInt(LocalizationManager.GetString("UiSettings_MaxNotifications"), ref CerberusConfig.Notifications.MaxNotifications, 1, 20);
			string[] array = new string[]
			{
				LocalizationManager.GetString("UiSettings_NotificationSize_Small"),
				LocalizationManager.GetString("UiSettings_NotificationSize_Medium"),
				LocalizationManager.GetString("UiSettings_NotificationSize_Large")
			};
			int fontSize = CerberusConfig.Notifications.FontSize;
			bool flag2 = ImGuiWidgets.Combo(LocalizationManager.GetString("UiSettings_NotificationSize"), ref fontSize, array);
			if (flag2)
			{
				CerberusConfig.Notifications.FontSize = fontSize;
			}
			ImGuiWidgets.Toggle(LocalizationManager.GetString("UiSettings_IgnoreSizeCheck"), ref CerberusConfig.Notifications.IgnoreSizeCheck);
			ImGui.End();
			ImGui.PopFont();
		}
	}
}
