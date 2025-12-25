using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using Hexa.NET.ImGui;


[CompilerGenerated]
public static class ImGuiWidgets
{
	public unsafe static bool Toggle(string label, ref bool value)
	{
		string text = "##Toggle_" + label;
		bool flag = !ImGuiWidgets._toggleAnimations.ContainsKey(text);
		if (flag)
		{
			ImGuiWidgets._toggleAnimations[text] = (value ? 1f : 0f);
		}
		Vector2 vector = new Vector2(ImGui.GetContentRegionAvail().X - 20f, 50f);
		float num = 50f;
		float num2 = 23f;
		ImGui.BeginGroup();
		Vector2 cursorScreenPos = ImGui.GetCursorScreenPos();
		ImDrawListPtr windowDrawList = ImGui.GetWindowDrawList();
		Vector2 vector2 = new Vector2(cursorScreenPos.X + 10f, cursorScreenPos.Y);
		Vector2 vector3 = new Vector2(cursorScreenPos.X + vector.X + 10f, cursorScreenPos.Y + vector.Y);
		windowDrawList.AddRectFilled(vector2, vector3, ImGui.GetColorU32(ImGuiWidgets.BgColor), 5f);
		ImGui.SetCursorScreenPos(new Vector2(vector2.X + 10f, vector2.Y + (vector.Y - ImGui.GetTextLineHeight()) / 2f));
		ImGui.Text(label);
		Vector2 vector4 = new Vector2(vector3.X - num - 10f, vector2.Y + (vector.Y - num2) / 2f);
		ImGui.SetCursorScreenPos(vector4);
		ImGui.PushID(text);
		bool flag2 = false;
		bool flag3 = ImGui.InvisibleButton("##ToggleButton", new Vector2(num, num2));
		if (flag3)
		{
			value = !value;
			flag2 = true;
		}
		ImGui.PopID();
		float num3 = ImGuiWidgets._toggleAnimations[text];
		float num4 = (value ? 1f : 0f);
		float num5 = 10f;
		float num6 = ImGui.GetIO().DeltaTime;
		float num7 = num5 * num6;
		bool flag4 = num3 < num4;
		if (flag4)
		{
			num3 = Math.Min(num3 + num7, num4);
		}
		else
		{
			bool flag5 = num3 > num4;
			if (flag5)
			{
				num3 = Math.Max(num3 - num7, num4);
			}
		}
		ImGuiWidgets._toggleAnimations[text] = num3;
		Vector4 vector5 = Vector4.Lerp(ImGuiWidgets.KnobColorOff, ImGuiWidgets.KnobColorOn, num3);
		float num8 = vector4.X + (num - num2) * num3;
		float y = vector4.Y;
		Vector2 vector6 = new Vector2(num8, y);
		Vector2 vector7 = new Vector2(num8 + num2, y + num2);
		windowDrawList.AddRectFilled(vector4, new Vector2(vector4.X + num, vector4.Y + num2), ImGui.GetColorU32(ImGuiWidgets.SlotColor), 5f);
		windowDrawList.AddRectFilled(vector6, vector7, ImGui.GetColorU32(vector5), 5f);
		ImGui.EndGroup();
		ImGui.Dummy(new Vector2(0f, 15f));
		return flag2;
	}
	public unsafe static bool ToggleWithSettings(string label, ref bool value, string uniquePopupId, out bool settingsClicked)
	{
		settingsClicked = false;
		string text = "##ToggleWithSettings_" + uniquePopupId;
		bool flag = !ImGuiWidgets._toggleAnimations.ContainsKey(text);
		if (flag)
		{
			ImGuiWidgets._toggleAnimations[text] = (value ? 1f : 0f);
		}
		Vector2 vector = new Vector2(ImGui.GetContentRegionAvail().X - 20f, 50f);
		float num = 50f;
		float num2 = 23f;
		Vector2 vector2 = new Vector2(35f, 30f);
		Vector2 vector3 = new Vector2(30f, 30f);
		float num3 = 5f;
		ImGui.BeginGroup();
		Vector2 cursorScreenPos = ImGui.GetCursorScreenPos();
		ImDrawListPtr windowDrawList = ImGui.GetWindowDrawList();
		Vector2 vector4 = new Vector2(cursorScreenPos.X + 10f, cursorScreenPos.Y);
		Vector2 vector5 = new Vector2(cursorScreenPos.X + vector.X + 10f, cursorScreenPos.Y + vector.Y);
		windowDrawList.AddRectFilled(vector4, vector5, ImGui.GetColorU32(ImGuiWidgets.BgColor), 5f);
		ImGui.SetCursorScreenPos(new Vector2(vector4.X + 10f, vector4.Y + (vector.Y - ImGui.GetTextLineHeight()) / 2f));
		ImGui.Text(label);
		float num4 = vector4.Y + (vector.Y - vector2.Y) / 2f;
		float num5 = vector4.Y + (vector.Y - num2) / 2f;
		float num6 = vector2.X + num3 + num;
		float num7 = vector5.X - 10f - num6;
		Vector2 vector6 = new Vector2(num7, num4);
		Vector2 vector7 = new Vector2(num7 + vector2.X + num3, num5);
		ImGui.SetCursorScreenPos(vector6);
		ImGui.PushID(text + "_SettingsIcon");
		bool flag2 = ImGui.InvisibleButton("##SettingsButton", vector2);
		if (flag2)
		{
			ImGuiWidgets._popupToOpen = uniquePopupId;
			settingsClicked = true;
		}
		windowDrawList.AddRectFilled(vector6, vector6 + vector2, ImGui.GetColorU32(ImGuiWidgets.SettingsIconColor), 5f);
		float num8 = (ImGui.IsItemHovered() ? 1f : 0.8f);
		Vector4 vector8 = new Vector4(num8, num8, num8, 1f);
		ImTextureID image = TextureLoader.GetImage("5");
		Vector2 vector9 = new Vector2(vector6.X + (vector2.X - vector3.X) * 0.5f, vector6.Y + (vector2.Y - vector3.Y) * 0.5f);
		windowDrawList.AddImage(image, vector9, vector9 + vector3, new Vector2(0f, 0f), new Vector2(1f, 1f), ImGui.GetColorU32(vector8));
		ImGui.PopID();
		ImGui.SetCursorScreenPos(vector7);
		ImGui.PushID(text + "_Toggle");
		bool flag3 = false;
		bool flag4 = ImGui.InvisibleButton("##ToggleButton", new Vector2(num, num2));
		if (flag4)
		{
			value = !value;
			flag3 = true;
		}
		ImGui.PopID();
		float num9 = ImGuiWidgets._toggleAnimations[text];
		float num10 = 10f;
		float num11 = ImGui.GetIO().DeltaTime;
		num9 = (value ? Math.Min(num9 + num10 * num11, 1f) : Math.Max(num9 - num10 * num11, 0f));
		ImGuiWidgets._toggleAnimations[text] = num9;
		windowDrawList.AddRectFilled(vector7, new Vector2(vector7.X + num, vector7.Y + num2), ImGui.GetColorU32(ImGuiWidgets.SlotColor), 5f);
		Vector4 vector10 = Vector4.Lerp(ImGuiWidgets.KnobColorOff, ImGuiWidgets.KnobColorOn, num9);
		float num12 = vector7.X + (num - num2) * num9;
		Vector2 vector11 = new Vector2(num12, vector7.Y);
		Vector2 vector12 = new Vector2(num12 + num2, vector7.Y + num2);
		windowDrawList.AddRectFilled(vector11, vector12, ImGui.GetColorU32(vector10), 5f);
		ImGui.EndGroup();
		ImGui.Dummy(new Vector2(0f, 15f));
		return flag3;
	}
	public static bool KeyBindInput(string label, ref ImGuiKey keyBind)
	{
		string text = "##KeyBindInputStyled_" + label;
		bool flag = ImGuiWidgets._activeKeybind == text;
		bool flag2 = false;
		Vector2 vector = new Vector2(ImGui.GetContentRegionAvail().X - 20f, 50f);
		float num = 110f;
		float num2 = 35f;
		ImGui.BeginGroup();
		Vector2 cursorScreenPos = ImGui.GetCursorScreenPos();
		ImDrawListPtr windowDrawList = ImGui.GetWindowDrawList();
		Vector2 vector2 = cursorScreenPos;
		vector2.X = cursorScreenPos.X + 10f;
		Vector2 vector3 = vector2;
		Vector2 vector4 = new Vector2(cursorScreenPos.X + vector.X + 10f, cursorScreenPos.Y + vector.Y);
		windowDrawList.AddRectFilled(vector3, vector4, ImGui.GetColorU32(ImGuiWidgets.BgColor), 5f);
		ImGui.SetCursorScreenPos(new Vector2(vector3.X + 10f, vector3.Y + (vector.Y - ImGui.GetTextLineHeight()) / 2f));
		ImGui.Text(label);
		Vector2 vector5 = new Vector2(vector4.X - num - 10f, vector3.Y + (vector.Y - num2) / 2f);
		Vector2 vector6 = new Vector2(num, num2);
		ImGui.SetCursorScreenPos(vector5);
		ImGui.PushID(text);
		bool flag3 = ImGui.InvisibleButton("##BindTrigger", vector6);
		if (flag3)
		{
			bool flag4 = flag;
			if (flag4)
			{
				ImGuiWidgets._activeKeybind = null;
			}
			else
			{
				bool flag5 = ImGuiWidgets._activeKeybind == null;
				if (flag5)
				{
					ImGuiWidgets._activeKeybind = text;
				}
			}
		}
		ImGui.PopID();
		Vector4 vector7 = (flag ? ImGuiWidgets.BindWaitColor : (ImGui.IsItemHovered() ? ImGuiWidgets.ButtonHover : ImGuiWidgets.SettingsIconColor));
		windowDrawList.AddRectFilled(vector5, vector5 + vector6, ImGui.GetColorU32(vector7), 5f);
		string text2 = (flag ? "Press Key..." : ((keyBind == ImGuiKey.None) ? "[None]" : keyBind.ToString()));
		Vector2 vector8 = ImGui.CalcTextSize(text2);
		Vector2 vector9 = new Vector2(vector5.X + (vector6.X - vector8.X) * 0.5f, vector5.Y + (vector6.Y - vector8.Y) * 0.5f);
		windowDrawList.AddText(vector9, ImGui.GetColorU32(ImGuiWidgets.TextColor), text2);
		bool flag6 = flag;
		if (flag6)
		{
			ImGuiKey imGuiKey = ImGuiKey.None;
			for (ImGuiKey imGuiKey2 = ImGuiKey.NamedKeyBegin; imGuiKey2 < ImGuiKey.NamedKeyEnd; imGuiKey2++)
			{
				bool flag7 = imGuiKey2 >= ImGuiKey.ModCtrl && imGuiKey2 <= ImGuiKey.ModSuper;
				if (!flag7)
				{
					bool flag8 = imGuiKey2 == ImGuiKey.Delete || imGuiKey2 == ImGuiKey.Backspace || imGuiKey2 == ImGuiKey.Escape;
					if (!flag8)
					{
						bool flag9 = ImGui.IsKeyPressed(imGuiKey2, false);
						if (flag9)
						{
							imGuiKey = imGuiKey2;
							break;
						}
					}
				}
			}
			bool flag10 = imGuiKey == ImGuiKey.None;
			if (flag10)
			{
				for (int i = 0; i < 5; i++)
				{
					bool flag11 = ImGui.IsMouseClicked((ImGuiMouseButton)i, false);
					if (flag11)
					{
						if (!true)
						{
						}
						ImGuiKey imGuiKey3;
						switch (i)
						{
						case 0:
							imGuiKey3 = ImGuiKey.MouseLeft;
							break;
						case 1:
							imGuiKey3 = ImGuiKey.MouseRight;
							break;
						case 2:
							imGuiKey3 = ImGuiKey.MouseMiddle;
							break;
						case 3:
							imGuiKey3 = ImGuiKey.MouseX1;
							break;
						case 4:
							imGuiKey3 = ImGuiKey.MouseX2;
							break;
						default:
							imGuiKey3 = ImGuiKey.None;
							break;
						}
						if (!true)
						{
						}
						imGuiKey = imGuiKey3;
						bool flag12 = imGuiKey > ImGuiKey.None;
						if (flag12)
						{
							break;
						}
					}
				}
			}
			bool flag13 = imGuiKey > ImGuiKey.None;
			if (flag13)
			{
				bool flag14 = keyBind != imGuiKey;
				if (flag14)
				{
					keyBind = imGuiKey;
					flag2 = true;
				}
				ImGuiWidgets._activeKeybind = null;
			}
			else
			{
				bool flag15 = ImGui.IsKeyPressed(ImGuiKey.Escape, false);
				if (flag15)
				{
					ImGuiWidgets._activeKeybind = null;
				}
				else
				{
					bool flag16 = ImGui.IsKeyPressed(ImGuiKey.Backspace, false);
					if (flag16)
					{
						bool flag17 = keyBind > ImGuiKey.None;
						if (flag17)
						{
							keyBind = ImGuiKey.None;
							flag2 = true;
						}
						ImGuiWidgets._activeKeybind = null;
					}
				}
			}
		}
		ImGui.EndGroup();
		ImGui.Dummy(new Vector2(0f, 15f));
		return flag2;
	}
	public static void RequestOpenSettingsPopup(string uniquePopupId)
	{
		ImGuiWidgets._popupToOpen = uniquePopupId;
	}
	public static void RenderSettingsPopups(Action<string> renderPopupContentDelegate)
	{
		bool flag = ImGuiWidgets._popupToOpen != null;
		if (flag)
		{
			ImGuiWidgets._activeSettingsPopups[ImGuiWidgets._popupToOpen] = true;
			ImGui.SetNextWindowFocus();
			ImGuiWidgets._popupToOpen = null;
		}
		List<string> list = ImGuiWidgets._activeSettingsPopups.Keys.ToList<string>();
		foreach (string text in list)
		{
			bool flag3;
			bool flag2 = ImGuiWidgets._activeSettingsPopups.TryGetValue(text, out flag3) && flag3;
			if (flag2)
			{
				string text2 = text ?? "";
				ImGui.SetNextWindowSizeConstraints(new Vector2(350f, 0f), new Vector2(350f, float.MaxValue));
				bool flag4 = true;
				ImGui.PushFont(FontManager.GetFont("global"));
				bool flag5 = ImGui.Begin(text2, ref flag4, ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoDocking);
				if (flag5)
				{
					if (renderPopupContentDelegate != null)
					{
						renderPopupContentDelegate(text);
					}
				}
				ImGui.End();
				ImGui.PopFont();
				bool flag6 = !flag4;
				if (flag6)
				{
					ImGuiWidgets._activeSettingsPopups[text] = false;
				}
			}
		}
		ImGuiWidgets._activeSettingsPopups.Where((KeyValuePair<string, bool> kv) => !kv.Value).ToList<KeyValuePair<string, bool>>().ForEach(delegate(KeyValuePair<string, bool> kv)
		{
			ImGuiWidgets._activeSettingsPopups.Remove(kv.Key);
		});
	}
	public static bool SliderInt(string label, ref int value, int min, int max)
	{
		return ImGuiWidgets.CustomSlider(label, ref value, min, max, false);
	}
	public static bool SliderFloat(string label, ref float value, float min, float max)
	{
		return ImGuiWidgets.CustomSlider(label, ref value, min, max, true);
	}
	private static bool CustomSlider(string label, ref int intValue, int min, int max, bool isFloat)
	{
		float num = (float)intValue;
		ImGuiWidgets.CustomSlider(label, ref num, (float)min, (float)max, isFloat);
		int num2 = (int)Math.Round((double)num);
		bool flag = num2 != intValue;
		bool flag2;
		if (flag)
		{
			intValue = num2;
			flag2 = true;
		}
		else
		{
			flag2 = false;
		}
		return flag2;
	}
	private unsafe static bool CustomSlider(string label, ref float value, float min, float max, bool isFloat)
	{
		string text = "##CustomSlider_" + label + (isFloat ? "_float" : "_int");
		bool flag = !ImGuiWidgets._sliderAnimations.ContainsKey(text);
		if (flag)
		{
			ImGuiWidgets._sliderAnimations[text] = (value - min) / (max - min);
		}
		Vector2 vector = new Vector2(ImGui.GetContentRegionAvail().X - 20f, 70f);
		float num = vector.X - 20f;
		float num2 = 20f;
		ImGui.BeginGroup();
		ImDrawListPtr windowDrawList = ImGui.GetWindowDrawList();
		Vector2 cursorScreenPos = ImGui.GetCursorScreenPos();
		Vector2 vector2 = new Vector2(cursorScreenPos.X + 10f, cursorScreenPos.Y);
		Vector2 vector3 = new Vector2(cursorScreenPos.X + vector.X + 10f, cursorScreenPos.Y + vector.Y);
		windowDrawList.AddRectFilled(vector2, vector3, ImGui.GetColorU32(ImGuiWidgets.BgColor), 5f);
		float num3 = vector2.X + 10f;
		float num4 = vector2.Y + 5f;
		windowDrawList.AddText(new Vector2(num3, num4), ImGui.GetColorU32(ImGuiWidgets.TextColor), label);
		string text2 = (isFloat ? value.ToString("0.00") : ((int)Math.Round((double)value)).ToString());
		Vector2 vector4 = ImGui.CalcTextSize(text2);
		float num5 = vector3.X - vector4.X - 10f;
		float num6 = vector2.Y + 5f;
		windowDrawList.AddText(new Vector2(num5, num6), ImGui.GetColorU32(ImGuiWidgets.TextColor), text2);
		float num7 = vector2.X + 10f;
		float num8 = vector2.Y + 40f;
		Vector2 vector5 = new Vector2(num7, num8);
		ImGui.PushID(text);
		ImGui.SetCursorScreenPos(vector5);
		bool flag2 = false;
		bool flag3 = ImGui.InvisibleButton("##SliderButton", new Vector2(num, num2));
		if (flag3)
		{
			Vector2 vector6 = ImGui.GetIO().MousePos;
			float num9 = Math.Clamp((vector6.X - vector5.X) / num, 0f, 1f);
			float num10 = min + num9 * (max - min);
			bool flag4 = Math.Abs(value - num10) > float.Epsilon;
			if (flag4)
			{
				value = num10;
				flag2 = true;
			}
		}
		ImGui.PopID();
		bool flag5 = ImGui.IsItemActive();
		bool flag6 = false;
		bool flag7 = flag5;
		if (flag7)
		{
			Vector2 vector7 = ImGui.GetIO().MousePos;
			float num11 = Math.Clamp((vector7.X - vector5.X) / num, 0f, 1f);
			float num12 = min + num11 * (max - min);
			bool flag8 = Math.Abs(value - num12) > float.Epsilon;
			if (flag8)
			{
				value = num12;
				flag6 = true;
			}
		}
		float num13 = (value - min) / (max - min);
		float num14 = ImGuiWidgets._sliderAnimations[text];
		float num15 = num13;
		float num16 = 10f;
		float num17 = ImGui.GetIO().DeltaTime;
		float num18 = num16 * num17;
		bool flag9 = num14 < num15;
		if (flag9)
		{
			num14 = Math.Min(num14 + num18, num15);
		}
		else
		{
			bool flag10 = num14 > num15;
			if (flag10)
			{
				num14 = Math.Max(num14 - num18, num15);
			}
		}
		ImGuiWidgets._sliderAnimations[text] = num14;
		float num19 = num2;
		float num20 = vector5.X + (num - num19) * num14;
		float y = vector5.Y;
		Vector2 vector8 = new Vector2(num20, y);
		windowDrawList.AddRectFilled(vector5, new Vector2(vector5.X + num, vector5.Y + num2), ImGui.GetColorU32(ImGuiWidgets.SlotColor), 5f);
		windowDrawList.AddRectFilled(new Vector2(vector5.X, vector5.Y), new Vector2(vector5.X + num * num14, vector5.Y + num2), ImGui.GetColorU32(ImGuiWidgets.KnobColorOn), 5f);
		windowDrawList.AddRectFilled(vector8, new Vector2(vector8.X + num19, vector8.Y + num2), ImGui.GetColorU32(ImGuiWidgets.SliderFillColor), 3f);
		windowDrawList.AddRect(vector5, new Vector2(vector5.X + num, vector5.Y + num2), ImGuiWidgets.BorderColorU32, 5f, ImDrawFlags.None, 1f);
		windowDrawList.AddRect(new Vector2(vector8.X, vector8.Y), new Vector2(vector8.X + num19, vector8.Y + num2), ImGuiWidgets.BorderColorU32, 3f, ImDrawFlags.None, 1f);
		ImGui.EndGroup();
		ImGui.Dummy(new Vector2(0f, 15f));
		return flag2 || flag6;
	}
	public static bool SliderIntWithSettings(string label, ref int intValue, int min, int max, string uniquePopupId, out bool settingsClicked)
	{
		float num = (float)intValue;
		bool flag = ImGuiWidgets.CustomSliderWithSettings(label, ref num, (float)min, (float)max, false, uniquePopupId, out settingsClicked);
		int num2 = (int)MathF.Round(num);
		bool flag2 = num2 != intValue;
		bool flag3;
		if (flag2)
		{
			intValue = num2;
			flag3 = true;
		}
		else
		{
			flag3 = flag;
		}
		return flag3;
	}
	public static bool SliderFloatWithSettings(string label, ref float value, float min, float max, string uniquePopupId, out bool settingsClicked)
	{
		return ImGuiWidgets.CustomSliderWithSettings(label, ref value, min, max, true, uniquePopupId, out settingsClicked);
	}
	private unsafe static bool CustomSliderWithSettings(string label, ref float value, float min, float max, bool isFloat, string uniquePopupId, out bool settingsClicked)
	{
		settingsClicked = false;
		string text = "##CustomSlider_" + uniquePopupId + (isFloat ? "_float" : "_int");
		bool flag = !ImGuiWidgets._sliderAnimations.ContainsKey(text);
		if (flag)
		{
			ImGuiWidgets._sliderAnimations[text] = (value - min) / (max - min);
		}
		Vector2 vector = new Vector2(ImGui.GetContentRegionAvail().X - 20f, 70f);
		float num = vector.X - 20f;
		float num2 = 20f;
		Vector2 vector2 = new Vector2(32f, 30f);
		Vector2 vector3 = new Vector2(30f, 30f);
		float num3 = 5f;
		ImGui.BeginGroup();
		ImDrawListPtr windowDrawList = ImGui.GetWindowDrawList();
		Vector2 cursorScreenPos = ImGui.GetCursorScreenPos();
		Vector2 vector4 = new Vector2(cursorScreenPos.X + 10f, cursorScreenPos.Y);
		Vector2 vector5 = new Vector2(cursorScreenPos.X + vector.X + 10f, cursorScreenPos.Y + vector.Y);
		windowDrawList.AddRectFilled(vector4, vector5, ImGui.GetColorU32(ImGuiWidgets.BgColor), 5f);
		Vector2 vector6 = new Vector2(vector4.X + 10f, vector4.Y + 5f);
		windowDrawList.AddText(vector6, ImGui.GetColorU32(ImGuiWidgets.TextColor), label);
		string text2 = (isFloat ? value.ToString("0.00") : ((int)Math.Round((double)value)).ToString());
		Vector2 vector7 = ImGui.CalcTextSize(text2);
		float num4 = vector5.X - 10f - vector2.X;
		float num5 = vector4.Y + 5f + (ImGui.GetTextLineHeight() - vector2.Y) * 0.5f;
		Vector2 vector8 = new Vector2(num4, num5);
		float num6 = num4 - num3 - vector7.X;
		float num7 = vector4.Y + 5f;
		Vector2 vector9 = new Vector2(num6, num7);
		windowDrawList.AddText(vector9, ImGui.GetColorU32(ImGuiWidgets.TextColor), text2);
		ImGui.SetCursorScreenPos(vector8);
		ImGui.PushID(text + "_SettingsIcon");
		bool flag2 = ImGui.InvisibleButton("##SettingsButton", vector2);
		if (flag2)
		{
			ImGuiWidgets._popupToOpen = uniquePopupId;
			settingsClicked = true;
		}
		windowDrawList.AddRectFilled(vector8, vector8 + vector2, ImGui.GetColorU32(ImGuiWidgets.SettingsIconColor), 5f);
		float num8 = (ImGui.IsItemHovered() ? 1f : 0.8f);
		Vector4 vector10 = new Vector4(num8, num8, num8, 1f);
		Vector2 vector11 = new Vector2(vector8.X + (vector2.X - vector3.X) * 0.5f, vector8.Y + (vector2.Y - vector3.Y) * 0.5f);
		ImTextureID image = TextureLoader.GetImage("5");
		windowDrawList.AddImage(image, vector11, vector11 + vector3, new Vector2(0f, 0f), new Vector2(1f, 1f), ImGui.GetColorU32(vector10));
		ImGui.PopID();
		Vector2 vector12 = new Vector2(vector4.X + 10f, vector4.Y + 40f);
		ImGui.PushID(text);
		ImGui.SetCursorScreenPos(vector12);
		bool flag3 = false;
		bool flag4 = ImGui.InvisibleButton("##SliderButton", new Vector2(num, num2));
		if (flag4)
		{
			Vector2 vector13 = ImGui.GetIO().MousePos;
			float num9 = Math.Clamp((vector13.X - vector12.X) / num, 0f, 1f);
			float num10 = min + num9 * (max - min);
			bool flag5 = Math.Abs(num10 - value) > float.Epsilon;
			if (flag5)
			{
				value = num10;
				flag3 = true;
			}
		}
		ImGui.PopID();
		bool flag6 = false;
		bool flag7 = ImGui.IsItemActive();
		if (flag7)
		{
			Vector2 vector14 = ImGui.GetIO().MousePos;
			float num11 = Math.Clamp((vector14.X - vector12.X) / num, 0f, 1f);
			float num12 = min + num11 * (max - min);
			bool flag8 = Math.Abs(num12 - value) > float.Epsilon;
			if (flag8)
			{
				value = num12;
				flag6 = true;
			}
		}
		float num13 = (value - min) / (max - min);
		float num14 = ImGuiWidgets._sliderAnimations[text];
		float num15 = 10f * ImGui.GetIO().DeltaTime;
		num14 = ((num14 < num13) ? Math.Min(num14 + num15, num13) : Math.Max(num14 - num15, num13));
		ImGuiWidgets._sliderAnimations[text] = num14;
		windowDrawList.AddRectFilled(vector12, new Vector2(vector12.X + num, vector12.Y + num2), ImGui.GetColorU32(ImGuiWidgets.SlotColor), 5f);
		windowDrawList.AddRectFilled(vector12, new Vector2(vector12.X + num * num14, vector12.Y + num2), ImGui.GetColorU32(ImGuiWidgets.KnobColorOn), 5f);
		float num16 = num2;
		float num17 = vector12.X + (num - num16) * num14;
		Vector2 vector15 = new Vector2(num17, vector12.Y);
		windowDrawList.AddRectFilled(vector15, new Vector2(vector15.X + num16, vector15.Y + num2), ImGui.GetColorU32(ImGuiWidgets.SliderFillColor), 3f);
		windowDrawList.AddRect(vector12, new Vector2(vector12.X + num, vector12.Y + num2), ImGuiWidgets.BorderColorU32, 5f, ImDrawFlags.None, 1f);
		windowDrawList.AddRect(vector15, new Vector2(vector15.X + num16, vector15.Y + num2), ImGuiWidgets.BorderColorU32, 3f, ImDrawFlags.None, 1f);
		ImGui.EndGroup();
		ImGui.Dummy(new Vector2(0f, 15f));
		return flag3 || flag6;
	}
	public static bool Button(string label, float? customWidth = null, float? customHeight = null, bool useDummy = true)
	{
		string text = "##Button_" + label;
		string text2 = label;
		int num = label.IndexOf("##", StringComparison.Ordinal);
		bool flag = num != -1;
		if (flag)
		{
			text2 = label.Substring(0, num);
		}
		float num2 = customWidth ?? (ImGui.GetContentRegionAvail().X - 20f);
		float num3 = 60f;
		Vector2 vector = new Vector2(num2, num3);
		float num4 = num2;
		float valueOrDefault = customHeight.GetValueOrDefault(50f);
		ImGui.BeginGroup();
		ImDrawListPtr windowDrawList = ImGui.GetWindowDrawList();
		Vector2 cursorScreenPos = ImGui.GetCursorScreenPos();
		Vector2 vector2 = new Vector2(cursorScreenPos.X + 10f, cursorScreenPos.Y + (vector.Y - valueOrDefault) / 2f);
		ImGui.SetCursorScreenPos(vector2);
		ImGui.PushID(text);
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = ImGui.InvisibleButton("##ButtonInner", new Vector2(num4, valueOrDefault));
		if (flag4)
		{
			flag2 = true;
		}
		bool flag5 = ImGui.IsItemActive();
		if (flag5)
		{
			flag3 = true;
		}
		ImGui.PopID();
		Vector4 vector3 = (flag3 ? ImGuiWidgets.ButtonActive2 : ImGuiWidgets.ButtonNormal);
		windowDrawList.AddRectFilled(vector2, new Vector2(vector2.X + num4, vector2.Y + valueOrDefault), ImGui.GetColorU32(vector3), 5f);
		Vector2 vector4 = ImGui.CalcTextSize(text2);
		Vector2 vector5 = new Vector2(vector2.X + (num4 - vector4.X) * 0.5f, vector2.Y + (valueOrDefault - vector4.Y) * 0.5f);
		windowDrawList.AddText(vector5, ImGui.GetColorU32(ImGuiWidgets.TextColor), text2);
		windowDrawList.AddRect(vector2, new Vector2(vector2.X + num4, vector2.Y + valueOrDefault), ImGuiWidgets.BorderColorU32, 5f, ImDrawFlags.None, 1f);
		ImGui.EndGroup();
		if (useDummy)
		{
			ImGui.Dummy(new Vector2(0f, 10f));
		}
		return flag2;
	}
	public static bool Combo(string label, ref int selectedIndex, string[] items)
	{
		string text = "##Combo_" + label;
		float num = 10f;
		float num2 = 55f;
		float num3 = 10f;
		float num4 = 35f;
		ImGui.BeginGroup();
		ImDrawListPtr windowDrawList = ImGui.GetWindowDrawList();
		Vector2 cursorScreenPos = ImGui.GetCursorScreenPos();
		Vector2 vector = new Vector2(cursorScreenPos.X + num, cursorScreenPos.Y);
		Vector2 vector2 = new Vector2(cursorScreenPos.X + ImGui.GetContentRegionAvail().X - num, cursorScreenPos.Y + num2);
		windowDrawList.AddRectFilled(vector, vector2, ImGui.GetColorU32(ImGuiWidgets.BgColor), 5f);
		Vector2 vector3 = ImGui.CalcTextSize(label);
		float num5 = vector.X + num;
		float num6 = vector.Y + (num2 - vector3.Y) / 2f;
		windowDrawList.AddText(new Vector2(num5, num6), ImGui.GetColorU32(ImGuiWidgets.TextColor), label);
		bool flag = selectedIndex < 0 || items.Length == 0;
		if (flag)
		{
			selectedIndex = 0;
		}
		else
		{
			bool flag2 = selectedIndex >= items.Length;
			if (flag2)
			{
				selectedIndex = items.Length - 1;
			}
		}
		float num7 = vector2.X - vector.X;
		float num8 = num7 - 2f * num - vector3.X - num3;
		Vector2 vector4 = new Vector2(num5 + vector3.X + num3, vector.Y + (num2 - num4) / 2f);
		ImGui.PushStyleColor(ImGuiCol.FrameBg, ImGuiWidgets.FrameBg);
		ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, ImGuiWidgets.FrameBgHover);
		ImGui.PushStyleColor(ImGuiCol.FrameBgActive, ImGuiWidgets.FrameBgActive);
		ImGui.PushStyleColor(ImGuiCol.PopupBg, ImGuiWidgets.BgColor);
		ImGui.PushStyleColor(ImGuiCol.Text, ImGuiWidgets.TextColor);
		ImGui.PushStyleColor(ImGuiCol.TextSelectedBg, ImGuiWidgets.TextSelectedBg);
		ImGui.PushStyleColor(ImGuiCol.Header, ImGuiWidgets.FrameBg);
		ImGui.PushStyleColor(ImGuiCol.HeaderHovered, ImGuiWidgets.FrameBgHover);
		ImGui.PushStyleColor(ImGuiCol.HeaderActive, ImGuiWidgets.FrameBgActive);
		ImGui.SetCursorScreenPos(vector4);
		ImGui.PushID(text);
		ImGui.SetNextItemWidth(num8);
		bool flag3 = false;
		string text2 = ((items.Length != 0 && selectedIndex >= 0 && selectedIndex < items.Length) ? items[selectedIndex] : "");
		bool flag4 = ImGui.BeginCombo("##ComboInner", text2, ImGuiComboFlags.HeightLargest);
		if (flag4)
		{
			for (int i = 0; i < items.Length; i++)
			{
				bool flag5 = i == selectedIndex;
				bool flag6 = ImGui.Selectable(items[i], flag5);
				if (flag6)
				{
					selectedIndex = i;
				}
				bool flag7 = flag5;
				if (flag7)
				{
					ImGui.SetItemDefaultFocus();
				}
			}
			ImGui.EndCombo();
			flag3 = true;
		}
		ImGui.PopID();
		ImGui.PopStyleColor(9);
		ImGui.EndGroup();
		ImGui.Dummy(new Vector2(0f, 15f));
		return flag3;
	}
	public static bool InputText(string label, ref string text)
	{
		string text2 = "##InputTextCustom_" + label;
		float num = 10f;
		float num2 = 50f;
		float num3 = 10f;
		ImGui.BeginGroup();
		ImDrawListPtr windowDrawList = ImGui.GetWindowDrawList();
		Vector2 cursorScreenPos = ImGui.GetCursorScreenPos();
		Vector2 vector = new Vector2(cursorScreenPos.X + num, cursorScreenPos.Y);
		Vector2 vector2 = new Vector2(cursorScreenPos.X + ImGui.GetContentRegionAvail().X - num, cursorScreenPos.Y + num2);
		windowDrawList.AddRectFilled(vector, vector2, ImGui.GetColorU32(ImGuiWidgets.BgColor), 5f);
		Vector2 vector3 = ImGui.CalcTextSize(label);
		float num4 = vector.X + num;
		float num5 = vector.Y + (num2 - vector3.Y) / 2f;
		windowDrawList.AddText(new Vector2(num4, num5), ImGui.GetColorU32(ImGuiWidgets.TextColor), label);
		float num6 = vector2.X - vector.X;
		float num7 = num6 - 2f * num - vector3.X - num3;
		Vector2 vector4 = new Vector2(num4 + vector3.X + num3, vector.Y + (num2 - ImGui.GetTextLineHeight() - 5f) / 2f);
		ImGui.PushStyleColor(ImGuiCol.FrameBg, ImGuiWidgets.FrameBg);
		ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, ImGuiWidgets.FrameBgHover);
		ImGui.PushStyleColor(ImGuiCol.FrameBgActive, ImGuiWidgets.FrameBgActive);
		ImGui.PushStyleColor(ImGuiCol.Text, ImGuiWidgets.TextColor);
		ImGui.SetCursorScreenPos(vector4);
		ImGui.PushID(text2);
		ImGui.SetNextItemWidth(num7);
		bool flag = ImGui.InputText("##InputTextInner", ref text, (UIntPtr)((IntPtr)256), ImGuiInputTextFlags.EnterReturnsTrue);
		ImGui.PopID();
		ImGui.PopStyleColor(4);
		ImGui.EndGroup();
		ImGui.Dummy(new Vector2(0f, 10f));
		return flag;
	}
	public static bool InputTextWithColor(string id, ref string text, ref Vector4 color, out bool deletePressed, float colorPickerWidth = 40f, float buttonWidth = 40f)
	{
		deletePressed = false;
		string text2 = "##InputTextWithColor_" + id;
		float num = 10f;
		float num2 = 50f;
		float num3 = 5f;
		float num4 = ImGui.GetTextLineHeight() + 5f;
		ImGui.BeginGroup();
		ImDrawListPtr windowDrawList = ImGui.GetWindowDrawList();
		Vector2 cursorScreenPos = ImGui.GetCursorScreenPos();
		Vector2 vector = new Vector2(cursorScreenPos.X + num, cursorScreenPos.Y);
		Vector2 vector2 = new Vector2(cursorScreenPos.X + ImGui.GetContentRegionAvail().X - num, cursorScreenPos.Y + num2);
		windowDrawList.AddRectFilled(vector, vector2, ImGui.GetColorU32(ImGuiWidgets.BgColor), 5f);
		float num5 = vector2.X - vector.X - 2f * num;
		float num6 = num5 - colorPickerWidth - buttonWidth - 2f * num3;
		float num7 = (num2 - num4) / 2f;
		float num8 = vector.X + num;
		float num9 = vector.Y + num7;
		ImGui.SetCursorScreenPos(new Vector2(num8, num9));
		ImGui.SetNextItemWidth(num6);
		ImGui.PushID(text2 + "_input");
		bool flag = ImGui.InputText("##Input", ref text, (UIntPtr)((IntPtr)256));
		ImGui.PopID();
		float num10 = num8 + num6 + num3;
		ImGui.SetCursorScreenPos(new Vector2(num10, num9));
		ImGui.SetNextItemWidth(colorPickerWidth);
		ImGui.PushID(text2 + "_color");
		bool flag2 = ImGui.ColorEdit4("##Color", ref color, ImGuiColorEditFlags.NoInputs | ImGuiColorEditFlags.NoLabel);
		ImGui.PopID();
		float num11 = num10 + colorPickerWidth + num3;
		ImGui.SetCursorScreenPos(new Vector2(num11, num9));
		ImGui.PushStyleColor(ImGuiCol.Button, ImGuiWidgets.FrameBgActive);
		ImGui.PushStyleColor(ImGuiCol.ButtonHovered, ImGuiWidgets.ButtonHover);
		ImGui.PushStyleColor(ImGuiCol.ButtonActive, ImGuiWidgets.ButtonActive);
		ImGui.PushID(text2 + "_btn");
		bool flag3 = ImGui.Button("-", new Vector2(buttonWidth, num4));
		if (flag3)
		{
			deletePressed = true;
		}
		ImGui.PopID();
		ImGui.PopStyleColor(3);
		ImGui.EndGroup();
		ImGui.Dummy(new Vector2(0f, 10f));
		return flag || flag2;
	}
	public static bool ColorPicker(string label, ref Vector4 color)
	{
		string text = "##ColorPickerCustom_" + label;
		float num = 10f;
		float num2 = 50f;
		float num3 = 40f;
		float num4 = ImGui.GetTextLineHeight() + 5f;
		ImGui.BeginGroup();
		ImDrawListPtr windowDrawList = ImGui.GetWindowDrawList();
		Vector2 cursorScreenPos = ImGui.GetCursorScreenPos();
		Vector2 vector = new Vector2(cursorScreenPos.X + num, cursorScreenPos.Y);
		Vector2 vector2 = new Vector2(cursorScreenPos.X + ImGui.GetContentRegionAvail().X - num, cursorScreenPos.Y + num2);
		windowDrawList.AddRectFilled(vector, vector2, ImGui.GetColorU32(ImGuiWidgets.BgColor), 5f);
		Vector2 vector3 = ImGui.CalcTextSize(label);
		float num5 = vector.X + num;
		float num6 = vector.Y + (num2 - vector3.Y) / 2f;
		windowDrawList.AddText(new Vector2(num5, num6), ImGui.GetColorU32(ImGuiWidgets.TextColor), label);
		Vector2 vector4 = new Vector2(vector2.X - num - num3, vector.Y + (num2 - num4) / 2f);
		ImGui.PushStyleColor(ImGuiCol.FrameBg, ImGuiWidgets.FrameBg);
		ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, ImGuiWidgets.FrameBg);
		ImGui.PushStyleColor(ImGuiCol.FrameBgActive, ImGuiWidgets.FrameBg);
		ImGui.PushStyleColor(ImGuiCol.Text, ImGuiWidgets.TextColor);
		ImGui.SetCursorScreenPos(vector4);
		ImGui.PushID(text);
		ImGui.SetNextItemWidth(num3);
		bool flag = ImGui.ColorEdit4("##ColorEditInner", ref color, ImGuiColorEditFlags.NoInputs | ImGuiColorEditFlags.NoLabel);
		ImGui.PopID();
		ImGui.PopStyleColor(4);
		ImGui.EndGroup();
		ImGui.Dummy(new Vector2(0f, 15f));
		return flag;
	}
	public static void CenteredTextInRect(string text, Vector2 rectSize)
	{
		Vector2 vector = ImGui.CalcTextSize(text);
		Vector2 cursorPos = ImGui.GetCursorPos();
		float num = cursorPos.X + (rectSize.X - vector.X) * 0.5f;
		float num2 = cursorPos.Y + (rectSize.Y - vector.Y) * 0.5f;
		ImGui.SetCursorPos(new Vector2(num, num2));
		ImGui.Text(text);
	}
	public static bool IsKeyPressed(ImGuiKey key, bool repeat = false)
	{
		bool flag = key == ImGuiKey.None;
		return !flag && ImGui.IsKeyPressed(key, repeat);
	}
	private static readonly Dictionary<string, float> _toggleAnimations = new Dictionary<string, float>();
	private static readonly Dictionary<string, float> _sliderAnimations = new Dictionary<string, float>();
	
	private static string _activeKeybind;
	private static readonly Dictionary<string, bool> _activeSettingsPopups = new Dictionary<string, bool>();
	
	private static string _popupToOpen;
	private static readonly Vector4 BgColor = new Vector4(0.10980392f, 0.11372549f, 0.1254902f, 1f);
	private static readonly Vector4 SlotColor = new Vector4(0.14117648f, 0.14509805f, 0.16078432f, 1f);
	private static readonly Vector4 KnobColorOff = new Vector4(0.78039217f, 0.78039217f, 0.78039217f, 1f);
	private static readonly Vector4 KnobColorOn = new Vector4(0.38039216f, 0.5019608f, 0.92941177f, 1f);
	private static readonly Vector4 SettingsIconColor = new Vector4(0.19607843f, 0.19607843f, 0.21568628f, 1f);
	private static readonly Vector4 ButtonHover = new Vector4(0.27450982f, 0.27450982f, 0.29411766f, 1f);
	private static readonly Vector4 ButtonActive = new Vector4(0.3529412f, 0.3529412f, 0.37254903f, 1f);
	private static readonly Vector4 BindWaitColor = new Vector4(0.92941177f, 0.5019608f, 0.38039216f, 1f);
	private static readonly Vector4 MediumGray = new Vector4(0.5882353f, 0.5882353f, 0.5882353f, 1f);
	private static readonly Vector4 SliderFillColor = new Vector4(0.78431374f, 0.78431374f, 0.78431374f, 1f);
	private static readonly Vector4 ButtonNormal = new Vector4(0.13725491f, 0.14117648f, 0.15686275f, 1f);
	private static readonly Vector4 ButtonActive2 = new Vector4(0.09803922f, 0.101960786f, 0.11764706f, 1f);
	private static readonly Vector4 FrameBg = new Vector4(0.07058824f, 0.07450981f, 0.09019608f, 1f);
	private static readonly Vector4 FrameBgHover = new Vector4(0.13725491f, 0.13725491f, 0.13725491f, 1f);
	private static readonly Vector4 FrameBgActive = new Vector4(0.1764706f, 0.1764706f, 0.1764706f, 1f);
	private static readonly Vector4 TextSelectedBg = new Vector4(0.19607843f, 0.19607843f, 0.19607843f, 1f);
	private static readonly Vector4 TextColor = Vector4.One;
	private static readonly Vector4 BorderColor = new Vector4(0f, 0f, 0f, 0.2f);
	private static readonly uint BorderColorU32 = ImGui.GetColorU32(ImGuiWidgets.BorderColor);
}
