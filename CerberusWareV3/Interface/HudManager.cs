using System;
using System.Runtime.CompilerServices;
using CerberusWareV3.Configuration;
using Robust.Shared.GameObjects;
using Robust.Shared.Player;


[CompilerGenerated]
public class HudManager : EntitySystem
{
	public override void Initialize()
	{
		base.SubscribeLocalEvent<LocalPlayerAttachedEvent>(new EntityEventHandler<LocalPlayerAttachedEvent>(this.OnLocalPlayerAttached), null, null);
	}
	private void OnLocalPlayerAttached(LocalPlayerAttachedEvent ev)
	{
		EntityUid entity = ev.Entity;
		bool flag = CerberusConfig.Hud.ShowAntag && this._componentManager.ComponentExists("ShowAntagIcons");
		if (flag)
		{
			this._componentManager.AddComponent("ShowAntagIcons", new EntityUid?(entity));
		}
		bool flag2 = CerberusConfig.Hud.ShowJobIcons && this._componentManager.ComponentExists("ShowJobIcons");
		if (flag2)
		{
			this._componentManager.AddComponent("ShowJobIcons", new EntityUid?(entity));
		}
		bool flag3 = CerberusConfig.Hud.ShowMindShieldIcons && this._componentManager.ComponentExists("ShowMindShieldIcons");
		if (flag3)
		{
			this._componentManager.AddComponent("ShowMindShieldIcons", new EntityUid?(entity));
		}
		bool flag4 = CerberusConfig.Hud.ShowCriminalRecordIcons && this._componentManager.ComponentExists("ShowCriminalRecordIcons");
		if (flag4)
		{
			this._componentManager.AddComponent("ShowCriminalRecordIcons", new EntityUid?(entity));
		}
		bool flag5 = CerberusConfig.Hud.ShowSyndicateIcons && this._componentManager.ComponentExists("ShowSyndicateIcons");
		if (flag5)
		{
			this._componentManager.AddComponent("ShowSyndicateIcons", new EntityUid?(entity));
		}
		bool flag6 = CerberusConfig.Hud.ChemicalAnalysis && this._componentManager.ComponentExists("SolutionScanner");
		if (flag6)
		{
			this._componentManager.AddComponent("SolutionScanner", new EntityUid?(entity));
		}
		bool flag7 = CerberusConfig.Hud.ChemicalAnalysis && this._componentManager.ComponentExists("ShowElectrocutionHUD");
		if (flag7)
		{
			this._componentManager.AddComponent("ShowElectrocutionHUD", new EntityUid?(entity));
		}
	}
	public override void Update(float frameTime)
	{
		this.SyncComponent("ShowAntagIcons", CerberusConfig.Hud.ShowAntag);
		this.SyncComponent("ShowJobIcons", CerberusConfig.Hud.ShowJobIcons);
		this.SyncComponent("ShowMindShieldIcons", CerberusConfig.Hud.ShowMindShieldIcons);
		this.SyncComponent("ShowCriminalRecordIcons", CerberusConfig.Hud.ShowCriminalRecordIcons);
		this.SyncComponent("ShowSyndicateIcons", CerberusConfig.Hud.ShowSyndicateIcons);
		this.SyncComponent("SolutionScanner", CerberusConfig.Hud.ChemicalAnalysis);
	}
	private void SyncComponent(string componentName, bool configValue)
	{
		bool flag = !this._componentManager.ComponentExists(componentName);
		if (!flag)
		{
			bool flag2 = this._componentManager.HasComponent(componentName, null);
			bool flag3 = configValue && !flag2;
			if (flag3)
			{
				this._componentManager.AddComponent(componentName, null);
			}
			else
			{
				bool flag4 = !configValue && flag2;
				if (flag4)
				{
					this._componentManager.RemoveComponent(componentName, null);
				}
			}
		}
	}
	private readonly ComponentManager _componentManager = new ComponentManager();
}
