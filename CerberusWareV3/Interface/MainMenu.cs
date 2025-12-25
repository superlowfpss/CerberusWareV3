using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using CerberusWareV3.Configuration;
using CerberusWareV3.Localization;
using CerberusWareV3.MyImGui;
using Content.Shared.Chat;
using Hexa.NET.ImGui;
using Microsoft.CSharp.RuntimeBinder;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Network;
using Robust.Shared.Player;
[CompilerGenerated]
public class MainMenu : IOverlay
{
	public MainMenu(IEntitySystemManager systemManager)
	{
		this._configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CerberusWare");
		this._componentManager = new ComponentManager();
		this._spammerSystem = new SpammerSystem();
		this._mainController = new MainController();
		this._tabAlpha = new Dictionary<int, float>();
		this._tabSwitchSpeed = 5f;
		this._logs = new Dictionary<string, MainMenu.ConnectStatus>();
		this._playerWindows = new Dictionary<NetUserId, bool>();
		this._renderedPreviews = new HashSet<NetUserId>();
		this._playerSearch = "";
		this._sortOptions = new string[] { "Online", "Offline", "Name Ascending", "Name Desc", "Char Ascending", "Char Descending" };
		this._fonts = new string[][]
		{
			new string[] { "Boxfont Round", "/Fonts/Boxfont-round/Boxfont Round.ttf" },
			new string[] { "NotoSans Regular", "/Fonts/NotoSans/NotoSans-Regular.ttf" },
			new string[] { "NotoSans Bold", "/Fonts/NotoSans/NotoSans-Bold.ttf" },
			new string[] { "NotoSans Italic", "/Fonts/NotoSans/NotoSans-Italic.ttf" }
		};
		IoCManager.InjectDependencies<MainMenu>(this);
		this._playerTracker = systemManager.GetEntitySystem<PlayerTrackerSystem>();
		this._entityPreview = systemManager.GetEntitySystem<EntityPreviewSystem>();
		this.InitializePlayersTab();
		this._tabs = new List<MainMenu.TabInfo>
		{
			new MainMenu.TabInfo("AimBot", new List<string>
			{
				LocalizationManager.GetString("Main_Gun"),
				LocalizationManager.GetString("Main_Melee")
			}, new Action(this.RenderAimBotTab)),
			new MainMenu.TabInfo("Visuals", new List<string>
			{
				LocalizationManager.GetString("Main_Esp"),
				LocalizationManager.GetString("Main_Eye"),
				LocalizationManager.GetString("Main_Fun")
			}, new Action(this.RenderVisualsTab)),
			new MainMenu.TabInfo("Players", new List<string>
			{
				LocalizationManager.GetString("Main_Players"),
				LocalizationManager.GetString("Main_Logs")
			}, new Action(this.RenderPlayersTab)),
			new MainMenu.TabInfo("Misc", new List<string>
			{
				LocalizationManager.GetString("Main_Misc"),
				LocalizationManager.GetString("Main_Spammer")
			}, new Action(this.RenderMiscTab)),
			new MainMenu.TabInfo("Settings", new List<string>
			{
				LocalizationManager.GetString("Main_Settings"),
				LocalizationManager.GetString("Main_Configs")
			}, new Action(this.RenderSettingsTab))
		};
	}
public void Render()
{
    this.HandleInput();
    if (CerberusConfig.Settings.ShowMenu)
    {
        MainMenu.ApplyStyles();
        ImGui.SetNextWindowSize(MainMenu.WindowSize);
        ImGui.Begin("Menu", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoScrollbar);
        if (!this._imagesLoaded)
        {
            this.LoadImage();
            this._imagesLoaded = true;
        }
        ImGui.PushFont(FontManager.GetFont("global"));
        ImGui.PushStyleVar(ImGuiStyleVar.ChildRounding, 0f);
        ImGui.PushStyleColor(ImGuiCol.ChildBg, MainMenu.ChildBgColor);
        ImGui.BeginChild("Name", new Vector2(180f, 70f));
        ImGuiWidgets.CenteredTextInRect("t.me/RobusterHome", new Vector2(180f, 70f));
        ImGui.EndChild();
        ImGui.PopStyleColor();
        ImGui.PopStyleVar();
        ImGui.SameLine(0f, 0f);
        ImGui.PushStyleVar(ImGuiStyleVar.ChildRounding, 0f);
        ImGui.PushStyleColor(ImGuiCol.ChildBg, MainMenu.ChildBgColor2);
        ImGui.BeginChild("SubTabs", new Vector2(700f, 70f));
        
        MainMenu.TabInfo tabInfo = this._tabs[this._currentTab];
        this.RenderSubTabs(tabInfo);
        
        ImGui.EndChild();
        ImGui.PopStyleColor();
        ImGui.PopStyleVar();
        ImGui.PushStyleVar(ImGuiStyleVar.ChildRounding, 0f);
        ImGui.PushStyleColor(ImGuiCol.ChildBg, MainMenu.ChildBgColor2);
        ImGui.BeginChild("Tabs", new Vector2(180f, 500f));
        ImGui.Dummy(new Vector2(0f, 10f));
        for (int i = 0; i < this._tabs.Count; i++)
        {
            this.RenderMainTabButton(this._tabs[i].Name, i);
        }
        ImGui.Separator();
        ImGui.SetCursorPos(new Vector2(10f, 220f));
        ImGui.TextColored(new Vector4(0.23921569f, 0.16862746f, 0.69803923f, 1f), "");
        ImGui.SetCursorPos(new Vector2(5f, 235f));
        ImGui.Text("");
        ImGui.SetCursorPos(new Vector2(15f, 410f));
        ImGui.SetWindowFontScale(0.8f);
        ImGui.TextLinkOpenURL("t.me/RobusterHome", "https://t.me/RobusterHome");
        ImGui.SetWindowFontScale(1f);
        ImGui.SetCursorPos(new Vector2(40f, 440f));
        ImGuiWidgets.CenteredTextInRect(CerberusConfig.NoSavedConfig.Version, new Vector2(100f, 20f));
        if (CerberusConfig.NoSavedConfig.HasAntiCheat)
        {
            ImGui.SetCursorPos(new Vector2(40f, 465f));
            ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(1f, 0f, 0f, 1f));
            ImGuiWidgets.CenteredTextInRect("AntiCheat", new Vector2(100f, 20f));
            ImGui.PopStyleColor();
        }
        ImGui.EndChild();
        ImGui.PopStyleColor();
        ImGui.PopStyleVar();
        ImGui.SameLine(0f, 0f);
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 10f);
        ImGui.BeginChild("Content", new Vector2(690f, 490f));
        Action renderAction = tabInfo.RenderAction;
        if (renderAction != null)
        {
            renderAction();
        }
        ImGui.EndChild();
        ImGui.PopFont();
        ImGui.End();
        ImGuiWidgets.RenderSettingsPopups(new Action<string>(this.RenderSpecificPopupContent));
    }
}
private void LoadImage()
{
	Assembly executingAssembly = Assembly.GetExecutingAssembly();
	string text = "CerberusWareV3.Resources";
	int i = 0;
	while (i <= 5)
	{
		string text2 = $"{text}.{i}.png";
		using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(text2))
		{
			bool flag = manifestResourceStream == null;
			if (!flag)
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					manifestResourceStream.CopyTo(memoryStream);
					byte[] array = memoryStream.ToArray();
					string text3 = i.ToString();
					TextureLoader.AddImage(text3, array, TextureLoader.ImageFilterMode.Linear);
				}
			}
		}
		i++;
	}
}
	private void HandleInput()
	{
		bool flag = ImGuiWidgets.IsKeyPressed(CerberusConfig.Settings.ShowMenuHotKey, false);
		if (flag)
		{
			CerberusConfig.Settings.ShowMenu = !CerberusConfig.Settings.ShowMenu;
		}
		bool flag2 = ImGuiWidgets.IsKeyPressed(CerberusConfig.Eye.FovHotKey, false);
		if (flag2)
		{
			CerberusConfig.Eye.FovEnabled = !CerberusConfig.Eye.FovEnabled;
		}
		bool flag3 = ImGuiWidgets.IsKeyPressed(CerberusConfig.Eye.FullBrightHotKey, false);
		if (flag3)
		{
			CerberusConfig.Eye.FullBrightEnabled = !CerberusConfig.Eye.FullBrightEnabled;
		}
		bool flag4 = ImGuiWidgets.IsKeyPressed(CerberusConfig.Eye.ZoomUpHotKey, true);
		if (flag4)
		{
			CerberusConfig.Eye.Zoom += 0.5f;
		}
		bool flag5 = ImGuiWidgets.IsKeyPressed(CerberusConfig.Eye.ZoomDownHotKey, true) && CerberusConfig.Eye.Zoom > 0.5f;
		if (flag5)
		{
			CerberusConfig.Eye.Zoom -= 0.5f;
		}
	}
	private void RenderSpecificPopupContent(string popupId)
	{
		switch (popupId)
		{
			case "StorageViewer":
				ImGuiWidgets.KeyBindInput(LocalizationManager.GetString("Keybind_StorageViewer"), ref CerberusConfig.StorageViewer.HotKey);
				break;
			case "MeleeAimBot":
				ImGuiWidgets.KeyBindInput(LocalizationManager.GetString("Keybind_LightAttack"), ref CerberusConfig.MeleeAimBot.LightHotKey);
				ImGuiWidgets.KeyBindInput(LocalizationManager.GetString("Keybind_HeavyAttack"), ref CerberusConfig.MeleeAimBot.HeavyHotKey);
				break;
			case "FullBright":
				ImGuiWidgets.KeyBindInput(LocalizationManager.GetString("Keybind_FullBright"), ref CerberusConfig.Eye.FullBrightHotKey);
				break;
			case "Zoom":
				ImGuiWidgets.KeyBindInput(LocalizationManager.GetString("Keybind_Zoom_Up"), ref CerberusConfig.Eye.ZoomUpHotKey);
				ImGuiWidgets.KeyBindInput(LocalizationManager.GetString("Keybind_Zoom_Down"), ref CerberusConfig.Eye.ZoomDownHotKey);
				break;
			case "Settings":
				ImGuiWidgets.KeyBindInput(LocalizationManager.GetString("Keybind_OpenSettings"), ref CerberusConfig.Settings.ShowMenuHotKey);
				break;
			case "FOV":
				ImGuiWidgets.KeyBindInput(LocalizationManager.GetString("Keybind_FOV"), ref CerberusConfig.Eye.FovHotKey);
				break;
			case "GunAimBot":
				ImGuiWidgets.KeyBindInput(LocalizationManager.GetString("Keybind_Fire"), ref CerberusConfig.GunAimBot.HotKey);
				break;
		}
	}
	private unsafe static void ApplyStyles()
	{
		ImGuiStylePtr style = ImGui.GetStyle();
		style.ItemSpacing = new Vector2(0f, 0f);
		style.WindowPadding = new Vector2(0f, 0f);
		style.WindowRounding = 7f;
		style.FrameRounding = 7f;
		style.GrabRounding = 7f;
		style.PopupRounding = 7f;
		style.ChildRounding = 7f;
		style.Colors[2] = MainMenu.WindowBgColor;
		style.Colors[4] = ColorPalette.ThemeBg; 
		style.Colors[21] = MainMenu.ButtonActiveColor;
		style.Colors[22] = MainMenu.ButtonColor;
		style.Colors[23] = MainMenu.ButtonColor;
		style.Colors[10] = ColorPalette.ThemeSecondary;
		style.Colors[11] = ColorPalette.ThemeSecondary;
		style.Colors[12] = ColorPalette.ThemeSecondary;
		style.Colors[7] = ColorPalette.ThemeBg;
		style.Colors[8] = ColorPalette.ThemeButton;
		style.Colors[9] = ColorPalette.ThemeButtonHover;
		style.ButtonTextAlign = new Vector2(0.5f, 0.5f); 
		style.WindowTitleAlign = new Vector2(0.5f, 0.5f);
	}
	private void RenderMainTabButton(string label, int tabIndex)
	{
		bool flag = this._currentTab == tabIndex;
		float x = ImGui.GetContentRegionAvail().X;
		ImGui.SetCursorPosX(ImGui.GetCursorPosX() + (x - 160f) * 0.5f);
		Vector2 vector = new Vector2(160f, 40f);
		DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 1);
		defaultInterpolatedStringHandler.AppendLiteral("##tab_");
		defaultInterpolatedStringHandler.AppendFormatted<int>(tabIndex);
		string text = defaultInterpolatedStringHandler.ToStringAndClear();
		bool flag2 = ImGui.InvisibleButton(text, vector);
		if (flag2)
		{
			this._currentTab = tabIndex;
			this._currentSubTab = 0;
		}
		Vector2 itemRectMin = ImGui.GetItemRectMin();
		Vector2 itemRectMax = ImGui.GetItemRectMax();
		ImDrawListPtr windowDrawList = ImGui.GetWindowDrawList();
		Vector4 vector2 = (flag ? MainMenu.ButtonHoverColor : MainMenu.ButtonActiveColor);
		bool flag3 = ImGui.IsItemHovered();
		if (flag3)
		{
			vector2 = (flag ? MainMenu.ButtonHoverColor : MainMenu.ButtonColor);
		}
		bool flag4 = ImGui.IsItemActive();
		if (flag4)
		{
			vector2 = MainMenu.ButtonHoverColor;
		}
		windowDrawList.AddRectFilled(itemRectMin, itemRectMax, ImGui.ColorConvertFloat4ToU32(vector2), 5f);
		ImTextureID image = TextureLoader.GetImage(tabIndex.ToString());
		bool flag5 = !image.Equals(default(ImTextureID));
		if (flag5)
		{
			Vector2 vector3 = new Vector2(30f, 30f);
			Vector2 vector4 = new Vector2(itemRectMin.X + 5f, itemRectMin.Y + (40f - vector3.Y) / 2f);
			windowDrawList.AddImage(image, vector4, vector4 + vector3, Vector2.Zero, Vector2.One, ImGui.ColorConvertFloat4ToU32(flag ? Vector4.One : MainMenu.TextColor));
		}
		Vector4 vector5 = (flag ? Vector4.One : MainMenu.TextColor);
		Vector2 vector6 = ImGui.CalcTextSize(label);
		Vector2 vector7 = new Vector2(itemRectMin.X + 50f, itemRectMin.Y + (40f - vector6.Y) / 2f);
		ImGui.SetCursorScreenPos(vector7);
		ImGui.PushStyleColor(ImGuiCol.Text, vector5);
		ImGui.TextUnformatted(label);
		ImGui.PopStyleColor();
		ImGui.Dummy(new Vector2(0f, 5f));
	}
	private void RenderSubTabs(TabInfo currentTab)
{
    if (currentTab.SubTabs == null || currentTab.SubTabs.Count == 0)
    {
        return;
    }

    float contentWidth = ImGui.GetContentRegionAvail().X;
    float buttonWidth = contentWidth / currentTab.SubTabs.Count;

    for (int i = 0; i < currentTab.SubTabs.Count; i++)
    {
        if (!_tabAlpha.ContainsKey(i))
        {
            _tabAlpha[i] = 0f;
        }
    }

    for (int i = 0; i < currentTab.SubTabs.Count; i++)
    {
        ImGui.PushID(i);
        
        bool isSelected = _currentSubTab == i;
        if (isSelected)
        {
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, MainMenu.ButtonActiveColor);
            ImGui.PushStyleColor(ImGuiCol.Text, Vector4.One);
        }
        else
        {
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, MainMenu.ButtonActiveColor);
            ImGui.PushStyleColor(ImGuiCol.Text, MainMenu.TextColor);
        }

        if (ImGui.Button(currentTab.SubTabs[i], new Vector2(buttonWidth, 70f)))
        {
            _currentSubTab = i;
        }

        ImGui.PopStyleColor(2);

        bool isHovered = ImGui.IsItemHovered();
        float targetAlpha = 0f;
        
        if (isSelected)
        {
            targetAlpha = 1f;
        }
        else if (isHovered)
        {
            targetAlpha = 0.3f;
        }

        float currentAlpha = _tabAlpha[i];
        float speed = _tabSwitchSpeed * 0.01f;

        if (currentAlpha < targetAlpha)
        {
            currentAlpha += speed;
            if (currentAlpha > targetAlpha) currentAlpha = targetAlpha;
        }
        else if (currentAlpha > targetAlpha)
        {
            currentAlpha -= speed;
            if (currentAlpha < targetAlpha) currentAlpha = targetAlpha;
        }
        _tabAlpha[i] = currentAlpha;

        if (currentAlpha > 0.01f)
        {
            var drawList = ImGui.GetWindowDrawList();
            var min = ImGui.GetItemRectMin();
            var max = ImGui.GetItemRectMax();
            
            float lineWidth = (max.X - min.X) * currentAlpha;
            float lineY = max.Y - 2f;
            float centerX = (min.X + max.X) * 0.5f;
            
            drawList.AddLine(
                new Vector2(centerX - lineWidth / 2f, lineY),
                new Vector2(centerX + lineWidth / 2f, lineY),
                ImGui.GetColorU32(Vector4.One),
                2f
            );
        }

        if (i < currentTab.SubTabs.Count - 1)
        {
            ImGui.SameLine(0f, 0f);
        }

        ImGui.PopID();
    }
}
	private void RenderAimBotTab()
	{
		ImGui.Dummy(new Vector2(0f, 10f));
		float x = ImGui.GetContentRegionAvail().X;
		float num = (x - 10f) / 2f;
		float num2 = (x - 10f) / 2f;
		ImGui.PushStyleColor(ImGuiCol.ChildBg, MainMenu.ChildBgColor2);
		ImGui.BeginChild("ContentLeft_Tab1", new Vector2(num, 480f), ImGuiChildFlags.None, ImGuiWindowFlags.NoScrollbar);
		int subTabIdx = this._currentSubTab;
		int num3 = subTabIdx;
		if (num3 != 0)
		{
			if (num3 == 1)
			{
				this.RenderMeleeLeft();
			}
		}
		else
		{
			this.RenderGunLeft();
		}
		ImGui.EndChild();
		ImGui.PopStyleColor();
		ImGui.SameLine(0f, 10f);
		ImGui.BeginChild("ContentRight_Tab1", new Vector2(num2, 480f), ImGuiChildFlags.None, ImGuiWindowFlags.NoScrollbar);
		ImGui.PushStyleColor(ImGuiCol.ChildBg, MainMenu.ChildBgColor2);
		ImGui.BeginChild("RightTop_Tab1", new Vector2(num2, 235f), ImGuiChildFlags.None, ImGuiWindowFlags.NoScrollbar);
		int subTabIdx2 = this._currentSubTab;
		int num4 = subTabIdx2;
		if (num4 != 0)
		{
			if (num4 == 1)
			{
				this.RenderMeleeRightTop();
			}
		}
		else
		{
			this.RenderGunRightTop();
		}
		ImGui.EndChild();
		ImGui.PopStyleColor();
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGui.PushStyleColor(ImGuiCol.ChildBg, MainMenu.ChildBgColor2);
		ImGui.BeginChild("RightBottom_Tab1", new Vector2(num2, 235f), ImGuiChildFlags.None, ImGuiWindowFlags.NoScrollbar);
		int subTabIdx3 = this._currentSubTab;
		int num5 = subTabIdx3;
		if (num5 != 0)
		{
			if (num5 == 1)
			{
				this.RenderMeleeRightDown();
			}
		}
		else
		{
			this.RenderGunRightDown();
		}
		ImGui.EndChild();
		ImGui.PopStyleColor();
		ImGui.EndChild();
	}
	private void RenderGunLeft()
	{
		ImGui.Text(LocalizationManager.GetString("AimBot_Gun_General"));
		ImGui.Dummy(new Vector2(0f, 10f));
		string text = "GunAimBot";
		bool flag;
		ImGuiWidgets.ToggleWithSettings(LocalizationManager.GetString("AimBot_Gun_Enabled"), ref CerberusConfig.GunAimBot.Enabled, text, out flag);
		bool flag2 = flag;
		if (flag2)
		{
			ImGuiWidgets.RequestOpenSettingsPopup(text);
		}
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.SliderFloat(LocalizationManager.GetString("AimBot_Gun_Radius"), ref CerberusConfig.GunAimBot.CircleRadius, 0f, 10f);
		ImGuiWidgets.Combo(LocalizationManager.GetString("AimBot_Gun_Priority"), ref CerberusConfig.GunAimBot.TargetPriority, this.GetTargetPriorityNames());
		ImGuiWidgets.Toggle(LocalizationManager.GetString("AimBot_OnlyPriority"), ref CerberusConfig.GunAimBot.OnlyPriority);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("AimBot_Gun_Critical"), ref CerberusConfig.GunAimBot.TargetCritical);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("AimBot_Gun_MinimalSpread"), ref CerberusConfig.GunAimBot.MinSpread);
		ImGui.Dummy(new Vector2(0f, 5f));
		ImGuiWidgets.Toggle(LocalizationManager.GetString("AimBot_Gun_HitScan"), ref CerberusConfig.GunAimBot.HitScan);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("AimBot_Gun_AutoPredict"), ref CerberusConfig.GunAimBot.AutoPredict);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("AimBot_Gun_Predict"), ref CerberusConfig.GunAimBot.PredictEnabled);
		ImGuiWidgets.SliderFloat(LocalizationManager.GetString("AimBot_Gun_PredictCorrection"), ref CerberusConfig.GunAimBot.PredictCorrection, 0f, 1000f);
	}
	private void RenderGunRightTop()
	{
		ImGui.Text(LocalizationManager.GetString("AimBot_Gun_Visual"));
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.Toggle(LocalizationManager.GetString("AimBot_Gun_Circle"), ref CerberusConfig.GunAimBot.ShowCircle);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("AimBot_Gun_Line"), ref CerberusConfig.GunAimBot.ShowLine);
		ImGuiWidgets.ColorPicker(LocalizationManager.GetString("AimBot_Gun_Color"), ref CerberusConfig.GunAimBot.Color);
	}
	private void RenderGunRightDown()
	{
		ImGui.Text(LocalizationManager.GetString("AimBot_Gun_Helpers"));
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.Toggle(LocalizationManager.GetString("AimBot_Gun_EnabledHelper"), ref CerberusConfig.GunHelper.Enabled);
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.Toggle(LocalizationManager.GetString("AimBot_Gun_ShowAmmo"), ref CerberusConfig.GunHelper.ShowAmmo);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("AimBot_Gun_AutoBolt"), ref CerberusConfig.GunHelper.AutoBolt);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("AimBot_Gun_AutoReload"), ref CerberusConfig.GunHelper.AutoReload);
		ImGuiWidgets.SliderFloat(LocalizationManager.GetString("AimBot_Gun_AutoReloadDelay"), ref CerberusConfig.GunHelper.AutoReloadDelay, 0.01f, 0.5f);
	}
	private void RenderMeleeLeft()
	{
		ImGui.Text(LocalizationManager.GetString("AimBot_Melee_General"));
		ImGui.Dummy(new Vector2(0f, 10f));
		string text = "MeleeAimBot";
		bool flag;
		ImGuiWidgets.ToggleWithSettings(LocalizationManager.GetString("AimBot_Melee_Enabled"), ref CerberusConfig.MeleeAimBot.Enabled, text, out flag);
		bool flag2 = flag;
		if (flag2)
		{
			ImGuiWidgets.RequestOpenSettingsPopup(text);
		}
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.SliderFloat(LocalizationManager.GetString("AimBot_Melee_Radius"), ref CerberusConfig.MeleeAimBot.CircleRadius, 0f, 10f);
		ImGuiWidgets.Combo(LocalizationManager.GetString("AimBot_Melee_Priority"), ref CerberusConfig.MeleeAimBot.TargetPriority, this.GetTargetPriorityNames());
		ImGuiWidgets.Toggle(LocalizationManager.GetString("AimBot_OnlyPriority"), ref CerberusConfig.MeleeAimBot.OnlyPriority);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("AimBot_Melee_Critical"), ref CerberusConfig.MeleeAimBot.TargetCritical);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("AimBot_Melee_FixNetworkDelay"), ref CerberusConfig.MeleeAimBot.FixNetworkDelay);
		bool fixNetworkDelay = CerberusConfig.MeleeAimBot.FixNetworkDelay;
		if (fixNetworkDelay)
		{
			ImGuiWidgets.SliderFloat(LocalizationManager.GetString("AimBot_Melee_FixDelay"), ref CerberusConfig.MeleeAimBot.FixDelay, 0.1f, 2f);
		}
		ImGuiWidgets.Toggle(LocalizationManager.GetString("AimBot_Melee_RotateToTarget"), ref CerberusConfig.MeleeHelper.RotateToTarget);
	}
	private void RenderMeleeRightTop()
	{
		ImGui.Text(LocalizationManager.GetString("AimBot_Melee_Visual"));
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.Toggle(LocalizationManager.GetString("AimBot_Melee_Circle"), ref CerberusConfig.MeleeAimBot.ShowCircle);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("AimBot_Melee_Line"), ref CerberusConfig.MeleeAimBot.ShowLine);
		ImGuiWidgets.ColorPicker(LocalizationManager.GetString("AimBot_Melee_Color"), ref CerberusConfig.MeleeAimBot.Color);
	}
	private void RenderMeleeRightDown()
	{
		ImGui.Text(LocalizationManager.GetString("AimBot_Melee_Helpers"));
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.Toggle(LocalizationManager.GetString("AimBot_Melee_EnabledHelper"), ref CerberusConfig.MeleeHelper.Enabled);
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.Toggle(LocalizationManager.GetString("AimBot_Melee_Attack360"), ref CerberusConfig.MeleeHelper.Attack360);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("AimBot_Melee_AutoAttack"), ref CerberusConfig.MeleeHelper.AutoAttack);
	}
	private string[] GetTargetPriorityNames()
	{
		return new string[]
		{
			LocalizationManager.GetString("AimBot_TargetPriority_DistanceToPlayer"),
			LocalizationManager.GetString("AimBot_TargetPriority_DistanceToMouse"),
			LocalizationManager.GetString("AimBot_TargetPriority_LowestHealth")
		};
	}
	private void RenderMiscTab()
	{
		ImGui.Dummy(new Vector2(0f, 10f));
		float x = ImGui.GetContentRegionAvail().X;
		float num = (x - 10f) / 2f;
		float num2 = (x - 10f) / 2f;
		ImGui.PushStyleColor(ImGuiCol.ChildBg, MainMenu.ChildBgColor2);
		ImGui.BeginChild("ContentLeft_Tab3", new Vector2(num, 480f), ImGuiChildFlags.None, ImGuiWindowFlags.NoScrollbar);
		int subTabIdx = this._currentSubTab;
		int num3 = subTabIdx;
		if (num3 != 0)
		{
			if (num3 == 1)
			{
				this.RenderMiscSpammerLeft();
			}
		}
		else
		{
			this.RenderMiscGeneralLeft();
		}
		ImGui.EndChild();
		ImGui.PopStyleColor();
		ImGui.SameLine(0f, 10f);
		ImGui.BeginChild("ContentRight_Tab3", new Vector2(num2, 480f));
		ImGui.PushStyleColor(ImGuiCol.ChildBg, MainMenu.ChildBgColor2);
		ImGui.BeginChild("RightTop_Tab3", new Vector2(num2, 235f), ImGuiChildFlags.None, ImGuiWindowFlags.NoScrollbar);
		int subTabIdx2 = this._currentSubTab;
		int num4 = subTabIdx2;
		if (num4 != 0)
		{
			if (num4 == 1)
			{
				this.RenderMiscSpammerRightTop();
			}
		}
		else
		{
			this.RenderMiscGeneralRightTop();
		}
		ImGui.EndChild();
		ImGui.PopStyleColor();
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGui.PushStyleColor(ImGuiCol.ChildBg, MainMenu.ChildBgColor2);
		ImGui.BeginChild("RightBottom_Tab3", new Vector2(num2, 235f), ImGuiChildFlags.None, ImGuiWindowFlags.NoScrollbar);
		int subTabIdx3 = this._currentSubTab;
		int num5 = subTabIdx3;
		if (num5 != 0)
		{
			if (num5 == 1)
			{
				this.RenderMiscSpammerRightDown();
			}
		}
		else
		{
			this.RenderMiscGeneralRightDown();
		}
		ImGui.EndChild();
		ImGui.PopStyleColor();
		ImGui.EndChild();
	}
	private void RenderMiscGeneralLeft()
	{
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Misc_General_AntiSoap"), ref CerberusConfig.Misc.AntiSoapEnabled);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Misc_General_AntiAFK"), ref CerberusConfig.Misc.AntiAfkEnabled);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Misc_General_ShowExplosions"), ref CerberusConfig.Misc.ShowExplosive);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Misc_General_ShowTrajectory"), ref CerberusConfig.Misc.ShowTrajectory);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Misc_General_DamageOverlay"), ref CerberusConfig.Misc.DamageOverlayEnabled);
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Misc_General_AntiAim"), ref CerberusConfig.Misc.AntiAimEnabled);
		ImGuiWidgets.SliderFloat(LocalizationManager.GetString("Misc_General_Speed"), ref CerberusConfig.Misc.AutoRotateSpeed, 180f, 3600f);
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Misc_General_TrashTalk"), ref CerberusConfig.Misc.TrashTalkEnabled);
		bool flag = ImGuiWidgets.Button(LocalizationManager.GetString("Misc_General_OpenFolder"), null, null, true);
		if (flag)
		{
			Process.Start("explorer", this._configPath);
		}
	}
	private void RenderMiscGeneralRightTop()
	{
		ImGui.Text(LocalizationManager.GetString("Misc_General_Translator"));
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Misc_General_Translator_TranslateChat"), ref CerberusConfig.Settings.TranslateChatPatch);
		bool translateChatPatch = CerberusConfig.Settings.TranslateChatPatch;
		if (translateChatPatch)
		{
			ImGuiWidgets.InputText(LocalizationManager.GetString("Misc_General_Translator_LanguageChat"), ref CerberusConfig.Settings.TranslateChatLang);
		}
		ImGui.TextWrapped((CerberusConfig.Settings.CurrentLanguage == Language.En) ? "At the moment, translators for aHelp and a self-translator are being developed" : "В данный момент разрабатываются переводчики для ahelp и самопереводчик");
	}
	private void RenderMiscGeneralRightDown()
	{
		ImGui.Text(LocalizationManager.GetString("Misc_General_SearchingItems"));
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Misc_General_SearchingItems_Enabled"), ref CerberusConfig.Misc.ItemSearcherEnabled);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Misc_General_SearchingItems_ShowName"), ref CerberusConfig.Misc.ItemSearcherShowName);
		for (int i = 0; i < CerberusConfig.Misc.ItemSearchEntries.Count; i++)
		{
			ItemSearchEntry searchEntry = CerberusConfig.Misc.ItemSearchEntries[i];
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 1);
			defaultInterpolatedStringHandler.AppendLiteral("search_entry_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(i);
			ImGui.PushID(defaultInterpolatedStringHandler.ToStringAndClear());
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 1);
			defaultInterpolatedStringHandler.AppendLiteral("entry_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(i);
			bool flag2;
			bool flag = ImGuiWidgets.InputTextWithColor(defaultInterpolatedStringHandler.ToStringAndClear(), ref searchEntry.ItemName, ref searchEntry.Color, out flag2, 40f, 40f);
			bool flag3 = flag2;
			if (flag3)
			{
				CerberusConfig.Misc.ItemSearchEntries.RemoveAt(i);
				i--;
			}
			else
			{
				bool flag4 = flag;
				if (flag4)
				{
					CerberusConfig.Misc.ItemSearchEntries[i] = new ItemSearchEntry
					{
						ItemName = searchEntry.ItemName,
						Color = searchEntry.Color
					};
				}
			}
			ImGui.PopID();
		}
		ImGui.Dummy(new Vector2(0f, 5f));
		bool flag5 = ImGuiWidgets.Button(LocalizationManager.GetString("Misc_General_SearchingItems_Add"), null, null, true);
		if (flag5)
		{
			CerberusConfig.Misc.ItemSearchEntries.Add(new ItemSearchEntry());
		}
	}
	private void RenderMiscSpammerLeft()
	{
		ImGui.Text(LocalizationManager.GetString("Misc_Spammer_Settings"));
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Misc_Spammer_Settings_ProtectWord"), ref CerberusConfig.Spammer.ProtectTextEnabled);
		bool protectTextEnabled = CerberusConfig.Spammer.ProtectTextEnabled;
		if (protectTextEnabled)
		{
			ImGuiWidgets.Toggle(LocalizationManager.GetString("Misc_Spammer_Settings_RandomLength"), ref CerberusConfig.Spammer.ProtectRandomLength);
			bool flag = !CerberusConfig.Spammer.ProtectRandomLength;
			if (flag)
			{
				ImGuiWidgets.SliderInt(LocalizationManager.GetString("Misc_Spammer_Settings_Length"), ref CerberusConfig.Spammer.ProtectLength, 1, 12);
			}
		}
		this.ChannelAddToggle("Local", 1);
		this.ChannelAddToggle("Whisper", 2);
		this.ChannelAddToggle("Radio", 16);
		this.ChannelAddToggle("LOOC", 32);
		this.ChannelAddToggle("OOC", 64);
		this.ChannelAddToggle("Emotes", 512);
		this.ChannelAddToggle("Dead", 1024);
		this.ChannelAddToggle("Admin", 8192);
		ImGui.Dummy(new Vector2(0f, 10f));
	}
	private void ChannelAddToggle(string label, int channel)
	{
		bool flag = CerberusConfig.Spammer.Channels.Contains(channel);
		bool flag2 = ImGuiWidgets.Toggle(label, ref flag);
		if (flag2)
		{
			bool flag3 = flag;
			if (flag3)
			{
				CerberusConfig.Spammer.Channels.Add(channel);
			}
			else
			{
				CerberusConfig.Spammer.Channels.Remove(channel);
			}
		}
	}
	private void RenderMiscSpammerRightTop()
	{
		ImGui.Text(LocalizationManager.GetString("Misc_Spammer_Chat"));
		ImGui.Dummy(new Vector2(0f, 10f));
		bool flag = ImGuiWidgets.Toggle(LocalizationManager.GetString("Misc_Spammer_Chat_Enabled"), ref CerberusConfig.Spammer.ChatEnabled);
		if (flag)
		{
			this._spammerSystem.StartSpamChat();
		}
		ImGuiWidgets.SliderInt(LocalizationManager.GetString("Misc_Spammer_Chat_Delay"), ref CerberusConfig.Spammer.ChatDelay, 10, 1000);
		ImGuiWidgets.InputText(LocalizationManager.GetString("Misc_Spammer_Chat_Text"), ref CerberusConfig.Spammer.ChatText);
	}
	private void RenderMiscSpammerRightDown()
	{
		ImGui.Text(LocalizationManager.GetString("Misc_Spammer_AHelp"));
		ImGui.Dummy(new Vector2(0f, 10f));
		bool flag = ImGuiWidgets.Toggle(LocalizationManager.GetString("Misc_Spammer_AHelp_Enabled"), ref CerberusConfig.Spammer.AHelpEnabled);
		if (flag)
		{
			this._spammerSystem.StartSpamAHelp();
		}
		ImGuiWidgets.SliderInt(LocalizationManager.GetString("Misc_Spammer_AHelp_Delay"), ref CerberusConfig.Spammer.AHelpDelay, 10, 1000);
		ImGuiWidgets.InputText(LocalizationManager.GetString("Misc_Spammer_AHelp_Text"), ref CerberusConfig.Spammer.AHelpText);
	}
	private void InitializePlayersTab()
	{
		this._playerTracker.OnPlayerJoined += this.OnPlayerJoined;
		this._playerTracker.OnPlayerLeft += this.OnPlayerLeft;
	}
	private void OnPlayerJoined(ICommonSession session)
	{
		NotificationManager.ShowNotification(LocalizationManager.GetString("Players_Logs_Join", new object[] { session.Name }), 5f, 0.3f, 0.5f, new Vector4?(ColorPalette.LogJoin), true);
		Dictionary<string, MainMenu.ConnectStatus> logsDict = this._logs;
		DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
		defaultInterpolatedStringHandler.AppendLiteral("[");
		defaultInterpolatedStringHandler.AppendFormatted<DateTime>(DateTime.Now, "HH:mm:ss");
		defaultInterpolatedStringHandler.AppendLiteral("] ");
		defaultInterpolatedStringHandler.AppendFormatted(LocalizationManager.GetString("Players_Logs_Join", new object[] { session.Name }));
		logsDict.Add(defaultInterpolatedStringHandler.ToStringAndClear(), MainMenu.ConnectStatus.Join);
	}
	private void OnPlayerLeft(ICommonSession session)
	{
		NotificationManager.ShowNotification(LocalizationManager.GetString("Players_Logs_Leave", new object[] { session.Name }), 5f, 0.3f, 0.5f, new Vector4?(ColorPalette.LogLeave), true);
		Dictionary<string, MainMenu.ConnectStatus> logsDict = this._logs;
		DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
		defaultInterpolatedStringHandler.AppendLiteral("[");
		defaultInterpolatedStringHandler.AppendFormatted<DateTime>(DateTime.Now, "HH:mm:ss");
		defaultInterpolatedStringHandler.AppendLiteral("] ");
		defaultInterpolatedStringHandler.AppendFormatted(LocalizationManager.GetString("Players_Logs_Leave", new object[] { session.Name }));
		logsDict.Add(defaultInterpolatedStringHandler.ToStringAndClear(), MainMenu.ConnectStatus.Leave);
		this._playerWindows.Remove(session.UserId);
		this._renderedPreviews.Remove(session.UserId);
	}
	private void RenderPlayersTab()
	{
		ImGui.Dummy(new Vector2(0f, 10f));
		int subTabIdx = this._currentSubTab;
		int num = subTabIdx;
		if (num != 0)
		{
			if (num == 1)
			{
				this.RenderLogsTab();
			}
		}
		else
		{
			this.RenderGeneralTab();
		}
		foreach (KeyValuePair<NetUserId, PlayerData> keyValuePair in this._playerTracker.AllPlayerSessions)
		{
			NetUserId key = keyValuePair.Key;
			PlayerData value = keyValuePair.Value;
			bool flag = value != null && value.IsVisible && value.AttachedEntity != null && !this._renderedPreviews.Contains(key);
			if (flag)
			{
				this._entityPreview.RenderSpriteAsync(value.AttachedEntity.Value, 0, default(CancellationToken));
				this._renderedPreviews.Add(key);
			}
			bool flag3;
			bool flag2 = this._playerWindows.TryGetValue(key, out flag3) && flag3;
			bool flag4 = flag2;
			if (flag4)
			{
				this.RenderPlayerWindow(value);
			}
		}
	}
	private void RenderGeneralTab()
{
    ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X * 0.6f);
    ImGui.InputTextWithHint("##PlayerSearch", LocalizationManager.GetString("Players_General_SearchHint"), ref this._playerSearch, 100);
    ImGui.SameLine();
    string[] array = this._sortOptions.Select((string key) => LocalizationManager.GetString(key)).ToArray<string>();
    ImGui.PushStyleColor(ImGuiCol.Header, new Vector4(0.19607843f, 0.19607843f, 0.19607843f, 1f));
    ImGui.PushStyleColor(ImGuiCol.HeaderHovered, new Vector4(0.27450982f, 0.27450982f, 0.27450982f, 1f));
    ImGui.PushStyleColor(ImGuiCol.HeaderActive, new Vector4(0.3529412f, 0.3529412f, 0.3529412f, 1f));
    ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X);
    ImGui.Combo(LocalizationManager.GetString("Players_General_SortBy"), ref this._selectedSort, array, array.Length);
    ImGui.PopStyleColor(3);
    ImGui.Dummy(new Vector2(0f, 10f));
    
    if (ImGui.BeginChild("PlayerScroll"))
    {
        float x = ImGui.GetContentRegionAvail().X;
        ImGui.Columns(5, "##columns_header", false);
        ImGui.SetColumnWidth(0, x * 0.3f);
        ImGui.SetColumnWidth(1, x * 0.3f);
        ImGui.SetColumnWidth(2, x * 0.15f);
        ImGui.SetColumnWidth(3, x * 0.15f);
        ImGui.Text(LocalizationManager.GetString("Players_General_Name"));
        ImGui.NextColumn();
        ImGui.Text(LocalizationManager.GetString("Players_General_Character"));
        ImGui.NextColumn();
        ImGui.Text(LocalizationManager.GetString("Players_General_Entity"));
        ImGui.NextColumn();
        ImGui.Text(LocalizationManager.GetString("Players_General_Status"));
        ImGui.NextColumn();
        ImGui.Text("");
        ImGui.NextColumn();
        ImGui.Columns(1);
        ImGui.Separator();
        ImGui.Dummy(new Vector2(0f, 10f));
        
        IEnumerable<PlayerData> enumerable = this._playerTracker.AllPlayerSessions.Values.AsEnumerable<PlayerData>();
        if (!string.IsNullOrWhiteSpace(this._playerSearch))
        {
            string lowerSearch = this._playerSearch.ToLowerInvariant();
            enumerable = enumerable.Where((PlayerData p) => p.Session.Name.ToLowerInvariant().Contains(lowerSearch) || p.EntityName.ToLowerInvariant().Contains(lowerSearch));
        }
        
        int sortIndex = this._selectedSort;
        IOrderedEnumerable<PlayerData> orderedEnumerable;
        switch (sortIndex)
        {
        case 0:
            orderedEnumerable = enumerable.OrderBy((PlayerData p) => p.Status == "Offline").ThenBy((PlayerData p) => p.Session.Name, StringComparer.OrdinalIgnoreCase);
            break;
        case 1:
            orderedEnumerable = enumerable.OrderByDescending((PlayerData p) => p.Status == "Offline").ThenBy((PlayerData p) => p.Session.Name, StringComparer.OrdinalIgnoreCase);
            break;
        case 2:
            orderedEnumerable = enumerable.OrderBy((PlayerData p) => p.Session.Name, StringComparer.OrdinalIgnoreCase);
            break;
        case 3:
            orderedEnumerable = enumerable.OrderByDescending((PlayerData p) => p.Session.Name, StringComparer.OrdinalIgnoreCase);
            break;
        case 4:
            orderedEnumerable = enumerable.OrderBy((PlayerData p) => p.EntityName == "Unknown").ThenBy((PlayerData p) => p.EntityName, StringComparer.OrdinalIgnoreCase);
            break;
        case 5:
            orderedEnumerable = enumerable.OrderBy((PlayerData p) => p.EntityName == "Unknown").ThenByDescending((PlayerData p) => p.EntityName, StringComparer.OrdinalIgnoreCase);
            break;
        default:
            orderedEnumerable = enumerable.OrderBy((PlayerData p) => p.Status == "Offline").ThenBy((PlayerData p) => p.Session.Name, StringComparer.OrdinalIgnoreCase);
            break;
        }
        
        enumerable = orderedEnumerable;
        List<PlayerData> list = enumerable.ToList<PlayerData>();
        foreach (PlayerData playerData in list)
        {
            this.RenderPlayerCard(playerData);
            ImGui.Dummy(new Vector2(0f, 5f));
        }
        ImGui.EndChild();
    }
}
	private void RenderPlayerCard(PlayerData sessionInfo)
	{
		ImGui.PushStyleColor(ImGuiCol.ChildBg, new Vector4(0.10980392f, 0.11372549f, 0.1254902f, 1f));
		ImGui.PushStyleVar(ImGuiStyleVar.ChildRounding, 5f);
		DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
		defaultInterpolatedStringHandler.AppendLiteral("##player_");
		defaultInterpolatedStringHandler.AppendFormatted<NetUserId>(sessionInfo.Session.UserId);
		bool flag = ImGui.BeginChild(defaultInterpolatedStringHandler.ToStringAndClear(), new Vector2(0f, 60f));
		if (flag)
		{
			float x = ImGui.GetContentRegionAvail().X;
			ImGui.Columns(5, "##player_columns", false);
			ImGui.SetColumnWidth(0, x * 0.3f);
			ImGui.SetColumnWidth(1, x * 0.3f);
			ImGui.SetColumnWidth(2, x * 0.15f);
			ImGui.SetColumnWidth(3, x * 0.15f);
			ImGui.TextUnformatted(sessionInfo.Session.Name);
			ImGui.NextColumn();
			ImGui.Text(sessionInfo.EntityName);
			ImGui.NextColumn();
			EntityUid? entityUid = sessionInfo.Session.AttachedEntity;
			ImGui.Text(((entityUid != null) ? entityUid.GetValueOrDefault().ToString() : null) ?? "None");
			ImGui.NextColumn();
			ImGui.TextColored((sessionInfo.Status == "Online") ? new Vector4(0f, 1f, 0f, 1f) : new Vector4(1f, 0f, 0f, 1f), sessionInfo.Status);
			ImGui.NextColumn();
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 1);
			defaultInterpolatedStringHandler.AppendLiteral("More##");
			defaultInterpolatedStringHandler.AppendFormatted<NetUserId>(sessionInfo.Session.UserId);
			bool flag2 = ImGuiWidgets.Button(defaultInterpolatedStringHandler.ToStringAndClear(), null, null, false);
			if (flag2)
			{
				this._playerWindows[sessionInfo.Session.UserId] = true;
				bool flag3;
				if (sessionInfo != null && sessionInfo.IsVisible)
				{
					entityUid = sessionInfo.AttachedEntity;
					flag3 = entityUid != null;
				}
				else
				{
					flag3 = false;
				}
				bool flag4 = flag3;
				if (flag4)
				{
					EntityPreviewSystem previewSys = this._entityPreview;
					entityUid = sessionInfo.AttachedEntity;
					previewSys.RenderSpriteAsync(entityUid.Value, 0, default(CancellationToken));
				}
			}
			ImGui.Columns(1);
		}
		ImGui.EndChild();
		ImGui.PopStyleVar();
		ImGui.PopStyleColor();
	}
	private unsafe void RenderPlayerWindow(PlayerData sessionInfo)
	{
		NetUserId userId = sessionInfo.Session.UserId;
		bool flag2;
		bool flag = this._playerWindows.TryGetValue(userId, out flag2) && flag2;
		ImGui.SetNextWindowSize(new Vector2(550f, 400f), ImGuiCond.FirstUseEver);
		bool flag3 = !ImGui.Begin(LocalizationManager.GetString("Players_WindowTitle") + " " + sessionInfo.Session.Name, ref flag, ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoSavedSettings);
		if (flag3)
		{
			ImGui.End();
			this._playerWindows[userId] = flag;
		}
		else
		{
			MainMenu.ApplyStyles();
			ImGui.PushStyleColor(ImGuiCol.WindowBg, new Vector4(0.07058824f, 0.07450981f, 0.09019608f, 1f));
			ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(12f, 12f));
			ImGui.PushStyleVar(ImGuiStyleVar.ChildRounding, 5f);
			Vector2 contentRegionAvail = ImGui.GetContentRegionAvail();
			float num = contentRegionAvail.X * 0.5f;
			float y = contentRegionAvail.Y;
			ImGui.BeginChild("PreviewPanel", new Vector2(num, y), ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse);
			ImTextureID imTextureID = default(ImTextureID);
			bool flag4 = sessionInfo.AttachedEntity != null && sessionInfo.EntityName != "Unknown";
			if (flag4)
			{
				imTextureID = this._entityPreview.GetImGuiTexture(sessionInfo.AttachedEntity.Value);
			}
			bool flag5 = imTextureID.Handle != (ulong)((ushort)IntPtr.Zero);
			if (flag5)
			{
				float num2 = num - ImGui.GetStyle().WindowPadding.X * 2f;
				ImGui.SetCursorPosX((num - num2) / 2f);
				ImGui.SetCursorPosY((y - num2) / 2f);
				ImGui.Image(imTextureID, new Vector2(num2, num2));
			}
			else
			{
				string @string = LocalizationManager.GetString("Players_Window_NoPreview");
				Vector2 vector = ImGui.CalcTextSize(@string);
				ImGui.SetCursorPosX((num - vector.X) / 2f);
				ImGui.SetCursorPosY((y - vector.Y) / 2f);
				ImGui.TextDisabled(@string);
			}
			ImGui.EndChild();
			ImGui.SameLine();
			ImGui.PushStyleColor(ImGuiCol.ChildBg, new Vector4(0.09019608f, 0.09411765f, 0.10980392f, 1f));
			ImGui.BeginChild("InfoPane", new Vector2(0f, y), ImGuiWindowFlags.NoScrollbar);
			float x = ImGui.GetContentRegionAvail().X;
			ImDrawListPtr windowDrawList = ImGui.GetWindowDrawList();
			Vector2 vector2 = ImGui.GetStyle().WindowPadding;
			float num3 = 3f;
			string[] array = new string[]
			{
				LocalizationManager.GetString("Players_Window_Name"),
				LocalizationManager.GetString("Players_General_Status"),
				LocalizationManager.GetString("Players_Window_LastPosition"),
				LocalizationManager.GetString("Players_Window_Job")
			};
			string[] array2 = new string[] { sessionInfo.EntityName, sessionInfo.Status, null, sessionInfo.Job };
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				Vector2 vector3 = ImGui.CalcTextSize(text);
				Vector2 cursorScreenPos = ImGui.GetCursorScreenPos();
				float num4 = cursorScreenPos.X + vector2.X + num3;
				float num5 = cursorScreenPos.X + vector2.X + x - num3;
				float num6 = cursorScreenPos.Y - num3;
				float num7 = cursorScreenPos.Y + vector3.Y + num3;
				windowDrawList.AddRectFilled(new Vector2(num4, num6), new Vector2(num5, num7), ImGui.GetColorU32(new Vector4(0.10980392f, 0.11372549f, 0.1254902f, 1f)), 4f);
				ImGui.SetCursorPosX((x - vector3.X) / 2f);
				ImGui.TextUnformatted(text);
				ImGui.Dummy(new Vector2(0f, 5f));
				bool flag6 = i == 2;
				string text2;
				if (flag6)
				{
					Vector2 lastKnownPosition = sessionInfo.LastKnownPosition;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 2);
					defaultInterpolatedStringHandler.AppendLiteral("X: ");
					defaultInterpolatedStringHandler.AppendFormatted<float>(MathF.Round(lastKnownPosition.X));
					defaultInterpolatedStringHandler.AppendLiteral(" Y: ");
					defaultInterpolatedStringHandler.AppendFormatted<float>(MathF.Round(lastKnownPosition.Y));
					text2 = defaultInterpolatedStringHandler.ToStringAndClear();
				}
				else
				{
					text2 = array2[i] ?? string.Empty;
				}
				Vector2 vector4 = ImGui.CalcTextSize(text2);
				ImGui.SetCursorPosX((x - vector4.X) / 2f);
				ImGui.TextUnformatted(text2);
				ImGui.Dummy(new Vector2(0f, 10f));
			}
			ImGui.EndChild();
			ImGui.PopStyleColor();
			ImGui.PopStyleVar(2);
			ImGui.PopStyleColor();
			ImGui.End();
			this._playerWindows[userId] = flag;
		}
	}
	private void RenderLogsTab()
	{
		ImGui.Text(LocalizationManager.GetString("Players_Logs_Title"));
		ImGui.Separator();
		bool flag = ImGui.BeginChild("LogScroll");
		if (flag)
		{
			foreach (KeyValuePair<string, MainMenu.ConnectStatus> keyValuePair in this._logs)
			{
				bool flag2 = keyValuePair.Value == MainMenu.ConnectStatus.Join;
				if (flag2)
				{
					ImGui.TextColored(new Vector4(0f, 1f, 0f, 1f), keyValuePair.Key);
				}
				else
				{
					bool flag3 = keyValuePair.Value == MainMenu.ConnectStatus.Leave;
					if (flag3)
					{
						ImGui.TextColored(new Vector4(1f, 0f, 0f, 1f), keyValuePair.Key);
					}
					else
					{
						ImGui.TextUnformatted(keyValuePair.Key);
					}
				}
			}
			ImGui.EndChild();
		}
	}
	private void RenderSettingsTab()
{
    ImGui.Dummy(new Vector2(0f, 10f));
    float x = ImGui.GetContentRegionAvail().X;
    float num = (x - 10f) / 2f;
    float num2 = (x - 10f) / 2f;
    ImGui.PushStyleColor(ImGuiCol.ChildBg, MainMenu.ChildBgColor2);
    
    if (ImGui.BeginChild("ContentLeft_Tab3", new Vector2(num, 480f), ImGuiChildFlags.None, ImGuiWindowFlags.NoScrollbar))
    {
        if (this._currentSubTab == 0)
        {
            this.RenderSettingsGeneralLeft();
        }
        else if (this._currentSubTab == 1)
        {
            ImGui.Text(LocalizationManager.GetString("Configs_Title"));
            if (Directory.Exists(ConfigManager.configDir))
            {
                IEnumerable<string> files = Directory.GetFiles(ConfigManager.configDir, "*.json");
                ConfigManager.ConfigFiles = files.Select(Path.GetFileNameWithoutExtension).ToList<string>();
            }
            else
            {
                ConfigManager.ConfigFiles = new List<string>();
            }
            
            ImGui.InputText(LocalizationManager.GetString("Configs_Name"), ref ConfigManager.ConfigName, 64);
            
            for (int i = 0; i < ConfigManager.ConfigFiles.Count; i++)
            {
                bool flag = i == ConfigManager.SelectedConfigIndex;
                if (ImGui.Selectable(ConfigManager.ConfigFiles[i], flag))
                {
                    ConfigManager.SelectedConfigIndex = i;
                }
            }
            
            if (ImGuiWidgets.Button(LocalizationManager.GetString("Configs_Save"), null, null, true) && !string.IsNullOrWhiteSpace(ConfigManager.ConfigName))
            {
                ConfigManager.SaveConfig(ConfigManager.ConfigName);
            }
            
            if (ConfigManager.SelectedConfigIndex >= 0 && ConfigManager.SelectedConfigIndex < ConfigManager.ConfigFiles.Count)
            {
                ImGui.SameLine();
                if (ImGuiWidgets.Button(LocalizationManager.GetString("Configs_Load"), null, null, true))
                {
                    ConfigManagerData configManagerData = ConfigManager.LoadConfig(ConfigManager.ConfigFiles[ConfigManager.SelectedConfigIndex]);
                    if (configManagerData != null)
                    {
                        ConfigManager.ApplyConfig(configManagerData);
                    }
                }
            }
            
            if (ConfigManager.SelectedConfigIndex >= 0 && ConfigManager.SelectedConfigIndex < ConfigManager.ConfigFiles.Count)
            {
                ImGui.SameLine();
                if (ImGuiWidgets.Button(LocalizationManager.GetString("Configs_Delete"), null, null, true))
                {
                    string text = Path.Combine(ConfigManager.configDir, ConfigManager.ConfigFiles[ConfigManager.SelectedConfigIndex] + ".json");
                    if (File.Exists(text))
                    {
                        File.Delete(text);
                        ConfigManager.SelectedConfigIndex = -1;
                    }
                }
            }
            
            if (ImGuiWidgets.Button(LocalizationManager.GetString("Configs_OpenFolder"), null, null, true))
            {
                if (!Directory.Exists(ConfigManager.configDir)) Directory.CreateDirectory(ConfigManager.configDir);
                Process.Start("explorer.exe", ConfigManager.configDir);
            }
        }
        ImGui.EndChild();
    }
    ImGui.PopStyleColor();
    
    ImGui.SameLine(0f, 10f);
    
    ImGui.BeginChild("ContentRight_Tab3", new Vector2(num2, 480f));
    ImGui.PushStyleColor(ImGuiCol.ChildBg, MainMenu.ChildBgColor2);
    if (this._currentSubTab != 1)
    {
        ImGui.BeginChild("RightTop_Tab3", new Vector2(num2, 270f));
        if (this._currentSubTab == 0)
        {
            this.RenderSettingsGeneralRight();
        }
        ImGui.EndChild();
    }
    ImGui.PopStyleColor();
    ImGui.EndChild();
    
    UiSettingsRenderer.Render();
}
	private void RenderSettingsGeneralLeft()
	{
		ImGui.Text(LocalizationManager.GetString("Settings_General"));
		ImGui.Dummy(new Vector2(0f, 10f));
		string text = "Settings";
		bool flag;
		ImGuiWidgets.ToggleWithSettings(LocalizationManager.GetString("Settings_ShowMenu"), ref CerberusConfig.Settings.ShowMenu, text, out flag);
		bool flag2 = flag;
		if (flag2)
		{
			ImGuiWidgets.RequestOpenSettingsPopup(text);
		}
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Settings_UICustomizable"), ref CerberusConfig.Settings.UiCustomizable);
		bool flag3 = ImGuiWidgets.Button((CerberusConfig.Settings.CurrentLanguage == Language.Ru) ? LocalizationManager.GetString("Settings_General_SwitchToEnglish") : LocalizationManager.GetString("Settings_General_SwitchToRussian"), null, null, true);
		if (flag3)
		{
			LocalizationManager.Switch();
		}
		bool flag4 = ImGuiWidgets.Button(LocalizationManager.GetString("Settings_General_Unload"), null, null, true);
		if (flag4)
		{
			this._mainController.PanicUnload();
		}
	}
	private void RenderSettingsGeneralRight()
	{
		ImGui.Text(LocalizationManager.GetString("Settings_Patches"));
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Settings_Patches_AdminPrivilege"), ref CerberusConfig.Settings.AdminPatch);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Settings_Patches_NoDamageFriend"), ref CerberusConfig.Settings.NoDmgFriendPatch);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Settings_Patches_NoDamageForceSay"), ref CerberusConfig.Settings.DamageForcePatch);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Settings_Patches_AntiScreenGrub"), ref CerberusConfig.Settings.AntiScreenGrubPatch);
	}
	private void RenderVisualsTab()
	{
		ImGui.Dummy(new Vector2(0f, 10f));
		float x = ImGui.GetContentRegionAvail().X;
		float num = (x - 10f) / 2f;
		float num2 = (x - 10f) / 2f;
		ImGui.PushStyleColor(ImGuiCol.ChildBg, MainMenu.ChildBgColor2);
		ImGui.BeginChild("ContentLeft_Tab2", new Vector2(num, 480f), ImGuiChildFlags.None, ImGuiWindowFlags.NoScrollbar);
		switch (this._currentSubTab)
		{
		case 0:
			this.RenderEspLeft();
			break;
		case 1:
			this.RenderEyeLeft();
			break;
		case 2:
			this.RenderFunLeft();
			break;
		}
		ImGui.EndChild();
		ImGui.PopStyleColor();
		ImGui.SameLine(0f, 10f);
		ImGui.BeginChild("ContentRight_Tab2", new Vector2(num2, 480f));
		ImGui.PushStyleColor(ImGuiCol.ChildBg, MainMenu.ChildBgColor2);
		ImGui.BeginChild("RightTop_Tab2", new Vector2(num2, 235f), ImGuiChildFlags.None, ImGuiWindowFlags.NoScrollbar);
		switch (this._currentSubTab)
		{
		case 0:
			this.RenderEspRightTop();
			break;
		case 1:
			this.RenderEyeRightTop();
			break;
		case 2:
			this.RenderFunRightTop();
			break;
		}
		ImGui.EndChild();
		ImGui.PopStyleColor();
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGui.PushStyleColor(ImGuiCol.ChildBg, MainMenu.ChildBgColor2);
		ImGui.BeginChild("RightBottom_Tab2", new Vector2(num2, 235f), ImGuiChildFlags.None, ImGuiWindowFlags.NoScrollbar);
		switch (this._currentSubTab)
		{
		case 0:
			this.RenderEspRightDown();
			break;
		case 1:
			this.RenderEyeRightDown();
			break;
		case 2:
			this.RenderFunRightDown();
			break;
		}
		ImGui.EndChild();
		ImGui.PopStyleColor();
		ImGui.EndChild();
	}
	private void RenderEspLeft()
	{
		ImGui.Text(LocalizationManager.GetString("Visuals_ESP_General"));
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_ESP_Enabled"), ref CerberusConfig.Esp.Enabled);
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_ESP_Name"), ref CerberusConfig.Esp.ShowName);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_ESP_CKey"), ref CerberusConfig.Esp.ShowCKey);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_ESP_Antag"), ref CerberusConfig.Esp.ShowAntag);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_ESP_Friend"), ref CerberusConfig.Esp.ShowFriend);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_ESP_Priority"), ref CerberusConfig.Esp.ShowPriority);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_ESP_CombatMode"), ref CerberusConfig.Esp.ShowCombatMode);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_ESP_Implants"), ref CerberusConfig.Esp.ShowImplants);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_ESP_Contraband"), ref CerberusConfig.Esp.ShowContraband);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("ESP_Weapon"), ref CerberusConfig.Esp.ShowWeapon);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("ESP_NoSlip"), ref CerberusConfig.Esp.ShowNoSlip);
	}
	private void RenderEspRightTop()
	{
		ImGui.Text(LocalizationManager.GetString("Visuals_ESP_Colors"));
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.ColorPicker(LocalizationManager.GetString("Visuals_ESP_Color_Name"), ref CerberusConfig.Esp.NameColor);
		ImGuiWidgets.ColorPicker(LocalizationManager.GetString("Visuals_ESP_Color_CKey"), ref CerberusConfig.Esp.CKeyColor);
		ImGuiWidgets.ColorPicker(LocalizationManager.GetString("Visuals_ESP_Color_Antag"), ref CerberusConfig.Esp.AntagColor);
		ImGuiWidgets.ColorPicker(LocalizationManager.GetString("Visuals_ESP_Color_Friend"), ref CerberusConfig.Esp.FriendColor);
		ImGuiWidgets.ColorPicker(LocalizationManager.GetString("Visuals_ESP_Color_Priority"), ref CerberusConfig.Esp.PriorityColor);
		ImGuiWidgets.ColorPicker(LocalizationManager.GetString("Visuals_ESP_Color_CombatMode"), ref CerberusConfig.Esp.CombatModeColor);
		ImGuiWidgets.ColorPicker(LocalizationManager.GetString("Visuals_ESP_Color_Implants"), ref CerberusConfig.Esp.ImplantsColor);
		ImGuiWidgets.ColorPicker(LocalizationManager.GetString("Visuals_ESP_Color_Contraband"), ref CerberusConfig.Esp.ContrabandColor);
		ImGuiWidgets.ColorPicker(LocalizationManager.GetString("ESP_Weapon"), ref CerberusConfig.Esp.WeaponColor);
		ImGuiWidgets.ColorPicker(LocalizationManager.GetString("ESP_NoSlip"), ref CerberusConfig.Esp.NoSlipColor);
	}
	private void RenderEspRightDown()
	{
		ImGui.Text(LocalizationManager.GetString("Visuals_ESP_Font"));
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.SliderInt(LocalizationManager.GetString("Visuals_ESP_Font_Interval"), ref CerberusConfig.Esp.FontInterval, 1, 50);
		ImGui.Dummy(new Vector2(0f, 5f));
		bool flag = ImGuiWidgets.Combo(LocalizationManager.GetString("Visuals_ESP_Font_MainFont"), ref CerberusConfig.Esp.MainFontIndex, this._fonts.Select((string[] x) => x[0]).ToArray<string>());
		if (flag)
		{
			CerberusConfig.Esp.MainFontPath = this._fonts[CerberusConfig.Esp.MainFontIndex][1];
		}
		ImGuiWidgets.SliderInt(LocalizationManager.GetString("Visuals_ESP_Font_Size"), ref CerberusConfig.Esp.MainFontSize, 6, 30);
		ImGui.Dummy(new Vector2(0f, 5f));
		bool flag2 = ImGuiWidgets.Combo(LocalizationManager.GetString("Visuals_ESP_Font_OtherFont"), ref CerberusConfig.Esp.OtherFontIndex, this._fonts.Select((string[] x) => x[0]).ToArray<string>());
		if (flag2)
		{
			CerberusConfig.Esp.OtherFontPath = this._fonts[CerberusConfig.Esp.OtherFontIndex][1];
		}
		ImGuiWidgets.SliderInt(LocalizationManager.GetString("Visuals_ESP_Font_Size_Other"), ref CerberusConfig.Esp.OtherFontSize, 6, 30);
	}
	private void RenderEyeLeft()
	{
		string text = "FOV";
		string text2 = "FullBright";
		string text3 = "Zoom";
		ImGui.Text(LocalizationManager.GetString("Visuals_Eye_General"));
		ImGui.Dummy(new Vector2(0f, 10f));
		bool flag;
		ImGuiWidgets.ToggleWithSettings(LocalizationManager.GetString("Visuals_Eye_FOV"), ref CerberusConfig.Eye.FovEnabled, text, out flag);
		bool flag2 = flag;
		if (flag2)
		{
			ImGuiWidgets.RequestOpenSettingsPopup(text);
		}
		bool flag3;
		ImGuiWidgets.ToggleWithSettings(LocalizationManager.GetString("Visuals_Eye_FullBright"), ref CerberusConfig.Eye.FullBrightEnabled, text2, out flag3);
		bool flag4 = flag3;
		if (flag4)
		{
			ImGuiWidgets.RequestOpenSettingsPopup(text2);
		}
		ImGui.Dummy(new Vector2(0f, 5f));
		bool flag5;
		ImGuiWidgets.SliderFloatWithSettings(LocalizationManager.GetString("Visuals_Eye_Zoom"), ref CerberusConfig.Eye.Zoom, 0.5f, 30f, text3, out flag5);
		bool flag6 = flag5;
		if (flag6)
		{
			ImGuiWidgets.RequestOpenSettingsPopup(text3);
		}
		bool flag7 = ImGuiWidgets.Button(LocalizationManager.GetString("Visuals_Eye_Reset"), null, null, true);
		if (flag7)
		{
			CerberusConfig.Eye.Zoom = 1f;
			this._resetCount++;
			this._debugMode = this._resetCount >= 10;
			bool debugActive = this._debugMode;
			if (debugActive)
			{
				CerberusConfig.Settings.ShowDebugConsole = !CerberusConfig.Settings.ShowDebugConsole;
			}
		}
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGui.Text(LocalizationManager.GetString("Visuals_Eye_StorageViewer"));
		ImGui.Dummy(new Vector2(0f, 10f));
		string text4 = "StorageViewer";
		bool flag8;
		ImGuiWidgets.ToggleWithSettings(LocalizationManager.GetString("Visuals_Eye_StorageViewer_Enabled"), ref CerberusConfig.StorageViewer.Enabled, text4, out flag8);
		bool flag9 = flag8;
		if (flag9)
		{
			ImGuiWidgets.RequestOpenSettingsPopup(text4);
		}
		ImGuiWidgets.ColorPicker(LocalizationManager.GetString("Visuals_Eye_StorageViewer_Color"), ref CerberusConfig.StorageViewer.Color);
	}
	private void RenderEyeRightTop()
	{
		ImGui.Text(LocalizationManager.GetString("Visuals_Eye_HUD"));
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_Eye_HUD_Health"), ref CerberusConfig.Hud.ShowHealth);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_Eye_HUD_Stamina"), ref CerberusConfig.Hud.ShowStamina);
		ImGuiWidgets.ColorPicker(LocalizationManager.GetString("Visuals_Eye_HUD_Color"), ref CerberusConfig.Hud.StaminaColor);
		this.HudIconToggle(LocalizationManager.GetString("Visuals_Eye_HUD_AntagIcons"), ref CerberusConfig.Hud.ShowAntag, "ShowAntagIcons");
		this.HudIconToggle(LocalizationManager.GetString("Visuals_Eye_HUD_JobIcons"), ref CerberusConfig.Hud.ShowJobIcons, "ShowJobIcons");
		this.HudIconToggle(LocalizationManager.GetString("Visuals_Eye_HUD_MindShieldIcons"), ref CerberusConfig.Hud.ShowMindShieldIcons, "ShowMindShieldIcons");
		this.HudIconToggle(LocalizationManager.GetString("Visuals_Eye_HUD_CriminalRecordIcons"), ref CerberusConfig.Hud.ShowCriminalRecordIcons, "ShowCriminalRecordIcons");
		this.HudIconToggle(LocalizationManager.GetString("Visuals_Eye_HUD_SyndicateIcons"), ref CerberusConfig.Hud.ShowSyndicateIcons, "ShowSyndicateIcons");
		this.HudIconToggle(LocalizationManager.GetString("Visuals_Eye_HUD_ChemicalAnalysis"), ref CerberusConfig.Hud.ChemicalAnalysis, "SolutionScanner");
		this.HudIconToggle(LocalizationManager.GetString("Visuals_Eye_HUD_ShowElectrocution"), ref CerberusConfig.Hud.ShowElectrocution, "ShowElectrocutionHUD");
	}
	private void HudIconToggle(string labelName, ref bool value, string iconName)
	{
		bool flag = !this._componentManager.ComponentExists(iconName);
		if (!flag)
		{
			bool flag2 = ImGuiWidgets.Toggle(labelName, ref value);
			if (flag2)
			{
				bool flag3 = value;
				if (flag3)
				{
					this._componentManager.AddComponent(iconName, null);
				}
				else
				{
					this._componentManager.RemoveComponent(iconName, null);
				}
			}
		}
	}
	private void RenderEyeRightDown()
	{
		ImGui.Text(LocalizationManager.GetString("Visuals_Eye_Patches"));
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_Eye_Patches_NoClyde"), ref CerberusConfig.Settings.ClydePatch);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_Eye_Patches_NoSmoke"), ref CerberusConfig.Settings.SmokePatch);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_Eye_Patches_NoBadOverlays"), ref CerberusConfig.Settings.OverlaysPatch);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_Eye_Patches_NoCameraRecoil"), ref CerberusConfig.Settings.NoCameraKickPatch);
	}
	private void RenderFunLeft()
	{
		ImGui.Text(LocalizationManager.GetString("Visuals_Fun_General"));
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_Fun_Enabled"), ref CerberusConfig.Fun.Enabled);
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_Fun_Rotation"), ref CerberusConfig.Fun.RotationEnabled);
		ImGuiWidgets.SliderFloat(LocalizationManager.GetString("Visuals_Fun_Speed"), ref CerberusConfig.Fun.RotationSpeed, 0f, 360f);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_Fun_Jump"), ref CerberusConfig.Fun.JumpEnabled);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_Fun_Snake"), ref CerberusConfig.Fun.ShakeEnabled);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_Fun_Rainbow"), ref CerberusConfig.Fun.RainbowEnabled);
		ImGuiWidgets.ColorPicker(LocalizationManager.GetString("Visuals_Fun_Color"), ref CerberusConfig.Fun.Color);
	}
	private void RenderFunRightTop()
	{
		ImGui.Text(LocalizationManager.GetString("Visuals_Fun_Filters"));
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_Fun_Filters_Player"), ref CerberusConfig.Fun.AffectPlayer);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_Fun_Filters_Mobs"), ref CerberusConfig.Fun.AffectMobs);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_Fun_Filters_Others"), ref CerberusConfig.Fun.AffectOthers);
	}
	private void RenderFunRightDown()
	{
		ImGui.Text(LocalizationManager.GetString("Visuals_Fun_TextureOverlay"));
		ImGui.Dummy(new Vector2(0f, 10f));
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_Fun_TextureOverlay_Enabled"), ref CerberusConfig.Texture.Enabled);
		bool flag = ImGuiWidgets.Button(LocalizationManager.GetString("Visuals_Fun_TextureOverlay_OpenFolder"), null, null, true);
		if (flag)
		{
			Process.Start("explorer", this._configPath);
		}
		ImGuiWidgets.SliderFloat(LocalizationManager.GetString("Visuals_Fun_TextureOverlay_Size"), ref CerberusConfig.Texture.Size, 0.1f, 5f);
		ImGuiWidgets.Toggle(LocalizationManager.GetString("Visuals_Fun_TextureOverlay_Invisible"), ref CerberusConfig.Texture.MakeEntitiesInvisible);
	}
	private bool _imagesLoaded;
	private readonly string _configPath;
	private readonly ComponentManager _componentManager;
	private readonly SpammerSystem _spammerSystem;
	private readonly MainController _mainController;
	private readonly PlayerTrackerSystem _playerTracker;
	private readonly EntityPreviewSystem _entityPreview;
	private int _currentTab;
	private int _currentSubTab;
	private readonly List<MainMenu.TabInfo> _tabs;
	private readonly Dictionary<int, float> _tabAlpha;
	private readonly float _tabSwitchSpeed;
	private static readonly Vector4 ChildBgColor = new Vector4(0.3921569f, 0.5058823f, 0.9647059f, 1f);
	private static readonly Vector4 ChildBgColor2 = new Vector4(0.0901961f, 0.0941177f, 0.1098039f, 1f);
	private static readonly Vector4 WindowBgColor = new Vector4(0.0705882f, 0.0745098f, 0.0901961f, 1f);
	private static readonly Vector4 ButtonColor = new Vector4(0.1372549f, 0.14117648f, 0.15686274f, 1f);
	private static readonly Vector4 ButtonHoverColor = new Vector4(0.1372549f, 0.14117648f, 0.15686274f, 1f);
	private static readonly Vector4 ButtonActiveColor = new Vector4(0.0901961f, 0.0941177f, 0.1098039f, 1f);
	private static readonly Vector4 TextColor = new Vector4(0.81f, 0.81f, 0.82f, 1f);
	private static readonly Vector2 WindowSize = new Vector2(880f, 570f);
	private readonly Dictionary<string, MainMenu.ConnectStatus> _logs;
	private readonly Dictionary<NetUserId, bool> _playerWindows;
	private readonly HashSet<NetUserId> _renderedPreviews;
	private string _playerSearch;
	private int _selectedSort;
	private readonly string[] _sortOptions;
	private readonly string[][] _fonts;
	private int _resetCount;
	private bool _debugMode;
	[Obfuscation(Exclude = true, ApplyToMembers = true)]
	private class TabInfo
	{
		public TabInfo(string name, List<string> subTabs, Action renderAction)
		{
			this.Name = name;
			this.SubTabs = subTabs;
			this.RenderAction = renderAction;
		}
		public readonly string Name;
		public List<string> SubTabs;
		public readonly Action RenderAction;
	}
	private enum ConnectStatus
	{
		Join,
		Leave
	}
}
